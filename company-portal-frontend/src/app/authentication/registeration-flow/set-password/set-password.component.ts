import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CompanyService } from '../../../services/company.service';
import { SessionStorageService } from '../../../services/session-storage.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CompanyRegisterDto } from '../../../models/Company';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-set-password',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './set-password.component.html',
  styleUrl: './set-password.component.css'
})
export class SetPasswordComponent {
  @Input() registrationData: any = {};
  @Output() registrationComplete = new EventEmitter<void>();
  
  passwordForm: FormGroup;
  isLoading = false;
  showNewPassword = false;
  showConfirmPassword = false;

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private sessionStorage: SessionStorageService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.passwordForm = this.fb.group({
      newPassword: ['', [
        Validators.required,
        Validators.minLength(6),
      ]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('newPassword')?.value === form.get('confirmPassword')?.value 
      ? null 
      : { mismatch: true };
  }


  completeRegistration(): void {
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    
    // Get all registration data from session
    const registrationData = this.sessionStorage.getItem<any>('registrationData');
    if (!registrationData || !registrationData.isVerified) {
      this.isLoading = false;
      this.toastr.error('Registration data is missing or not verified', 'Error');
      return;
    }
    const logoFile = this.companyService.getStoredLogoFile();

    // Prepare the DTO object
    const registerDto: CompanyRegisterDto = {
      arabicName: registrationData.arabicName,
      englishName: registrationData.englishName,
      email: registrationData.email,
      phoneNumber: registrationData.phoneNumber || undefined,
      websiteUrl: registrationData.websiteUrl || undefined,
      newPassword: this.passwordForm.value.newPassword,
      confirmPassword: this.passwordForm.value.confirmPassword,
      logo: logoFile || undefined,
    };

    // Call register service with the DTO
    this.companyService.registerCompany(registerDto).subscribe({
      next: (response) => {
        this.toastr.success(response.message!, 'Success', {
          timeOut: 3000,
          progressBar: true
        });

        this.isLoading = false;
        this.sessionStorage.removeItem('registrationData');
        this.companyService.clearStoredLogoFile(); // Clear the stored file
        
        this.registrationComplete.emit();
        
        this.router.navigate(['auth/login']);
      },
      error: (error) => {
        this.isLoading = false;
        this.toastr.error(error.error.Message || 'Registration failed', 'Error');
        console.error('Registration error:', error);
      }
    });
  }


  getPasswordStrength(): string {
    const password = this.passwordForm.controls['newPassword'].value || '';
    if (password.length >= 8) return 'Strong password';
    if (password.length >= 6) return 'Good password';
    if (password.length > 0) return 'Weak password';
    return '';
  }

  getPasswordStrengthClass(): string {
    const password = this.passwordForm.controls['newPassword'].value || '';
    if (password.length >= 8) return 'text-success';
    if (password.length >= 6) return 'text-warning';
    if (password.length > 0) return 'text-danger';
    return '';
  }
}