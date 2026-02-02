import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

export interface LoginDto {
    username: string;
    password: string;
}

export interface RegisterDto {
    username: string;
    email: string;
    password: string;
    role?: string;
}

export interface AuthResponse {
    token: string;
    username: string;
    email: string;
    role: string;
}

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = 'http://localhost:5000/api/auth';
    private tokenKey = 'auth_token';
    private currentUserSubject = new BehaviorSubject<AuthResponse | null>(null);
    public currentUser$ = this.currentUserSubject.asObservable();

    constructor(private http: HttpClient) {
        this.loadUserFromStorage();
    }

    private loadUserFromStorage(): void {
        const token = this.getToken();
        if (token) {
            // Decode token to get user info
            const userInfo = this.decodeToken(token);
            if (userInfo) {
                this.currentUserSubject.next(userInfo);
            }
        }
    }

    login(loginDto: LoginDto): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/login`, loginDto).pipe(
            tap((response) => {
                localStorage.setItem(this.tokenKey, response.token);
                this.currentUserSubject.next(response);
            })
        );
    }

    register(registerDto: RegisterDto): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/register`, registerDto).pipe(
            tap((response) => {
                localStorage.setItem(this.tokenKey, response.token);
                this.currentUserSubject.next(response);
            })
        );
    }

    logout(): void {
        localStorage.removeItem(this.tokenKey);
        this.currentUserSubject.next(null);
    }

    getToken(): string | null {
        return localStorage.getItem(this.tokenKey);
    }

    isAuthenticated(): boolean {
        return this.getToken() !== null;
    }

    private decodeToken(token: string): AuthResponse | null {
        try {
            const payload = token.split('.')[1];
            const decoded = JSON.parse(atob(payload));
            return {
                token: token,
                username: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || '',
                email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || '',
                role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || ''
            };
        } catch (error) {
            return null;
        }
    }
}
