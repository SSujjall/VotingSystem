# Voting App

A modern voting application built with .NET Web API backend and Angular frontend.

## Overview

This voting application allows users to participate in polls and elections with a clean, responsive interface. The backend is powered by .NET Web API, providing robust data management and security, while the frontend uses Angular for a dynamic user experience.

## Prerequisites

Before running the application, ensure you have the following installed:

### Backend (.NET API)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) (Community, Professional, or Enterprise)
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- SQL Server or SQL Server Express (if using SQL Server database)

### Frontend (Angular)
- [Node.js](https://nodejs.org/) (LTS version recommended)
- [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
- A modern web browser (Chrome, Firefox, Safari, Edge)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd voting-app
```

### 2. Backend Setup (.NET API)

1. **Open in Visual Studio**
   - Launch Visual Studio 2022
   - Open the solution file (`.sln`) in the backend directory
   - Or use "Open a project or solution" and navigate to your .NET project

2. **Configure Database (if applicable)**
   - Update connection strings in `appsettings.json`
   - Run database migrations if using Entity Framework:
     ```bash
     dotnet ef database update
     ```

3. **Run the API**
   - In Visual Studio, press `F5` or click the "Start" button
   - The API will typically run on `https://localhost:7000` or similar
   - Verify the API is running by navigating to the Swagger documentation (usually at `/swagger`)

### 3. Frontend Setup (Angular)

1. **Navigate to the Angular project directory**
   ```bash
   cd frontend
   # or wherever your Angular project is located
   ```

2. **Install Dependencies**
   ```bash
   npm install
   ```

3. **Configure API Endpoint**
   - Update the API base URL in your environment files (`src/environments/environment.ts`)
   - Ensure it matches your .NET API URL (e.g., `https://localhost:7000`)

4. **Start the Development Server**
   ```bash
   ng serve
   ```
   - The application will be available at `http://localhost:4200`
   - The app will automatically reload when you make changes to the source files

## Usage

### For Voters
1. Navigate to `http://localhost:4200` in your web browser
2. Browse available polls/elections
3. Cast your vote by selecting your preferred option
4. View real-time results (if enabled)

### For Administrators
1. Access the admin panel (route may vary based on implementation)
2. Create new polls/elections
3. Manage voting options
4. Monitor voting statistics
5. Close/open voting periods

## Development

### Backend Development
- Use Visual Studio's debugging tools
- Set breakpoints and inspect variables
- Use the built-in terminal for CLI commands
- Leverage IntelliSense for code completion

### Frontend Development
- Use `ng generate` commands to create components, services, etc.
- Take advantage of Angular DevTools browser extension
- Use the Angular CLI for building and testing

## Building for Production

### Backend
```bash
dotnet publish -c Release
```

### Frontend
```bash
ng build --prod
```

## Troubleshooting

### Common Issues

**API not starting:**
- Ensure all NuGet packages are restored
- Check that the correct .NET SDK version is installed
- Verify database connection string (if applicable)

**Angular app not loading:**
- Run `npm install` to ensure all dependencies are installed
- Check that Node.js and Angular CLI are properly installed
- Verify the API URL in environment configuration

**CORS errors:**
- Ensure CORS is properly configured in the .NET API
- Check that the Angular app URL is allowed in the API's CORS policy

### Getting Help

If you encounter issues:
1. Check the console output for error messages
2. Review the browser's developer tools for frontend issues
3. Check Visual Studio's output window for backend issues
4. Ensure all prerequisites are properly installed

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

**Happy Voting! 🗳️**
