import api from './api';
import { WorkOrder, CreateWorkOrderDto, UpdateWorkOrderDto, AssignWorkOrderDto } from '../types';

export const workOrderService = {
  async getAll(): Promise<WorkOrder[]> {
    const response = await api.get<WorkOrder[]>('/workorders');
    return response.data;
  },

  async getById(id: string): Promise<WorkOrder> {
    const response = await api.get<WorkOrder>(`/workorders/${id}`);
    return response.data;
  },

  async getByStatus(status: string): Promise<WorkOrder[]> {
    const response = await api.get<WorkOrder[]>(`/workorders/status/${status}`);
    return response.data;
  },

  async getAssignedToMe(): Promise<WorkOrder[]> {
    const response = await api.get<WorkOrder[]>('/workorders/assigned-to-me');
    return response.data;
  },

  async getByGroup(group: string): Promise<WorkOrder[]> {
    const response = await api.get<WorkOrder[]>(`/workorders/group/${group}`);
    return response.data;
  },

  async create(data: CreateWorkOrderDto): Promise<WorkOrder> {
    const response = await api.post<WorkOrder>('/workorders', data);
    return response.data;
  },

  async update(id: string, data: UpdateWorkOrderDto): Promise<WorkOrder> {
    const response = await api.put<WorkOrder>(`/workorders/${id}`, data);
    return response.data;
  },

  async assign(id: string, data: AssignWorkOrderDto): Promise<WorkOrder> {
    const response = await api.put<WorkOrder>(`/workorders/${id}/assign`, data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/workorders/${id}`);
  },
};
