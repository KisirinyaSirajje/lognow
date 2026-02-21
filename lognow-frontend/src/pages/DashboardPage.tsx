import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { dashboardService } from '../services/dashboardService';
import { Dashboard } from '../types';

const DashboardPage = () => {
  const [dashboard, setDashboard] = useState<Dashboard | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      const data = await dashboardService.getDashboard();
      setDashboard(data);
    } catch (error) {
      console.error('Failed to load dashboard:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="text-center py-10">Loading...</div>;
  }

  if (!dashboard) {
    return <div className="text-center py-10">Failed to load dashboard</div>;
  }

  return (
    <div className="px-4 py-6">
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="mt-1 text-sm text-gray-500">Welcome to Log Now Incident Management</p>
      </div>

      {/* Statistics Cards */}
      <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-4 mb-8">
        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <dt className="text-sm font-medium text-gray-500 truncate">Total Incidents</dt>
            <dd className="mt-1 text-3xl font-semibold text-gray-900">{dashboard.totalIncidents}</dd>
          </div>
        </div>

        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <dt className="text-sm font-medium text-gray-500 truncate">Pending Incidents</dt>
            <dd className="mt-1 text-3xl font-semibold text-red-600">{dashboard.openIncidents}</dd>
          </div>
        </div>

        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <dt className="text-sm font-medium text-gray-500 truncate">In Progress</dt>
            <dd className="mt-1 text-3xl font-semibold text-yellow-600">{dashboard.inProgressIncidents}</dd>
          </div>
        </div>

        <div className="bg-white overflow-hidden shadow rounded-lg">
          <div className="px-4 py-5 sm:p-6">
            <dt className="text-sm font-medium text-gray-500 truncate">Resolved</dt>
            <dd className="mt-1 text-3xl font-semibold text-green-600">{dashboard.resolvedIncidents}</dd>
          </div>
        </div>
      </div>

      {/* Charts Section */}
      <div className="grid grid-cols-1 gap-5 lg:grid-cols-2 mb-8">
        {/* Incidents by Severity */}
        <div className="bg-white shadow rounded-lg p-6">
          <h3 className="text-lg font-medium text-gray-900 mb-4">Incidents by Severity</h3>
          <div className="space-y-3">
            {Object.entries(dashboard.incidentsBySeverity).map(([severity, count]) => (
              <div key={severity} className="flex justify-between items-center">
                <span className={`px-3 py-1 rounded-full text-sm font-semibold ${
                  severity === 'SEV1' ? 'bg-red-100 text-red-800' :
                  severity === 'SEV2' ? 'bg-orange-100 text-orange-800' :
                  severity === 'SEV3' ? 'bg-yellow-100 text-yellow-800' :
                  'bg-green-100 text-green-800'
                }`}>
                  {severity}
                </span>
                <span className="text-2xl font-bold text-gray-900">{count}</span>
              </div>
            ))}
          </div>
        </div>

        {/* Incidents by Status */}
        <div className="bg-white shadow rounded-lg p-6">
          <h3 className="text-lg font-medium text-gray-900 mb-4">Incidents by Status</h3>
          <div className="space-y-3">
            {Object.entries(dashboard.incidentsByStatus).map(([status, count]) => (
              <div key={status} className="flex justify-between items-center">
                <span className="text-gray-700">{status}</span>
                <span className="text-2xl font-bold text-gray-900">{count}</span>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* SLA Breaches Warning */}
      {dashboard.slaBreaches > 0 && (
        <div className="bg-red-50 border-l-4 border-red-400 p-4 mb-8">
          <div className="flex">
            <div className="flex-shrink-0">
              <svg className="h-5 w-5 text-red-400" viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
              </svg>
            </div>
            <div className="ml-3">
              <p className="text-sm text-red-700">
                <span className="font-medium">Warning:</span> {dashboard.slaBreaches} incident(s) have breached SLA!
              </p>
            </div>
          </div>
        </div>
      )}

      {/* Recent Incidents */}
      <div className="bg-white shadow rounded-lg overflow-hidden">
        <div className="px-6 py-4 border-b border-gray-200 flex justify-between items-center">
          <h3 className="text-lg font-medium text-gray-900">Recent Incidents</h3>
          <Link to="/incidents" className="text-blue-600 hover:text-blue-800 text-sm font-medium">
            View All
          </Link>
        </div>
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Incident #
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Title
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Severity
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Status
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Service
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {dashboard.recentIncidents.map((incident) => (
                <tr key={incident.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap">
                    <Link to={`/incidents/${incident.id}`} className="text-blue-600 hover:text-blue-800">
                      {incident.incidentNumber}
                    </Link>
                  </td>
                  <td className="px-6 py-4">
                    <div className="text-sm text-gray-900">{incident.title}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      incident.severity === 'SEV1' ? 'bg-red-100 text-red-800' :
                      incident.severity === 'SEV2' ? 'bg-orange-100 text-orange-800' :
                      incident.severity === 'SEV3' ? 'bg-yellow-100 text-yellow-800' :
                      'bg-green-100 text-green-800'
                    }`}>
                      {incident.severity}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap">
                    <span className={`px-2 inline-flex text-xs leading-5 font-semibold rounded-full ${
                      incident.status === 'Pending' ? 'bg-gray-100 text-gray-800' :
                      incident.status === 'Assigned' || incident.status === 'InProgress' ? 'bg-yellow-100 text-yellow-800' :
                      incident.status === 'OnHold' ? 'bg-purple-100 text-purple-800' :
                      incident.status === 'Resolved' ? 'bg-green-100 text-green-800' :
                      'bg-red-100 text-red-800'
                    }`}>
                      {incident.status}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {incident.serviceName}
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

export default DashboardPage;
