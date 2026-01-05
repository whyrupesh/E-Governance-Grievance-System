import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NotificationService } from '../../core/services/notification.service';
import { Notification } from '../../shared/models/notification.model';
import { Router } from '@angular/router';

@Component({
    selector: 'app-notification-list',
    standalone: true,
    imports: [
        CommonModule,
        DatePipe,
        MatCardModule,
        MatIconModule,
        MatButtonModule,
        MatListModule,
        MatTooltipModule
    ],
    templateUrl: './notification-list.component.html',
    styleUrls: ['./notification-list.component.css']
})
export class NotificationListComponent implements OnInit {

    notifications = signal<Notification[]>([]);
    loading = signal(true);

    unreadCount = computed(() => {
        return this.notifications().filter(n => !n.isRead).length;
    });

    constructor(
        private notificationService: NotificationService,
        private router: Router
    ) { }

    ngOnInit() {
        this.loadNotifications();
    }

    loadNotifications() {
        this.loading.set(true);
        this.notificationService.getMyNotifications().subscribe({
            next: (res) => {
                this.notifications.set(res);
                this.loading.set(false);
            },
            error: (err) => {
                console.error('Error loading notifications', err);
                this.loading.set(false);
            }
        });
    }

    markAsRead(n: Notification) {
        if (n.isRead) return;

        this.notificationService.markAsRead(n.id).subscribe(() => {
            this.notifications.update(list =>
                list.map(item => item.id === n.id ? { ...item, isRead: true } : item)
            );
        });
    }

    markAllAsRead() {
        this.notificationService.markAllAsRead().subscribe(() => {
            this.notifications.update(list =>
                list.map(item => ({ ...item, isRead: true }))
            );
            this.notificationService.success('All notifications marked as read');
        });
    }

    viewGrievance(n: Notification) {
        // If unread, mark as read then navigate
        if (!n.isRead) {
            this.markAsRead(n);
        }

        if (n.relatedGrievanceId) {
            // We need to know user role to construct correct URL, 
            // OR we can just check URL pattern if simple. 
            // For simplicity, let's assume route /grievances/{id} works for Citizen.
            // Officer might need /officer/assigned-grievances with selection logic, or detail page.
            // Let's rely on standard routing or just navigate to generic detail if possible.
            // Re-evaluating: user role is needed for correct routing.
            // But for now, let's just mark read. User can navigate manually.
            // Or specific feature: navigate based on context. 
            // Simpler for now: just navigate to /grievances/{id} (Citizen main case)
            this.router.navigate(['/grievances', n.relatedGrievanceId]);
        }
    }
}
