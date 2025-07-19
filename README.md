# ByHandDeliveryApi

A backend RESTful API built with **ASP.NET Core** to support a localized delivery platform. It handles secure user authentication, distance-based delivery cost calculations, product uploads (including image blob storage), and modular service layers.

---

## 🚀 Features

- 🔐 Password hashing for secure customer authentication
- 📍 Integration with Google Distance Matrix API
- 🧳 Product image blob storage
- 🧩 Clean architecture using DTOs and models
- 📤 Unified and standardized API responses
- 🗃️ Database access via Entity Framework Core

---

## 🧱 Tech Stack

- ASP.NET Core (C#)
- Entity Framework Core
- SQL Server
- Google Maps API
- Local/Cloud Blob Storage

---

## 📁 Folder Structure Overview

```plaintext
ByHandDeliveryApi/
├── Controllers/             # API route handlers
├── DTO/                     # Data Transfer Objects
├── DataModel/               # Database schema using EF Core
├── GenericResponses/        # API response format definitions
├── Models/                  # Domain models
├── Properties/              # Launch profiles
├── Security/                # Authentication, password hashing
├── Services/                # Business logic and service layer
├── images/blob/products/    # Stored product images
├── Program.cs               # Entry point of the app
├── Startup.cs               # Middleware and dependency config
├── appsettings.json         # App config file
├── appsettings.Development.json
```

---

## 🔧 Getting Started

### ✅ Prerequisites

Make sure you have the following installed:

- [.NET 5 SDK or higher](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or Cloud instance)
- Google Maps API key (for distance calculation)

### 📦 Installation & Setup

```bash
# Clone the repository
git clone https://github.com/RAFI-006/ByHandDeliveyApi.git

# Navigate to project directory
cd ByHandDeliveyApi/ByHandDeliveryApi

# Restore dependencies
dotnet restore

# Run EF Core migrations (optional if DB already exists)
dotnet ef database update

# Start the application
dotnet run
```

---

## 🔌 Sample API Endpoints

| Method | Endpoint                   | Description                          |
|--------|----------------------------|--------------------------------------|
| POST   | `/api/auth/register`       | Register a new customer              |
| POST   | `/api/auth/login`          | Authenticate customer, return token  |
| GET    | `/api/distance/calculate`  | Calculates delivery distance         |
| POST   | `/api/products/upload`     | Upload product with image blob       |

> Swagger integration can be added using `Swashbuckle.AspNetCore` for full documentation.

---

## 🔒 Security

- Passwords are stored using secure hash functions.
- Secrets and environment settings are separated using `appsettings.Development.json`.

---

## 🧪 Testing (Optional Guidance)

This project is ready to be extended with tests using:

- `xUnit` for unit testing
- `Moq` for mocking services and repositories
- `Microsoft.AspNetCore.TestHost` for integration testing

---

## 🖼️ Image Storage

Uploaded product images are stored in the directory:  
`images/blob/products/`  
Ensure the backend has write access to this folder. Future enhancements can include moving to cloud blob storage (Azure Blob or AWS S3).

---

## 🙋‍♂️ Author

**[RAFI-006](https://github.com/RAFI-006)**  
Crafted to support real-world, by-hand delivery workflows with modular and scalable backend APIs.

---

## 📜 License

This project is licensed under the **MIT License**.  
Feel free to use, modify, and contribute.
