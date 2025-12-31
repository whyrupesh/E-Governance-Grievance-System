import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { ReportService, StatusCount, DeptPerformance } from '../../core/services/report.service';

@Component({
    selector: 'app-analytics',
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        MatIconModule
    ],
    templateUrl: './analytics.html',
    styleUrls: ['./analytics.css']
})
export class AnalyticsComponent implements OnInit {

    statusCounts: StatusCount[] = [];
    deptPerformance: DeptPerformance[] = [];

    constructor(private reportService: ReportService) { }

    ngOnInit() {
        this.reportService.getStatusCounts()
            .subscribe(res => this.statusCounts = res);

        this.reportService.getDepartmentPerformance()
            .subscribe(res => this.deptPerformance = res);
    }

    getIcon(status: string): string {
        switch (status.toLowerCase()) {
            case 'pending': return 'hourglass_empty';
            case 'resolved': return 'check_circle';
            case 'inreview': return 'rate_review';
            case 'closed': return 'lock';
            default: return 'info';
        }
    }
}
