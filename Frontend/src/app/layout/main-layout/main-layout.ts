import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../core/services/auth.service';

import { NotificationService } from '../../core/services/notification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MatToolbarModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatButtonModule,
    RouterModule
  ],
  templateUrl: './main-layout.html',
  styleUrls: ['./main-layout.css']
})
export class MainLayoutComponent {
  currentUser: any;

  constructor(
    public authService: AuthService,
    private router: Router,
    public notificationService: NotificationService
  ) {
    this.currentUser = this.authService.getUser();
  }

  ngOnInit() {
    if (this.currentUser) {
      this.notificationService.refreshUnreadCount();
    }
  }

  getRoleDisplay(): string {
    return this.authService.getRole() || 'User';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
