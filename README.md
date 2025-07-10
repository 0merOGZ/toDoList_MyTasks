# MyTasks - Team and Personal Task Management Application

This project is a modern ASP.NET Core MVC application designed for both individual and team-based task management. Users can add tasks, categorize them, track their status, and assign tasks to team members.

## Features

- **User Management:** Sign up, log in, log out.
- **Role System:** Team Leader and Team Member roles.
- **Team Management:** Create a team (leader), join a team via invitation code (member).
- **Task Management:**
  - Add, edit, delete tasks.
  - Filter and sort tasks by category, urgency, and status.
  - Team leaders can assign tasks to team members.
  - Archive completed tasks.
- **Categories:** Personal, work, school, relationship, health, finance, other.
- **Statuses:** Pending, completed.
- **Modern and responsive UI:** Stylish design with Bootstrap and custom CSS.

## Setup and Run

### Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- SQL Server (or LocalDB)
- Visual Studio 2022+ or VS Code

### Steps

1. **Clone the project:**
   ```sh
   git clone <repo-url>
   cd tod
   ```

2. **Restore dependencies:**
   - NuGet packages will be restored automatically. If needed:
     ```sh
     dotnet restore
     ```

3. **Configure the database connection:**
   - Update the `ConnectionStrings:Context` section in `appsettings.json` to match your SQL Server setup.
   - Default:  
     "Context": "Server=(localdb)\\mssqllocaldb;Database=tod;Trusted_Connection=True;MultipleActiveResultSets=true"

4. **Apply database migrations:**
   ```sh
   dotnet ef database update
   ```

5. **Run the project:**
   ```sh
   dotnet run
   ```
   or press F5 in Visual Studio

6. **Access the application:**
   - [http://localhost:5244](http://localhost:5244) or [https://localhost:7140](https://localhost:7140)

## Usage

- **Sign Up:** Register by selecting a role (Team Leader or Member).
- **Team Leader:** After registration, a team is created automatically. Members can join using the invitation code.
- **Team Member:** After registration, you can join a team using an invitation code.
- **Add Task:** Add your own tasks or, as a leader, assign tasks to team members.
- **Filtering & Sorting:** Filter tasks by category, status, date, and urgency.
- **Archive:** Archive completed tasks.

## Project Structure

- `Controllers/` - MVC Controllers
- `Models/` - Entity and data models
- `Views/` - Razor View files (UI pages)
- `Migrations/` - EF Core database migrations
- `wwwroot/` - Static files (CSS, JS, images)

## Dependencies

- ASP.NET Core MVC
- Entity Framework Core (SQL Server)
- Bootstrap 5
- jQuery

## Contribution & License

This project is an internship prototype. You are welcome to fork and submit pull requests. 