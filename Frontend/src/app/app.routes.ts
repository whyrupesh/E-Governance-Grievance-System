import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { RegisterComponent } from './auth/register/register';
import { AuthGuard } from './core/guards/auth.guard';
import { RoleGuard } from './core/guards/role.guard';
import { MainLayoutComponent } from './layout/main-layout/main-layout';

export const routes: Routes = [
  // =====================
  // LANDING PAGE (Priority)
  // =====================
  {
    path: '',
    loadComponent: () => import('./landing/landing.component').then(m => m.LandingComponent),
    pathMatch: 'full'
  },

  // =====================
  // PUBLIC ROUTES
  // =====================
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // =====================
  // PROTECTED ROUTES
  // =====================
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [

      // -------- Shared --------
      {
        path: 'notifications',
        loadComponent: () =>
          import('./features/notification-list/notification-list.component')
            .then(m => m.NotificationListComponent)
      },

      // -------- Citizen --------
      {
        path: 'grievances',
        canActivate: [RoleGuard],
        data: { roles: ['Citizen'] },
        children: [
          {
            path: '',
            loadComponent: () =>
              import('./grievances/my-grievances/my-grievances')
                .then(m => m.MyGrievancesComponent)
          },
          {
            path: 'new',
            loadComponent: () =>
              import('./grievances/lodge-grievance/lodge-grievance')
                .then(m => m.LodgeGrievanceComponent)
          }
        ]
      },
      {
        path: 'grievances/:id',
        loadComponent: () =>
          import('./grievances/my-grievance-detail/my-grievance-detail')
            .then(m => m.MyGrievanceDetail)
      },

      // -------- Officer --------
      {
        path: 'officer',
        canActivate: [RoleGuard],
        data: { roles: ['Officer'] },
        children: [
          { path: 'assigned-grievances', loadComponent: () => import('./officer/assigned-grievances/assigned-grievances').then(m => m.AssignedGrievancesComponent) },
          { path: 'analytics', loadComponent: () => import('./officer/reports/officer-analytics').then(m => m.OfficerAnalyticsComponent) },
          { path: '', redirectTo: 'assigned-grievances', pathMatch: 'full' }
        ]
      },

      // -------- Supervisor --------
      {
        path: 'supervisor',
        canActivate: [RoleGuard],
        data: { roles: ['Supervisor'] },
        children: [
          { path: 'all-grievances', loadComponent: () => import('./supervisor/all-grievances/all-grievances').then(m => m.SupervisorGrievancesComponent) },
          { path: 'detail/:id', loadComponent: () => import('./supervisor/supervisor-grievance-detail/supervisor-grievance-detail').then(m => m.SupervisorGrievanceDetailComponent) },
          { path: 'analytics', loadComponent: () => import('./supervisor/reports/supervisor-analytics').then(m => m.SupervisorAnalyticsComponent) },
          { path: '', redirectTo: 'all-grievances', pathMatch: 'full' }
        ]
      },

      // -------- Admin --------
      {
        path: 'admin',
        canActivate: [RoleGuard],
        data: { roles: ['Admin'] },
        children: [
          {
            path: 'departments',
            loadComponent: () =>
              import('./admin/departments/departments')
                .then(m => m.DepartmentsComponent)
          },
          {
            path: 'categories',
            loadComponent: () =>
              import('./admin/categories/categories')
                .then(m => m.CategoriesComponent)
          },
          {
            path: 'officers',
            loadComponent: () =>
              import('./admin/officers/officers')
                .then(m => m.OfficersComponent)
          },
          {
            path: 'supervisors',
            loadComponent: () =>
              import('./admin/supervisors/supervisors')
                .then(m => m.SupervisorsComponent)
          },
          {
            path: 'reports',
            loadComponent: () =>
              import('./admin/reports/analytics')
                .then(m => m.AnalyticsComponent)
          }
        ]
      }
    ]
  },

  // =====================
  // DEFAULT
  // =====================
  { path: '**', redirectTo: '' }
];
