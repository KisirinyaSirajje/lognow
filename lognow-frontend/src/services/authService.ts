import api from './api';
import { LoginDto, RegisterDto, AuthResponse } from '../types';

export const authService = {
  async login(credentials: LoginDto): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/auth/login', credentials);
    return response.data;
  },

  async register(data: RegisterDto): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/auth/register', data);
    return response.data;
  },

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  getToken(): string | null {
    return localStorage.getItem('token');
  },

  setToken(token: string): void {
    localStorage.setItem('token', token);
  },

  getCurrentUser(): any {
    const userStr = localStorage.getItem('user');
    return userStr ? JSON.parse(userStr) : null;
  },

  setCurrentUser(user: any): void {
    localStorage.setItem('user', JSON.stringify(user));
  },
};
