# Messages API — Role & Constraints

## 1. Send Message

**Endpoint:** `POST /api/messages`

**Role:**
Sends a private message from the authenticated user to another user and creates a notification for the receiver.

**Request Body:**

```json
{
  "receiverId": "USER_GUID",
  "content": "Hello!"
}
```

**Constraints:**

* Authentication is required.
* `ReceiverId` is required.
* Receiver must exist.
* User **cannot send a message to themselves**.
* `Content` is required.
* `Content` maximum length: **3000 characters**.
* `SenderId` is taken from the authenticated JWT.
* Client cannot choose the sender.
* A `"Message"` notification is created for the receiver after the message is saved.


**Possible responses:**

```text
200 OK          → Message sent
400 Bad Request → Cannot message yourself / save failed
404 Not Found   → Receiver does not exist
401 Unauthorized → Authentication missing/invalid
```

---

## 2. Get Conversation

**Endpoint:** `GET /api/messages/conversation/{userId}`

**Role:**
Returns the conversation between the authenticated user and another user.

**Constraints:**

* Authentication is required.
* `{userId}` must be a valid `Guid`.
* Target user must exist.
* User cannot request a conversation with themselves.
* The current user's ID comes from the JWT.
* Conversation is between the current user and the specified user.

**Possible responses:**

```text
200 OK          → Conversation returned
400 Bad Request → Cannot have conversation with yourself
404 Not Found   → User does not exist
```

---

## 3. Mark Message as Read

**Endpoint:** `PUT /api/messages/{id}/read`

**Role:**
Marks a received message as read and records the UTC time when it was read.

**Behavior:**

```text
IsRead = true
ReadAt = DateTime.UtcNow
```

**Constraints:**

* Authentication is required.
* `{id}` must be a valid `Guid`.
* Message must exist.
* **Only the receiver can mark the message as read.**
* If the message is already read, the API returns `"already marked"` without changing it again.

**Authorization:**

```text
Message receiver → Allowed
Sender → 403 Forbidden
Other user → 403 Forbidden
```

**Possible responses:**

```text
200 OK          → Message marked as read
200 OK          → Already marked
404 Not Found   → Message does not exist
403 Forbidden   → Current user is not the receiver
400 Bad Request → Database update failed
```

---

## 4. Delete Message

**Endpoint:** `DELETE /api/messages/{id}`

**Role:**
Hides a message from the current user's side without immediately deleting the message itself for both users.

**Important:**
This is a **soft delete per user**, using separate flags.

### If current user is the sender:

```text
SenderDeleted = true
```

### If current user is the receiver:

```text
RecipientDeleted = true
```

**Constraints:**

* Authentication is required.
* `{id}` must be a valid `Guid`.
* Message must exist.
* Only the sender or receiver can delete/hide the message.
* Sender and receiver have separate deletion states.
* The current code does not physically remove the message from the database.

**Authorization:**

```text
Sender → Can delete for sender
Receiver → Can delete for receiver
Other user → 403 Forbidden
```

**Possible responses:**

```text
200 OK          → Message deleted for current user
404 Not Found   → Message does not exist
403 Forbidden   → User is not sender or receiver
400 Bad Request → Database update failed
```
