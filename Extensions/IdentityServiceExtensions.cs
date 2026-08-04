using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        var tokenKey = config["TokenKey"]
            ?? throw new Exception("TokenKey not found");

        if (tokenKey.Length < 64)
            throw new Exception("TokenKey must be at least 64 characters.");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = key,

                    ValidateIssuer = false,

                    ValidateAudience = false
                };
            });

        services.AddAuthorization();

        return services;
    }
}