using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (id == null)
            throw new UnauthorizedAccessException("User ID not found.");

        return Guid.Parse(id);
    }
}