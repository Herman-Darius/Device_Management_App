export interface Device {
  id: number;
  name: string;
  manufacturer: string;
  type: string;
  os: string;
  osVersion: string;
  processor: string;
  ramAmount: string;
  description: string;
  assignedUserId?: number;
  assignedUser?: { name: string };
}
