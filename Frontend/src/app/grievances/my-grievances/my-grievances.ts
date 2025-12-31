import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { GrievanceService } from '../../core/services/grievance.service';
import { Grievance } from '../../shared/models/grievance.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-my-grievances',
  standalone: true,
  imports: [CommonModule, MatCardModule,DatePipe],
  templateUrl: './my-grievances.html'
})
export class MyGrievancesComponent implements OnInit {

  grievances: Grievance[] = [];

  constructor(private grievanceService: GrievanceService) {}


  
  ngOnInit() {
    this.grievanceService.getMyGrievances()
      .subscribe(res => this.grievances = res);
  }

  
  
}
