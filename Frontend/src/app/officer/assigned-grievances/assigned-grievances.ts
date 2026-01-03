import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { OfficerService } from '../../core/services/officer.service';
import { OfficerGrievance } from '../../shared/models/officer-grievance.model';

import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-assigned-grievances',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './assigned-grievances.html',
  styleUrls: ['./assigned-grievances.css']
})
export class AssignedGrievancesComponent implements OnInit {

  grievances = signal<OfficerGrievance[]>([]);
  selected?: OfficerGrievance;
  form!: FormGroup;

  constructor(
    private officerService: OfficerService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      status: ['', Validators.required],
      remarks: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.load();
  }

  load() {
    this.officerService.getAssignedGrievances()
      .subscribe(res => this.grievances.set(res));
  }

  select(g: OfficerGrievance) {
    this.selected = g;
    this.form.reset();
  }

  submit() {
    if (!this.selected || this.form.invalid) return;

    this.officerService
      .updateStatus(this.selected.id, this.form.value)
      .subscribe(() => {
        alert('Status updated');
        this.selected = undefined;
        this.load();
      });
  }
}
