using System;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUserNameAsync(string username);
    Task<IEnumerable<User>> GetAllAsync();
    void Update(User user);
    Task<bool> SaveAllAsync();
}