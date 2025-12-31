import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { AdminService } from '../../core/services/admin.service';
import { Officer, Department } from '../../shared/models/admin.model';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
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
    MatListModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './officers.html',
  styleUrls: ['./officers.css']
})
export class OfficersComponent implements OnInit {

  officers: Officer[] = [];
  departments: Department[] = [];
  form!: FormGroup;

  constructor(
    private adminService: AdminService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      departmentId: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.load();
    this.loadDepartments();
  }

  load() {
    this.adminService.getOfficers()
      .subscribe(res => this.officers = res);
  }

  loadDepartments() {
    this.adminService.getDepartments()
      .subscribe(res => this.departments = res);
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
}
