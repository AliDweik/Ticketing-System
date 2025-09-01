# 🎫 Ticketing System – ASP.NET Core Backend

A robust, secure, and scalable ticketing system backend built with ASP.NET Core 8 and Entity Framework Core.
It provides comprehensive ticket management with role-based access control, real-time updates, and a RESTful API.

# ✨ Features
# 🔐 Authentication & Authorization

JWT-based authentication

Role-based access control (Admin, Support, Client)

Policy-based authorization for fine-grained control

# 🎫 Ticket Management

CRUD operations for tickets

Ticket status tracking (New, In Progress, Resolved, Closed)

Priority levels (Low, Medium, High)

Ticket assignment & reassignment

# 💬 Collaboration

Ticket comments and discussions

File attachments support

Real-time updates (SignalR-ready architecture)

# 📊 Dashboard & Analytics

Comprehensive dashboard with statistics

Ticket status distribution

User performance metrics

Export capabilities

# 🛡️ Security

Input validation with FluentValidation

SQL injection prevention

XSS protection

Rate limiting ready

# 🚀 Getting Started
# 📌 Prerequisites

.NET 8.0 SDK

SQL Server
 (LocalDB, Express, or full version)

Visual Studio 2022 / VS Code

# ⚙️ Installation

Clone the repository

git clone https://github.com/your-username/ticketing-system.git
cd ticketing-system


Configure the database
Update the connection string in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TicketingSystemDB;Trusted_Connection=True;"
}


Apply database migrations

dotnet ef database update --project TicketingSystemm.Data --startup-project TicketingSystem.API


Run the application

dotnet run --project TicketingSystem.API

# 🛠️ Technology Stack

Backend Framework: ASP.NET Core 8.0

Database: SQL Server with Entity Framework Core

Authentication: JWT Bearer Tokens

Validation: FluentValidation

API Documentation: Swagger / OpenAPI

Logging: Serilog

# 🔧 Configuration
# 🌍 Environment Variables

# JWT Configuration
JWT__Secret=your-super-secret-key
JWT__Issuer=your-issuer
JWT__Audience=your-audience

# Database
ConnectionStrings__DefaultConnection=Server=localhost;Database=TicketingSystem;Trusted_Connection=True;

# CORS
AllowedOrigins=http://localhost:3000,http://localhost:4200

# 📂 appsettings.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TicketingSystem;Trusted_Connection=True;"
  },
  "Jwt": {
    "Secret": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  },
  "AllowedHosts": "*"
}

# 📡 Access the API

API Base URL → https://localhost:7000

Swagger UI → https://localhost:7000/swagger

# 🤝 Contributing

Fork the project

Create your feature branch → git checkout -b feature/AmazingFeature

Commit your changes → git commit -m 'Add some AmazingFeature'

Push to the branch → git push origin feature/AmazingFeature

Open a Pull Request

# 📄 License

This project is licensed under the MIT License – see the LICENSE.md
 file for details.

⭐ Star this repo if you find it helpful!
