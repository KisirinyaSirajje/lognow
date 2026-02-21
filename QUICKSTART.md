# 🚀 Quick Start Guide - Log Now

Follow these steps to get Log Now up and running in minutes!

## Step 1: Start PostgreSQL Database

Open a terminal in the `lognow` folder and run:

```bash
docker-compose up -d
```

This will start PostgreSQL in a Docker container.

## Step 2: Setup and Run Backend

Open a new terminal:

```bash
cd LogNow.API

# Restore dependencies
dotnet restore

# Create database migrations
dotnet ef migrations add InitialCreate

# Apply migrations to database
dotnet ef database update

# Run the API
dotnet run
```

The API will be available at: **http://localhost:5000**  
Swagger UI: **http://localhost:5000/swagger**

## Step 3: Setup and Run Frontend

Open another terminal:

```bash
cd lognow-frontend

# Install dependencies
npm install

# Start the development server
npm run dev
```

The frontend will be available at: **http://localhost:3000**

## Step 4: Access the Application

1. Open your browser and go to: **http://localhost:3000**

2. Click **Register** and create your first account:
   - Full Name: `Admin User`
   - Username: `admin`
   - Email: `admin@lognow.com`
   - Password: `Admin123!`
   - Role: **Admin**
   - Team: `IT Operations`

3. Click **Register** - you'll be automatically logged in

## Step 5: Create Your First Service

1. Click on **Services** in the navigation
2. Click **Add Service**
3. Fill in:
   - Name: `Web Application`
   - Description: `Main customer-facing web application`
   - Owner Team: `Engineering`
   - Status: `Active`
4. Click **Create**

## Step 6: Create Your First Incident

1. Click on **Incidents** in the navigation
2. Click **Create Incident**
3. Fill in:
   - Title: `Database connection timeout`
   - Description: `Users experiencing slow page loads due to database timeout errors`
   - Service: Select `Web Application`
   - Severity: `SEV2`
4. Click **Create Incident**

## Step 7: Test Real-time Features

1. Click on the incident you just created
2. Add a comment: `Investigating the issue`
3. Change the status to **In Progress**
4. Open another browser tab/window to http://localhost:3000
5. Login with the same credentials
6. Navigate to the same incident
7. Add a comment in one tab and watch it appear in real-time in the other tab! 🎉

## 🎯 You're All Set!

Your production-grade incident management system is now running!

### What to explore next:

- ✅ View the **Dashboard** for statistics
- ✅ Create more **Services** for organization
- ✅ Invite team members by having them **Register**
- ✅ Test different severity levels (SEV1, SEV2, SEV3, SEV4)
- ✅ Watch SLA timers and breach warnings
- ✅ Test real-time collaboration with comments

## 🛑 To Stop the Application

### Stop Frontend:
Press `Ctrl+C` in the frontend terminal

### Stop Backend:
Press `Ctrl+C` in the backend terminal

### Stop Database:
```bash
docker-compose down
```

## 🔄 To Restart

Just run the commands from Steps 2 and 3 again (no need to run migrations again).

## ⚠️ Troubleshooting

### "Port already in use" error
- **Backend (5000)**: Change port in `LogNow.API/Properties/launchSettings.json`
- **Frontend (3000)**: Change port in `lognow-frontend/vite.config.ts`
- **Database (5432)**: Change port in `docker-compose.yml`

### Database connection error
```bash
# Check if PostgreSQL is running
docker ps

# If not running, start it
docker-compose up -d

# Check logs
docker-compose logs postgres
```

### Entity Framework not found
```bash
# Install EF Core tools globally
dotnet tool install --global dotnet-ef
```

## 📚 Need More Help?

Check the main **README.md** for detailed documentation!

---

**Happy Incident Managing! 🎊**
