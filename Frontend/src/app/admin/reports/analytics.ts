import { Component, OnInit, signal, ViewChildren, QueryList, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { ReportService, StatusCount, DeptPerformance, CategoryCount } from '../../core/services/report.service';
import { forkJoin } from 'rxjs';
@Component({
    selector: 'app-analytics',
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        MatIconModule,
        MatProgressSpinnerModule,
        BaseChartDirective
    ],
    templateUrl: './analytics.html',
    styleUrls: ['./analytics.css']
})
export class AnalyticsComponent implements OnInit {
    @ViewChildren(BaseChartDirective) charts?: QueryList<BaseChartDirective>;

    statusCounts = signal<StatusCount[]>([]);
    deptPerformance = signal<DeptPerformance[]>([]);
    categoryCounts = signal<CategoryCount[]>([]);
    loading = signal(true);

    // Chart Options
    public pieChartOptions: ChartConfiguration['options'] = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: { display: true, position: 'right' }
        }
    };

    public barChartOptions: ChartConfiguration['options'] = {
        responsive: true,
        maintainAspectRatio: false,
        indexAxis: 'y',
        plugins: {
            legend: { display: false }
        },
        scales: {
            x: { beginAtZero: true }
        }
    };

    public doughnutChartOptions: ChartConfiguration['options'] = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: { display: true, position: 'right' }
        }
    };

    // Chart Data
    public pieChartData: ChartData<'pie', number[], string | string[]> = {
        labels: [],
        datasets: [{ data: [] }]
    };

    public doughnutChartData: ChartData<'doughnut'> = {
        labels: [],
        datasets: [{ data: [] }]
    };

    public barChartData: ChartData<'bar'> = {
        labels: [],
        datasets: [
            { data: [], label: 'Resolved', backgroundColor: '#4caf50' },
            { data: [], label: 'Pending', backgroundColor: '#ff9800' }
        ]
    };

    constructor(private reportService: ReportService) {
        // Effects to update charts
        effect(() => {
            const statusData = this.statusCounts();
            if (statusData.length) {
                this.pieChartData = {
                    labels: statusData.map(s => s.status),
                    datasets: [{
                        data: statusData.map(s => s.count),
                        backgroundColor: ['#2196f3', '#ff9800', '#9c27b0', '#4caf50', '#757575']
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
                        backgroundColor: [
                            '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF',
                            '#FF9F40', '#C9CBCF', '#7BC225', '#B554FF', '#E46651'
                        ]
                    }]
                };
                setTimeout(() => this.charts?.forEach(c => c.update()));
            }
        });

        effect(() => {
            const deptData = this.deptPerformance();
            if (deptData.length) {
                this.barChartData = {
                    labels: deptData.map(d => d.department),
                    datasets: [
                        { data: deptData.map(d => d.resolvedGrievances), label: 'Resolved', backgroundColor: '#4caf50' },
                        { data: deptData.map(d => d.pendingGrievances), label: 'Pending', backgroundColor: '#ff9800' }
                    ]
                };
                setTimeout(() => this.charts?.forEach(c => c.update()));
            }
        });
    }

    ngOnInit() {
        this.loadData();
    }

    loadData() {
        this.loading.set(true);

        forkJoin({
            status: this.reportService.getGrievanceCountByStatus(),
            dept: this.reportService.getDepartmentPerformance(),
            category: this.reportService.getGrievanceCountByCategory()
        }).subscribe({
            next: ({ status, dept, category }) => {
                this.statusCounts.set(status);
                this.deptPerformance.set(dept);
                this.categoryCounts.set(category);
                this.loading.set(false);
            },
            error: () => this.loading.set(false)
        });
    }

    getStatusIcon(status: string): string {
        switch (status.toLowerCase()) {
            case 'pending': return 'hourglass_empty';
            case 'resolved': return 'check_circle';
            case 'inreview': return 'rate_review';
            case 'closed': return 'lock';
            case 'submitted': return 'send';
            case 'assigned': return 'assignment';
            default: return 'info';
        }
    }
}
