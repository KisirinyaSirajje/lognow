import api from './api';
import { Service, CreateServiceDto } from '../types';

export const serviceService = {
  async getAll(): Promise<Service[]> {
    const response = await api.get<Service[]>('/services');
    return response.data;
  },

  async getById(id: string): Promise<Service> {
    const response = await api.get<Service>(`/services/${id}`);
    return response.data;
  },

  async create(data: CreateServiceDto): Promise<Service> {
    const response = await api.post<Service>('/services', data);
    return response.data;
  },

  async update(id: string, data: Partial<CreateServiceDto>): Promise<Service> {
    const response = await api.put<Service>(`/services/${id}`, data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/services/${id}`);
  },
};
