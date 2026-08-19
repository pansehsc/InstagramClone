# Stories API — Role & Constraints

## 1. Create Story

**Endpoint:** `POST /api/stories`

**Role:**
Creates a story containing **text, an image, or both**. Image files are uploaded to Cloudinary.

**Request:** `multipart/form-data`

| Field     | Type | Required |
| --------- | ---- | -------: |
| `Content` | Text |      No* |
| `File`    | File |      No* |

* At least **one of them is required**.

**Constraints:**

* Authentication required.
* Story must contain text **or** an image.
* If a file is provided, it is uploaded to Cloudinary.
* Story automatically gets:

  * `CreatedAt = UTC now`
  * `ExpiresAt = UTC now + 24 hours`
* `UserId` comes from the authenticated JWT.
* Client cannot specify the story owner.
* Cloudinary `PublicId` is stored for later deletion.

**Possible responses:**

```text
200 OK          → Story created
400 Bad Request → Empty story / upload failed / database error
```

---

## 2. Get All Active Stories

**Endpoint:** `GET /api/stories`

**Role:**
Returns all stories that have **not expired**.

**Constraints:**

* Authentication required.
* Only stories where:

```text
ExpiresAt > DateTime.UtcNow
```

are returned.

* Ordered newest first.
* No pagination currently implemented.

---

## 3. Get My Active Stories

**Endpoint:** `GET /api/stories/mine`

**Role:**
Returns the authenticated user's active stories.

**Constraints:**

* Authentication required.
* User ID comes from JWT.
* Only the current user's stories are returned.
* Expired stories are excluded.
* Ordered newest first.
* No pagination.

---

## 4. Get One Story

**Endpoint:** `GET /api/stories/{id}`

**Role:**
Returns one story by its ID.

**Constraints:**

* Authentication required.
* `{id}` must be a valid `Guid`.
* Story must exist.
* Expired stories cannot be retrieved.
* There is **no owner restriction**; any authenticated user can request an active story.

**Responses:**

```text
200 OK        → Story returned
404 Not Found → Story doesn't exist or has expired
```

---

## 5. Delete Story

**Endpoint:** `DELETE /api/stories/{id}`

**Role:**
Deletes the authenticated user's story and, if it contains an image, removes the image from Cloudinary.

**Constraints:**

* Authentication required.
* Story must exist.
* **Only the story owner can delete it.**
* If `PublicId` exists, the Cloudinary image is deleted.
* Story is then removed from the database.

**Authorization:**

```text
Story owner → Allowed
Other user  → 403 Forbidden
```

**Responses:**

```text
204 No Content   → Deleted successfully
404 Not Found    → Story doesn't exist
403 Forbidden    → User doesn't own story
400 Bad Request  → Database deletion failed
```

---

## 6. Get Stories From Following

**Endpoint:** `GET /api/stories/following`

**Role:**
Returns active stories posted by users that the authenticated user follows.

**Constraints:**

* Authentication required.
* User ID comes from JWT.
* Only users followed by the current user are included.
* Expired stories are excluded.
* Ordered newest first.
* No pagination.

**Logic:**

```text
Current User
     ↓
Users they follow
     ↓
Their active stories
     ↓
Newest first
```
