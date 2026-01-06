
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { AdminService } from '../../core/services/admin.service';
import { Supervisor } from '../../shared/models/admin.model';
import { FormBuilder, Validators, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { NotificationService } from '../../core/services/notification.service';

@Component({
    selector: 'app-supervisors',
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatTableModule,
        MatIconModule,
        MatDividerModule
    ],
    templateUrl: './supervisors.html',
    styleUrls: ['./supervisors.css']
})
export class SupervisorsComponent implements OnInit {

    dataSource = new MatTableDataSource<Supervisor>([]);
    displayedColumns: string[] = ['name', 'email', 'actions'];
    form!: FormGroup;

    constructor(
        private adminService: AdminService,
        private fb: FormBuilder,
        private notificationService: NotificationService
    ) {
        this.form = this.fb.group({
            fullName: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required]
        });
    }

    ngOnInit() {
        this.load();
    }

    load() {
        this.adminService.getSupervisors()
            .subscribe(res => this.dataSource.data = res);
    }

    add() {
        if (this.form.invalid) return;

        this.adminService.createSupervisor(this.form.value)
            .subscribe(() => {
                this.notificationService.success('Supervisor registered successfully');
                this.form.reset();
                this.load();
            });
    }

    delete(id: number) {
        if (confirm('Are you sure you want to delete this supervisor?')) {
            this.adminService.deleteSupervisor(id).subscribe(() => {
                this.load();
            });
        }
    }
}
