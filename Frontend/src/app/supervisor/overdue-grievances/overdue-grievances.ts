import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { SupervisorService } from '../../core/services/supervisor.service';
import { SupervisorGrievance } from '../../shared/models/supervisor-grievance.model';

@Component({
  selector: 'app-overdue-grievances',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './overdue-grievances.html'
})
export class OverdueGrievancesComponent implements OnInit {

  grievances: SupervisorGrievance[] = [];

  constructor(private supervisorService: SupervisorService) {}

  ngOnInit() {
    this.supervisorService.getOverdue(7)
      .subscribe(res => this.grievances = res);
  }
}
