The RaceDay system is a web-based event management system designed for the South African road running, walking, and cycling community. The system provides a platform where event organisers can create and manage events, define event categories, manage participant enrolments, and record race results. Participants can browse available events, enrol in events by selecting a category, and view their personal results and performance history.

An Organiser is responsible for managing RaceDay events. Organisers can create, update, and delete events, define age or distance categories for events, view participant enrolments for their events, and capture participants' finishing times and positions.

A Participant uses RaceDay to browse available events, view event categories, enrol in events by selecting a category, and view their own race results and performance information.

<img width="1901" height="571" alt="image" src="https://github.com/user-attachments/assets/ad30a201-2f29-4e51-8a12-f8c01d6d2eea" />


PART 2 

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









