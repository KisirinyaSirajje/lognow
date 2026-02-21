import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { incidentService } from '../services/incidentService';
import { Incident } from '../types';

const IncidentsPage = () => {
  const [incidents, setIncidents] = useState<Incident[]>([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('all');

  useEffect(() => {
    loadIncidents();
  }, []);

  const loadIncidents = async () => {
    try {
      const data = await incidentService.getAll();
      setIncidents(data);
    } catch (error) {
      console.error('Failed to load incidents:', error);
    } finally {
      setLoading(false);
    }
  };

  const filteredIncidents = incidents.filter((incident) => {
    if (filter === 'all') return true;
    if (filter === 'pending') return incident.status === 'Pending';
    if (filter === 'inprogress') return incident.status === 'InProgress' || incident.status === 'Assigned';
    if (filter === 'resolved') return incident.status === 'Resolved' || incident.status === 'Cancelled';
    return true;
  });

  if (loading) {
    return <div className="text-center py-10">Loading...</div>;
  }

  return (
    <div className="px-4 py-6">
      <div className="mb-6 flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Incidents</h1>
          <p className="mt-1 text-sm text-gray-500">Manage all service incidents</p>
        </div>
        <Link
          to="/incidents/new"
          className="px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700"
        >
          Create Incident
        </Link>
      </div>

      {/* Filters */}
      <div className="mb-6 flex space-x-4">
        <button
          onClick={() => setFilter('all')}
          className={`px-4 py-2 text-sm font-medium rounded-md ${
            filter === 'all'
              ? 'bg-blue-600 text-white'
              : 'bg-white text-gray-700 hover:bg-gray-50 border border-gray-300'
          }`}
        >
          All ({incidents.length})
        </button>
        <button
          onClick={() => setFilter('pending')}
          className={`px-4 py-2 text-sm font-medium rounded-md ${
            filter === 'pending'
              ? 'bg-blue-600 text-white'
              : 'bg-white text-gray-700 hover:bg-gray-50 border border-gray-300'
          }`}
        >
          Pending ({incidents.filter((i) => i.status === 'Pending').length})
        </button>
        <button
          onClick={() => setFilter('inprogress')}
          className={`px-4 py-2 text-sm font-medium rounded-md ${
            filter === 'inprogress'
              ? 'bg-blue-600 text-white'
              : 'bg-white text-gray-700 hover:bg-gray-50 border border-gray-300'
          }`}
        >
          In Progress ({incidents.filter((i) => i.status === 'InProgress' || i.status === 'Assigned').length})
        </button>
        <button
          onClick={() => setFilter('resolved')}
          className={`px-4 py-2 text-sm font-medium rounded-md ${
            filter === 'resolved'
              ? 'bg-blue-600 text-white'
              : 'bg-white text-gray-700 hover:bg-gray-50 border border-gray-300'
          }`}
        >
          Resolved ({incidents.filter((i) => i.status === 'Resolved' || i.status === 'Cancelled').length})
        </button>
      </div>

      {/* Incidents Table */}
      <div className="bg-white shadow rounded-lg overflow-hidden">
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
                  Service
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Severity
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Status
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Assigned To
                </th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Created
                </th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {filteredIncidents.map((incident) => (
                <tr key={incident.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap">
                    <Link to={`/incidents/${incident.id}`} className="text-blue-600 hover:text-blue-800 font-medium">
                      {incident.incidentNumber}
                    </Link>
                  </td>
                  <td className="px-6 py-4">
                    <div className="text-sm text-gray-900">{incident.title}</div>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {incident.serviceName}
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
                    {incident.assignedToUserName || 'Unassigned'}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                    {new Date(incident.createdAt).toLocaleDateString()}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {filteredIncidents.length === 0 && (
        <div className="text-center py-10 text-gray-500">
          No incidents found
        </div>
      )}
    </div>
  );
};

export default IncidentsPage;
