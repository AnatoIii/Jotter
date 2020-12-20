import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

import { BehaviorSubject, Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { User } from 'src/app/shared/classes/user';
import { Response } from 'src/app/shared/classes/response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authenticated = new BehaviorSubject<boolean>(false);
  public currentUserData = new BehaviorSubject<User>(null);

  apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  get token(): string {
    return localStorage.getItem('token');
  }

  get isAuthenticated(): Observable<boolean> {
    return this.authenticated.asObservable();
  }

  get getUserData(): Observable<User> {
    return this.currentUserData.asObservable();
  }

  setToken(token: string): void {
    localStorage.setItem('token', token);
  }

  saveUser(): Observable<User> {
    this.http.get<Response>(
      `${this.apiUrl}/user`)
      .subscribe(
        response => {
          this.currentUserData.next(response.responseResult);
          this.authenticated.next(true);
        },
        error => {
          console.log(error.message.error || error.message);
        }
      );

    return this.currentUserData;
  }

  login(user: User): Observable<Response> {
    return this.http.post<Response>(
      `${this.apiUrl}/login`,
      { ...user });
  }

  register(user: User): Observable<Response> {
    return this.http.post<Response>(
      `${this.apiUrl}/register`,
      { ...user });
  }

  logout(): void {
    this.authenticated.next(false);
    localStorage.removeItem('user');
    this.router.navigate(['/', 'login']);
  }
}
