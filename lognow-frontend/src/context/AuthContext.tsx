import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User, LoginDto, RegisterDto } from '../types';
import { authService } from '../services/authService';

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (credentials: LoginDto) => Promise<void>;
  register: (data: RegisterDto) => Promise<void>;
  logout: () => void;
  isAuthenticated: boolean;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadUser = () => {
      const savedToken = authService.getToken();
      const savedUser = authService.getCurrentUser();

      if (savedToken && savedUser) {
        setToken(savedToken);
        setUser(savedUser);
      }
      setLoading(false);
    };

    loadUser();
  }, []);

  const login = async (credentials: LoginDto) => {
    try {
      const response = await authService.login(credentials);
      authService.setToken(response.token);
      authService.setCurrentUser(response.user);
      setToken(response.token);
      setUser(response.user);
    } catch (error) {
      throw error;
    }
  };

  const register = async (data: RegisterDto) => {
    try {
      const response = await authService.register(data);
      authService.setToken(response.token);
      authService.setCurrentUser(response.user);
      setToken(response.token);
      setUser(response.user);
    } catch (error) {
      throw error;
    }
  };

  const logout = () => {
    authService.logout();
    setToken(null);
    setUser(null);
  };

  const value: AuthContextType = {
    user,
    token,
    login,
    register,
    logout,
    isAuthenticated: !!token && !!user,
    loading,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
