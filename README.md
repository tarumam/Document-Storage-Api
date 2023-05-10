# Document Storage API

This is a simple document storage API that allows users to log in, upload and download documents with metadata (posted date, name, description, and category), create groups, and manage users. Users can belong to one or more groups, and document access can be granted to groups or directly to users. The API includes at least three roles:

- Regular user: Can download documents
- Manager user: Can upload and download documents
- Admin: Can CRUD users and groups, upload and download documents

The API is built using REST principles, allowing users to perform all actions via the API, including authentication.

## Technical Details

The project was built using .NET 6 and PostgreSQL 15. The following user accounts are available for testing:

- Admin account:
    - Username: Admin
    - Password: string
- Regular user account:
    - Username: User
    - Password: string
- Manager user account:
    - Username: Manager
    - Password: string

To run the tests, Docker is required. 

## How to Use

1. Clone the repository to your local machine.
2. Set up the database connection in `appsettings.json`.
3. Build the API
4. Sign Up with a user provided above to have access to private endpoints.

## Known Gaps
Implementation of mediator pattern for better separation of concerns
CQRS
Event Sourcing
Caching
