# Account API

## Purpose

The **Account API** manages user authentication, registration, password recovery, and profile photos.

### Main Dependencies

* `IUserRepository` — retrieves and stores user data.
* `ITokenService` — creates JWT authentication tokens.
* `IEmailService` — sends password-reset emails.
* `IPhotoService` — uploads/deletes images using Cloudinary.
* `IPhotoRepository` — manages photo records in the database.
* `IMapper` — maps between DTOs and Entities.

---

## 1. Register

**Endpoint:** `POST /api/account/register`

### Purpose

Creates a new user account and returns a JWT token for authentication.

### Request Body

```json
{
  "userName": "john",
  "email": "john@example.com",
  "password": "123456",
  "gender": "Male",
  "dateOfBirth": "2003-05-10",
  "country": "Egypt",
  "city": "Cairo"
}
```
### Response

```json
{
  "id": "user-id",
  "userName": "john",
  "email": "john@example.com",
  "token": "jwt-token",
  "profilePictureUrl": null
}
```

---

## 2. Login

**Endpoint:** `POST /api/account/login`

### Request Body

```json
{
  "email": "john@example.com",
  "password": "123456"
}
```

## 3. Forgot Password
//need correct email and password in appsettings.json
**Endpoint:** `POST /api/account/forgot-password`

### Purpose

Starts the password-reset process.

### Request Body

```json
{
  "email": "john@example.com"
}
```

### Process

1. Search for the user by email.
2. Generate a secure random reset token.
3. Store the token in the database.
4. Set token expiration to **15 minutes**.
5. Send the token to the user's email.

## 4. Reset Password

**Endpoint:** `POST /api/account/reset-password`

### Purpose
Changes the user's password using a valid reset token.

### Request Body

```json
{
  "email": "john@example.com",
  "token": "reset-token",
  "newPassword": "newPassword123"
}
```

### Validation

* User must exist.
* Reset token must exist.
* Token must match.
* Token must not be expired.(15min)

### Process

1. Validate the reset request.
2. Hash the new password using `HMACSHA512`.
3. Generate a new password salt.
4. Replace the old password hash and salt.
5. Remove the reset token and expiration date.
6. Save the changes.

---

## 5. Add Profile Photo

**Endpoint:** `POST /api/account/profile-photo`

### Request
`form-data`

### Process

1. Get the authenticated user's ID from the JWT claims.
2. Load the user and existing photos.
3. Validate the uploaded file.
4. Upload the image to **Cloudinary**.
5. If another photo is currently the main photo, set `IsMain = false`.
6. Create a new `Photo` entity.
7. Store:

   * Cloudinary URL
   * Cloudinary Public ID
   * User ID
   * `IsMain = true`
8. Save the photo in the database.
9. Return `PhotoDto`.

### Response

```json
{
  "id": "photo-id",
  "url": "https://cloudinary.com/...",
  "uploadedAt": "2026-08-18T...",
  "isMain": true
}
```

