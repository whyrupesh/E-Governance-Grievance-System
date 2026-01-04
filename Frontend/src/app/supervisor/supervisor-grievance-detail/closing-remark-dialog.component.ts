import { Component, model } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
    MatDialogModule,
    MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
    selector: 'app-closing-remark-dialog',
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        MatDialogModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
    ],
    template: `
    <h2 mat-dialog-title>Close Grievance</h2>
    <mat-dialog-content>
      <p>Please provide a closing remark for this grievance.</p>
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Remarks</mat-label>
        <textarea matInput [(ngModel)]="remarks" rows="4" placeholder="Enter reason for closing..."></textarea>
      </mat-form-field>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Cancel</button>
      <button mat-raised-button color="warn" [disabled]="!remarks().trim()" (click)="onConfirm()">
        Close Grievance
      </button>
    </mat-dialog-actions>
  `,
    styles: [`
    .full-width {
      width: 100%;
    }
  `]
})
export class ClosingRemarkDialogComponent {
    remarks = model('');

    constructor(
        public dialogRef: MatDialogRef<ClosingRemarkDialogComponent>
    ) { }

    onCancel(): void {
        this.dialogRef.close();
    }

    onConfirm(): void {
        this.dialogRef.close(this.remarks());
    }
}
