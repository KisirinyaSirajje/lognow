import React, { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import { signalRService } from '../services/signalRService';
import { useAuth } from './AuthContext';
import { Incident, IncidentComment } from '../types';

interface NotificationContextType {
  notifications: string[];
  addNotification: (message: string) => void;
  clearNotifications: () => void;
}

const NotificationContext = createContext<NotificationContextType | undefined>(undefined);

export const NotificationProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const { isAuthenticated } = useAuth();
  const [notifications, setNotifications] = useState<string[]>([]);

  useEffect(() => {
    if (isAuthenticated) {
      signalRService.start();

      // Listen for incident events
      signalRService.on('IncidentCreated', (incident: Incident) => {
        addNotification(`New incident created: ${incident.incidentNumber}`);
      });

      signalRService.on('IncidentUpdated', (incident: Incident) => {
        addNotification(`Incident ${incident.incidentNumber} updated`);
      });

      signalRService.on('CommentAdded', (comment: IncidentComment) => {
        addNotification(`New comment from ${comment.userFullName}`);
      });

      signalRService.on('ReceiveNotification', (message: string) => {
        addNotification(message);
      });

      return () => {
        signalRService.stop();
      };
    }
  }, [isAuthenticated]);

  const addNotification = (message: string) => {
    setNotifications((prev) => [...prev, message]);
    
    // Auto-remove notification after 5 seconds
    setTimeout(() => {
      setNotifications((prev) => prev.filter((n) => n !== message));
    }, 5000);
  };

  const clearNotifications = () => {
    setNotifications([]);
  };

  const value: NotificationContextType = {
    notifications,
    addNotification,
    clearNotifications,
  };

  return <NotificationContext.Provider value={value}>{children}</NotificationContext.Provider>;
};

export const useNotifications = (): NotificationContextType => {
  const context = useContext(NotificationContext);
  if (!context) {
    throw new Error('useNotifications must be used within a NotificationProvider');
  }
  return context;
};
