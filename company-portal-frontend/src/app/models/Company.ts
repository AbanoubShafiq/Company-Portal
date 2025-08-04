export interface ResponseDto<T> {
  data: T | null;
  isSuccess: boolean;
  message: string | null;
  statusCode: number;
}

export class CompanyRegisterDto {
  arabicName: string;
  englishName: string;
  email: string;
  newPassword: string;
  confirmPassword: string;
  isVerified?: boolean;
  phoneNumber?: string;
  websiteUrl?: string;
  logo?: File;

  constructor(
    arabicName: string,
    englishName: string,
    email: string,
    newPassword: string,
    confirmPassword: string,
    isVerified: boolean = false,
    phoneNumber?: string,
    websiteUrl?: string,
    logo?: File
  ) {
    this.arabicName = arabicName;
    this.englishName = englishName;
    this.email = email;
    this.newPassword = newPassword;
    this.confirmPassword = confirmPassword;
    this.isVerified = isVerified;
    this.phoneNumber = phoneNumber;
    this.websiteUrl = websiteUrl;
    this.logo = logo;
  }
}


export interface LoginDto {
  email: string;
  password: string;
}

export interface SendOtpDto {
  email: string;
  phoneNumber:string|null;
}

export interface VerifyOtpDto {
  email: string;
  otp: string;
}

export interface UserProfileDto
{
  id: string,
  email: string,
  name: string 
}