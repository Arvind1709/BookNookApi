using Microsoft.EntityFrameworkCore;
using TheBookNookApi.AppDbContext;
using TheBookNookApi.Model;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Service
{
    /// <summary>
    /// Service implementation for managing user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly BookNookDbContext _context;
        #region Constructor
        public UserService(BookNookDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Get Users
        /// <inheritdoc/>
        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        #endregion
        #region Get User By Id
        /// <inheritdoc/>
        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        #endregion
        #region Get User By Username
        /// <summary>
        /// <inheritdoc/>    
        public async Task<UserModel> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        #endregion

        #region Create User
        /// <inheritdoc/>
        public async Task<UserModel?> CreateUserAsync(UserModel userModel)
        {
            if (string.IsNullOrWhiteSpace(userModel.Password))
                throw new ArgumentException("Password is required.");

            // Hash the user's password for security
            userModel.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }
        #endregion
        #region Update User
        /// <inheritdoc/>
        public async Task<bool> UpdateUserAsync(int id, UserModel userModel)
        {
            if (id != userModel.Id)
                throw new ArgumentException("ID mismatch.");

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.Id == id))
                    return false;

                throw;
            }
        }
        #endregion
        #region Delete User
        /// <inheritdoc/>
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
