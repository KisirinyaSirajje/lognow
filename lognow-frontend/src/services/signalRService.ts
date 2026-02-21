import * as signalR from '@microsoft/signalr';
import { authService } from './authService';

const SIGNALR_URL = 'http://localhost:5000/notificationHub';

class SignalRService {
  private connection: signalR.HubConnection | null = null;

  async start(): Promise<void> {
    const token = authService.getToken();
    
    if (!token) {
      console.warn('No token available for SignalR connection');
      return;
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(SIGNALR_URL, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    try {
      await this.connection.start();
      console.log('SignalR Connected');
    } catch (error) {
      console.error('SignalR Connection Error:', error);
    }
  }

  async stop(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      console.log('SignalR Disconnected');
    }
  }

  on(eventName: string, callback: (...args: any[]) => void): void {
    if (this.connection) {
      this.connection.on(eventName, callback);
    }
  }

  off(eventName: string, callback: (...args: any[]) => void): void {
    if (this.connection) {
      this.connection.off(eventName, callback);
    }
  }

  async invoke(methodName: string, ...args: any[]): Promise<any> {
    if (this.connection) {
      return await this.connection.invoke(methodName, ...args);
    }
  }
}

export const signalRService = new SignalRService();
