import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpClient } from '@angular/common/http';
import { Notification } from '../../shared/models/notification.model';

@Injectable({
    providedIn: 'root'
})
export class NotificationService {

    private readonly API_URL = 'http://localhost:5280/api/notifications';

    constructor(
        private snackBar: MatSnackBar,
        private http: HttpClient
    ) { }

    // --- Backend API Methods ---

    getMyNotifications() {
        return this.http.get<Notification[]>(`${this.API_URL}/my`);
    }

    markAsRead(id: number) {
        return this.http.put(`${this.API_URL}/${id}/read`, {});
    }

    markAllAsRead() {
        return this.http.put(`${this.API_URL}/read-all`, {});
    }

    // --- Existing SnackBar Methods ---
    success(message: string) {
        this.show(message, 'success-snackbar');
    }

    error(message: string) {
        this.show(message, 'error-snackbar');
    }

    info(message: string) {
        this.show(message, 'info-snackbar');
    }

    private show(message: string, panelClass: string) {
        this.snackBar.open(message, 'Close', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top',
            panelClass: [panelClass]
        });
    }
}
