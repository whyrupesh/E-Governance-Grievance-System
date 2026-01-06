import { Injectable, signal } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
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

    unreadCount = signal(0);

    getMyNotifications() {
        return this.http.get<Notification[]>(`${this.API_URL}/my`).pipe(
            tap(notifications => {
                this.updateUnreadCount(notifications);
            })
        );
    }

    refreshUnreadCount() {
        this.getMyNotifications().subscribe();
    }

    private updateUnreadCount(notifications: Notification[]) {
        const count = notifications.filter(n => !n.isRead).length;
        this.unreadCount.set(count);
    }

    markAsRead(id: number) {
        return this.http.put(`${this.API_URL}/${id}/read`, {}).pipe(
            tap(() => {
                // Optimistic update or refresh
                this.unreadCount.update(c => Math.max(0, c - 1));
            })
        );
    }

    markAllAsRead() {
        return this.http.put(`${this.API_URL}/read-all`, {}).pipe(
            tap(() => this.unreadCount.set(0))
        );
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
