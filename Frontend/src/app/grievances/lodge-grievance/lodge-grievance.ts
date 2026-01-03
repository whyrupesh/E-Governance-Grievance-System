import { Component, signal, OnInit } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { GrievanceService } from '../../core/services/grievance.service';
import { CommonModule } from '@angular/common';

import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { Category } from '../../shared/models/admin.model';
import { Router } from '@angular/router';


@Component({
  selector: 'app-lodge-grievance',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    CommonModule,
    MatIconModule,
    RouterModule
  ],
  templateUrl: './lodge-grievance.html',
  styleUrls: ['./lodge-grievance.css']
})
export class LodgeGrievanceComponent implements OnInit {

  form!: FormGroup;

  categories = signal<Category[]>([]);

  constructor(
    private fb: FormBuilder,
    private grievanceService: GrievanceService,
    private router: Router
  ) {
    this.form = this.fb.group({
      categoryId: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  async ngOnInit(){
     this.grievanceService.getCategories().subscribe( res =>{
        this.categories.set(res);
      })
  }

  submit() {
    if (this.form.invalid) return;

    this.grievanceService.createGrievance(this.form.value)
      .subscribe(() => {
        alert('Grievance submitted successfully');
        this.router.navigate(['/grievances']);
        this.form.reset();
      });
  }
}
