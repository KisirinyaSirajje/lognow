import api from './api';
import { User } from '../types';

export const userService = {
  async getAll(): Promise<User[]> {
    const response = await api.get<User[]>('/users');
    return response.data;
  },

  async getById(id: string): Promise<User> {
    const response = await api.get<User>(`/users/${id}`);
    return response.data;
  },

  async getByTeam(team: string): Promise<User[]> {
    const response = await api.get<User[]>(`/users/team/${team}`);
    return response.data;
  },

  async getAllTeams(): Promise<string[]> {
    const response = await api.get<string[]>('/users/teams');
    return response.data;
  },
};
