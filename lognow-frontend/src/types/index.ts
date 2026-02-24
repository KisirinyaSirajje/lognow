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
  assignedGroup?: string;
  assignedToUserId?: string;
  assignedToUserName?: string;
  assignedByUserId?: string;
  assignedByUserName?: string;
  createdByUserId: string;
  createdByUserName: string;
  createdAt: string;
  updatedAt?: string;
  resolvedAt?: string;
  resolutionNote?: string;
  onHoldReason?: string;
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
  Pending = 'Pending',
  Assigned = 'Assigned',
  InProgress = 'InProgress',
  OnHold = 'OnHold',
  Resolved = 'Resolved',
  Cancelled = 'Cancelled',
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
  assignedGroup?: string;
  assignedToUserId?: string;
}

export interface AssignIncidentDto {
  assignedGroup?: string;
  userId?: string;
}

export interface UpdateIncidentStatusDto {
  status: string;
  resolutionNote?: string;
  onHoldReason?: string;
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

export interface Notification {
  id: string;
  userId: string;
  title: string;
  message: string;
  type: string;
  relatedEntityId?: string;
  isRead: boolean;
  createdAt: string;
}

export interface AdminStats {
  totalUsers: number;
  activeUsers: number;
  totalIncidents: number;
  totalServices: number;
  usersByRole: Record<string, number>;
  incidentsByStatus: Record<string, number>;
}

export interface WorkOrder {
  id: string;
  workOrderNumber: string;
  title: string;
  description: string;
  type: WorkOrderType;
  priority: WorkOrderPriority;
  status: WorkOrderStatus;
  serviceId?: string;
  serviceName?: string;
  incidentId?: string;
  incidentNumber?: string;
  assignedToUserId?: string;
  assignedToUserName?: string;
  assignedGroup?: string;
  createdByUserId: string;
  createdByUserName: string;
  scheduledStartDate?: string;
  scheduledEndDate?: string;
  actualStartDate?: string;
  actualEndDate?: string;
  estimatedCost?: number;
  actualCost?: number;
  estimatedHours?: number;
  actualHours?: number;
  location?: string;
  partsRequired?: string;
  completionNotes?: string;
  createdAt: string;
  updatedAt?: string;
  completedAt?: string;
}

export enum WorkOrderType {
  Corrective = 'Corrective',
  Preventive = 'Preventive',
  Inspection = 'Inspection',
  Installation = 'Installation',
  Upgrade = 'Upgrade',
  Emergency = 'Emergency',
}

export enum WorkOrderPriority {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
  Critical = 'Critical',
}

export enum WorkOrderStatus {
  Draft = 'Draft',
  Scheduled = 'Scheduled',
  Assigned = 'Assigned',
  InProgress = 'InProgress',
  OnHold = 'OnHold',
  Completed = 'Completed',
  Verified = 'Verified',
  Cancelled = 'Cancelled',
}

export interface CreateWorkOrderDto {
  title: string;
  description: string;
  type: string;
  priority: string;
  serviceId?: string;
  incidentId?: string;
  location?: string;
  scheduledStartDate?: string;
  scheduledEndDate?: string;
  estimatedCost?: number;
  estimatedHours?: number;
  partsRequired?: string;
}

export interface UpdateWorkOrderDto {
  title?: string;
  description?: string;
  priority?: string;
  status?: string;
  assignedToUserId?: string;
  assignedGroup?: string;
  scheduledStartDate?: string;
  scheduledEndDate?: string;
  actualStartDate?: string;
  actualEndDate?: string;
  actualCost?: number;
  actualHours?: number;
  location?: string;
  partsRequired?: string;
  completionNotes?: string;
}

export interface AssignWorkOrderDto {
  assignedGroup?: string;
  userId?: string;
}

export interface WorkOrderNote {
  id: string;
  workOrderId: string;
  userId: string;
  userName: string;
  commentText: string;
  createdAt: string;
}

export interface CreateWorkOrderNoteDto {
  commentText: string;
}
