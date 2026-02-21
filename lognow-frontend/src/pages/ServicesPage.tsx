import { useState, useEffect, FormEvent } from 'react';
import { serviceService } from '../services/serviceService';
import { Service } from '../types';
import { useAuth } from '../context/AuthContext';

const ServicesPage = () => {
  const { user } = useAuth();
  const [services, setServices] = useState<Service[]>([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    ownerTeam: '',
    status: 'Active',
  });

  useEffect(() => {
    loadServices();
  }, []);

  const loadServices = async () => {
    try {
      const data = await serviceService.getAll();
      setServices(data);
    } catch (error) {
      console.error('Failed to load services:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    try {
      await serviceService.create(formData);
      setShowModal(false);
      setFormData({ name: '', description: '', ownerTeam: '', status: 'Active' });
      loadServices();
    } catch (error) {
      console.error('Failed to create service:', error);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const isAdmin = user?.role === 'Admin';

  if (loading) {
    return <div className="text-center py-10">Loading...</div>;
  }

  return (
    <div className="px-4 py-6">
      <div className="mb-6 flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Services</h1>
          <p className="mt-1 text-sm text-gray-500">Manage application services</p>
        </div>
        {isAdmin && (
          <button
            onClick={() => setShowModal(true)}
            className="px-4 py-2 border border-transparent text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700"
          >
            Add Service
          </button>
        )}
      </div>

      {/* Services Grid */}
      <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
        {services.map((service) => (
          <div key={service.id} className="bg-white shadow rounded-lg p-6">
            <div className="flex justify-between items-start mb-4">
              <h3 className="text-lg font-medium text-gray-900">{service.name}</h3>
              <span className={`px-2 py-1 text-xs font-semibold rounded-full ${
                service.status === 'Active' ? 'bg-green-100 text-green-800' :
                service.status === 'Inactive' ? 'bg-gray-100 text-gray-800' :
                'bg-red-100 text-red-800'
              }`}>
                {service.status}
              </span>
            </div>
            <p className="text-sm text-gray-600 mb-4">{service.description}</p>
            <div className="border-t pt-4">
              <dl className="space-y-2">
                <div>
                  <dt className="text-xs font-medium text-gray-500">Owner Team</dt>
                  <dd className="mt-1 text-sm text-gray-900">{service.ownerTeam}</dd>
                </div>
                <div>
                  <dt className="text-xs font-medium text-gray-500">Created</dt>
                  <dd className="mt-1 text-sm text-gray-900">
                    {new Date(service.createdAt).toLocaleDateString()}
                  </dd>
                </div>
              </dl>
            </div>
          </div>
        ))}
      </div>

      {services.length === 0 && (
        <div className="text-center py-10 text-gray-500">
          No services found
        </div>
      )}

      {/* Create Service Modal */}
      {showModal && (
        <div className="fixed z-10 inset-0 overflow-y-auto">
          <div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
            <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" onClick={() => setShowModal(false)}></div>

            <div className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
              <form onSubmit={handleSubmit}>
                <div className="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                  <h3 className="text-lg font-medium text-gray-900 mb-4">Create New Service</h3>
                  <div className="space-y-4">
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Name</label>
                      <input
                        type="text"
                        name="name"
                        required
                        className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                        value={formData.name}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Description</label>
                      <textarea
                        name="description"
                        rows={3}
                        required
                        className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                        value={formData.description}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Owner Team</label>
                      <input
                        type="text"
                        name="ownerTeam"
                        required
                        className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                        value={formData.ownerTeam}
                        onChange={handleChange}
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium text-gray-700">Status</label>
                      <select
                        name="status"
                        className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
                        value={formData.status}
                        onChange={handleChange}
                      >
                        <option value="Active">Active</option>
                        <option value="Inactive">Inactive</option>
                        <option value="Deprecated">Deprecated</option>
                      </select>
                    </div>
                  </div>
                </div>
                <div className="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
                  <button
                    type="submit"
                    className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:ml-3 sm:w-auto sm:text-sm"
                  >
                    Create
                  </button>
                  <button
                    type="button"
                    onClick={() => setShowModal(false)}
                    className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
                  >
                    Cancel
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ServicesPage;
