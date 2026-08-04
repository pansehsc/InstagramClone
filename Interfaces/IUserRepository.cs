using System;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUserNameAsync(string username);

    Task<User?> GetCurrentUserAsync(Guid id);
    Task<User?> GetUserWithPhotosAsync(Guid id);
    Task<User?> GetUserByUserNameWithPhotosAsync(string userName);

    Task<IEnumerable<User>> GetAllAsync();
    void Update(User user);
    void Add(User user);
    Task<bool> SaveAllAsync();

    //Task<PagedList<User>> GetUsersAsync(UserParams userParams);
    // Task<bool> UserExistsAsync(string username);
    // Task<bool> EmailExistsAsync(string email);
}