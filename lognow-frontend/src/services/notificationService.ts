import api from './api';
import { Notification } from '../types';

export const notificationService = {
  async getAll(): Promise<Notification[]> {
    const response = await api.get<Notification[]>('/notifications');
    return response.data;
  },

  async getUnread(): Promise<Notification[]> {
    const response = await api.get<Notification[]>('/notifications/unread');
    return response.data;
  },

  async getUnreadCount(): Promise<number> {
    const response = await api.get<number>('/notifications/unread/count');
    return response.data;
  },

  async markAsRead(id: string): Promise<void> {
    await api.put(`/notifications/${id}/read`);
  },

  async markAllAsRead(): Promise<void> {
    await api.put('/notifications/read-all');
  },
};
