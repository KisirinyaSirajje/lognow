import { useState, useEffect } from 'react';
import { adminService }from '../services/adminService';
import { User, AdminStats } from '../types';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const AdminDashboardPage = () => {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [users, setUsers] = useState<User[]>([]);
  const [stats, setStats] = useState<AdminStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState<string | null>(null);

  useEffect(() => {
    // Check if user is admin
    if (user?.role !== 'Admin') {
      navigate('/dashboard');
      return;
    }

    loadData();
  }, [user, navigate]);

  const loadData = async () => {
    try {
      const [usersData, statsData] = await Promise.all([
        adminService.getAllUsers(),
        adminService.getAdminStats(),
      ]);
      setUsers(usersData);
      setStats(statsData);
    } catch (error) {
      console.error('Error loading admin data:', error);
      alert('Failed to load admin data');
    } finally {
      setLoading(false);
    }
  };

  const handleToggleUserStatus = async (userId: string, currentStatus: boolean) => {
    setUpdating(userId);
    try {
      await adminService.updateUserStatus(userId, !currentStatus);
      await loadData();
      alert('User status updated successfully');
    } catch (error) {
      console.error('Error updating user status:', error);
      alert('Failed to update user status');
    } finally {
      setUpdating(null);
    }
  };

  const handleChangeUserRole = async (userId: string) => {
    const role = prompt('Enter new role (Admin, TeamLead, Engineer, Viewer):');
    if (!role) return;

    setUpdating(userId);
    try {
      await adminService.updateUserRole(userId, role);
      await loadData();
      alert('User role updated successfully');
    } catch (error) {
      console.error('Error updating user role:', error);
      alert('Failed to update user role');
    } finally {
      setUpdating(null);
    }
  };

  if (loading) {
    return <div className="text-center py-8">Loading...</div>;
  }

  if (!stats) {
    return <div className="text-center py-8">No data available</div>;
  }

  return (
    <div className="max-w-7xl mx-auto">
      <h1 className="text-3xl font-bold text-gray-900 mb-8">Admin Dashboard</h1>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div className="bg-white shadow rounded-lg p-6">
          <div className="text-sm font-medium text-gray-500">Total Users</div>
          <div className="mt-2 text-3xl font-semibold text-gray-900">{stats.totalUsers}</div>
          <div className="mt-1 text-sm text-gray-600">{stats.activeUsers} active</div>
        </div>

        <div className="bg-white shadow rounded-lg p-6">
          <div className="text-sm font-medium text-gray-500">Total Incidents</div>
          <div className="mt-2 text-3xl font-semibold text-gray-900">{stats.totalIncidents}</div>
        </div>

        <div className="bg-white shadow rounded-lg p-6">
          <div className="text-sm font-medium text-gray-500">Total Services</div>
          <div className="mt-2 text-3xl font-semibold text-gray-900">{stats.totalServices}</div>
        </div>

        <div className="bg-white shadow rounded-lg p-6">
          <div className="text-sm font-medium text-gray-500">Users by Role</div>
          <div className="mt-2 space-y-1">
            {Object.entries(stats.usersByRole).map(([role, count]) => (
              <div key={role} className="text-sm">
                <span className="font-medium">{role}:</span> {count}
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Users Table */}
      <div className="bg-white shadow rounded-lg overflow-hidden">
        <div className="px-6 py-4 border-b border-gray-200">
          <h2 className="text-lg font-medium text-gray-900">User Management</h2>
        </div>

        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">
                  User
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">
                  Email
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">
                  Role
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">
                  Team
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">
                  Status
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">
                  Actions
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {users.map((u) => (
                <tr key={u.id}>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <div className="text-sm font-medium text-gray-900">{u.fullName}</div>
                    <div className="text-sm text-gray-500">@{u.username}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {u.email}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span
                      className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                        u.role === 'Admin'
                          ? 'bg-purple-100 text-purple-800'
                          : u.role === 'TeamLead'
                          ? 'bg-blue-100 text-blue-800'
                          : u.role === 'Engineer'
                          ? 'bg-green-100 text-green-800'
                          : 'bg-gray-100 text-gray-800'
                      }`}
                    >
                      {u.role}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    {u.team || '-'}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span
                      className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                        u.isActive
                          ? 'bg-green-100 text-green-800'
                          : 'bg-red-100 text-red-800'
                      }`}
                    >
                      {u.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm space-x-2">
                    <button
                      onClick={() => handleToggleUserStatus(u.id, u.isActive)}
                      disabled={updating === u.id || u.id === user?.id}
                      className="text-blue-600 hover:text-blue-900 disabled:opacity-50"
                    >
                      {u.isActive ? 'Deactivate' : 'Activate'}
                    </button>
                    <button
                      onClick={() => handleChangeUserRole(u.id)}
                      disabled={updating === u.id || u.id === user?.id}
                      className="text-green-600 hover:text-green-900 disabled:opacity-50"
                    >
                      Change Role
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default AdminDashboardPage;
