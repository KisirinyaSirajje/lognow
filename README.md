# Log Now - SaaS Incident Management System

A production-grade incident management system built with .NET 8, React, TypeScript, PostgreSQL, and SignalR for real-time collaboration.

## 🚀 Features

- **User Authentication** - JWT-based authentication with role-based access control
- **Incident Management** - Create, track, and resolve incidents with SLA monitoring
- **Real-time Updates** - SignalR integration for instant notifications and updates
- **Comment System** - Collaborate on incidents with real-time comments
- **Service Management** - Organize incidents by services
- **Dashboard** - Comprehensive overview with statistics and charts
- **SLA Tracking** - Automatic SLA breach detection and alerts
- **Role-Based Access** - Admin, TeamLead, Engineer, and Viewer roles

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 15+](https://www.postgresql.org/download/) or Docker
- [Git](https://git-scm.com/)

## 🛠️ Installation & Setup

### Option 1: Using Docker for PostgreSQL (Recommended)

1. **Start PostgreSQL with Docker**
   ```bash
   cd lognow
   docker-compose up -d
   ```

### Option 2: Local PostgreSQL Installation

1. Install PostgreSQL locally and create a database named `lognow_db`

### Backend Setup

1. **Navigate to the API project**
   ```bash
   cd LogNow.API
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection (if needed)**
   Edit `appsettings.json` and update the connection string if your PostgreSQL credentials differ:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=lognow_db;Username=postgres;Password=postgres"
   }
   ```

4. **Run database migrations**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Run the API**
   ```bash
   dotnet run
   ```
   
   The API will start at `http://localhost:5000`
   Swagger UI: `http://localhost:5000/swagger`

### Frontend Setup

1. **Navigate to the frontend project**
   ```bash
   cd ../lognow-frontend
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Start the development server**
   ```bash
   npm run dev
   ```
   
   The frontend will start at `http://localhost:3000`

## 📱 Usage

1. **Access the application**
   Open your browser and navigate to `http://localhost:3000`

2. **Register a new account**
   - Click "Register" on the login page
   - Fill in your details and select a role
   - First user should register as "Admin"

3. **Login**
   Use your credentials to log in

4. **Create Services**
   - Navigate to Services
   - Add services that your incidents will be associated with

5. **Create Incidents**
   - Go to Incidents → Create Incident
   - Fill in the details, select service and severity
   - The system will automatically calculate SLA times

6. **Manage Incidents**
   - View all incidents on the Incidents page
   - Click on an incident to view details
   - Change status, add comments in real-time
   - Collaborate with team members

## 🏗️ Architecture

### Backend (.NET 8)
- **Clean Architecture** with separation of concerns
- **Repository Pattern** for data access
- **Entity Framework Core** with PostgreSQL
- **JWT Authentication** for security
- **SignalR** for real-time features
- **Swagger** for API documentation

### Frontend (React + TypeScript)
- **React 18** with TypeScript
- **React Router** for navigation
- **Axios** for API calls
- **SignalR Client** for real-time updates
- **TailwindCSS** for styling
- **Context API** for state management

### Database (PostgreSQL)
- Users
- Services
- Incidents
- IncidentComments
- IncidentTimeline
- SLAs

## 🔐 User Roles

- **Admin** - Full access, can manage users and services
- **Team Lead** - Can assign incidents and escalate
- **Engineer** - Can update and resolve assigned incidents
- **Viewer** - Read-only access

## 📊 SLA Definitions

| Severity | Response Time | Resolution Time |
|----------|---------------|-----------------|
| SEV1     | 5 minutes     | 30 minutes      |
| SEV2     | 15 minutes    | 2 hours         |
| SEV3     | 1 hour        | 24 hours        |
| SEV4     | 4 hours       | 72 hours        |

## 🔧 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login

### Services
- `GET /api/services` - Get all services
- `POST /api/services` - Create service (Admin only)
- `PUT /api/services/{id}` - Update service (Admin only)
- `DELETE /api/services/{id}` - Delete service (Admin only)

### Incidents
- `GET /api/incidents` - Get all incidents
- `GET /api/incidents/{id}` - Get incident details
- `POST /api/incidents` - Create incident
- `PUT /api/incidents/{id}` - Update incident
- `PUT /api/incidents/{id}/assign` - Assign incident
- `PUT /api/incidents/{id}/status` - Update status
- `DELETE /api/incidents/{id}` - Delete incident (Admin only)

### Comments
- `GET /api/incidents/{id}/comments` - Get comments
- `POST /api/incidents/{id}/comments` - Add comment

### Dashboard
- `GET /api/dashboard` - Get dashboard data

## 🔄 Real-time Features

The application uses SignalR for real-time updates:
- New incident notifications
- Status change notifications
- Real-time comment updates
- Live collaboration on incidents

## 🐛 Troubleshooting

### Database Connection Issues
- Ensure PostgreSQL is running
- Check connection string in `appsettings.json`
- Verify database exists: `lognow_db`

### Port Conflicts
- Backend: Change port in `launchSettings.json`
- Frontend: Change port in `vite.config.ts`

### SignalR Connection Issues
- Ensure backend is running before starting frontend
- Check CORS settings in `Program.cs`
- Verify SignalR URL in frontend `signalRService.ts`

## 📝 Development

### Running Migrations
```bash
cd LogNow.API
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Building for Production
```bash
# Backend
cd LogNow.API
dotnet publish -c Release

# Frontend
cd lognow-frontend
npm run build
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 👥 Support

For issues and questions:
- Open an issue on GitHub
- Contact the development team

## 🎯 Roadmap

- [ ] Email notifications
- [ ] Incident templates
- [ ] Advanced reporting
- [ ] Mobile app
- [ ] Integration with monitoring tools
- [ ] Multi-tenancy support

---

**Built with ❤️ using .NET 8, React, TypeScript, and PostgreSQL**
