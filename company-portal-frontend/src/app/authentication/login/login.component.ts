import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CompanyService } from '../../services/company.service';
import { SessionStorageService } from '../../services/session-storage.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  showPassword = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private sessionStorage: SessionStorageService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false]
    });
  }


  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const { email, password, rememberMe } = this.loginForm.value;

    this.companyService.handleLogin(email, password, rememberMe).subscribe({
      next: (response) => {
        this.isLoading = false;
        
        if (response.isSuccess) {
          // Navigation is the only responsibility left for the component
          this.router.navigate(['/landing']);
        } else {
          this.errorMessage = response.message || 'Login failed. Please try again.';
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Login error:', error);
        
        // Improved error message handling
        this.errorMessage = error.error?.message || 
                          error.error?.Message || 
                          'An unexpected error occurred during login.';
      }
    });
  }


  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  ngOnInit(): void {
    // Auto-fill email if remembered
    const rememberedEmail = localStorage.getItem('rememberedEmail');
    if (rememberedEmail) {
      this.loginForm.patchValue({
        email: rememberedEmail,
        rememberMe: true
      });
    }
  }

  clearError(): void {
    this.errorMessage = '';
  }
}