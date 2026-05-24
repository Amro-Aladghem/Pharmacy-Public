# 💊 Pharmacy Management System

A comprehensive web-based pharmacy management application designed to streamline pharmaceutical operations, inventory management, and customer service.

---

## 📋 Table of Contents

- [About](#about)
- [Project Overview](#project-overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Features](#features)
- [Screenshots](#screenshots)
- [Getting Started](#getting-started)
- [Contributing](#contributing)
- [License](#license)

---

## 💡 About

The **Pharmacy Management System** is a modern solution that helps pharmacy businesses manage their operations efficiently. From inventory tracking to customer management and prescription processing, this system provides an integrated platform for pharmacy staff to deliver better service.

---

## 🎯 Project Overview

### The Idea

This project aims to digitize and automate pharmacy operations by providing:

- **Real-time inventory management** - Track stock levels and automate reordering
- **Customer prescription management** - Maintain prescription history and patient information
- **Sales and billing** - Process transactions quickly and accurately
- **Staff management** - Organize shift scheduling and staff performance
- **Reporting & Analytics** - Generate comprehensive business insights

---

## 🛠️ Tech Stack

| Technology | Percentage | Usage |
|------------|-----------|-------|
| **C#** | 70.7% | Backend services, business logic, API |
| **JavaScript** | 27.4% | Frontend interactivity, client-side features |
| **CSS** | 1.7% | Styling and responsive design |
| **HTML** | 0.2% | Markup structure |

### Architecture
- **Backend**: ASP.NET Core (C#)
- **Frontend**: JavaScript, HTML, CSS
- **Database**: SQL Server / Entity Framework
- **UI Framework**: Bootstrap / Custom CSS

---

## 📁 Project Structure

```
Pharmacy-Public/
│
├── Backend/                    # C# ASP.NET Core API
│   ├── Controllers/            # API endpoints
│   ├── Models/                 # Data models
│   ├── Services/               # Business logic
│   ├── Data/                   # Database context
│   └── Migrations/             # Database migrations
│
├── Frontend/                   # JavaScript + HTML + CSS
│   ├── Views/                  # HTML templates
│   ├── Scripts/                # JavaScript files
│   ├── Styles/                 # CSS stylesheets
│   └── Assets/                 # Images and resources
│
├── Database/                   # Database scripts
│   └── Schemas/                # SQL schema definitions
│
├── README.md                   # Project documentation
└── .gitignore                  # Git configuration

```

---

## ✨ Features

- ✅ User authentication and authorization
- ✅ Inventory management and tracking
- ✅ Prescription processing
- ✅ Point-of-sale (POS) system
- ✅ Customer management
- ✅ Report generation
- ✅ Responsive web interface
- ✅ Real-time data updates

---

## 📸 Screenshots

### Dashboard Overview
<img width="958" height="443" alt="Dashboard - Main Overview" src="https://github.com/user-attachments/assets/51e7919c-529c-4064-9dd8-6f27b537b657" />

### Inventory Management
<img width="951" height="445" alt="Inventory Management - Stock Tracking" src="https://github.com/user-attachments/assets/dccc6936-f34e-4b4c-a641-a42fd56526b2" />

### Prescription Management
<img width="953" height="443" alt="Prescription Management - Patient Records" src="https://github.com/user-attachments/assets/34279427-3612-4301-b324-7005fb7b6e5f" />

### Sales & Billing System
<img width="954" height="440" alt="Sales & Billing - Point of Sale" src="https://github.com/user-attachments/assets/8528cc0a-b6ff-4d05-b240-96dddb45cdc6" />

### Customer Management
<img width="956" height="440" alt="Customer Management - Patient Database" src="https://github.com/user-attachments/assets/20c14fb7-4914-4c22-a4ea-677ecefcb68d" />

### Reports & Analytics
<img width="955" height="440" alt="Reports & Analytics - Business Insights" src="https://github.com/user-attachments/assets/91157316-d5f6-4941-bda5-2b034566388f" />

### Settings & Configuration
<img width="953" height="441" alt="Settings & Configuration - System Setup" src="https://github.com/user-attachments/assets/0b7a6831-68fc-4683-bfe4-3de9b6b41ef6" />

---

## 🚀 Getting Started

### Prerequisites

- .NET 5.0 or higher
- SQL Server or compatible database
- Node.js (for frontend dependencies, if applicable)
- Visual Studio or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Amro-Aladghem/Pharmacy-Public.git
   cd Pharmacy-Public
   ```

2. **Set up the backend**
   ```bash
   cd Backend
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

3. **Set up the frontend**
   ```bash
   cd Frontend
   # Install dependencies if needed
   npm install
   ```

4. **Access the application**
   - Open your browser and navigate to `http://localhost:5000`

---

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## 📞 Contact

**Amro Aladghem**
- GitHub: [@Amro-Aladghem](https://github.com/Amro-Aladghem)
- Repository: [Pharmacy-Public](https://github.com/Amro-Aladghem/Pharmacy-Public)

---

## 📝 Additional Notes

This pharmacy management system is built with scalability and user-friendliness in mind. Whether you're managing a small independent pharmacy or a larger chain, this system can be customized to fit your specific needs.

**Last Updated**: May 24, 2026
