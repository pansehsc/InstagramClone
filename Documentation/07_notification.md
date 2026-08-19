# Notifications API — Role & Constraints

## 1. Get Notifications

**Endpoint:** `GET /api/notifications`

**Role:**
Returns all notifications belonging to the authenticated user.

**Constraints:**

* Authentication is required.
* The user can only retrieve **their own notifications**.
* User ID comes from the authenticated JWT.
* Notifications are ordered from newest to oldest.
* Actor information is included.
* No pagination is currently implemented.

**Returned data:**

```text id="8t7d7v"
Id
ActorId
ActorUserName
ActorProfilePictureUrl
Type
PostId
CommentId
CreatedAt
IsRead
```

---

## 2. Mark Notification as Read

**Endpoint:** `PATCH /api/notifications/{id}/read`

**Role:**
Marks a notification as read for the authenticated user.

**Behavior:**

```text id="5z5x5u"
IsRead = true
```

**Constraints:**

* Authentication is required.
* `{id}` must be a valid `Guid`.
* Notification must exist.
* **Only the notification owner can modify it.**
* The current user's ID must match `Notification.UserId`.
* No `ReadAt` timestamp is stored by the current implementation.

**Authorization:**

```text id="kq1d9p"
Notification owner → Allowed
Other user → 403 Forbidden
```

**Responses:**

```text id="n7i6m2"
204 No Content    → Successfully marked as read
404 Not Found     → Notification does not exist
403 Forbidden     → Notification belongs to another user
400 Bad Request   → Database update failed
```

---

## 3. Delete Notification

**Endpoint:** `DELETE /api/notifications/{id}`

**Role:**
Permanently removes a notification belonging to the authenticated user.

**Constraints:**

* Authentication is required.
* `{id}` must be a valid `Guid`.
* Notification must exist.
* **Only the notification owner can delete it.**
* The current user's ID must match `Notification.UserId`.

**Authorization:**

```text id="3q6r1v"
Notification owner → Allowed
Other user → 403 Forbidden
```

**Responses:**

```text id="v4k7p2"
204 No Content    → Successfully deleted
404 Not Found     → Notification does not exist
403 Forbidden     → Notification belongs to another user
400 Bad Request   → Database deletion failed
```

---

# CreateNotificationDto

**Role:**
Defines the data used internally when creating a notification.

### Fields

| Field              | Role                             | Required |
| ------------------ | -------------------------------- | -------: |
| `UserId`           | User who receives notification   |      Yes |
| `ActorId`          | User who caused the notification |      Yes |
| `NotificationType` | Type of notification             |      Yes |
| `PostId`           | Related post                     |       No |
| `CommentId`        | Related comment                  |       No |

### Notification examples

```text id="n3gq5f"
Like
Comment
Follow
Message
```

The actual notification types currently used by the shown controllers are:
