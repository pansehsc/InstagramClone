using System.Text;
using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Extensions;
using API.Services;
using API.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using API.Helpers;

var builder = WebApplication.CreateBuilder(args);
// Console.WriteLine(
//     $"PROGRAM CloudName = '{builder.Configuration["CloudinarySettings:CloudName"]}'"
// );
builder.Services.AddControllers();
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));
// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Mapping
builder.Services.AddAutoMapper(autoMapperConfiguration  =>
{
    autoMapperConfiguration.AddProfile<MappingProfile>();
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
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
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