import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { AdminService } from '../../core/services/admin.service';
import { Category, Department } from '../../shared/models/admin.model';

import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

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
    MatListModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './categories.html',
  styleUrls: ['./categories.css']
})
export class CategoriesComponent implements OnInit {

  categories = signal<Category[]>([]);
  departments: Department[] = [];
  name = '';
  departmentId!: number;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.adminService.getDepartments()
      .subscribe(res => this.departments = res);

    this.load();
  }

  load() {
    this.adminService.getCategories()
      .subscribe(res => this.categories.set(res));
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
