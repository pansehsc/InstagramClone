# Users Module

## Purpose

The Users module is responsible for managing user profiles and user-related
operations.

It provides APIs for:

- Getting the currently authenticated user's profile.
- Searching for users by username.
- Updating the authenticated user's profile.
- Getting another user's profile.

The module uses:

- `UsersController`
- `UserRepository`
- `IUserRepository`
- `UserProfileDto`
- `UpdateUserDto`
- `PostDto`
- `MappingProfile`

---

# 1. UsersController

File:

API/Controllers/UsersController.cs

The controller is protected by:

[Authorize]
 unless an endpoint explicitly uses `[AllowAnonymous]`.

The controller receives:

- `IUserRepository`
- `IMapper`

through dependency injection.

---

# 2. GET Current User

## Endpoint

GET /api/users/me

## Purpose

Returns the profile of the currently authenticated user.


## Authentication

Required.

Postman:

Authorization → Bearer Token
