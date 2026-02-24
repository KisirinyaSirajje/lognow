import api from './api';

export interface WorkOrderComment {
  id: string;
  workOrderId: string;
  userId: string;
  userName: string;
  commentText: string;
  createdAt: string;
}

export interface CreateWorkOrderCommentDto {
  commentText: string;
}

export const workOrderCommentService = {
  async getComments(workOrderId: string): Promise<WorkOrderComment[]> {
    const response = await api.get<WorkOrderComment[]>(`/workorders/${workOrderId}/comments`);
    return response.data;
  },

  async addComment(workOrderId: string, data: CreateWorkOrderCommentDto): Promise<WorkOrderComment> {
    const response = await api.post<WorkOrderComment>(`/workorders/${workOrderId}/comments`, data);
    return response.data;
  },
};
