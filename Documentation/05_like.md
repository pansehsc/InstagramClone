# Likes API — Role & Constraints

## 1. Like Post

**Endpoint:** `POST /api/posts/{postId}/like`

**Role:**
Adds a like from the authenticated user to a specific post. If the user is not the post owner, a notification is created for the post owner.

**Constraints:**

* Authentication is required.
* `{postId}` must be a valid `Guid`.
* The post must exist.
* The authenticated user can like the post only once.
* Duplicate likes are rejected.
* `CreatedById` comes from the authenticated JWT.
* `PostId` comes from the URL.
* A notification is created only when the liker is not the post owner.

**Duplicate Like:**

```text id="x0h5qg"
User already liked post → 200```

**Possible responses:**

```text id="a3c1jz"
200 OK          → Post liked successfully
400 Bad Request → Already liked / database error
404 Not Found   → Post does not exist
```

---

## 2. Unlike Post

**Endpoint:** `DELETE /api/posts/{postId}/like`

**Role:**
Removes the authenticated user's like from a post.

**Constraints:**

* Authentication is required.
* `{postId}` must be a valid `Guid`.
* The user's like must exist.
* Only the user who created the like can remove it.
* No physical Post data is deleted; only the Like record is removed.

**Authorization:**

```text id="s5x1xz"
Like owner → Can remove like
Other user → 403 Forbidden
```

**Possible responses:**

```text id="z7p9yw"
200 OK          → Like removed
404 Not Found   → Like does not exist
403 Forbidden   → User does not own the like
400 Bad Request → Database deletion failed
```

---

## 3. Get Likes Count

**Endpoint:** `GET /api/posts/{postId}/likes/count`

**Role:**
Returns the total number of users who liked a post.

**Constraints:**

* Authentication is required.
* `{postId}` must be a valid `Guid`.
* The current implementation counts Like records matching the Post ID.
* There is no ownership restriction.
* The repository does not explicitly check that the Post exists before counting.

**Response example:**

```text id="bdygpx"
"no of users who like this post:15"
```

**Important:**
Although the method is declared as:

```csharp
ActionResult<int>
```

the controller currently returns a **string**, not a numeric JSON value.

---

## 4. Get Post Likes

**Endpoint:** `GET /api/posts/{postId}/likes`

**Role:**
Returns the users who liked a specific post.

**Constraints:**

* Authentication is required.
* `{postId}` must be a valid `Guid`.
* No ownership restriction.
* Returns the users associated with Like records for the post.
* No pagination is currently implemented.

### Returned information

`PostLikeDto` is mapped from `Like` and includes information such as:

```text id="j2p3k1"
UserId
UserName
ProfilePictureUrl
```

---
