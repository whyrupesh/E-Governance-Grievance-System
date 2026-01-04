import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { ClosingRemarkDialogComponent } from './closing-remark-dialog.component';

import { SupervisorService } from '../../core/services/supervisor.service';
import { SupervisorGrievance } from '../../shared/models/supervisor-grievance.model';

@Component({
  standalone: true,
  selector: 'app-supervisor-grievance-detail',
  imports: [
    CommonModule,
    DatePipe,
    FormsModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatSlideToggleModule,
    MatSelectModule,
    MatInputModule,
    MatDialogModule
  ],
  templateUrl: './supervisor-grievance-detail.html',
  styleUrls: ['./supervisor-grievance-detail.css']
})
export class SupervisorGrievanceDetailComponent implements OnInit {

  grievance = signal<SupervisorGrievance | null>(null);
  officers = signal<{ id: number; fullName: string; email: string; }[]>([]);

  selectedOfficerId: number | null = null;


  loading = true;
  error?: string;

  constructor(
    private route: ActivatedRoute,
    private supervisorService: SupervisorService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.supervisorService.getById(id).subscribe({
      next: res => {
        this.grievance.set(res);
        this.loadOfficers();
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load grievance';
        this.loading = false;
      }
    });
  }

  loadOfficers() {
    const grievance = this.grievance();
    if (!grievance) return;

    this.supervisorService
      .getOfficersByDepartment(grievance.departmentId)
      .subscribe({
        next: res => this.officers.set(res),
        error: err => console.error(err)
      });
  }

  assignOfficer() {
    if (!this.selectedOfficerId || !this.grievance()) return;
    console.log(this.selectedOfficerId);
    console.log(this.grievance()!.id);

    this.supervisorService
      .assignOfficer(this.grievance()!.id, this.selectedOfficerId)
      .subscribe({
        next: () => this.reload(),
        error: err => console.error(err)
      });
  }

  closeGrievance() {
    const dialogRef = this.dialog.open(ClosingRemarkDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.supervisorService
          .closeGrievance(this.grievance()!.id, result)
          .subscribe({
            next: () => this.reload(),
            error: err => console.error(err)
          });
      }
    });
  }

  onEscalateToggle() {
    if (!this.grievance()) return;

    this.supervisorService
      .escalateGrievance(this.grievance()!.id)
      .subscribe(() => this.reload());
  }

  reload() {
    window.location.reload();
  }

  isReached(step: string): boolean {
    const order = ['Submitted', 'Assigned', 'InReview', 'Resolved', 'Closed'];
    return order.indexOf(this.grievance()?.status ?? '') >= order.indexOf(step);
  }
}
