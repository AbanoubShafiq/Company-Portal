
import { Component, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CompanyService } from '../../../services/company.service';
import { SessionStorageService } from '../../../services/session-storage.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { SendOtpDto } from '../../../models/Company';

@Component({
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  @Output() stepComplete = new EventEmitter<any>();
  
  registerForm: FormGroup;
  isLoading = false;
  logoPreview: string | ArrayBuffer | null = null;

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private sessionStorage: SessionStorageService,
    private toastr: ToastrService
  ) {
    this.registerForm = this.fb.group({
      arabicName: ['', Validators.required],
      englishName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.pattern(/^\+?[0-9\s\-()]{7,}$/)],
      websiteUrl: ['', [Validators.pattern(/^(https?:\/\/)?([\w-]+(\.[\w-]+)+)(\/[\w-]*)*\/?$/)]],
      logo: [null]
    });


  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const formData = this.registerForm.value;
    
    // Convert logo File to Base64 if it exists
    const logoFile = this.registerForm.get('logo')?.value;
    let logoBase64 = null;

    if (logoFile) {
      const reader = new FileReader();
      reader.onload = () => {
        logoBase64 = reader.result as string;
        
        // Save data to session storage with Base64 logo
        this.saveToSessionStorage(formData);
        
        // Call sendOtp service
        this.sendOtpRequest(formData.email);
      };
      reader.readAsDataURL(logoFile);
    } else {
      // Save without logo
      this.saveToSessionStorage(formData);
      this.sendOtpRequest(formData.email);
    }
  }

  private saveToSessionStorage(formData: any): void {
    // Create a copy without the logo property
    const { logo, ...dataWithoutLogo } = formData;
    
    this.sessionStorage.setItem('registrationData', {
      ...dataWithoutLogo,
      isVerified: false
    });
  }


  private sendOtpRequest(email: string): void {
    const otpRequest: SendOtpDto = {
      email: email,
      phoneNumber: this.registerForm.get('phoneNumber')?.value || null
    };
    console.log(otpRequest)
    this.companyService.sendOtp(otpRequest).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.toastr.success(response.message!, 'Success', {
          timeOut: 3000,
          progressBar: true
        });
        this.stepComplete.emit(this.registerForm.value);
        console.log(otpRequest)

      },

      error: (error) => {
        this.isLoading = false;
        this.toastr.error(error.error.Message, 'Registration Error');
        console.log(error)
      }
    });
  }


  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      
      this.companyService.storeLogoFile(file);
      
      const reader = new FileReader();
      reader.onload = () => {
        this.logoPreview = reader.result;
        this.toastr.success('Logo uploaded successfully!', '', { timeOut: 2000 });
      };
      reader.readAsDataURL(file);
    }
  }

  removeLogo(event: MouseEvent): void {
    event.stopPropagation();
    event.preventDefault();
    
    this.logoPreview = null;
    this.companyService.clearStoredLogoFile();
    const fileInput = document.getElementById('logo') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
    this.toastr.info('Logo removed', '', { timeOut: 2000 });
  }

}