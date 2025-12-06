# Stock Management & POS System

This web application targets a clothing retail franchise. A franchise manager can administer branches, clothing items and variants, suppliers, customers and more. Items are received from suppliers into branch inventory using **Receive Orders**, and sold to retail customers via **Sales Invoices** through branch Point Of Sale (POS) pages. The system uses role-based access control so administrators can grant or limit users' access to features.

## 🌟 Features

### 👥 CRUD & Catalog Management
- Manage clothing items and variants
- Manage branches
- Manage discounts & taxes
- Manage customers and suppliers

### 🔐 User & Authentication
- User registration and login
- OTP-based password recovery
- Edit user profile
- User CRUD operations

### 🛡 Role & Permission Management
- Create and manage roles
- Assign fine-grained permissions to roles
- Built-in, non-removable **Super Admin** role

### 📥 Receive Orders
- Create Receive Orders to add stock to a branch
- Assign suppliers and taxes
- Specify quantity and buying price per item

### 🛒 Point of Sale (POS)
- Non-refreshing, user-friendly POS page (AJAX)
- Fast item search and image preview
- Cart system with per-item discounts
- Apply active discounts
- Generate PDF sales receipts after checkout

### 📊 Dashboard
- Stock and sales metrics
- Monthly purchasing & sales charts
- Best-selling branches and items
- Low-stock and slow-moving items alerts

### 🧰 Utilities
- Server-side and client-side validation
- Searching, filtering, pagination and sorting for lists (invoices, receive orders, branch items, etc.)
- Database initializer to seed Super Admin and role/permission data
- Image upload and storage
- Email sending service

### 🏗 Infrastructure
- SOLID principles applied throughout the codebase
- Repository pattern with Unit of Work for data access
- Layered architecture separating Presentation, Application, and Core concerns
- Centralized configuration for logging, email, and Database Initializer
- Migration-friendly EF Core setup and seeders for reproducible environments

## 👨‍💻 Team

### Mohamed Saeed  
[![GitHub](https://img.shields.io/badge/GitHub-Profile-blue?logo=github)](https://github.com/Mohmaed-Saaed)

### Ahmed Khaled  
[![GitHub](https://img.shields.io/badge/GitHub-Profile-blue?logo=github)](https://github.com/AhmedMekheimer)

## 🛠 Tech Stack

![.NET](https://img.shields.io/badge/.NET-ASP.NET%20Core%20MVC-blue?logo=dotnet)  
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-red?logo=microsoft-sql-server)  
![Entity Framework](https://img.shields.io/badge/ORM-Entity%20Framework-green)  
![VS2022](https://img.shields.io/badge/IDE-VS2022-purple?logo=visual-studio)  
![GitHub](https://img.shields.io/badge/Platform-GitHub-black?logo=github)  
