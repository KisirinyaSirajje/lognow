import { useState, FormEvent, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { incidentService } from '../services/incidentService';
import { serviceService } from '../services/serviceService';
import { Service } from '../types';

const CreateIncidentPage = () => {
  const navigate = useNavigate();
  const [services, setServices] = useState<Service[]>([]);
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    serviceId: '',
    severity: 'SEV3',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    loadServices();
  }, []);

  const loadServices = async () => {
    try {
      const data = await serviceService.getAll();
      setServices(data);
    } catch (error) {
      console.error('Failed to load services:', error);
    }
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const incident = await incidentService.create(formData);
      navigate(`/incidents/${incident.id}`);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to create incident');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  return (
    <div className="px-4 py-6 max-w-3xl mx-auto">
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-gray-900">Create New Incident</h1>
        <p className="mt-1 text-sm text-gray-500">Report a new service incident</p>
      </div>

      <div className="bg-white shadow rounded-lg p-6">
        <form onSubmit={handleSubmit} className="space-y-6">
          {error && (
            <div className="rounded-md bg-red-50 p-4">
              <div className="text-sm text-red-800">{error}</div>
            </div>
          )}

          <div>
            <label htmlFor="title" className="block text-sm font-medium text-gray-700">
              Title *
            </label>
            <input
              type="text"
              name="title"
              id="title"
              required
              className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
              value={formData.title}
              onChange={handleChange}
            />
          </div>

          <div>
            <label htmlFor="description" className="block text-sm font-medium text-gray-700">
              Description *
            </label>
            <textarea
              name="description"
              id="description"
              rows={4}
              required
              className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
              value={formData.description}
              onChange={handleChange}
            />
          </div>

          <div>
            <label htmlFor="serviceId" className="block text-sm font-medium text-gray-700">
              Service *
            </label>
            <select
              name="serviceId"
              id="serviceId"
              required
              className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
              value={formData.serviceId}
              onChange={handleChange}
            >
              <option value="">Select a service</option>
              {services.map((service) => (
                <option key={service.id} value={service.id}>
                  {service.name}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label htmlFor="severity" className="block text-sm font-medium text-gray-700">
              Severity *
            </label>
            <select
              name="severity"
              id="severity"
              className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
              value={formData.severity}
              onChange={handleChange}
            >
              <option value="SEV1">SEV1 - Critical (Response: 5 min, Resolution: 30 min)</option>
              <option value="SEV2">SEV2 - High (Response: 15 min, Resolution: 2 hours)</option>
              <option value="SEV3">SEV3 - Medium (Response: 1 hour, Resolution: 24 hours)</option>
              <option value="SEV4">SEV4 - Low (Response: 4 hours, Resolution: 72 hours)</option>
            </select>
          </div>

          <div className="flex justify-end space-x-3">
            <button
              type="button"
              onClick={() => navigate('/incidents')}
              className="px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 disabled:opacity-50"
            >
              {loading ? 'Creating...' : 'Create Incident'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateIncidentPage;
