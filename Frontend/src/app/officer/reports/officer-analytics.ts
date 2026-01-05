import { Component, OnInit, signal, effect, ViewChildren, QueryList } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { ReportService, StatusCount, DeptPerformance, CategoryCount } from '../../core/services/report.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { forkJoin } from 'rxjs';

@Component({
    selector: 'app-officer-analytics',
    standalone: true,
    imports: [
        CommonModule,
        BaseChartDirective,
        MatCardModule,
        MatButtonModule,
        MatIconModule,
        MatTableModule
    ],
    templateUrl: './officer-analytics.html',
    styleUrls: ['./officer-analytics.css']
})
export class OfficerAnalyticsComponent implements OnInit {
    @ViewChildren(BaseChartDirective) charts?: QueryList<BaseChartDirective>;

    statusCounts = signal<StatusCount[]>([]);
    categoryCounts = signal<CategoryCount[]>([]);
    deptPerformance = signal<DeptPerformance[]>([]);

    // Pie Chart (Status Distribution)
    public pieChartOptions: ChartConfiguration['options'] = {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
            },
        }
    };
    public pieChartData: ChartData<'pie', number[], string | string[]> = {
        labels: [],
        datasets: [{ data: [], backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0'] }]
    };

    // Doughnut Chart (Category Distribution)
    public doughnutChartOptions: ChartConfiguration['options'] = {
        responsive: true
    };
    public doughnutChartData: ChartData<'doughnut', number[], string | string[]> = {
        labels: [],
        datasets: [{ data: [], backgroundColor: ['#FF9F40', '#9966FF', '#C9CBCF', '#FFCD56'] }]
    };

    constructor(private reportService: ReportService) {
        effect(() => {
            const data = this.statusCounts();
            if (data.length) {
                this.pieChartData = {
                    labels: data.map(s => s.status),
                    datasets: [{
                        data: data.map(s => s.count),
                        backgroundColor: ['#4caf50', '#2196f3', '#ff9800', '#f44336']
                    }]
                };
                setTimeout(() => this.charts?.forEach(c => c.update()));
            }
        });

        effect(() => {
            const catData = this.categoryCounts();
            if (catData.length) {
                this.doughnutChartData = {
                    labels: catData.map(c => c.category),
                    datasets: [{
                        data: catData.map(c => c.count),
                        backgroundColor: ['#3f51b5', '#e91e63', '#009688', '#ffc107', '#607d8b']
                    }]
                };
                setTimeout(() => this.charts?.forEach(c => c.update()));
            }
        });
    }

    ngOnInit(): void {
        forkJoin({
            status: this.reportService.getGrievanceCountByStatus(),
            dept: this.reportService.getDepartmentPerformance(),
            category: this.reportService.getGrievanceCountByCategory()
        }).subscribe({
            next: (res) => {
                this.statusCounts.set(res.status);
                this.deptPerformance.set(res.dept);
                this.categoryCounts.set(res.category);
                console.log('Officer Analytics Data Loaded:', res);
            },
            error: (err) => console.error('Error loading analytics', err)
        });
    }

    getStatusIcon(status: string): string {
        switch (status.toLowerCase()) {
            case 'resolved': return 'check_circle';
            case 'submitted': return 'inbox';
            case 'assigned': return 'assignment_ind';
            case 'inreview': return 'rate_review';
            case 'closed': return 'lock';
            default: return 'help';
        }
    }
}
