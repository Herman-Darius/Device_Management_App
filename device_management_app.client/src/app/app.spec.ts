import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { App } from './app';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [App],
      imports: [HttpClientTestingModule, ReactiveFormsModule]
    }).compileComponents();
  });

  it('should toggle isRegisterMode when toggleAuthMode is called', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;

    // Change .toBeFalse() to .toBe(false)
    expect(app.isRegisterMode).toBe(false);

    // Trigger the logic defined in your App component
    app.toggleAuthMode();

    // Change .toBeTrue() to .toBe(true)
    expect(app.isRegisterMode).toBe(true);
  });

  it('should reset the form after logout', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;

    app.currentUser.set({ name: 'User' });
    app.logout();

    expect(app.currentUser()).toBeNull();
  });
});
