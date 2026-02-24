import api from './api';
import { WorkOrderNote, CreateWorkOrderNoteDto } from '../types';

export const workOrderNoteService = {
  async getNotes(workOrderId: string): Promise<WorkOrderNote[]> {
    const response = await api.get<WorkOrderNote[]>(`/workorders/${workOrderId}/comments`);
    return response.data;
  },

  async addNote(workOrderId: string, noteData: CreateWorkOrderNoteDto): Promise<WorkOrderNote> {
    const response = await api.post<WorkOrderNote>(`/workorders/${workOrderId}/comments`, noteData);
    return response.data;
  },
};
