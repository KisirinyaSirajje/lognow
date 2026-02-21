import api from './api';
import { IncidentComment, CreateCommentDto } from '../types';

export const commentService = {
  async getComments(incidentId: string): Promise<IncidentComment[]> {
    const response = await api.get<IncidentComment[]>(`/incidents/${incidentId}/comments`);
    return response.data;
  },

  async createComment(incidentId: string, data: CreateCommentDto): Promise<IncidentComment> {
    const response = await api.post<IncidentComment>(`/incidents/${incidentId}/comments`, data);
    return response.data;
  },

  async deleteComment(incidentId: string, commentId: string): Promise<void> {
    await api.delete(`/incidents/${incidentId}/comments/${commentId}`);
  },
};
