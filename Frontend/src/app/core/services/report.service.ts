import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface StatusCount {
    status: string;
    count: number;
}

export interface DeptPerformance {
    departmentName: string;
    total: number;
    resolved: number;
    pending: number;
    avgResolutionTimeDays: number;
}

@Injectable({
    providedIn: 'root'
})
export class ReportService {

    private readonly API_URL = 'http://localhost:5280/api/reports';

    constructor(private http: HttpClient) { }

    getStatusCounts(): Observable<StatusCount[]> {
        return this.http.get<StatusCount[]>(`${this.API_URL}/status-count`);
    }

    getDepartmentPerformance(): Observable<DeptPerformance[]> {
        return this.http.get<DeptPerformance[]>(`${this.API_URL}/department-performance`);
    }
}
