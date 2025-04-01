#   Coffee Machine Application - Technical Documentation By Waref HENI for UserCube Test

##   1. Introduction

    This document provides a technical overview of the Coffee Machine application, a full-stack solution designed to control and monitor a smart coffee machine.The application consists of a backend REST API (built with ASP.NET Core) and a frontend UI (built with Angular).

    The application allows users to:
    
    * Display current state (Off, Idle, Active, Alert)
    * Display current alerts
    * Allow users to carry out actions (turn on/off, make coffee).
    * Show Utilisation (first/last cup times, average cups per hour).

    This documentation outlines the architecture, components, technologies, and key design decisions of the application.

##   2. Backend Architecture (CoffeeMachine)

    The backend is structured using a layered architecture, promoting separation of concerns and maintainability. It follows the Model-View-Controller (MVC) pattern, as initially suggested, to organize the codebase. 
    It's organized into several components, which can be mapped to the MVC pattern as follows:

**Model**

CoffeeMachine.Models: Contains entity classes representing data models, mapped to the database, fitting the Model's role of managing data.
CoffeeMachine.Services: Implements business logic, like logging actions and managing machine operations, which is part of the Model.
CoffeeMachine.Data: Handles data access, including database interactions and seeding, also part of the Model.

**View**

The Front End project : **CoffeeMachine-ui**

**Controller**

CoffeeMachine.WebAPI: Includes controllers : CoffeeMachineController and UtilizationController, which handle HTTP requests, interact with services (Model), and return responses, fitting the Controller role to the **View**.

