import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { VerifyOtpComponent } from './verify-otp/verify-otp.component';
import { SetPasswordComponent } from './set-password/set-password.component';

interface Testimonial {
  name: string;
  role: string;
  avatar: string;
  quote: string;
}

interface BusinessImage {
  title: string;
  subtitle: string;
  testimonial?: Testimonial;
  securityTips?: string[];
}

interface Step {
  number: number;
  label: string;
  completed: boolean;
}

@Component({
  selector: 'app-registration-flow',
  imports: [CommonModule, RegisterComponent, VerifyOtpComponent, SetPasswordComponent],
  templateUrl: './registeration-flow.component.html',
  styleUrl: './registeration-flow.component.css'
})
export class RegistrationFlowComponent {
  currentStep = 1;
  registrationData: any = {};
  email = '';

  steps: Step[] = [
    { number: 1, label: 'Company Info', completed: false },
    { number: 2, label: 'Verify Email', completed: false },
    { number: 3, label: 'Set Password', completed: false }
  ];

  businessImages: BusinessImage[] = [
    {
      title: 'Welcome to Our Platform',
      subtitle: 'Join thousands of companies already growing their business with us',
      testimonial: {
        name: 'Ahmed Al-Mansour',
        role: 'CEO, Perfume Inc.',
        avatar: 'assets/testimonial-avatar.jpg',
        quote: 'Registering our company was seamless. The verification process was quick and the platform has helped us reach new customers.'
      }
    },
    {
      title: 'Secure Verification',
      subtitle: 'We\'ve sent a verification code to your email to ensure it\'s really you',
      testimonial: {
        name: 'Sarah Johnson',
        role: 'Marketing Director, TechSolutions',
        avatar: 'assets/testimonial-avatar2.jpg',
        quote: 'The verification process was quick and secure. It gave me confidence that my company data is protected.'
      }
    },
    {
      title: 'Account Security',
      subtitle: 'Protect your company account with a strong password',
      securityTips: [
        'Use at least 8 characters',
        'Include numbers and symbols',
        'Mix uppercase and lowercase letters',
        'Avoid common words or phrases'
      ]
    }
  ];

  getCurrentBusinessImage(): BusinessImage | null {
    return this.businessImages[this.currentStep - 1] || null;
  }

  hasTestimonial(): boolean {
    const currentImage = this.getCurrentBusinessImage();
    return currentImage?.testimonial !== undefined;
  }

  onStepComplete(data: any) {
    this.steps[this.currentStep - 1].completed = true;
    
    if (this.currentStep === 1) {
      this.registrationData = { ...data };
      this.email = data.email;
    } else if (this.currentStep === 2) {
      this.registrationData = { ...this.registrationData, isVerified: true };
    }
    
    if (this.currentStep < 3) {
      this.currentStep++;
    }
  }

  onRegistrationComplete() {
    console.log('Registration completed successfully');
  }

  // goBack() {
  //   if (this.currentStep > 1) {
  //     this.currentStep--;
  //   }
  // }
}