using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserService(AppDbContext context) : IUserService
    {
        private readonly AppDbContext _context = context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public async Task<User> CreateUserAsync(string userName, string password)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == userName))
            {
                throw new InvalidOperationException("Username already exists");
            }

            var user = new User { UserName = userName };

            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> AuthenticateAsync(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context
                .Users.Include(u => u.ToDonts)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context
                .Users.Include(u => u.ToDonts)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
