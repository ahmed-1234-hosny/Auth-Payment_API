# 💳 Payment Authentication API

## 📌 Description

A secure Payment Authentication API built using Clean Architecture principles.
The system handles user authentication and authorization using access and refresh tokens, ensuring high security and scalability.

---

## 🏗️ Architecture

This project follows **Clean Architecture**, separating concerns into four main layers:

* **Domain Layer** → Contains core entities and business logic
* **Application Layer** → Contains services, DTOs, and interfaces
* **Infrastructure Layer** → Handles database access and external services
* **Presentation Layer** → API controllers and endpoints

---

## 📂 Project Structure

Each layer includes:

* **Models** → Core data structures
* **DTOs** → Data Transfer Objects
* **Repositories** → Data access logic
* **Services** → Business logic
* **Interfaces** → Abstractions for dependency inversion
* **Controllers** → API endpoints

---

## 🚀 Features

* 🔐 User Authentication (Login / Register)
* 🎟️ JWT Access Token Generation
* 🔄 Refresh Token Mechanism
* 🧱 Clean Architecture Implementation
* 🗄️ Repository Pattern
* ⚙️ Dependency Injection

---

## 🛠️ Tech Stack

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication

---

## 🔑 Authentication Flow

1. User logs in with credentials
2. System generates:
   * Access Token (short-lived)
   * Refresh Token (long-lived)
3. Refresh token is stored in database
4. When access token expires, a new one is generated using the refresh token

---

## ▶️ How to Run

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/your-repo.git
   ```

2. Open the project in Visual Studio

3. Update the **Connection String** in `appsettings.json`

4. Apply migrations:

   ```bash
   update-database
   ```

5. Run the project

---

## 📡 API Endpoints

### 🔐 Auth

* `POST /api/auth/register` → Register new user
* `POST /api/auth/login` → Login and get tokens
* `POST /api/auth/refresh-token` → Refresh access token

### 🔐 Payment
* `POST /api/payment/checkout` 


---

## 👨‍💻 Author

Ahmed Hosny
