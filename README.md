# SMT Manager

Hi! 👋 This is my submission for the SMT Manager code challenge.


## 🛠️ Tech Stack

- **Backend:** .NET Web API (Clean Architecture)
- **Frontend:** Angular & Bootstrap 5
- **Database:** SQL Server
- **Testing:** NUnit & Moq

## 🚀 How to Run

The easiest way to run everything is with Docker:

```bash
docker compose up --build
```

Once it's up:
- **Web App:** [http://localhost:4200](http://localhost:4200)
- **API / Swagger:** [http://localhost:5000/swagger](http://localhost:5000/swagger)

## 🧪 Running Tests

If you want to run the backend tests:

```bash
dotnet test
```

## 📝 Notes

- I focused on clean architecture and separation of concerns.
- Authentication is implemented with JWT.
- You can log in with: `admin` / `admin123` (or register a new user).

---
*Created by [Adnan Jukic](https://github.com/nan9000)*
