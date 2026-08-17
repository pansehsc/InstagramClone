using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        foreach (var claim in user.Claims)
        {
            Console.WriteLine($"Type: {claim.Type}");
            Console.WriteLine($"Value: {claim.Value}");
        }
        if (id == null)
            throw new UnauthorizedAccessException("User ID not found.");

        return Guid.Parse(id);
    }
}
