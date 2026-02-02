import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterLink],
    template: `
    <div class="container mt-5">
      <div class="row justify-content-center">
        <div class="col-md-6">
          <div class="card shadow">
            <div class="card-header bg-primary text-white">
              <h4 class="mb-0">Login</h4>
            </div>
            <div class="card-body">
              <form (ngSubmit)="onSubmit()">
                <div class="mb-3">
                  <label for="username" class="form-label">Username</label>
                  <input
                    type="text"
                    class="form-control"
                    id="username"
                    [(ngModel)]="loginDto.username"
                    name="username"
                    required
                  />
                </div>
                <div class="mb-3">
                  <label for="password" class="form-label">Password</label>
                  <input
                    type="password"
                    class="form-control"
                    id="password"
                    [(ngModel)]="loginDto.password"
                    name="password"
                    required
                  />
                </div>
                @if (errorMessage) {
                  <div class="alert alert-danger">{{ errorMessage }}</div>
                }
                <button type="submit" class="btn btn-primary w-100" [disabled]="loading">
                  {{ loading ? 'Logging in...' : 'Login' }}
                </button>
              </form>
              <div class="mt-3 text-center">
                <a [routerLink]="['/register']">Don't have an account? Register</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
    styles: []
})
export class LoginComponent {
    loginDto = {
        username: '',
        password: ''
    };
    loading = false;
    errorMessage = '';

    constructor(
        private authService: AuthService,
        private router: Router
    ) { }

    onSubmit(): void {
        this.loading = true;
        this.errorMessage = '';

        this.authService.login(this.loginDto).subscribe({
            next: () => {
                this.router.navigate(['/']);
            },
            error: (error) => {
                this.errorMessage = error.error?.message || 'Login failed. Please try again.';
                this.loading = false;
            },
            complete: () => {
                this.loading = false;
            }
        });
    }
}
