import { Component, OnInit, signal } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { firstValueFrom } from 'rxjs';

import { GrievanceService } from '../../core/services/grievance.service';
import { Grievance } from '../../shared/models/grievance.model';

@Component({
  selector: 'app-my-grievances',
  standalone: true,
  imports: [CommonModule, DatePipe, MatCardModule, MatIconModule, MatButtonModule],
  templateUrl: './my-grievances.html',
  styleUrls: ['./my-grievances.css']
})
export class MyGrievancesComponent implements OnInit {

  grievances = signal<Grievance[]>([]);
  loading = true;
  error?: string;

  constructor(private grievanceService: GrievanceService) {}

  async ngOnInit() {
    try {
      this.loading = true;

      this.grievanceService.getMyGrievances().subscribe( res =>{
        this.grievances.set(res);
      })

    } catch (err) {
      console.error(err);
      this.error = 'Failed to load grievances';
    } finally {
      this.loading = false;
    }
  }
}
