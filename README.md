The RaceDay system is a web-based event management system designed for the South African road running, walking, and cycling community. The system provides a platform where event organisers can create and manage events, define event categories, manage participant enrolments, and record race results. Participants can browse available events, enrol in events by selecting a category, and view their personal results and performance history.

An Organiser is responsible for managing RaceDay events. Organisers can create, update, and delete events, define age or distance categories for events, view participant enrolments for their events, and capture participants' finishing times and positions.

A Participant uses RaceDay to browse available events, view event categories, enrol in events by selecting a category, and view their own race results and performance information.

<img width="1901" height="571" alt="image" src="https://github.com/user-attachments/assets/ad30a201-2f29-4e51-8a12-f8c01d6d2eea" />


## PART 2 

CHANGELOG:
Entity Framework Core: Integrated Entity Framework Core with SQL Server and scaffolded the existing RaceDay database into the API project.
Database integration: Configured the database connection string and registered RaceDayContext for dependency injection.
Authentication: Implemented JWT-based authentication for registration and login, including token expiry and claims.
Password security: Implemented password hashing to avoid storing user passwords in plain text.
Authorization: Added role-based and authenticated access controls for protected API endpoints.
User profiles: Developed or prepared authenticated user profile functionality, subject to endpoint verification.
Event management: Developed or prepared API functionality for managing events and event types, subject to endpoint verification.
Participant functionality: Developed or prepared functionality for participant enrolments and race results, subject to endpoint verification.
API documentation: Prepared Swagger/OpenAPI integration for inspecting and testing API endpoints, subject to verification.
Continuous integration: Updated the GitHub Actions workflow to reference the solution at docs/RaceDay/RaceDay.slnx, resolving the project-path error.
<img width="1919" height="840" alt="Screenshot 2026-10-20 142323" src="https://github.com/user-attachments/assets/a1474d1a-0e6b-4fc8-92a3-4505bb088bd9" />

TESTS:
Register: <img width="1919" height="930" alt="image" src="https://github.com/user-attachments/assets/bd86d863-ca3d-4363-b340-cbb58fa47bdf" />
<img width="1442" height="748" alt="image" src="https://github.com/user-attachments/assets/2e9850ea-b53d-41d8-a344-228433128e17" />

Login: <img width="1459" height="658" alt="image" src="https://github.com/user-attachments/assets/4aa59842-0263-47cf-b971-c96035ddd488" />
<img width="1446" height="802" alt="image" src="https://github.com/user-attachments/assets/4a95e73d-8a0c-4dbd-b35b-bd4e7bf3eb9a" />

Access without a JWT: <img width="1446" height="852" alt="image" src="https://github.com/user-attachments/assets/a290008e-5729-46ae-b8af-d76cdc4fb93b" />

Access with a valid JWT: <img width="663" height="304" alt="image" src="https://github.com/user-attachments/assets/6639b9fc-8f1d-444c-a198-3444b6d522e6" />
<img width="1684" height="790" alt="image" src="https://github.com/user-attachments/assets/d3f6aedd-5494-433f-93a2-ca6a6956c2ca" />

Events: <img width="1480" height="860" alt="image" src="https://github.com/user-attachments/assets/bb0b0093-07b7-4e43-b5e7-1485b0b76cd5" />
Event types: <img width="1448" height="839" alt="image" src="https://github.com/user-attachments/assets/8f674a0f-dcee-406b-b62a-458883bc519b" />
Get event from ID: <img width="1428" height="987" alt="image" src="https://github.com/user-attachments/assets/965be883-2ee8-4f12-a844-52956b5defb9" />
Get events containing specific id: <img width="1433" height="976" alt="image" src="https://github.com/user-attachments/assets/1d390b4d-a369-4e00-ab40-0a31e4eea9bc" />


Categories: <img width="1434" height="971" alt="image" src="https://github.com/user-attachments/assets/120837f3-2815-4f83-94a3-8667c957f3a0" />
Get event from category id: <img width="1436" height="932" alt="image" src="https://github.com/user-attachments/assets/d6529e38-0ee3-4d1b-8f84-ed0238015026" />

Enrolement:
Enrol a participant to an event from category and event id's: <img width="1434" height="779" alt="image" src="https://github.com/user-attachments/assets/d5ed069e-87ae-4eb9-988c-bb423214dd7b" />

## PART 3

Part 3 extends the existing RaceDay Web API by introducing an ASP.NET Core MVC frontend, role-based user experiences, image storage integration, and containerisation using Docker.

1. Project Overview

RaceDay provides two main user roles:

