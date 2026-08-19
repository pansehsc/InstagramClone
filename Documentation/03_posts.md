# Posts API — Role & Constraints

## 1. Create Post

**Endpoint:** `POST /api/posts`

**Role:**
Creates a new post for the authenticated user and uploads one or more photos to Cloudinary.

**Constraints:**

* Authentication is required.
* At least **1 photo** is required.
* `Caption` maximum length: **3000 characters**.
* `Location` is optional.
* `Hashtags` is optional.
* The post owner is taken from the authenticated JWT; the client cannot choose the `UserId`.
* The first uploaded photo automatically becomes the main photo (`IsMain = true`).
* The photo must successfully upload to Cloudinary before the post is saved.
* The shown code does **not** explicitly restrict image type or file size.

---

## 2. Get All Posts

**Endpoint:** `GET /api/posts`

**Role:**
Returns all posts in the system, ordered from newest to oldest.

**Constraints:**

* Authentication is required.
* No ownership restriction.
* Results include post photos, likes, comments, user information, and profile picture information.
* Posts are ordered by `CreatedAt DESC`.

---

## 3. Get Single Post

**Endpoint:** `GET /api/posts/{id}`

**Role:**
Returns one post using its unique `Guid` ID.

**Constraints:**

* Authentication is required.
* `{id}` must be a valid `Guid`.
* The post must exist.
* No ownership restriction; any authenticated user can view it.

---

## 4. Update Post

**Endpoint:** `PUT /api/posts/{id}`

**Role:**
Updates the content of an existing post.

**Constraints:**

* Authentication is required.
* `{id}` must be a valid `Guid`.
* The post must exist.
* **Only the post owner can update it.**
* `Caption`, `Location`, and `Hashtags` exist in `UpdatePostDto`.
* ⚠️ Current controller actually updates only `Caption` and `Hashtags`.
* `Location` is currently **not saved** during update.
* No `MaxLength` constraint is defined on `UpdatePostDto.Caption`.

**Authorization:**

```text
Post owner → Allowed
Other authenticated user → 403 Forbidden
```

---

## 5. Delete Post

**Endpoint:** `DELETE /api/posts/{id}`

**Role:**
Deletes an existing post belonging to the authenticated user.

**Constraints:**

* Authentication is required.
* `{id}` must be a valid `Guid`.
* The post must exist.
* **Only the post owner can delete it.**
* Related database photo records are deleted through cascade delete.

**Authorization:**

```text
Post owner → Allowed
Other authenticated user → 403 Forbidden
```

---

## 6. Get Feed

**Endpoint:** `GET /api/posts/feed`

**Role:**
Returns the personalized feed for the authenticated user.

The feed contains:

* The current user's posts.
* Posts from users the current user follows.

**Constraints:**

* Authentication is required.
* The current user's ID comes from the JWT.
* A post appears when its owner is the current user **or** the current user follows its owner.
* Results are ordered newest first.
* No pagination is currently implemented.

---

# Common API Constraints

| Constraint            | Current behavior                   |
| --------------------- | ---------------------------------- |
| Authentication        | Required for all Post APIs         |
| Authorization         | Update/Delete = owner only         |
| Post ID               | `Guid`                             |
| Caption on Create     | Maximum 3000 characters            |
| Caption on Update     | No explicit max length             |
| Photos on Create      | At least 1 required                |
| Multiple Photos       | Supported                          |
| Main Photo            | First uploaded photo               |
| Image type validation | Not explicitly implemented         |
| Image size validation | Not explicitly implemented         |
| Location              | Optional on Create                 |
| Hashtags              | Optional                           |
| Pagination            | Not implemented                    |
| Sorting               | Newest first                       |
| Cloudinary            | Used for photo upload              |
| UserId                | Taken from authenticated user      |
| Cloudinary deletion   | Not performed when Post is deleted |
