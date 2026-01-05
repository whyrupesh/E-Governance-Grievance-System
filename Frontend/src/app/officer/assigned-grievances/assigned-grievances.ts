import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormsModule } from '@angular/forms';

import { OfficerService } from '../../core/services/officer.service';
import { GrievanceService } from '../../core/services/grievance.service';
import { NotificationService } from '../../core/services/notification.service';
import { OfficerGrievance, GrievanceStatus } from '../../shared/models/officer-grievance.model';
import { Category } from '../../shared/models/admin.model';

@Component({
  selector: 'app-assigned-grievances',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatDividerModule,
    MatPaginatorModule,
    ReactiveFormsModule,
    FormsModule,
    DatePipe
  ],
  templateUrl: './assigned-grievances.html',
  styleUrls: ['./assigned-grievances.css']
})
export class AssignedGrievancesComponent implements OnInit {

  grievances = signal<OfficerGrievance[]>([]);
  categories = signal<Category[]>([]);
  selected: OfficerGrievance | undefined;
  form!: FormGroup;
  GrievanceStatus = GrievanceStatus;

  // Filter Signals
  filterStatus = signal<string>('All');
  filterCategory = signal<string>('All');
  sortOrder = signal<string>('newest');

  // Pagination Signals
  pageIndex = signal<number>(0);
  pageSize = signal<number>(10);

  // Status Options
  statuses = ['Submitted', 'Assigned', 'InReview', 'Resolved', 'Closed'];

  // Computed: Filtered & Sorted
  filteredGrievances = computed(() => {
    let list = this.grievances();
    const status = this.filterStatus();
    const category = this.filterCategory();
    const sort = this.sortOrder();

    // 1. Filter by Status
    if (status !== 'All') {
      list = list.filter(g => g.status.toLowerCase() === status.toLowerCase()); // Case insensitive check
    }

    // 2. Filter by Category
    if (category !== 'All') {
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

  // Computed: Paginated
  paginatedGrievances = computed(() => {
    const list = this.filteredGrievances();
    const index = this.pageIndex();
    const size = this.pageSize();
    const start = index * size;
    return list.slice(start, start + size);
  });

  constructor(
    private officerService: OfficerService,
    private grievanceService: GrievanceService,
    private fb: FormBuilder,
    private notificationService: NotificationService
  ) {
    this.form = this.fb.group({
      status: [null, Validators.required],
      resolutionRemarks: ['']
    });
  }

  ngOnInit() {
    this.load();
    this.loadCategories();
  }

  load() {
    this.officerService.getAssignedGrievances()
      .subscribe(res => this.grievances.set(res));
  }

  loadCategories() {
    this.grievanceService.getCategories().subscribe(res => {
      this.categories.set(res);
    });
  }

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

  select(g: OfficerGrievance) {
    this.selected = g;
    this.form.reset();
    // Not patching status because enum mismatch risk and "Update" intent usually starts fresh or requires mapping.
    // If needed we can map later.
  }

  submit() {
    if (!this.selected || this.form.invalid) return;

    this.officerService
      .updateStatus(this.selected.id, this.form.value)
      .subscribe(() => {
        this.notificationService.success('Status updated successfully');
        this.selected = undefined;
        this.load();
      });
  }
}
