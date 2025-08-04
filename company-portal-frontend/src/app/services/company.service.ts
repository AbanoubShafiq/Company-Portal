import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import {   CompanyRegisterDto, 
  LoginDto, 
  SendOtpDto, 
  VerifyOtpDto, 
  ResponseDto, 
  UserProfileDto
 } from '../models/Company';
import { SessionStorageService } from './session-storage.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { JwtHelperService } from '@auth0/angular-jwt';


@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = `${environment.apiUrl}/api/company`;
  private authState = new BehaviorSubject<boolean>(false);
  public authState$ = this.authState.asObservable();
  private logoFile: File | null = null;


  constructor(
    private http: HttpClient,
    private sessionStorage: SessionStorageService,
    private router: Router,
    private toastr: ToastrService,
    private jwtHelper: JwtHelperService
 ) { }

  sendOtp(request: SendOtpDto): Observable<ResponseDto<string>> {
    return this.http.post<ResponseDto<string>>(`${this.apiUrl}/send-otp`, request);
  }

  verifyOtp(request: VerifyOtpDto): Observable<ResponseDto<string>> {
    return this.http.post<ResponseDto<string>>(`${this.apiUrl}/verify-otp`, request);
  }

  
  login(request: LoginDto): Observable<ResponseDto<string>> {
    return this.http.post<ResponseDto<string>>(`${this.apiUrl}/login`, request);
  }
  
  registerCompany(request: CompanyRegisterDto): Observable<ResponseDto<string>> {
    const formData = this.createCompanyFormData(request);
    return this.http.post<ResponseDto<string>>(`${this.apiUrl}/register`, formData);
  }
  private createCompanyFormData(request: CompanyRegisterDto): FormData {
    const formData = new FormData();
    
    formData.append('arabicName', request.arabicName);
    formData.append('englishName', request.englishName);
    formData.append('email', request.email);
    formData.append('newPassword', request.newPassword);
    formData.append('confirmPassword', request.confirmPassword);
    
    if (request.phoneNumber) {
      formData.append('phoneNumber', request.phoneNumber);
    }
    
    if (request.websiteUrl) {
      formData.append('websiteUrl', request.websiteUrl);
    }
    
    if (request.logo) {
      formData.append('logo', request.logo);
    }
    
    return formData;
  }



  checkAuthState(): void {
    const token = this.sessionStorage.getItem<string>('authToken');
    this.authState.next(!!token);
  }

  logout(): void {
    this.sessionStorage.removeItem('authToken');
    this.sessionStorage.removeItem('currentUser');
    this.authState.next(false);
    this.router.navigate(['/']);
    this.toastr.info('You have been logged out');
  }

  getCurrentUser(): UserProfileDto | null {
    const token = this.sessionStorage.getItem<string>('authToken');
    if (!token) return null;

    
    try {
      const decodedToken = this.jwtHelper.decodeToken(token);
      
      return {
        id: decodedToken.Id || decodedToken.userId,
        email: decodedToken.Email,
        name: decodedToken.Name
      };
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }


  storeLogoFile(file: File): void {
    this.logoFile = file;
  }

  getStoredLogoFile(): File | null {
    return this.logoFile;
  }

  clearStoredLogoFile(): void {
    this.logoFile = null;
  }



  handleLogin(email: string, password: string, rememberMe: boolean): Observable<ResponseDto<string>> {
    const loginData: LoginDto = { email, password };
    
    return this.login(loginData).pipe(
      tap({
        next: (response) => {
          if (response.isSuccess && response.data) {
            this.sessionStorage.setItem('authToken', response.data);
            
            this.authState.next(true);
            
            if (rememberMe) {
              localStorage.setItem('rememberedEmail', email);
            } else {
              localStorage.removeItem('rememberedEmail');
            }
          }
        },
        error: (error) => {
          console.error('Login error:', error);
          throw error; 
        }
      })
    );
  }

}