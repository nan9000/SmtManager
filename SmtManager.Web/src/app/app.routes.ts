import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { HomeComponent } from './home/home.component';
import { authGuard } from './core/guards/auth.guard';
import { BoardListComponent } from './boards/board-list.component';
import { ComponentListComponent } from './components/component-list.component';

export const routes: Routes = [
    { path: '', component: HomeComponent, canActivate: [authGuard] },
    { path: 'boards', component: BoardListComponent, canActivate: [authGuard] },
    { path: 'boards/edit/:id', loadComponent: () => import('./boards/board-edit').then(m => m.BoardEdit), canActivate: [authGuard] },
    { path: 'boards/create', loadComponent: () => import('./boards/board-edit').then(m => m.BoardEdit), canActivate: [authGuard] },
    { path: 'components', component: ComponentListComponent, canActivate: [authGuard] },
    { path: 'components/edit/:id', loadComponent: () => import('./components/component-edit').then(m => m.ComponentEdit), canActivate: [authGuard] },
    { path: 'components/create', loadComponent: () => import('./components/component-edit').then(m => m.ComponentEdit), canActivate: [authGuard] },
    { path: 'orders', loadComponent: () => import('./orders/order-list').then(m => m.OrderList), canActivate: [authGuard] },
    { path: 'orders/edit/:id', loadComponent: () => import('./orders/order-edit').then(m => m.OrderEdit), canActivate: [authGuard] },
    { path: 'orders/create', loadComponent: () => import('./orders/order-edit').then(m => m.OrderEdit), canActivate: [authGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
];
