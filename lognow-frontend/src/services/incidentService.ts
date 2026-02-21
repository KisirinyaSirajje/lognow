import api from './api';
import { Incident, CreateIncidentDto, UpdateIncidentDto } from '../types';

export const incidentService = {
  async getAll(): Promise<Incident[]> {
    const response = await api.get<Incident[]>('/incidents');
    return response.data;
  },

  async getById(id: string): Promise<Incident> {
    const response = await api.get<Incident>(`/incidents/${id}`);
    return response.data;
  },

  async create(data: CreateIncidentDto): Promise<Incident> {
    const response = await api.post<Incident>('/incidents', data);
    return response.data;
  },

  async update(id: string, data: UpdateIncidentDto): Promise<Incident> {
    const response = await api.put<Incident>(`/incidents/${id}`, data);
    return response.data;
  },

  async assign(id: string, userId: string): Promise<Incident> {
    const response = await api.put<Incident>(`/incidents/${id}/assign`, { userId });
    return response.data;
  },

  async updateStatus(id: string, status: string): Promise<Incident> {
    const response = await api.put<Incident>(`/incidents/${id}/status`, { status });
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/incidents/${id}`);
  },
};
