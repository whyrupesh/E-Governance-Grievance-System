import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { GrievanceService } from '../../core/services/grievance.service';
import { Grievance } from '../../shared/models/grievance.model';

@Component({
  standalone: true,
  selector: 'app-my-grievance-detail',
  imports: [CommonModule, DatePipe, MatCardModule, MatIconModule],
  templateUrl: './my-grievance-detail.html',
  styleUrls: ['./my-grievance-detail.css']
})
export class MyGrievanceDetail implements OnInit {

  grievance = signal<Grievance | null>(null);
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private grievanceService: GrievanceService
  ) {}

  isReached(step: string): boolean {
  const order = ['Submitted','Assigned','InReview', 'Resolved','Closed'];
  const current = this.grievance()?.status;

  return order.indexOf(current!) >= order.indexOf(step);
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
}


