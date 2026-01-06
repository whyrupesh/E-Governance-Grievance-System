import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { GrievanceService } from '../../core/services/grievance.service';
import { Grievance } from '../../shared/models/grievance.model';
import { Location } from '@angular/common';
import { NotificationService } from '../../core/services/notification.service';


@Component({
  standalone: true,
  selector: 'app-my-grievance-detail',
  imports: [
    CommonModule,
    DatePipe,
    MatCardModule,
    MatIconModule,
    MatSlideToggleModule,
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDividerModule
  ],
  templateUrl: './my-grievance-detail.html',
  styleUrls: ['./my-grievance-detail.css']
})
export class MyGrievanceDetail implements OnInit {

  grievance = signal<Grievance | null>(null);
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private grievanceService: GrievanceService,
    private location: Location,
    private notificationService: NotificationService
  ) { }

  isReached(step: string): boolean {
    const order = ['Submitted', 'Assigned', 'InReview', 'Resolved', 'Closed'];
    const current = this.grievance()?.status;

    return order.indexOf(current!) >= order.indexOf(step);
  }

  deleteGrievance() {
    const id = this.grievance()?.id;
    if (!id) return;

    this.grievanceService.deleteMyGrievance(id).subscribe({
      next: () => {
        this.notificationService.success('Grievance deleted successfully.');
        this.location.back();
      },
      error: () => {
        this.notificationService.error('Failed to delete the grievance. Please try again later.');
      }
    });
  }


  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.grievanceService.getGrievanceById(id).subscribe({
      next: res => {
        this.grievance.set(res);
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  // Feedback
  rating = 0;
  feedbackText = '';

  setRating(star: number) {
    this.rating = star;
  }

  submitFeedback() {
    if (!this.grievance()) return;
    if (this.rating === 0) {
      this.notificationService.error('Please provide a rating.');
      return;
    }

    const data = {
      rating: this.rating,
      feedback: this.feedbackText
    };

    this.grievanceService.addFeedback(this.grievance()!.id, data).subscribe({
      next: () => {
        this.notificationService.success('Feedback submitted.');
        this.grievance.update(g => ({ ...g!, rating: this.rating, feedback: this.feedbackText }));
      },
      error: () => this.notificationService.error('Failed to submit feedback.')
    });
  }

  // Reopen
  canReopen(): boolean {
    const g = this.grievance();
    if (!g || g.status !== 'Resolved' || !g.resolvedAt) return false;

    const resolvedDate = new Date(g.resolvedAt);
    const now = new Date();
    const diffTime = Math.abs(now.getTime() - resolvedDate.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    return diffDays <= 7;
  }

  reopen() {
    if (!confirm('Are you sure you want to reopen this grievance? This will notify the authorities.')) return;

    this.loading = true;
    this.grievanceService.reopenGrievance(this.grievance()!.id).subscribe({
      next: () => {
        this.notificationService.success('Grievance reopened successfully.');
        this.ngOnInit(); // Reload
      },
      error: () => {
        this.loading = false;
        this.notificationService.error('Failed to reopen grievance.');
      }
    });
  }
  // Escalate
  canToggleEscalation(): boolean {
    const g = this.grievance();
    // Can toggle if not resolved/closed
    return !!g && g.status !== 'Resolved' && g.status !== 'Closed';
  }

  toggleEscalation() {
    const g = this.grievance();
    if (!g) return;

    const action = g.isEscalated ? 'de-escalate' : 'escalate';
    if (!confirm(`Are you sure you want to ${action} this grievance?`)) return;

    this.loading = true;
    this.grievanceService.escalateGrievance(g.id).subscribe({
      next: () => {
        this.notificationService.success(`Grievance ${action}d successfully.`);
        this.ngOnInit(); // Reload
      },
      error: () => {
        this.loading = false;
        this.notificationService.error(`Failed to ${action} grievance.`);
      }
    });
  }
}