###   2.1 Project Structure and Responsibilities

    The backend solution comprises the following projects:

    * **CoffeeMachine.Core:**
        * Contains Data Transfer Objects (DTOs) for communication between layers.
        * Defines constant strings for logging and other shared resources.
        * Specifies interfaces for services (e.g., `ICoffeeMachineService`, `IUtilizationService`).
        
    * **CoffeeMachine.Models:**
        * Defines the entity classes that represent the data models in the application. These entities are mapped to the database.
        
    * **CoffeeMachine.Data:**
        * Encapsulates data access logic.
        * Includes the `CoffeeMachineContext` (Entity Framework Core's DbContext) for database interactions.
        * Provides the database context factory, database settings, and Entity Framework Core migrations.
        * Contains the `InitDemoData()` method to seed the database with 100 records for testing and demonstration purposes.
        
    * **CoffeeMachine.Mappings:**
        * Contains the `MappingProfile` class, which uses AutoMapper to map between DTOs (in `CoffeeMachine.Core`) and entities (in `CoffeeMachine.Models`).
        
    * **CoffeeMachine.Services:**
        * Implements the service interfaces defined in `CoffeeMachine.Core`.
        * Includes the following services:
            * `CoffeeActionLogService`: Handles logging of coffee machine actions to the database.
            * `CoffeeMachineService`: Orchestrates coffee machine operations, interacting with a simulated machine (`CoffeeMachineStub`) and updating the machine's state. It also implements a semaphore to prevent concurrent coffee making requests.
            * `UtilizationService`: Retrieves usage data from the database to calculate statistics.
            
    * **CoffeeMachine.Tests:**
        * Contains unit tests. Currently, only `UtilizationServiceTests.cs` is implemented due to time constraints. The tests use Moq for mocking dependencies.
        
    * **CoffeeMachine.WebAPI:**
        * Exposes the backend API through RESTful endpoints.
        * Contains the `CoffeeMachineController` (for machine control) and `UtilizationController` (for statistics).
        * Includes `ErrorHandlingMiddleware` to manage exceptions and provide consistent error responses.

###   2.2 API Documentation

    The `CoffeeMachine.WebAPI` project provides the following API endpoints (Swagger documentation is available for detailed information):

    ####   2.2.1 CoffeeMachineController

    * **`GET /api/CoffeeMachine/state`**:
        * Purpose: Retrieves the current state of the coffee machine.
        * HTTP Method: GET
        * Response:
            * Status Code: 200 (OK)
            * Body: An object containing the machine's state (isOn, isMakingCoffee, waterLevelState, beanFeedState, wasteCoffeeState, waterTrayState).
            * Status Code: 500 (Internal Server Error)
            * Body: "Internal Server Error"
    * **`POST /api/CoffeeMachine/turnon`**:
        * Purpose: Turns on the coffee machine.
        * HTTP Method: POST
        * Response:
            * Status Code: 200 (OK)
            * Body: `{ "message": "Machine turned on." }`
            * Status Code: 400 (Bad Request)
            * Body: Error message (e.g., "Machine is already on.")
            * Status Code: 500 (Internal Server Error)
            * Body: "Internal Server Error"
    * **`POST /api/CoffeeMachine/turnoff`**:
        * Purpose: Turns off the coffee machine.
        * HTTP Method: POST
        * Response:
            * Status Code: 200 (OK)
            * Body: `{ "message": "Machine turned off." }`
            * Status Code: 400 (Bad Request)
            * Body: Error message (e.g., "Machine is already off.")
            * Status Code: 500 (Internal Server Error)
            * Body: "Internal Server Error"
    * **`POST /api/CoffeeMachine/makecoffee`**:
        * Purpose: Initiates the coffee making process.
        * HTTP Method: POST
        * Request Parameters:
            * Body: `CoffeeCreationOptionsDto` (e.g., number of espresso shots, add milk).
        * Response:
            * Status Code: 200 (OK)
            * Body: `{ "message": "Coffee making started." }`
            * Status Code: 400 (Bad Request)
            * Body: Error message (e.g., "Invalid state.")
            * Status Code: 500 (Internal Server Error)
            * Body: "Internal Server Error"

    ####   2.2.2 UtilizationController

    * **`GET /api/Utilization/firstLastCupTimes`**:
        * Purpose: Gets the first and last cup times for each day of the week.
        * HTTP Method: GET
        * Response:
            * Status Code: 200 (OK)
            * Body: A dictionary where keys are days of the week and values are strings representing the first and last cup times.
    * **`GET /api/Utilization/averageCupsPerHour`**:
        * Purpose: Gets the average number of cups made per hour.
        * HTTP Method: GET
        * Response:
            * Status Code: 200 (OK)
            * Body: A dictionary where keys are hours and values are the average number of cups made during that hour.

###   2.3 Data Access

    * **Database Technology:** SQLite
    * **ORM:** Entity Framework Core
    * The `CoffeeMachineContext` handles database interactions.
    * Migrations are used to manage database schema changes.
    * The `InitDemoData()` method within the `CoffeeMachineContext` can be used to populate the database with initial data (100 records). This is useful for testing and demonstration. To use it, call this method when the application starts (e.g., in `Program.cs` after ensuring the database is created).

**N.B** use: dotnet ef migrations to create the database and edit the connection string in CoffeeMachine.Data\dbsettings.Development.json dbsettings.Development.json
###   2.4 Error Handling

    * The backend employs a combination of exception handling and HTTP status codes for error management.
    * Specific exceptions like `InvalidOperationException` are thrown to indicate business rule violations (e.g., attempting to turn on an already-on machine).
    * HTTP status codes are used to convey the outcome of API requests:
        * 200 (OK): Successful request.
        * 400 (Bad Request): Client-side error (e.g., invalid input).
        * 500 (Internal Server Error): Server-side error.
    * The `ErrorHandlingMiddleware` centralizes exception handling, providing consistent error responses to the client.
    * Seq is used for logging errors and other important events. The `ILogger` interface is used for logging, and Seq is configured to collect these logs. Example: `_logger.LogError(ex, "Error making coffee.");`

###   2.5 Seq Integration

    * The application uses Serilog (and related Seq sink packages) for structured logging.
    * Logs are sent to a Seq server, allowing for centralized logging, filtering, and analysis.
    * To set up Seq monitoring:
        1.  Install Seq on a server.
        2.  Configure the Serilog sink to point to the Seq server's URL.
        3.  The application will then send log events to Seq.
        4.  Access the Seq web interface to view and analyze the logs.

###   2.6 MVC Architecture

    The backend architecture is designed to align with the Model-View-Controller (MVC) pattern, promoting a clear separation of concerns:

    * **Model:** The Model is responsible for managing the application's data and business logic. In this project, the Model is represented by:
        * `CoffeeMachine.Models`: This project contains the entity classes that define the data structures and how they are mapped to the database.
        * `CoffeeMachine.Services`: This project encapsulates the application's business logic, including operations on the coffee machine and data processing. It acts as a core part of the Model, defining how data is handled and manipulated.
        * `CoffeeMachine.Data`: This project handles all data access concerns, including database interactions, data retrieval, and storage. It is responsible for the persistence of the application's data.
    * **View:** The View is represented via the Angualr coffee-machine-ui project .
        * In this application, the primary View is the frontend (built with Angular), which displays the coffee machine's state, provides user controls, and presents usage statistics.
        * Additionally, the Web API endpoints (within `CoffeeMachine.WebAPI`) can also be considered a form of View, as they present data in a structured format (JSON) for consumption by other applications or the frontend. 
    * **Controller:** The Controller handles user input and mediates between the Model and the View.
        * `CoffeeMachine.WebAPI`: This project contains the controllers (e.g., `CoffeeMachineController`, `UtilizationController`) that receive HTTP requests from the client (frontend or other applications), process these requests by interacting with the services (Model) to retrieve or modify data, and then return the appropriate response to the client.

    This MVC structure helps to organize the codebase, making it more modular, maintainable, and easier to understand.

###   2.7 Development Process and Prioritization

    The backend development process was structured as follows:

    1.  **Database Design:** Initial focus was placed on designing a robust and extensible database architecture to accommodate future requirements. This included:
        * Creating an `Actions` table to store a history of actions performed on the coffee machine.
        * Creating an `ActionTypes` table (populated via database context seeding) to define and manage different action types, allowing for extensibility and the ability to enable/disable actions.
        * Creating a `CoffeeCreationOptions` table to store "recipe" information, detailing the consumption of resources (beans, milk) for each coffee type.
        * Establishing foreign key relationships between these tables and the `CoffeeActionLogs` table to ensure data integrity and facilitate efficient querying.
    2.  **Entity and DTO Creation:** Entities were created to reflect the designed database schema, followed by the creation of corresponding DTOs for data transfer.
    3.  **DbContext and Mapping Configuration:** The `DbContext` was implemented to interact with the database, and AutoMapper was configured to map between entities and DTOs.
    4.  **Test-Driven Development (TDD):** Development began with a TDD approach by creating `UtilizationServiceTests` to guide the implementation of the `UtilizationService`. Due to time constraints, this rigorous TDD approach was not applied to all services.
    5.  **Web API Development:** The `CoffeeMachine.WebAPI` project and its controllers were implemented to expose the backend functionality.
    6.  **Service Development:** The remaining services were developed, with testing performed using Swagger and Postman.
    7.  **Concurrency Control:** A semaphore was implemented in the `MakeCoffeeAsync` method to prevent concurrent coffee-making requests and ensure data consistency.

###   2.8 Completion Roadmap

    To fully complete the backend and enhance its robustness and maintainability, the following steps should be prioritized:

    1.  **Complete Unit Test Coverage:** Implement comprehensive unit tests for all services to ensure code quality, prevent regressions, and facilitate refactoring. This is the highest priority.
    2.  **Robust Database Architecture Implementation:** Fully implement the architecture, including:
        * Design Pattern Factory for making coffess.
    3.  **Implement Data Validation:** Add data validation to both DTOs and entities to prevent invalid data from entering the system.
    4.  **Enhance Error Handling:** Refine the error handling strategy to provide more informative error messages to the client and improve logging for debugging purposes.
    5.  **Performance Optimization:** Analyze and optimize database queries and add non clustred indexes (already we based tables in indexes) and service logic to improve performance, especially for data-intensive operations.
    7.  **Security Implementation:** Implement authentication and authorization to secure the API endpoints and protect sensitive data.

##   3. Frontend Architecture (CoffeeMachine-ui)

    The frontend is built using Angular and follows a component-based architecture.

###   3.1 Technology Stack

    * Angular CLI: 19.2.5 (Standalone Components)
    * Other significant libraries:
        * ngx-toastr: For displaying user notifications.
        * rxjs      : For subscription and toastr notification

###   3.2 Component Structure

    * **AppComponent:**
        * Serves as the main application container.
        * Defines the application's navigation using `<router-outlet>` and `<routerLink>`:

            ```html
            <h1>
                Coffee Machine
            </h1>
            <nav>
              <a routerLink="/coffee-machine" routerLinkActive="active">
                Coffee Machine
              </a>
              |
              <a routerLink="/statistics" routerLinkActive="active">
                Statistics
              </a>
            </nav>
            <router-outlet></router-outlet>
            ```

        * Loads the `CoffeeMachineComponent` and `StatisticsComponent` based on the route.
    * **CoffeeMachineComponent:**
        * Provides the user interface for controlling the coffee machine.
        * Allows users to turn the machine on/off, make coffee, and view the machine's status and alerts.
    * **StatisticsComponent:**
        * Displays usage statistics retrieved from the backend.
        * Shows information such as first/last cup times and average cups per hour.

###   3.3 Communication

    * The frontend communicates with the backend API using HTTP requests.
    * Angular's `HttpClient` module is used to make these requests.
    * Services (`CoffeeMachineService` and `UtilizationService`) encapsulate the HTTP communication logic for specific API controllers.

##   4. Build and Deployment

    * **Backend:**
        * The backend can be built using the .NET Core SDK.
        * Deployment typically involves publishing the application and hosting it on a web server (e.g., IIS, Kestrel).
    * **Frontend:**
        * The frontend can be built using the Angular CLI: `ng build`.
        * The output of the build process (static files) can be deployed to a web server or a static hosting service.