import { HttpClient } from '@angular/common/http';
import { Component, OnInit, signal } from '@angular/core';
import { Device } from './device.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App implements OnInit {
  public readonly title = signal('Device Management System');
  public devices = signal<Device[]>([]);
  public users = signal<any[]>([]);
  public currentUser = signal<any | null>(null);

  public deviceForm!: FormGroup;
  public authForm!: FormGroup;
  public selectedDevice: Device | null = null;
  public isEditing = false;
  public currentEditId: number | null = null;
  public showAuthForm = true;
  public isRegisterMode = false;
  public showUserManagement = false;
  public searchQuery = '';

  constructor(private http: HttpClient, private fb: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      this.currentUser.set(JSON.parse(savedUser));
      this.showAuthForm = false;
    } else {
      this.showAuthForm = true;
    }
    this.getDevices();
    this.getUsers();
  }

  private initForm() {
    this.deviceForm = this.fb.group({
      name: ['', Validators.required],
      manufacturer: ['', Validators.required],
      type: ['Laptop', Validators.required],
      os: ['', Validators.required],
      osVersion: ['', Validators.required],
      processor: ['', Validators.required],
      ramAmount: ['', Validators.required],
      description: [''],
      assignedUserId: [null]
    });

    this.authForm = this.fb.group({
      name: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmPassword: [''],
      role: ['Employee'],
      location: ['']
    });
  }

  toggleAuthMode() {
    this.isRegisterMode = !this.isRegisterMode;
  }

  onAuthSubmit() {
    if (!this.authForm.valid) return;

    const val = this.authForm.value;

    if (this.isRegisterMode) {
      if (val.password !== val.confirmPassword) {
        alert('Validation Error: Passwords do not match!');
        return;
      }
      if (val.role.toLowerCase().trim() === 'admin') {
        alert('Security Error: You cannot register as an Admin.');
        return;
      }
    }

    const { confirmPassword, ...dataToSend } = val;

    const url = this.isRegisterMode ? 'register' : 'login';
    this.http.post(`https://localhost:7249/api/account/${url}`, dataToSend).subscribe({
      next: (response: any) => {
        this.currentUser.set(response.user);
        localStorage.setItem('token', response.token);
        localStorage.setItem('user', JSON.stringify(response.user));
        this.showAuthForm = false;
        this.authForm.reset();
        this.getDevices();
      },
      error: (err) => alert(err.error || 'Login failed')
    });
  }

  logout() {
    this.currentUser.set(null);
    localStorage.removeItem('user');
    localStorage.removeItem('token');

    this.cancelEdit();
    this.selectedDevice = null;
    this.showUserManagement = false;
    this.showAuthForm = true;

    alert('Logged out successfully');
  }

  getDevices() {
    this.http.get<Device[]>('https://localhost:7249/api/devices').subscribe({
      next: (result) => this.devices.set(result),
      error: (err) => console.error('API Error:', err)
    });
  }

  onSubmit() {
    if (!this.deviceForm.valid) return;
    const deviceData = this.deviceForm.value;

    if (this.isEditing && this.currentEditId) {
      const updatedDevice = { ...deviceData, id: this.currentEditId };
      this.http.put(`https://localhost:7249/api/devices/${this.currentEditId}`, updatedDevice).subscribe({
        next: () => {
          this.getDevices();
          this.cancelEdit();
          alert('Device updated!');
        },
        error: (err) => alert('Update failed')
      });
    } else {
      this.http.post('https://localhost:7249/api/devices', deviceData).subscribe({
        next: () => {
          this.getDevices();
          this.deviceForm.reset({ type: 'Laptop' });
          alert('Device added!');
        }
      });
    }
  }

  onSearch() {
    const query = this.searchQuery.trim();

    if (!query) {
      this.getDevices();
      return;
    }
    this.http.get<Device[]>(`https://localhost:7249/api/devices/search?q=${query}`).subscribe({
      next: (results) => {
        this.devices.set(results);
      },
      error: (err) => {
        console.error('Search failed:', err);
        alert('Search encountered an error.');
      }
    });
  }

  clearSearch() {
    this.searchQuery = '';
    this.getDevices();
  }

  deleteDevice(id: number) {
    if (confirm('Permanently delete this device?')) {
      this.http.delete(`https://localhost:7249/api/devices/${id}`).subscribe({
        next: () => this.getDevices(),
        error: (err) => alert('Delete failed')
      });
    }
  }

  assignToMe(deviceId: number) {
    const userId = this.currentUser()?.id;
    if (!userId) return;
    this.http.patch(`https://localhost:7249/api/devices/${deviceId}/assign/${userId}`, {}).subscribe({
      next: () => this.getDevices()
    });
  }

  unassign(deviceId: number) {
    this.http.patch(`https://localhost:7249/api/devices/${deviceId}/unassign`, {}).subscribe({
      next: () => this.getDevices()
    });
  }

  getUsers() {
    this.http.get<any[]>('https://localhost:7249/api/users').subscribe({
      next: (data) => this.users.set(data)
    });
  }

  changeRole(userId: number, currentRole: string) {
    const newRole = currentRole === 'Admin' ? 'Employee' : 'Admin';
    if (confirm(`Change this user to ${newRole}?`)) {
      this.http.patch(`https://localhost:7249/api/users/${userId}/role`, `"${newRole}"`, {
        headers: { 'Content-Type': 'application/json' }
      }).subscribe({
        next: () => {
          this.getUsers();
          alert('User role updated successfully.');
        }
      });
    }
  }

  isAdminOrOwner(device: Device): boolean {
    const user = this.currentUser();
    return user ? (user.role === 'Admin' || device.assignedUserId === user.id) : false;
  }

  toggleUserManagement() {
    this.showUserManagement = !this.showUserManagement;
  }

  selectDevice(device: Device) {
    this.selectedDevice = (this.selectedDevice?.id === device.id) ? null : device;
  }

  editDevice(device: Device) {
    this.isEditing = true;
    this.currentEditId = device.id;
    this.deviceForm.patchValue(device);
    window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
  }

  cancelEdit() {
    this.isEditing = false;
    this.currentEditId = null;
    this.deviceForm.reset({ type: 'Laptop' });
  }

  // AI generation
  generateAIDescription() {
    const deviceData = this.deviceForm.value;

    this.deviceForm.patchValue({ description: 'AI is thinking...' });

    this.http.post('https://localhost:7249/api/devices/generate-description', deviceData).subscribe({
      next: (res: any) => {
        this.deviceForm.patchValue({ description: res.description });
      },
      error: () => {
        alert('AI Generation failed. Check your API key.');
        this.deviceForm.patchValue({ description: '' });
      }
    });
  }
}
