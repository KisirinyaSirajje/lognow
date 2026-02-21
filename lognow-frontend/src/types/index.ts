export interface User {
  id: string;
  username: string;
  email: string;
  fullName: string;
  role: UserRole;
  team?: string;
  isActive: boolean;
  createdAt: string;
}

export enum UserRole {
  Admin = 'Admin',
  Engineer = 'Engineer',
  TeamLead = 'TeamLead',
  Viewer = 'Viewer',
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  username: string;
  email: string;
  password: string;
  fullName: string;
  role: string;
  team?: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface Service {
  id: string;
  name: string;
  description: string;
  ownerTeam: string;
  status: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateServiceDto {
  name: string;
  description: string;
  ownerTeam: string;
  status?: string;
}

export interface Incident {
  id: string;
  incidentNumber: string;
  title: string;
  description: string;
  serviceId: string;
  serviceName: string;
  severity: Severity;
  status: IncidentStatus;
  assignedToUserId?: string;
  assignedToUserName?: string;
  createdByUserId: string;
  createdByUserName: string;
  createdAt: string;
  updatedAt?: string;
  resolvedAt?: string;
  responseDueAt?: string;
  resolutionDueAt?: string;
  isResponseBreached: boolean;
  isResolutionBreached: boolean;
}

export enum Severity {
  SEV1 = 'SEV1',
  SEV2 = 'SEV2',
  SEV3 = 'SEV3',
  SEV4 = 'SEV4',
}

export enum IncidentStatus {
  Open = 'Open',
  Assigned = 'Assigned',
  InProgress = 'InProgress',
  Resolved = 'Resolved',
  Closed = 'Closed',
}

export interface CreateIncidentDto {
  title: string;
  description: string;
  serviceId: string;
  severity: string;
}

export interface UpdateIncidentDto {
  title?: string;
  description?: string;
  severity?: string;
  status?: string;
  assignedToUserId?: string;
}

export interface IncidentComment {
  id: string;
  incidentId: string;
  userId: string;
  username: string;
  userFullName: string;
  commentText: string;
  createdAt: string;
}

export interface CreateCommentDto {
  commentText: string;
}

export interface IncidentTimeline {
  id: string;
  incidentId: string;
  actionType: string;
  description: string;
  userId?: string;
  username?: string;
  createdAt: string;
}

export interface Dashboard {
  totalIncidents: number;
  openIncidents: number;
  inProgressIncidents: number;
  resolvedIncidents: number;
  incidentsBySeverity: Record<string, number>;
  incidentsByService: Record<string, number>;
  incidentsByStatus: Record<string, number>;
  recentIncidents: Incident[];
  slaBreaches: number;
}
