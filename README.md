# E-Governance Grievance Redressal System 🏛️

A comprehensive web application designed to bridge the gap between citizens and government authorities. This system facilitates transparent, efficient, and accountable grievance handling, ensuring that every citizen's voice is heard and resolved.

## 🚀 Project Overview

The **E-Governance Grievance Redressal System** is a full-stack solution built to modernize public administration. It allows citizens to lodge complaints, track their status in real-time, and receive updates. On the administrative side, it empowers officers and supervisors with tools to manage, assign, and resolve grievances effectively, backed by powerful analytics.

### Key Goals
- **Transparency**: Real-time status tracking for citizens.
- **Efficiency**: Streamlined workflow for officers and supervisors.
- **Accountability**: Escalation mechanisms and resolution tracking.
- **Accessibility**: Modern, responsive, and user-friendly interface.

---

## 🛠️ Technology Stack

### Backend
- **Framework**: .NET 8 Web API
- **Language**: C#
- **Database**: Microsoft SQL Server (Production) / InMemory (Dev/Test)
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Testing**: xUnit, Moq

### Frontend
- **Framework**: Angular 18
- **Language**: TypeScript
- **Styling**: Modern CSS, Glassmorphism Design
- **Components**: Angular Material, Custom Premium Components
- **HTTP Client**: RxJS based generic HTTP service

---

## 📋 Prerequisites

Before running the application, ensure you have the following installed:

1.  **[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**
2.  **[Node.js](https://nodejs.org/)** (v18 or higher)
3.  **[Angular CLI](https://angular.io/cli)** (`npm install -g @angular/cli`)
4.  **[SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)** (Optional if using InMemory database)
5.  **Git**

---

## ⚡ Setup Instructions

### 1. Clone the Repository
```bash
git clone <repository-url>
cd E-Governance-Grievance-System
```

### 2. Backend Setup
The backend API handles all data operations, authentication, and business logic.

1.  Navigate to the Backend directory:
    ```bash
    cd Backend
    ```
2.  Restore dependencies:
    ```bash
    dotnet restore
    ```
3.  Run the application:
    ```bash
    dotnet run
    ```
    *The API will start at `http://localhost:5038` (or port configured in `launchSettings.json`).*
    *Swagger UI is available at `http://localhost:5038/swagger` for API exploration.*

    > **Note**: By default, the application may use an In-Memory database for quick testing. To use SQL Server, update the connection string in `appsettings.json` and run migrations (`dotnet ef database update`).

### 3. Frontend Setup
The frontend provides the user interface for Citizens, Officers, Supervisors, and Admins.

1.  Navigate to the Frontend directory:
    ```bash
    cd ../Frontend
    ```
2.  Install dependencies:
    ```bash
    npm install
    ```
3.  Start the development server:
    ```bash
    ng serve
    ```
4.  Open your browser and navigate to:
    ```
    http://localhost:4200
    ```

---

## 🌟 Key Features

### 👤 Citizen Module
- **Registration & Login**: Secure access to the portal.
- **Lodge Grievance**: Submit complaints with detailed descriptions and categories.
- **Dashboard**: View status of active and resolved grievances.
- **Notifications**: Receive updates when grievance status changes.
- **Feedback**: Rate the resolution experience.

### 👮 Officer Module
- **Assigned Grievances**: View complaints assigned to the officer's department.
- **Status Updates**: Change status to "In Review", "Resolved", etc.
- **Resolution Remarks**: Provide comments upon resolving an issue.

### 🕵️ Supervisor Module
- **Analytics Dashboard**: Visual reports on department performance and grievance trends.
- **Grievance Assignment**: Assign unassigned grievances to specific officers.
- **Escalation Management**: Handle escalated or overdue grievances.
- **Reports**: View detailed statistics and export data.

### 🔑 Admin Module
- **User Management**: Create and manage Officers and Supervisors.
- **Master Data**: Manage Departments and Grievance Categories.
- **System Overview**: High-level view of system usage.

---

## 🧪 Testing

The solution includes a comprehensive functional unit test suite for the backend services.

To run the tests:
1.  Navigate to the root or test directory.
2.  Execute:
    ```bash
    dotnet test Backend.Tests
    ```

---

## 📸 Screenshots
*(Add screenshots of Landing Page, Dashboard, and Grievance Form here)*

---

Built with ❤️ for better governance.
