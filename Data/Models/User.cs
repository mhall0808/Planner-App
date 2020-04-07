using AspNetCore.Identity.Mongo.Model;

namespace PlannerApp.Data.Models
{
    public class User : MongoUser
    {
        public byte[] PasswordHashByte { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
