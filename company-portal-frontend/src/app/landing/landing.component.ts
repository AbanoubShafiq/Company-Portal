import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { CompanyService } from '../services/company.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-landing',
  imports: [CommonModule, RouterLink],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent implements OnInit {
  isLoggedIn = false;
  username = '';
  userProfile: any;

  constructor(
    private companyService: CompanyService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.companyService.authState$.subscribe(isAuthenticated => {
      this.isLoggedIn = isAuthenticated;
      if (isAuthenticated) {
        this.userProfile = this.companyService.getCurrentUser();
        this.username = this.getUserDisplayName();
      } else {
        this.userProfile = null;
        this.username = '';
      }
    });

    console.log(this.isLoggedIn)
    console.log(this.userProfile)
    console.log(this.username)

  }

  logout(): void {
    this.companyService.logout();
    this.toastr.info('You have been logged out');
  }

  scrollToSection(sectionId: string): void {
    document.getElementById(sectionId)?.scrollIntoView({ behavior: 'smooth' });
  }

  getUserDisplayName(): string {
    const user = this.companyService.getCurrentUser();
    // Try English name first, then Arabic name, then email, then fallback
    console.log(user);
    return user?.name || user?.email || 'User';
  }
}