import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterLink],
    template: `
    <div class="container mt-5">
      <div class="row justify-content-center">
        <div class="col-md-6">
          <div class="card shadow">
            <div class="card-header bg-success text-white">
              <h4 class="mb-0">Register</h4>
            </div>
            <div class="card-body">
              <form (ngSubmit)="onSubmit()">
                <div class="mb-3">
                  <label for="username" class="form-label">Username</label>
                  <input
                    type="text"
                    class="form-control"
                    id="username"
                    [(ngModel)]="registerDto.username"
                    name="username"
                    required
                  />
                </div>
                <div class="mb-3">
                  <label for="email" class="form-label">Email</label>
                  <input
                    type="email"
                    class="form-control"
                    id="email"
                    [(ngModel)]="registerDto.email"
                    name="email"
                    required
                  />
                </div>
                <div class="mb-3">
                  <label for="password" class="form-label">Password</label>
                  <input
                    type="password"
                    class="form-control"
                    id="password"
                    [(ngModel)]="registerDto.password"
                    name="password"
                    required
                    minlength="6"
                  />
                </div>
                @if (errorMessage) {
                  <div class="alert alert-danger">{{ errorMessage }}</div>
                }
                <button type="submit" class="btn btn-success w-100" [disabled]="loading">
                  {{ loading ? 'Registering...' : 'Register' }}
                </button>
              </form>
              <div class="mt-3 text-center">
                <a [routerLink]="['/login']">Already have an account? Login</a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
    styles: []
})
export class RegisterComponent {
    registerDto = {
        username: '',
        email: '',
        password: '',
        role: 'User'
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

        this.authService.register(this.registerDto).subscribe({
            next: () => {
                this.router.navigate(['/']);
            },
            error: (error) => {
                this.errorMessage = error.error?.message || 'Registration failed. Please try again.';
                this.loading = false;
            },
            complete: () => {
                this.loading = false;
            }
        });
    }
}
