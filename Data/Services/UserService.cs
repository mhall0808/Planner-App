using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using PlannerApp.Data.DbContexts;
using PlannerApp.Data.Models;

namespace PlannerApp.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDbContext _dbContext;

        public UserService(IUserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> GetAll()
        {
            return _dbContext.Users.Find(_ => true).ToList();
        }

        public async Task<User> Create(User user, string password)
        {
            (user.PasswordSalt, user.PasswordHashByte) = CreatePasswordHash(password);
            await _dbContext.Users.InsertOneAsync(user);
            return user;
        }

        public void Delete(string id)
        {
            FilterDefinition<User> filterDefinition = Builders<User>.Filter.Eq("Id", id);
            _dbContext.Users.DeleteOne(filterDefinition);
        }

        public async Task<User> Authenticate(string username, string password)
        {
            FilterDefinition<User> filterDefinition = Builders<User>.Filter.Eq("UserName", username);
            var user = (await _dbContext.Users.FindAsync<User>(filterDefinition)).First();

            if (VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHashByte) == false)
                return null;

            return user;
        }

        #region private helper methods

        private static Tuple<byte[], byte[]> CreatePasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            return Tuple.Create(hmac.Key, hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedSalt, byte[] storedHash)
        {
            // if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            // if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        [Obsolete("Please use Create(), This won't create PasswordHash and PasswordSalt")]
        public void Insert(User user)
        {
            _dbContext.Users.InsertOne(user);
        }

        public async Task<User> GetById(string id)
        {
            FilterDefinition<User> filterDefinition = Builders<User>.Filter.Eq("Id", id);
            return (await _dbContext.Users.FindAsync(filterDefinition)).First();
        }

        #endregion
    }

    public interface IUserService
    {
        List<User> GetAll();
        void Insert(User user);
        void Delete(string id);
        Task<User> Create(User user, string password);
        Task<User> Authenticate(string username, string password);
        Task<User> GetById(string id);
    }
}
