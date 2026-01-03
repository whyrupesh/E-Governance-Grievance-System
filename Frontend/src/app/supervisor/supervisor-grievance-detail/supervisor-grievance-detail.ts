import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { SupervisorService } from '../../core/services/supervisor.service';
import { SupervisorGrievance } from '../../shared/models/supervisor-grievance.model';

@Component({
  standalone: true,
  selector: 'app-supervisor-grievance-detail',
  imports: [CommonModule, DatePipe, MatCardModule, MatIconModule, MatButtonModule, MatSlideToggleModule],
  templateUrl: './supervisor-grievance-detail.html',
  styleUrls: ['./supervisor-grievance-detail.css']
})
export class SupervisorGrievanceDetailComponent implements OnInit {

  grievance = signal<SupervisorGrievance | null>(null);
  loading = true;
  error?: string;

  constructor(
    private route: ActivatedRoute,
    private supervisorService: SupervisorService
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.supervisorService.getById(id).subscribe({
      next: res => {
        this.grievance.set(res);
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.error = 'Failed to load grievance';
        this.loading = false;
      }
    });
  }

  onEscalateToggle() {
  const grievance = this.grievance();
  if (!grievance) return;

  this.supervisorService.escalateGrievance(grievance.id).subscribe({
    next: updated => {
      alert('Escalated Modified successfully');
      this.reload();
    },
    error: err => {
      console.error('Escalation failed', err);
    }
  });
}




  reload() {
    window.location.reload();
  }

  isReached(step: string): boolean {
    const order = ['Submitted', 'Assigned', 'InReview', 'Resolved', 'Closed'];
    return order.indexOf(this.grievance()?.status ?? '') >= order.indexOf(step);
  }
}
