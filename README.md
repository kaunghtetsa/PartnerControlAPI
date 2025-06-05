# Partner Control API

A .NET Web API project for managing partner discounts and control settings.

## Project Description

This API provides functionality to manage discount settings for different partner types, including:
- Base discount settings
- Prime number-based discounts
- Special discounts for amounts ending with 5

## Features

- Discount management based on different types (Base, PrimeNumber, EndsWith5)
- Amount range-based discount settings (Min and Max amounts)
- Percentage-based discount calculations
- Active/Inactive status tracking
- Creation and update timestamp tracking

## Technical Stack

- ASP.NET Core Web API
- Entity Framework Core
- C# 
- SQL Server (Database)

## Project Structure

The project includes:
- Models for discount settings and configurations
- API Controllers for managing discount rules
- Data validation using Data Annotations
- DateTime tracking for created and updated records

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022 or VS Code
- SQL Server (Local or Express)

### Setup Instructions

1. Clone the repository:
```bash
git clone https://github.com/kaunghtetsa/PartnerControlAPI.git
```

2. Navigate to the project directory:
```bash
cd PartnerControlAPI
```

3. Restore NuGet packages:
```bash
dotnet restore
```

4. Run the application:
```bash
dotnet run
```

## API Documentation

The API includes a single endpoint for managing discount settings.

## Contributing

1. Create a new branch for your feature
2. Make your changes
3. Submit a pull request