Participant

Participants can:
Register and log in.
Browse available race events.
View event information and available categories.
Enrol in events.
View their enrolments.
Cancel eligible enrolments.
View published race results.
Manage their profile and profile picture.

Homepage
<img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/d0b5c8ad-39fa-43f2-a850-88a121bf7992" />

Events page
<img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/8305bd82-3051-44bc-b8fc-0a13e416cd15" />

Organiser

Organisers can:
Log in using an Organiser account.
Access an organiser dashboard.
Create and manage their own events.
Create and manage event categories.
View participants enrolled in their events.
Record and update race results.
Upload event banner images.


2. Features

User authentication - Registration and login
JWT authentication - API access secured using JSON Web Tokens
Session management - MVC stores the authenticated user's session and JWT
Role-based access - Participant and Organiser functionality
Event management - Create, view and manage events
Category management - Define event categories
Enrolment management - Register and manage event participation
Results management - Record and view race results
Profile management - Manage participant profile information
Image uploads - Event banners and profile pictures
Local blob storage - Azure Blob Storage development using Azurite
Responsive frontend - Bootstrap-based MVC interface
API documentation - Swagger/OpenAPI
Containerisation - Docker and Docker Compose

<img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/33fba4d7-5136-4896-ac9a-9b9b747ad4dd" />

<img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/6ccb2ac3-b4d8-4820-9241-8d91d9917533" />


<img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/32b5e869-33af-449f-b2de-f9826997d903" />

<img width="1919" height="706" alt="image" src="https://github.com/user-attachments/assets/a10d1e09-8e94-46ec-a438-7a350218ba28" />

Logged in
<img width="1919" height="695" alt="image" src="https://github.com/user-attachments/assets/ef152217-fb6f-4fdd-9d1c-146c1ed49c4d" />




3. Technology Stack

C# - Application programming language
ASP.NET Core Web API - Backend REST API
ASP.NET Core MVC - Frontend web application
.NET 10 - Application framework
Entity Framework Core - Database access in the API
Microsoft SQL Server - Relational database
JWT - API authentication
ASP.NET Core Session - MVC authentication state
Bootstrap - Responsive user interface
HTML, CSS and JavaScript - Frontend development
Azure Blob Storage SDK - Blob storage integration
Azurite - Local Azure Storage emulator
Docker - Application containerisation
API documentation and testing - GitHub


4. System Architecture

MVC frontend
Displays the user interface.
Manages the user's session.
Sends HTTP requests to the API.
Passes the JWT token when calling protected endpoints.
Displays API responses.

Web API
Processes business logic.
Validates requests.
Enforces authentication and role-based authorisation.
Communicates with SQL Server.
Handles image storage operations.

SQL Server
Stores users, roles, events, categories, enrolments and results.

Azurite
Provides local Azure Storage-compatible blob functionality.
Simulates Azure Blob Storage during development.
Stores uploaded event banners and profile pictures in local development storage.


5. Running the Project Locally

Open the solution
docs/RaceDay/RaceDay.slnx

Configure the API
Ensure the API has a valid SQL Server connection string and the required JWT configuration.

Configure the MVC application
Ensure the MVC application's API base URL points to the running API.
The configuration key is: RaceDayApi:BaseUrl

Start the API
From the repository root: dotnet run --project .\docs\RaceDay\RaceDay.Api\RaceDay.Api.csproj

Start the MVC application
Open a second terminal:
dotnet run --project .\docs\RaceDay\RaceDay.Mvc\RaceDay.Mvc.csproj
Open the MVC URL displayed in the terminal.


6. Running with Docker

Step 1: Start Docker Desktop
Ensure Docker Desktop is running.

Step 2: Navigate to the repository root
cd <YOUR-REPOSITORY-FOLDER>

Step 3: Check the Docker Compose configuration
Ensure the repository contains: docker-compose.yml
Also confirm that the Dockerfiles referenced by the Compose file exist.

Step 4: Build and start the containers
docker compose up --build
To run the containers in the background: docker compose up --build -d

Step 5: Check running containers
docker compose ps

Step 6: View container logs
View all service logs: docker compose logs
View logs for a specific service: docker compose logs -f raceday-api
Replace raceday-api with the actual service name in docker-compose.yml.

Step 7: Access the application
Use the host ports configured in docker-compose.yml.
For example, if the MVC service maps host port 8080 to its container port: http://localhost:1234
Use the actual port mapping in your Compose configuration.

Step 8: Stop the containers
docker compose down
To stop the containers and remove their associated Compose-managed volumes: docker compose down -v








