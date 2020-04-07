using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PlannerApp.Data.Models;

namespace PlannerApp.Data.DbContexts
{
    public class UserDbContext : IUserDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        private const string USERS_COLLECTION = "User";

        public UserDbContext(IOptions<AppSettings> appSettings)
        {
            var client = new MongoClient(appSettings.Value.ConnectionString);
            _mongoDatabase = client.GetDatabase(USERS_COLLECTION);
        }

        public IMongoCollection<User> Users =>
            _mongoDatabase.GetCollection<User>(USERS_COLLECTION);
    }

    public interface IUserDbContext
    {
        IMongoCollection<User> Users { get; }
    }
}
