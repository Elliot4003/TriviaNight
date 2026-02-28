using Microsoft.EntityFrameworkCore;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TriviaNightDbContext _context;

    public UserRepository(TriviaNightDbContext context)
    {
        _context = context;
    }

    public async Task AddUser(UserModel user)
    {
        _context.Add(user);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exists(string userName)
    {
        return await _context.Users.AnyAsync(u => u.Username == userName);
    }

    public async Task<UserModel?> GetUser(string userName)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Username == userName);
    }
}
