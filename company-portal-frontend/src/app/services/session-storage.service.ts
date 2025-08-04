import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionStorageService {
  setItem(key: string, value: any): void {
    sessionStorage.setItem(key, JSON.stringify(value));
  }

  getItem<T>(key: string): T | null {
    const data = sessionStorage.getItem(key);
    return data ? JSON.parse(data) : null;
  }
  
  removeItem(key: string): void {
    sessionStorage.removeItem(key);
  }


}