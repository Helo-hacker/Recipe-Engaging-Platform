# Recipe Sharing Platform

A modern web application for discovering, sharing, and managing recipes. Built with ASP.NET Core MVC, Entity Framework Core, and a stylish, user-friendly interface.

## Features

- 🔍 **Advanced Recipe Search:**  
	Search recipes by name, ingredients, meal type, diet preference, and cook time.

- 📝 **Recipe Management:**  
	Add, edit, and delete your own recipes with detailed instructions and ingredients.

- ❤️ **Favorites & Likes:**  
	Like recipes and save your favorites for quick access.

- 💬 **Comments & Ratings:**  
	Engage with the community by commenting on and rating recipes.

- 📅 **Meal Planning:**  
	Create meal plans and organize recipes for the week.

- 👤 **User Profiles:**  
	Manage your profile, view your activity, and see your shared recipes.

- 🔒 **Authentication & Authorization:**  
	Secure login, registration, and role-based access.

## Tech Stack

- **Backend:** ASP.NET Core MVC, Entity Framework Core
- **Frontend:** Razor Views, Bootstrap, custom CSS/JS
- **Database:** SQL Server (via EF Core)
- **Authentication:** ASP.NET Identity

## Getting Started

### Prerequisites

- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup

1. **Clone the repository:**
	 ```
	 https://github.com/Helo-hacker/Recipe-Engaging-Platform.git
	 cd RecipeSharingApp
	 ```

2. **Configure the database:**
	 - Update the connection string in `appsettings.json`.

3. **Apply migrations:**
	 ```
	 dotnet ef database update
	 ```

4. **Run the application:**
	 ```
	 dotnet run
	 ```

5. **Open in browser:**  
	 Visit `https://localhost:5001` (or the port shown in your terminal).

## Folder Structure

- `Controllers/` — MVC controllers for handling requests
- `Models/` — Entity and view models
- `Views/` — Razor views for UI
- `Data/` — Database context and migrations
- `Services/` — Business logic and helpers
- `wwwroot/` — Static files (CSS, JS, images)

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](LICENSE)

---

**Made with ❤️ for foodies and home chefs!**
