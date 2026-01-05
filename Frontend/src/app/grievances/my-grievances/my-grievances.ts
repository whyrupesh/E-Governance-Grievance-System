import { Component, OnInit, signal, computed } from '@angular/core';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { GrievanceService } from '../../core/services/grievance.service';
import { Grievance } from '../../shared/models/grievance.model';
import { Category } from '../../shared/models/admin.model';

@Component({
  selector: 'app-my-grievances',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    DatePipe,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatSelectModule,
    MatPaginatorModule,
    FormsModule
  ],
  templateUrl: './my-grievances.html',
  styleUrls: ['./my-grievances.css']
})
export class MyGrievancesComponent implements OnInit {

  grievances = signal<Grievance[]>([]);
  categories = signal<Category[]>([]);

  // Filter Signals
  filterStatus = signal<string>('All');
  filterCategory = signal<string>('All');
  sortOrder = signal<string>('newest');

  loading = true;
  error?: string;

  // Pagination Signals
  pageIndex = signal<number>(0);
  pageSize = signal<number>(10);

  // 1. Filtered & Sorted List
  filteredGrievances = computed(() => {
    // Reset page index when filters change - side effect handled in template or explicit handler
    // Ideally we track filter change. For now we will rely on UI events calling a reset or effect.
    // However, computed should be pure. We will handle page reset in setter methods.

    let list = this.grievances();
    const status = this.filterStatus();
    const category = this.filterCategory();
    const sort = this.sortOrder();

    // 1. Filter by Status
    if (status !== 'All') {
      list = list.filter(g => g.status.toLowerCase() === status.toLowerCase());
    }

    // 2. Filter by Category
    if (category !== 'All') {
      // Assuming 'g.category' is the category name. If it's ID, adjust accordingly.
      // Based on model: grievance.category is string. 
      list = list.filter(g => g.category === category);
    }

    // 3. Sort by Date
    list = list.sort((a, b) => {
      const dateA = new Date(a.createdAt).getTime();
      const dateB = new Date(b.createdAt).getTime();
      return sort === 'newest' ? dateB - dateA : dateA - dateB;
    });

    return list;
  });

  // 2. Paginated List
  paginatedGrievances = computed(() => {
    const list = this.filteredGrievances();
    const index = this.pageIndex();
    const size = this.pageSize();

    const start = index * size;
    return list.slice(start, start + size);
  });

  statuses = ['Submitted', 'Pending', 'Resolved', 'Closed'];

  constructor(private grievanceService: GrievanceService) { }

  onFilterChange() {
    this.pageIndex.set(0);
  }

  handlePageEvent(e: PageEvent) {
    this.pageIndex.set(e.pageIndex);
    this.pageSize.set(e.pageSize);
  }

  resetFilters() {
    this.filterStatus.set('All');
    this.filterCategory.set('All');
    this.pageIndex.set(0);
  }

  async ngOnInit() {
    try {
      this.loading = true;

      // Load Categories
      this.grievanceService.getCategories().subscribe({
        next: (cats) => this.categories.set(cats),
        error: (err) => console.error('Failed to load categories', err)
      });

      // Load Grievances
      this.grievanceService.getMyGrievances().subscribe({
        next: (res) => this.grievances.set(res),
        error: (err) => {
          console.error(err);
          this.error = 'Failed to load grievances';
        }
      });

    } catch (err) {
      console.error(err);
      this.error = 'Failed to load grievances';
    } finally {
      this.loading = false;
    }
  }
}
