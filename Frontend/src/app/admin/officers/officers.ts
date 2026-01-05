import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { AdminService } from '../../core/services/admin.service';
import { Officer, Department } from '../../shared/models/admin.model';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-officers',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './officers.html',
  styleUrls: ['./officers.css']
})
export class OfficersComponent implements OnInit {

  dataSource = new MatTableDataSource<Officer>([]);
  departments = signal<Department[]>([]);
  displayedColumns: string[] = ['name', 'email', 'actions'];
  form!: FormGroup;

  constructor(
    private adminService: AdminService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      departmentId: [null, Validators.required]
    });
  }

  ngOnInit() {
    this.load();
    this.loadDepartments();
  }

  load() {
    this.adminService.getOfficers()
      .subscribe(res => this.dataSource.data = res);
  }

  loadDepartments() {
    this.adminService.getDepartments()
      .subscribe(res => this.departments.set(res));
  }

  add() {
    if (this.form.invalid) return;

    this.adminService.createOfficer(this.form.value)
      .subscribe(() => {
        alert('Officer registered successfully');
        this.form.reset();
        this.load();
      });
  }

  delete(id: number) {
    if (confirm('Are you sure you want to delete this officer?')) {
      this.adminService.deleteOfficer(id).subscribe(() => {
        this.load();
      });
    }
  }
}
