import { Inject, Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHandler, HttpEvent, HttpInterceptor,
  HttpRequest, HttpResponse, HttpErrorResponse
} from "@angular/common/http";
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { LoginRequest } from '../interfaces/login-request';
import { LoginResult } from '../interfaces/login-result';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  public baseUrl: string = "";

  constructor(
    protected http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;

  }

  public tokenKey: string = "token";
  public isAuthenticated: boolean = false;

  login(item: LoginRequest): Observable<LoginResult> {

    let url = this.baseUrl + "api/login";

    item.url = url;

    return this.http.post<LoginResult>(url, item);
  }

  isLoggedIn(): any {
    return this.isAuthenticated;
  }
}
