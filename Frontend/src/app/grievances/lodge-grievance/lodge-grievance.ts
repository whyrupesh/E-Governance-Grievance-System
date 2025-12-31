import { Component } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { GrievanceService } from '../../core/services/grievance.service';
import { CommonModule } from '@angular/common';

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
    CommonModule
  ],
  templateUrl: './lodge-grievance.html'
})
export class LodgeGrievanceComponent {

  form!: FormGroup;

  categories = [
    { id: 1, name: 'Water Supply' },
    { id: 2, name: 'Electricity' },
    { id: 3, name: 'Sanitation' }
  ];

  constructor(
    private fb: FormBuilder,
    private grievanceService: GrievanceService
  ) {
    this.form = this.fb.group({
      categoryId: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  submit() {
    if (this.form.invalid) return;

    this.grievanceService.createGrievance(this.form.value)
      .subscribe(() => {
        alert('Grievance submitted successfully');
        this.form.reset();
      });
  }
}
