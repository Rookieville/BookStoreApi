# 📚 BookStore API

A modern, secure RESTful API for managing a bookstore built with .NET 8 minimal APIs, featuring JWT authentication, role-based authorization, and comprehensive book management capabilities.

## ✨ Features

- **🔐 JWT Authentication & Authorization** - Secure user registration and login
- **👥 Role-Based Access Control** - User and Admin roles with different permissions
- **📖 Complete Book Management** - CRUD operations for books
- **🗄️ PostgreSQL Database** - Robust data persistence with Entity Framework Core
- **📋 API Documentation** - Interactive Swagger/OpenAPI documentation
- **✅ Input Validation** - FluentValidation for request validation
- **🚨 Global Exception Handling** - Centralized error management
- **🏗️ Clean Architecture** - Well-organized, maintainable codebase

## 🛠️ Tech Stack

- **.NET 8** - Latest .NET framework
- **ASP.NET Core Minimal APIs** - Lightweight, high-performance APIs
- **Entity Framework Core 9** - Modern ORM for database operations
- **PostgreSQL** - Production-ready relational database
- **JWT Bearer Authentication** - Secure token-based authentication
- **Swagger/OpenAPI** - API documentation and testing
- **FluentValidation** - Elegant validation library

## 📋 Prerequisites

Before running this project, ensure you have:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (12+ recommended)
- [Git](https://git-scm.com/)
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd BookStoreApi
```

### 2. Database Setup

1. **Create PostgreSQL Database**:
   ```sql
   CREATE DATABASE bookstore;
   ```

2. **Update Connection String** (if needed):
   Edit `appsettings.json` or `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=bookstore;Username=postgres;Password=your_password"
     }
   }
   ```

### 3. Install Dependencies & Run Migrations

```bash
# Restore NuGet packages
dotnet restore

# Apply database migrations
dotnet ef database update
```

### 4. Run the Application

```bash
# Run in development mode
dotnet run

# Or run with watch (auto-reload on changes)
dotnet watch run
```

The API will be available at:
- **HTTPS**: `https://localhost:7001`
- **HTTP**: `http://localhost:5001`
- **Swagger UI**: `https://localhost:7001/swagger`

## 📚 API Documentation

### Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `POST` | `/api/v1/auth/register` | Register new user | ❌ |
| `POST` | `/api/v1/auth/login` | User login | ❌ |
| `GET` | `/api/v1/auth/profile` | Get user profile | ✅ |

### Book Endpoints

| Method | Endpoint | Description | Auth Required | Role Required |
|--------|----------|-------------|---------------|---------------|
| `GET` | `/api/v1/books` | Get all books | ❌ | - |
| `GET` | `/api/v1/books/{id}` | Get book by ID | ❌ | - |
| `POST` | `/api/v1/books` | Create new book | ✅ | User/Admin |
| `PUT` | `/api/v1/books/{id}` | Update book | ✅ | User/Admin |
| `DELETE` | `/api/v1/books/{id}` | Delete book | ✅ | Admin |

## 🔑 Authentication

This API uses JWT (JSON Web Tokens) for authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

### User Roles

- **User**: Can create and update books
- **Admin**: Can perform all operations including deleting books

## 💡 Usage Examples

### Register a New User

```bash
curl -X POST https://localhost:7001/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe", 
    "email": "john.doe@example.com",
    "password": "SecurePassword123!"
  }'
```

### Login

```bash
curl -X POST https://localhost:7001/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "SecurePassword123!"
  }'
```

### Create a Book (Authenticated)

```bash
curl -X POST https://localhost:7001/api/v1/books \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <your-jwt-token>" \
  -d '{
    "title": "The Great Gatsby",
    "author": "F. Scott Fitzgerald",
    "isbn": "978-0-7432-7356-5",
    "publishedDate": "1925-04-10",
    "genre": "Fiction",
    "price": 12.99,
    "description": "A classic American novel"
  }'
```

### Get All Books

```bash
curl -X GET https://localhost:7001/api/v1/books
```

## 🏗️ Project Structure

```
BookStoreApi/
├── 📁 AppContext/           # Database context
├── 📁 Configurations/       # Entity configurations
├── 📁 Contracts/           # DTOs and request/response models
├── 📁 Endpoints/           # API endpoint definitions
├── 📁 Exceptions/          # Custom exceptions and global handler
├── 📁 Extensions/          # Service extensions and middleware
├── 📁 Interfaces/          # Service interfaces
├── 📁 Migrations/          # EF Core migrations
├── 📁 Models/              # Domain models
├── 📁 Services/            # Business logic services
├── 📄 Program.cs           # Application entry point
├── 📄 appsettings.json     # Configuration
└── 📄 BookStoreApi.csproj  # Project file
```

## 🔧 Configuration

Key configuration settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=bookstore;Username=postgres;Password=postgres"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "BookStoreApi",
    "Audience": "BookStoreApiUsers", 
    "ExpireMinutes": "120"
  }
}
```

## 🧪 Testing

Access the interactive API documentation at `/swagger` when running in development mode. This provides a complete interface for testing all endpoints.

## 🔒 Security Notes

- **JWT Secret**: Change the JWT secret key in production
- **Database**: Use secure credentials for PostgreSQL
- **HTTPS**: Always use HTTPS in production
- **Password Policy**: Implement strong password requirements

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🐛 Issues & Support

If you encounter any issues or have questions:
1. Check the [Issues](../../issues) section
2. Create a new issue with detailed information
3. Include error logs and reproduction steps

---

**Happy Coding! 🚀** 