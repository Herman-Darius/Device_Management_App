import { signal } from '@angular/core';

export class AuthService {
  public currentUser = signal<any | null>(JSON.parse(localStorage.getItem('user') || 'null'));

  login(user: any) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
