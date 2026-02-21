import api from './api';
import { Dashboard } from '../types';

export const dashboardService = {
  async getDashboard(): Promise<Dashboard> {
    const response = await api.get<Dashboard>('/dashboard');
    return response.data;
  },
};
