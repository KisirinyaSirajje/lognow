import { useAuth } from '../context/AuthContext';

const ProfilePage = () => {
  const { user } = useAuth();

  if (!user) {
    return <div>Loading...</div>;
  }

  return (
    <div className="max-w-4xl mx-auto">
      <h1 className="text-3xl font-bold text-gray-900 mb-8">Profile</h1>

      <div className="bg-white shadow rounded-lg overflow-hidden">
        <div className="px-6 py-8">
          <div className="flex items-center mb-8">
            <div className="h-20 w-20 rounded-full bg-blue-600 flex items-center justify-center text-white text-2xl font-bold">
              {user.fullName
                .split(' ')
                .map((n) => n[0])
                .join('')
                .toUpperCase()}
            </div>
            <div className="ml-6">
              <h2 className="text-2xl font-bold text-gray-900">{user.fullName}</h2>
              <p className="text-gray-600">@{user.username}</p>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Email
              </label>
              <div className="text-gray-900 bg-gray-50 px-4 py-2 rounded-md">
                {user.email}
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Role
              </label>
              <div className="text-gray-900 bg-gray-50 px-4 py-2 rounded-md">
                <span
                  className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                    user.role === 'Admin'
                      ? 'bg-purple-100 text-purple-800'
                      : user.role === 'TeamLead'
                      ? 'bg-blue-100 text-blue-800'
                      : user.role === 'Engineer'
                      ? 'bg-green-100 text-green-800'
                      : 'bg-gray-100 text-gray-800'
                  }`}
                >
                  {user.role}
                </span>
              </div>
            </div>

            {user.team && (
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Team
                </label>
                <div className="text-gray-900 bg-gray-50 px-4 py-2 rounded-md">
                  {user.team}
                </div>
              </div>
            )}

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Status
              </label>
              <div className="text-gray-900 bg-gray-50 px-4 py-2 rounded-md">
                <span
                  className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                    user.isActive
                      ? 'bg-green-100 text-green-800'
                      : 'bg-red-100 text-red-800'
                  }`}
                >
                  {user.isActive ? 'Active' : 'Inactive'}
                </span>
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Member Since
              </label>
              <div className="text-gray-900 bg-gray-50 px-4 py-2 rounded-md">
                {new Date(user.createdAt).toLocaleDateString()}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProfilePage;
