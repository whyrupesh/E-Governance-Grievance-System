import { Component, OnInit, signal, computed, effect } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { SupervisorService } from '../../core/services/supervisor.service';
import { SupervisorGrievance } from '../../shared/models/supervisor-grievance.model';

@Component({
  selector: 'app-supervisor-grievances',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    MatIconModule,
    MatButtonModule,
    MatPaginatorModule,
    RouterLink,
    FormsModule
  ],
  templateUrl: './all-grievances.html',
  styleUrls: ['./all-grievances.css']
})
export class SupervisorGrievancesComponent implements OnInit {

  grievances = signal<SupervisorGrievance[]>([]);
  error?: string;

  /* ✅ FILTERS AS SIGNAL */
  filters = signal({
    escalated: '',
    department: '',
    status: ''
  });

  /* ✅ SORT AS SIGNAL */
  sortOrder = signal<'asc' | 'desc'>('desc');

  /* Pagination */
  pageIndex = signal(0);
  pageSize = signal(10);

  constructor(private supervisorService: SupervisorService) { }

  ngOnInit() {
    this.supervisorService.getAll().subscribe({
      next: res => this.grievances.set(res),
      error: () => this.error = 'Failed to load grievances'
    });

    /* ✅ Reset page when filter/sort changes */
    effect(() => {
      this.filters();
      this.sortOrder();
      this.pageIndex.set(0);
    });
  }

  /* ---------- Filter Options ---------- */

  // Removed categories since we filter by escalation now

  departments = computed(() =>
    [...new Set(this.grievances().map(g => g.department))]
  );

  statuses = computed(() =>
    [...new Set(this.grievances().map(g => g.status))]
  );

  /* ---------- FILTER + SORT ---------- */

  updateEscalated(value: string) {
    this.filters.set({
      ...this.filters(),
      escalated: value
    });
  }

  updateDepartment(value: string) {
    this.filters.set({
      ...this.filters(),
      department: value
    });
  }

  updateStatus(value: string) {
    this.filters.set({
      ...this.filters(),
      status: value
    });
  }

  updateSort(value: 'asc' | 'desc') {
    this.sortOrder.set(value);
  }


  filteredGrievances = computed(() => {
    const { escalated, department, status } = this.filters();
    const order = this.sortOrder();

    let data = [...this.grievances()];

    if (escalated) {
      const isEscalated = escalated === 'true';
      data = data.filter(g => g.isEscalated === isEscalated);
    }

    if (department) {
      data = data.filter(g => g.department === department);
    }

    if (status) {
      data = data.filter(g => g.status === status);
    }

    data.sort((a, b) =>
      order === 'desc'
        ? new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
        : new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
    );

    return data;
  });

  resetFilters() {
    this.filters.set({
      escalated: '',
      department: '',
      status: ''
    });
    this.sortOrder.set('desc');
  }

  /* ---------- PAGINATION ---------- */

  paginatedGrievances = computed(() => {
    const start = this.pageIndex() * this.pageSize();
    return this.filteredGrievances().slice(start, start + this.pageSize());
  });

  handlePageEvent(e: PageEvent) {
    this.pageIndex.set(e.pageIndex);
    this.pageSize.set(e.pageSize);
  }
}
