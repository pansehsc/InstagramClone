# Account & Authentication API Documentation

### Purpose

The `AccountController` handles **user registration, login, forget password and reset password**. It validates user input, securely hashes passwords, stores users through the repository, and generates a JWT token for authentication.

---

## 1. Components

| Component           | Responsibility                         |
| ------------------- | -------------------------------------- |
| `AccountController` | Handles register/login requests        |
| `RegisterDto`       | Data received when creating an account |
| `LoginDto`          | Data received when logging in          |
| `UserDto`           | Safe user data returned to the client  |
| `ResetPasswordDto`  | Data received when setting a new password using a reset token |
| `ForgetPasswordDto` | Data received when requesting a password-reset email |
| `IUserRepository`   | User database operations               |
| `IMapper`           | Converts DTOs ↔ `User` entity          |
| `ITokenService`     | Generates JWT authentication tokens    |
| `IEmailService`     | Defines the service responsible for sending emails    |
| `EmailService`      | Implements IEmailService and sends emails through the configured email provider/SMTP server    |

---

## 2. Register API

**Endpoint:** `POST /api/account/register`

**Request: `RegisterDto`**

```json
{
  "userName": "john123",
  "email": "john@example.com",
  "password": "123456",
  "gender": "Male",
  "dateOfBirth": "2004-05-10",
  "country": "Egypt",
  "city": "Cairo"
}
```

### Process

1. Checks whether the email already exists.
2. Checks whether the username already exists.
3. Maps `RegisterDto` → `User`.
4. Hashes the password using `HMACSHA512` and stores the hash and salt.
5. Sets `LastActive`.
6. Saves the user through `IUserRepository`.
7. Maps `User` → `UserDto`.
8. Generates a JWT using `ITokenService`.
9. Returns the user information and token.

**Success:** `200 OK` + `UserDto`

**Possible errors:**

* `400 Bad Request`Dto`**

```json
{
  "email": "john@example.com",
  "password": "123456"
}
```

### Process

1. Finds the user by email.
2. Returns `401 Unauthorized` if the user does not exist.
3. Hashes the entered password using the stored salt.
4. Compares the generated hash with the stored password hash.
5. Returns `401 Unauthorized` if the password is incorrect.
6. Updates `LastActive`.
7. Generates a JWT.
8. Returns `UserDto`.

**Success:** `200 OK` + `UserDto`

---
 — email/username already exists or database save fails.
* `400 Bad Request` — validation failure.

---

## 3. Login API

**Endpoint:** `POST /api/account/login`

**Request: `Login
## 4. DTOs

### `RegisterDto`

Used for **client → API** registration data.

* `UserName` — required
* `Email` — required, valid email format
* `Password` — required, minimum 6 characters
* `Gender` — required
* `DateOfBirth`
* `Country` — required
* `City` — required

### `LoginDto`

Used for **client → API** login data.

* `Email` — required, valid email format
* `Password` — required

### `UserDto`

Used for **API → client** response.

* `Id`
* `UserName`
* `Email`
* `Token`
* `ProfilePictureUrl`

Sensitive properties such as `PasswordHash` and `PasswordSalt` are **not returned to the client**.

---