import { Component, OnInit, signal, computed, effect } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
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
    category: '',
    department: '',
    status: ''
  });

  /* ✅ SORT AS SIGNAL */
  sortOrder = signal<'asc' | 'desc'>('desc');

  /* Pagination */
  page = signal(1);
  pageSize = 5;

  constructor(private supervisorService: SupervisorService) {}

  ngOnInit() {
    this.supervisorService.getAll().subscribe({
      next: res => this.grievances.set(res),
      error: () => this.error = 'Failed to load grievances'
    });

    /* ✅ Reset page when filter/sort changes */
    effect(() => {
      this.filters();
      this.sortOrder();
      this.page.set(1);
    });
  }

  /* ---------- Filter Options ---------- */

  categories = computed(() =>
    [...new Set(this.grievances().map(g => g.category))]
  );

  departments = computed(() =>
    [...new Set(this.grievances().map(g => g.department))]
  );

  statuses = computed(() =>
    [...new Set(this.grievances().map(g => g.status))]
  );

  /* ---------- FILTER + SORT ---------- */

  updateCategory(value: string) {
  this.filters.set({
    ...this.filters(),
    category: value
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
    const { category, department, status } = this.filters();
    const order = this.sortOrder();

    let data = [...this.grievances()];

    if (category) {
      data = data.filter(g => g.category === category);
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
      category: '',
      department: '',
      status: ''
    });
    this.sortOrder.set('desc');
  }

  /* ---------- PAGINATION ---------- */

  totalPages = computed(() =>
    Math.ceil(this.filteredGrievances().length / this.pageSize)
  );

  paginatedGrievances = computed(() => {
    const start = (this.page() - 1) * this.pageSize;
    return this.filteredGrievances().slice(start, start + this.pageSize);
  });

  nextPage() {
    if (this.page() < this.totalPages()) {
      this.page.update(p => p + 1);
    }
  }

  prevPage() {
    if (this.page() > 1) {
      this.page.update(p => p - 1);
    }
  }
}
