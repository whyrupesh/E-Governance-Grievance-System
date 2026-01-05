import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

import { OfficerService } from '../../core/services/officer.service';
import { NotificationService } from '../../core/services/notification.service';
import { OfficerGrievance, GrievanceStatus } from '../../shared/models/officer-grievance.model';

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
    ReactiveFormsModule
  ],
  templateUrl: './assigned-grievances.html',
  styleUrls: ['./assigned-grievances.css']
})
export class AssignedGrievancesComponent implements OnInit {

  grievances = signal<OfficerGrievance[]>([]);
  selected: OfficerGrievance | undefined;
  form!: FormGroup;
  GrievanceStatus = GrievanceStatus;

  constructor(
    private officerService: OfficerService,
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
  }

  load() {
    this.officerService.getAssignedGrievances()
      .subscribe(res => this.grievances.set(res));
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
