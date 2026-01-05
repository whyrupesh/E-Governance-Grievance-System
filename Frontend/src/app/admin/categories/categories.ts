import { Component, OnInit, signal, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { AdminService } from '../../core/services/admin.service';
import { Category, Department } from '../../shared/models/admin.model';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatDividerModule,
    MatPaginatorModule,
    MatSortModule
  ],
  templateUrl: './categories.html',
  styleUrls: ['./categories.css']
})
export class CategoriesComponent implements OnInit, AfterViewInit {

  // categories = signal<Category[]>([]); // Removed signal in favor of dataSource
  dataSource = new MatTableDataSource<Category>([]);
  departments: Department[] = [];
  displayedColumns: string[] = ['name', 'departmentName'];
  name = '';
  departmentId!: number;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.adminService.getDepartments()
      .subscribe(res => this.departments = res);

    this.load();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  load() {
    this.adminService.getCategories()
      .subscribe(res => {
        // this.categories.set(res);
        this.dataSource.data = res;
      });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  add() {
    if (!this.name || !this.departmentId) return;

    this.adminService.addCategory({
      name: this.name,
      departmentId: this.departmentId
    }).subscribe(() => {
      this.name = '';
      this.load();
    });
  }
}
