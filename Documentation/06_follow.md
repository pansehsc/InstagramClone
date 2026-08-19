# Follows API — Role & Constraints

## 1. Follow User

**Endpoint:** `POST /api/follows/{userId}`

**Role:**
Allows the authenticated user to follow another user and creates a follow notification for the followed user.

**Constraints:**

* Authentication is required.
* `{userId}` must be a valid `Guid`.
* Target user must exist.
* A user **cannot follow themselves**.
* A user cannot follow the same user more than once.
* `FollowerId` comes from the authenticated JWT.
* `FollowingId` comes from the URL.
* A `"Follow"` notification is created after the follow is successfully saved.

**Possible responses:**

```text id="x6x2x0"
200 OK          → Follow added
400 Bad Request → Cannot follow yourself / already following / save failed
404 Not Found   → Target user does not exist
```

---

## 2. Unfollow User

**Endpoint:** `DELETE /api/follows/{userId}`

**Role:**
Removes the current user's follow relationship with another user.

**Constraints:**

* Authentication is required.
* `{userId}` must be a valid `Guid`.
* The follow relationship must exist.
* The current user can remove only their own follow relationship.
* No notification is created when unfollowing.

**Possible responses:**

```text id="m0mbyf"
200 OK          → Unfollow successful
404 Not Found   → Follow relationship does not exist
400 Bad Request → Database deletion failed
```

---

## 3. Get Followers

**Endpoint:** `GET /api/follows/{userId}/followers`

**Role:**
Returns the users who follow the specified user.

**Constraints:**

* Authentication is required.
* `{userId}` must be a valid `Guid`.
* No ownership restriction.
* Results are ordered by newest follow first.
* No pagination is currently implemented.

**Returned data:**

```text id="dr4hrx"
UserId
UserName
ProfilePictureUrl
```

---

## 4. Get Following

**Endpoint:** `GET /api/follows/{userId}/following`

**Role:**
Returns the users followed by the specified user.

**Constraints:**

* Authentication is required.
* `{userId}` must be a valid `Guid`.
* No ownership restriction.
* Results are ordered by newest follow first.
* No pagination is currently implemented.

**Returned data:**

```text id="94kbr6"
UserId
UserName
ProfilePictureUrl
```

---

## 5. Get Follow Status

**Endpoint:** `GET /api/follows/status/{userId}`

**Role:**
Checks whether the authenticated user is following the specified user.

**Response:**

```json id="2g7xaq"
{
  "isFollowing": true
}
```

or:

```json id="mxm0z7"
{
  "isFollowing": false
}
```

**Constraints:**

* Authentication is required.
* `{userId}` must be a valid `Guid`.
* Target user must exist.
* A user cannot check their follow status against themselves.
* Only the relationship between the current user and target user is checked.

---
