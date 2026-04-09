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
  // 1. Data Signals
  public readonly title = signal('Device Management System');
  public devices = signal<Device[]>([]);
  public users = signal<any[]>([]); // Requirement: User list

  // 2. State Variables
  public deviceForm!: FormGroup;
  public selectedDevice: Device | null = null;
  public isEditing = false;
  public currentEditId: number | null = null;

  constructor(private http: HttpClient, private fb: FormBuilder) {
    this.initForm();
  }

  ngOnInit() {
    this.getDevices();
    this.getUsers(); // Load users for the dropdown
  }

  private initForm() {
    this.deviceForm = this.fb.group({
      name: ['', Validators.required],
      manufacturer: ['', Validators.required],
      type: ['Laptop', Validators.required],
      os: ['', Validators.required],
      osVersion: ['', Validators.required],
      processor: ['', Validators.required], //
      ramAmount: ['', Validators.required],  //
      description: [''],
      assignedUserId: [null]
    });
  }

  // --- API LOGIC ---

  getDevices() {
    this.http.get<Device[]>('https://localhost:7249/api/devices').subscribe({
      next: (result) => this.devices.set(result),
      error: (err) => console.error('API Error:', err)
    });
  }

  getUsers() {
    this.http.get<any[]>('https://localhost:7249/api/users').subscribe({
      next: (data) => this.users.set(data),
      error: (err) => console.error('Failed to load users:', err)
    });
  }

  deleteDevice(id: number) {
    if (confirm('Are you sure you want to delete this device?')) {
      this.http.delete(`https://localhost:7249/api/devices/${id}`).subscribe(() => {
        this.getDevices();
        alert('Device deleted successfully');
      });
    }
  }

  // --- UI ACTIONS ---

  selectDevice(device: Device) {
    // Toggle View/Hide logic
    if (this.selectedDevice?.id === device.id) {
      this.selectedDevice = null;
    } else {
      this.selectedDevice = device;
    }
  }

  editDevice(device: Device) {
    this.isEditing = true;
    this.currentEditId = device.id;

    this.deviceForm.patchValue({
      name: device.name,
      manufacturer: device.manufacturer,
      type: device.type,
      os: device.os,
      osVersion: device.osVersion,
      processor: device.processor,
      ramAmount: device.ramAmount,
      description: device.description,
      assignedUserId: device.assignedUserId
    });

    window.scrollTo({ top: document.body.scrollHeight, behavior: 'smooth' });
  }

  cancelEdit() {
    this.isEditing = false;
    this.currentEditId = null;
    this.deviceForm.reset({ type: 'Laptop' });
  }

  onSubmit() {
    if (!this.deviceForm.valid) return;

    const deviceData = this.deviceForm.value;

    if (this.isEditing && this.currentEditId) {
      // Logic for UPDATING (PUT)
      const updatedDevice = { ...deviceData, id: this.currentEditId };
      this.http.put(`https://localhost:7249/api/devices/${this.currentEditId}`, updatedDevice).subscribe({
        next: () => {
          this.getDevices();
          this.cancelEdit();
          alert('Device updated successfully!');
        },
        error: (err) => alert('Update failed: ' + (err.error || 'Check server console'))
      });
    } else {
      // Logic for CREATING (POST)
      this.http.post('https://localhost:7249/api/devices', deviceData).subscribe({
        next: () => {
          this.getDevices();
          this.deviceForm.reset({ type: 'Laptop' });
          alert('Device added successfully!');
        },
        error: (err) => {
          if (err.status === 400) {
            alert(err.error || 'A device with this name already exists.');
          }
        }
      });
    }
  }
}
