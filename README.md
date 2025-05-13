# **My Movies: A Scalable Movie Comparison Platform**  

## **1. Architecture Overview**  
![Architecture Diagram](https://github.com/jodhaaa/my-movies/blob/main/architecture.jpg?raw=true)  

### **Sync Function**  
Handles fetching, validating, and storing movie data from third-party APIs.  
**Tech Stack:**  
- **Azure Function** (Serverless execution)  
- **Dapper** (Lightweight ORM)  
- **Microsoft.Extensions.Http.Resilience (Polly)** (Resilience & retry mechanism)  

### **Web API**  
Public API that serves movie data from the data store, ensuring fast and reliable access.  
**Tech Stack:**  
- **Minimal API** (Lightweight, efficient web framework)  
- **Dapper** (Optimized database queries)  

### **Data Store**  
Stores movie details for efficient filtering, pagination, and fast access.  
**Tech Stack:**  
- **NoSQL (Preferred for scalability)**  
- **SQLite (Used for demonstration purposes)**  

### **Public Web**  
A sleek and responsive frontend application that enables users to compare movie prices effortlessly.  
**Tech Stack:**  
- **React.js** (Modern frontend framework)  
- **RTK & RTK Query** (State management and API caching)  

## **2. Local Installation**  
 
### Make sure Azurite is up and running 
[Running Azurite from the command line](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio%2Cblob-storage#running-azurite-from-the-command-line)

### Setup API Keys in Manage User Secrets for MyMovie.SyncFunction Project
```
{
  "ApiKey": "sjdxxxxxxxx"
}
```
[Manage User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows#use-visual-studio)

## **3. Alternative Approach: Why It Wasn't Chosen**  

### **Real-Time API Fetching**  
An alternative method where movie details are fetched dynamically on demand instead of using a persistent data store.

#### **Optimizations:**
- Implement Polly for intelligent request retries and failure handling.
- Leverage API data caching to reduce redundant network calls and enhance responsiveness.

#### **Challenges to Address:**
- Handling large data volumes efficiently
- Implementing robust pagination and data aggregation
