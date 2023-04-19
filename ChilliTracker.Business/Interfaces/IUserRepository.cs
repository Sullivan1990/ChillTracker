using ChilliTracker.Data.DataModels;
using ChilliTracker.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilliTracker.Business.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserByCredentials(UserLoginDTO userLogin);

        public User GetUserById(string userID);

        public void SetUserRefreshTokenDetails(RefreshTokenSetDTO refreshTokenSet, string userID);
        public void UpdateUserRefreshToken(string refreshTokenUpdate, string userID);
        public User CreateUser(UserCreateDTO newUser);

        public void SetUserInactive(string? userID = "", string? userName = "");
        public void UpdateUserPassword(string userName, string newPassword);

        public void UpdateUser(string userName, UserUpdateDTO updatedUser);
    }
}
