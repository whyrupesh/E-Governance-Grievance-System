import { Component, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import {RouterLink} from '@angular/router';

import { SupervisorService } from '../../core/services/supervisor.service';
import { SupervisorGrievance } from '../../shared/models/supervisor-grievance.model';

@Component({
  selector: 'app-supervisor-grievances',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    RouterLink
  ],
  templateUrl: './all-grievances.html',
  styleUrls: ['./all-grievances.css']
})
export class SupervisorGrievancesComponent implements OnInit {

  grievances = signal<SupervisorGrievance[]>([]);
  error?: string;

  constructor(private supervisorService: SupervisorService) {}

  ngOnInit() {
    this.supervisorService.getAll().subscribe({
      next: res => {
        this.grievances.set(res);

      },
      error: err => {
        console.error(err);
        this.error = 'Failed to load grievances';
      }
    });
  }
}
