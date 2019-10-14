import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home.component';
import { NewLComponent } from './Components/newL/newL.component';
import { LDetailsComponent } from './Components/lDetails/lDetails.component';
import { AssignmentComponent } from './Components/assignment/assignment.component';
import { DependentLComponent } from './Components/dependentL/dependentL.component';
import { ClosedLComponent } from './Components/closedL/closedL.component';
import { AuthGuard } from './Helpers/AuthGuard';
import { ApplicationRoles } from './Enums/ApplicationRoles';

const appRoutes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'newLead',
        component: NewLComponent,
        canActivate: [AuthGuard],
        data: { roles: [ApplicationRoles.User] },
        children: [
            {
                path: 'details/:lid',
                component: LDetailsComponent,
                canActivate: [AuthGuard],
                data: { roles: [ApplicationRoles.User] }
            }]
    },
    {
        path: 'dependentL',
        component: DependentLComponent,
        canActivate: [AuthGuard],
        data: { roles: [ApplicationRoles.User] },
        children: [
            {
                path: 'details/:lid',
                component: LDetailsComponent,
                canActivate: [AuthGuard],
                data: { roles: [ApplicationRoles.User] }
            }]
    },
    {
        path: 'closedL',
        component: ClosedLComponent,
        canActivate: [AuthGuard],
        data: { roles: [ApplicationRoles.User] }
    },
    {
        path: 'lDetails/:lid',
        component: LDetailsComponent,
        canActivate: [AuthGuard],
        data: { roles: [ApplicationRoles.User] }
    },
    {
        path: 'assignment',
        component: AssignmentComponent,
        canActivate: [AuthGuard],
        data: { roles: [ApplicationRoles.User] }
    },
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
