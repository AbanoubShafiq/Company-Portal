import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CompanyService } from '../../../services/company.service';
import { SessionStorageService } from '../../../services/session-storage.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { SendOtpDto } from '../../../models/Company';


@Component({
  selector: 'app-verify-otp',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './verify-otp.component.html',
  styleUrl: './verify-otp.component.css'
})
export class VerifyOtpComponent {
  @Input() email: string = '';
  @Output() stepComplete = new EventEmitter<any>();
  
  otpForm: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private sessionStorage: SessionStorageService,
    private toastr: ToastrService
  ) {
    this.otpForm = this.fb.group({
      otp: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]]
    });
  }

  verifyOtp(): void {
    if (this.otpForm.invalid || !this.email) return;

    this.isLoading = true;
    const request = {
      email: this.email,
      otp: this.otpForm.value.otp
    };

    this.companyService.verifyOtp(request).subscribe({
      next: (response) => {
        this.isLoading = false;
          this.toastr.success(response.message!, 'Success', {
          timeOut: 3000,
          progressBar: true
          });
        const registrationData = this.sessionStorage.getItem<any>('registrationData');
        if (registrationData) {
          this.sessionStorage.setItem('registrationData', {
            ...registrationData,
            isVerified: true
          });
        }
        
        this.stepComplete.emit({ isVerified: true });
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
        this.toastr.error(error.error.Message, 'Faild');

      }
    });
  }

  resendOtp(): void {
    if (!this.email) return;
    const otpRequest: SendOtpDto = {
      email: this.email,
      phoneNumber: null
    };
    this.companyService.sendOtp(otpRequest).subscribe({
      next: (response) => {
        console.log('OTP resent successfully');
        this.toastr.success(response.message!, 'Success', {
          timeOut: 3000,
          progressBar: true
        });
        // Show success message
      },
      error: (error) => {
        console.log(error);
        this.toastr.error(error.error.Message, 'Faild');

        // Handle error
      }
    });
  }
}