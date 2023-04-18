using BCrypt.Net;
using ChilliTracker.Business.Interfaces;
using ChilliTracker.Data.DataModels;
using ChilliTracker.Data.DTO;
using ChilliTracker.Shared.Connection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Business.Repository
{
    public class UserRepository : IUserRepository
    {
        private IMongoDatabase _database;
        private IMongoCollection<User> _users;
        public UserRepository(IMongoDatabaseConnection connection)
        {
            _database = connection.GetDatabase();
            _users = _database.GetCollection<User>("Users");
        }
        public User CreateUser(UserCreateDTO newUser)
        {
            var existingUser = CheckUserNameAndEmailNotExists(newUser.Username, newUser.Email);
            if (existingUser != null)
            {
                return null;
            }

            // Any further validation

            // Add the User 
            User user = new User()
            {
                Email = newUser.Email,
                Username = newUser.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password)
            };

            _users.InsertOne(user);

            return user;
        }

        private User CheckUserNameAndEmailNotExists(string username, string email)
        {
            var filter = Builders<User>.Filter.Where(c => c.Username == username || c.Email == email);

            return _users.Find(filter).FirstOrDefault();
        }

        public User GetUserByCredentials(UserLoginDTO userLogin)
        {
            var filter = Builders<User>.Filter.Where(c => c.Username == userLogin.Username);
            var locatedUser = _users.Find(filter).FirstOrDefault();
            if (locatedUser != null && BCrypt.Net.BCrypt.Verify(userLogin.Password, locatedUser.Password))
            {
                return locatedUser;
            }
            return null;
        }

        public void SetUserInactive(string? userID = "", string? userName = "")
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string userName, UserUpdateDTO updatedUser)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserPassword(string userName, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
