# Device Management System

An enterprise-grade solution for tracking, assigning, and managing corporate hardware assets. This full-stack application is built with a **.NET 10 Web API** backend and an **Angular** frontend, and includes AI-powered technical descriptions along with a custom weighted search engine.

---

## Getting Started

### 1. Prerequisites

Make sure you have the following installed:

- **Visual Studio 2022**
- **Node.js**
- **SQL Server**

---

### 2. Installation

```bash
# Clone the repository
git clone https://github.com/Herman-Darius/Device_Management_App

# Navigate to the project directory
cd Device_Management_App
```

---

## Running the Project Locally

### Configure Startup Projects (Visual Studio)

1. Open `Device_Management_App.sln`.
2. Right-click the **Solution** → select **Configure Startup Projects**.
3. Choose **Multiple startup projects**.
4. Set:
   - `Device_Management_App.Server` → **Start** (move it to the top)
   - `Device_Management_App.Client` → **Start**

---

### Database Setup

This application requires a local SQL Server instance.
1. Execute:
   - `CreateSchema.sql` → creates the database and tables
   - `SeedData.sql` → populates initial users and hardware data

---

## Authentication

Use the pre-seeded admin account or register a new user:

- **Email:** admin@example.com  
- **Password:** admin  
- **Role:** Admin  

---

## Application Features

### User Features

- **Inventory Dashboard**  
  View a real-time list of all company hardware.

- **Claim / Release Devices**  
  Employees can assign available devices to themselves or release them back to the pool.

- **AI Specification Generation**  
  Generate concise technical descriptions for hardware using **Google Gemini 3 Flash**.

- **Weighted Search Engine**  
  Custom ranking system based on field importance.

---

### Admin Capabilities

- **Global Oversight**  
  Edit or delete any hardware entry.

- **User Management**  
  Promote users to Admin via the **System Access Control** panel.

---

## Tech Stack

- **Backend:** .NET 10 Web API with JWT Authentication  
- **Frontend:** Angular 2.0+
- **Database:** MS Sql 
- **AI Integration:** Google Gemini 3 Flash   
- **Testing:** xUnit  

---

## Search Logic (Bonus Feature)

The search functionality uses a custom **non-AI, token-based ranking algorithm**:

- **Normalization**  
  Converts queries to lowercase and removes punctuation.

- **Tokenization**  
  Splits queries into individual tokens for flexible matching.

- **Weighted Ranking**
  - Name → **10 points**
  - Manufacturer → **5 points**
  - Processor → **3 points**
  - RAM → **1 point**

- **Sorting**  
  Results are sorted by score, with a deterministic tie-breaker using device name.

---

## Notes

- Ensure SQL Server is running before starting the application.
- Backend must start before the frontend for proper API communication.
