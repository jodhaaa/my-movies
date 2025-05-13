# My Movies

## 1. Archtecture 
![alt text](https://github.com/jodhaaa/my-movies/blob/main/architecture.jpg?raw=true)

### Sync Function
Fetch movie data from 3rd party APIs, validate, and save to the data store. 
##### Tech Stack - Azure Function, Dapper,  Microsoft.Extensions.Http.Resilience (Polly)

### Web Api
Public api to fetch data from the data store
##### Tech Stack - Minimal API, Dapper

### Data Store
Store the movie data for api access.
This will support data filtering and pagination.
##### Tech Stack - NoSQL preferred,  but using SQLite for demo purposes

### Public Web
ReactJs webapp for public access
##### Tech Stack - Reactjs, RTK, RTK Query


## 2. Other Solutions 
Remove the data store and fetch data from 3rd party APIs on the go. 
We can improve the performance of api with Polly and api data caching.
##### Issues need to be addressed -  
* Handle a large amount of data.
* Pagination and data aggregation
