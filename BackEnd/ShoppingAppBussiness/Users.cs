using ShoppingAppDB;
using static ShoppingAppDB.UsersData;

namespace ShoppingAppBussiness
{
    public class Users
    {
        public static UserDto? GetUserById(int id)
        {
            return UsersData.GetUserById(id);
        }

        public static int AddUser(UserDto user)
        {
            return UsersData.AddUser(user);
        }

        public static bool UpdateUser(UserDto user, string oldPassword)
        {
            return UsersData.UpdateUser(user, oldPassword);
        }

        public static bool DeleteUser(int userId)
        {
            return UsersData.DeleteUser(userId);
        }
        public static bool Login(string username, string password)
        {
            return UsersData.Login(username, password);
        }

        public static void Logout()
        {
            UsersData.ClearCurrentUser();
        }

        public static UserDto? GetCurrentUser()
        {
            return UsersData.GetCurrentUser();
        }
    }


}
