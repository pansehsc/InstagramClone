# Comments API — Role & Constraints

## 1. Add Comment

**Endpoint:** `POST /api/posts/{postId}/comments`

**Role:**
Creates a comment on a specific post for the authenticated user. If the commenter is not the post owner, a notification is created for the post owner.

**Request Body:**

```json
{
  "content": "Nice post!"
}
```

**Constraints:**

* Authentication is required.
* `{postId}` must be a valid `Guid`.
* The user must exist.
* The target post must exist.
* `Content` is **required**.
* `Content` maximum length: **1000 characters**.
* The comment owner (`CreatedById`) comes from the authenticated JWT.
* The `PostId` comes from the URL, not the request body.
* A notification is created only when the commenter is **not** the post owner.
* No image/file is supported by the current comment API.


**Possible responses:**

```text
200 OK       → Comment created
401          → User not authenticated / user not found
404          → Post not found
400          → Comment could not be saved
```

---

## 2. Get Comments

**Endpoint:** `GET /api/posts/{postId}/comments`

**Role:**
Returns all comments belonging to a specific post.

**Constraints:**

* Authentication is required.
* `{postId}` must be a valid `Guid`.
* Comments are retrieved using the Post ID.
* No ownership restriction; any authenticated user can retrieve comments.
* No pagination is implemented in the shown code.

**Possible response:**

```text
200 OK → List of CommentDto
```

---

## 3. Delete Comment

**Endpoint:** `DELETE /api/comments/{id}`

**Role:**
Deletes a comment created by the authenticated user.

**Constraints:**

* Authentication is required.
* `{id}` must be a valid `Guid`.
* Comment must exist.
* **Only the comment owner can delete it.**
* The owner is checked using `CreatedById`.
* No user can delete another user's comment.

**Authorization:**

```text
Comment owner → Allowed
Other authenticated user → 403 Forbidden
```

**Possible responses:**

```text
200 OK       → Comment deleted
404 Not Found → Comment does not exist
403 Forbidden → User does not own comment
400 Bad Request → Database deletion failed
```

---

# Comment Entity

## Comment.cs

**Role:**
Represents a comment stored in the database and connected to both a User and a Post.

### Important fields

| Field         | Role                                 |
| ------------- | ------------------------------------ |
| `Id`          | Unique comment ID                    |
| `Content`     | Comment text                         |
| `CreatedAt`   | Creation time in UTC                 |
| `CreatedById` | User who created the comment         |
| `PostId`      | Post containing the comment          |
| `User`        | Comment author's navigation property |
| `Post`        | Related post                         |

### Relationships

```text
User 1 ──────── * Comments

Post 1 ──────── * Comments
```
