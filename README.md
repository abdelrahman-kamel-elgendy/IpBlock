## IpBlock – .NET 8 Web API
IpBlock is a .NET 8 Web API that manages blocked countries and validates IP addresses using third-party geolocation APIs such as ipapi.co or ipgeolocation.io.
It stores all data in in-memory collections (no database) and provides endpoints for country blocking, IP lookup, and logging of blocked attempts.

## Features
### IP Features
- Lookup IP Country Info
```
GET /api/ip/lookup?ipAddress={ip}
```
Fetches country details (code, name, ISP) from the geolocation API.
Automatically detects the caller IP if not provided.

- Check if IP is Blocked
```
GET /api/ip/check-block
```
Determines if the caller’s country is in the blocked list.
Logs every request with timestamp, country code, and User-Agent.

### Country Management
- Block a Country
```
POST /api/countries/block
```
```
Body:
{ "countryCode": "US" }
```
Adds to the in-memory blocked list (no duplicates).

- Unblock a Country
```
DELETE /api/countries/block/{countryCode}
```

- Get All Blocked Countries
```
GET /api/countries/blocked?page=1&pageSize=10
```
Supports pagination and filtering by country code or name.

- Temporarily Block a Country
```
POST /api/countries/temporal-block
```
```
Request:
{
  "countryCode": "EG",
  "durationMinutes": 120
}
```
Automatically unblocks after the duration expires using a background service that runs every 5 minutes.

### Logs
- Retrieve Blocked Attempts
```
GET /api/logs/blocked-attempts?page=1&pageSize=10
```
Each log entry contains:
IP address
Timestamp
Country code
User-Agent
Blocked status

## Technologies Used
- .NET 8 Web API: Backend framework.
- HttpClientFactory: calling third-party geolocation APIs.
- ConcurrentDictionary / ConcurrentBag: Thread-safe in-memory data.
- Swagger / OpenAPI	API: documentation and testing.
- BackgroundService: Automatically cleans expired temporal blocks.
- Custom Middleware: Centralized exception handling via AppException.

## Getting Started
1- Clone the Repository
- git clone https://github.com/<abdelrahman-kamel-elgendy>/IpBlock.git
- cd IpBlock

2️- Configure API Settings
- Edit appsettings.json:
```
"IpApi": {
  "BaseUrl": "https://ipapi.co",
  "ApiKey": "your_api_key_here"
}
```

For ipgeolocation.io, replace the BaseUrl and include the key parameter if required.

3️- Run the Application
dotnet run

Navigate to Swagger UI:
https://localhost:5001/swagger

## Example Usage
🔹 Lookup an IP
GET /api/ip/lookup?ipAddress=8.8.8.8

🔹 Check if Caller’s IP is Blocked
GET /api/ip/check-block

🔹 Add a Blocked Country
POST /api/countries/block
Content-Type: application/json
```
{
  "countryCode": "US",
  "countryName": "United States"
}
```
🔹 Retrieve Block Logs
GET /api/logs/blocked-attempts?page=1&pageSize=10

## Architecture Overview
```
IpBlock/
 ┣ Controllers/
 ┃ ┣ IpController.cs
 ┃ ┣ CountriesController.cs
 ┃ ┗ LogsController.cs
 ┣ Services/
 ┃ ┣ IIpApiService.cs
 ┃ ┣ IpApiService.cs
 ┃ ┣ ICountryService.cs
 ┃ ┣ CountryService.cs
 ┃ ┣ ILogService.cs
 ┃ ┗ LogService.cs
 ┣ Repositories/
 ┃ ┣ ICountryRepository.cs
 ┃ ┣ CountryRepository.cs
 ┃ ┣ ILogRepository.cs
 ┃ ┗ LogRepository.cs
 ┣ Models/
 ┃ ┣ DTOs/
 ┃ ┃ ┣ Request/
 ┃ ┃ ┃ ┣ BlockCountryRequest.cs
 ┃ ┃ ┃ ┗ TemporalBlockRequest.cs
 ┃ ┃ ┗ Response/
 ┃ ┃   ┣ IpLookupResponse.cs
 ┃ ┃   ┗ PagedResponse.cs
 ┃ ┣ BlockCountryEntity.cs
 ┃ ┗ BlockLogEntry.cs
 ┣ Exceptionse/
 ┃ ┗ AppException.cs
 ┣ Middleware/
 ┃ ┗ ExceptionHandlingMiddleware.cs
 ┣ Options/
 ┃ ┣ IpApiOptions.cs
 ┃ ┗ TemporalBlockCleanupOptions.cs
 ┗ Program.cs
 ```