using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetByUserNameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public void Update(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User?> GetUserWithPhotosAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Photos)
            .Include(u => u.Posts)
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByUserNameWithPhotosAsync(string userName)
    {
        return await _context.Users
            .Include(u => u.Photos)
            .Include(u => u.Posts)
            .Include(u => u.Followers)
            .Include(u => u.Following)
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<User?> GetCurrentUserAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }
}