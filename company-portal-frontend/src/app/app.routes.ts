import { Routes } from '@angular/router';
import { RegisterComponent } from './authentication/registeration-flow/register/register.component';
import { LoginComponent } from './authentication/login/login.component';
import { RegistrationFlowComponent } from './authentication/registeration-flow/registeration-flow.component';
import { LandingComponent } from './landing/landing.component';

export const routes: Routes = [

    {
    path: 'auth/register',
    component: RegistrationFlowComponent
  },
  {
    path: '',
    redirectTo: '/landing',
    pathMatch: 'full'
  },
  {
  path: 'auth/login',
    component: LoginComponent
  },
  {path: "landing", component: LandingComponent}
];
