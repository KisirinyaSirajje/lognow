import api from './api';
import { User, AdminStats } from '../types';

export const adminService = {
  async getAllUsers(): Promise<User[]> {
    const response = await api.get<User[]>('/admin/users');
    return response.data;
  },

  async updateUserStatus(id: string, isActive: boolean): Promise<void> {
    await api.put(`/admin/users/${id}/status`, { isActive });
  },

  async updateUserRole(id: string, role: string): Promise<void> {
    await api.put(`/admin/users/${id}/role`, { role });
  },

  async getAdminStats(): Promise<AdminStats> {
    const response = await api.get<AdminStats>('/admin/stats');
    return response.data;
  },
};
