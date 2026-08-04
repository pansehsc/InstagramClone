using System.Text;
using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Extensions;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Dependency Injection
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

//builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
// builder.Services.AddScoped<IMessageRepository, MessageRepository>();
// builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

// JWT Authentication
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(policy =>
    policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();
}

app.Run();