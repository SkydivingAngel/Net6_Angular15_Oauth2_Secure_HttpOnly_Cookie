import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { LoginRequest } from '../interfaces/login-request';
import { LoginResult } from '../interfaces/login-result';
import {
  HttpClient,
  HttpHandler, HttpEvent, HttpInterceptor,
  HttpRequest, HttpResponse, HttpErrorResponse
} from "@angular/common/http";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public title?: string;
  public loginResult?: LoginResult;
  public form!: FormGroup;
  public token: string = "";
  public url: string = "";
  public baseUrl: string = "";

  constructor(
    public http: HttpClient,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public authService: AuthService,
    @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.form = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });

  }

  onSubmit() {
    var loginRequest = <LoginRequest>{};
    loginRequest.email = this.form.controls['email'].value;
    loginRequest.password = this.form.controls['password'].value;

    this.authService
      .login(loginRequest)
      .subscribe(result => {
        console.log(result);
        this.loginResult = result;
        if (result.success && result.token) {
          this.token = result.token;
          this.url = result.url;
          this.authService.isAuthenticated = true;
        }
      }, error => {
        console.log(JSON.stringify(error));

        if (error.status == 401) {
          this.loginResult = error.error;
        }
      });
  }

  check() {
    this.http.get<any>(this.baseUrl + 'api/isloggedin', { observe: 'response' }).subscribe({
      next: response => {
        alert("Ok: " + response.status);
      },
      error: error => {
        alert("Error: " + error.status + " - " + error.message);
        console.error('There was an error!', error);
      }
    });
  }
  

}
