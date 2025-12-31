import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { RegisterComponent } from './auth/register/register';
import { AuthGuard } from './core/guards/auth.guard';
import { RoleGuard } from './core/guards/role.guard';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },

    {
        path: 'grievances',
        canActivate: [AuthGuard, RoleGuard],
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
        path: 'officer',
        canActivate: [AuthGuard, RoleGuard],
        data: { roles: ['Officer'] },
        loadComponent: () =>
            import('./officer/assigned-grievances/assigned-grievances')
                .then(m => m.AssignedGrievancesComponent)
    },

    // {
    //     path: 'supervisor',
    //     canActivate: [AuthGuard, RoleGuard],
    //     data: { roles: ['Supervisor'] },
    //     loadComponent: () =>
    //         import('./supervisor/supervisor').then(m => m.SupervisorComponent)
    // },

    // {
    //     path: 'admin',
    //     canActivate: [AuthGuard, RoleGuard],
    //     data: { roles: ['Admin'] },
    //     loadComponent: () =>
    //         import('./admin/admin').then(m => m.AdminComponent)
    // },

    { path: '', redirectTo: 'login', pathMatch: 'full' }
];
