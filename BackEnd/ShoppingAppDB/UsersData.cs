using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppDB
{
    public class UsersData
    {
        public class UserDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; } = null;

            public UserDto(int id, string name, string? email, string? password = null)
            {
                Id = id;
                Name = name;
                Email = email;
                Password = password;
            }
        }


        public static UserDto? _currentUser;

        public static UserDto? GetCurrentUser()
        {
            return _currentUser;
        }

        public static void SetCurrentUser(UserDto user)
        {
            _currentUser = user;
        }

        public static void ClearCurrentUser()
        {
            _currentUser = null;
        }

        public static  UserDto? GetUserById(int id)
        {
            UserDto? userDto;
            using (var context = new AppDbContext())
            {
                userDto = context.Users.AsNoTracking()
                    .Where(u => u.Id == id) 
                    .Select(u => new UserDto(id, u.Name, u.Email,null))
                    .FirstOrDefault(); 
            }
            return userDto;
        }

        public static int AddUser(UserDto user)
        {
            using (var context = new AppDbContext())
            {
                User userToAdd = new User();

                userToAdd.Name = user.Name;
                userToAdd.Email = user.Email;
                userToAdd.PasswordHash = HashPassword(user.Password);
                userToAdd.CreatedAt = DateTime.Now;
                userToAdd.LastLogin = DateTime.Now;
                context.Users.Add(userToAdd);
                context.SaveChanges();

                return userToAdd.Id;

            }
        }

        public static bool UpdateUser(UserDto user, string oldPassword)
        {
            if(_currentUser == null) return false;
            if(!VerifyPassword(oldPassword, _currentUser.Password)) return false;

            using (var context = new AppDbContext())
            {
                var userToUpdate = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                if (userToUpdate != null)
                {
                    userToUpdate.Name = user.Name;
                    userToUpdate.Email = user.Email;
                    userToUpdate.PasswordHash = HashPassword(user.Password);
                    context.SaveChanges();
                    UsersData.SetCurrentUser(new UserDto(user.Id, user.Name, user.Email, user.Password));
                    return true;
                }
                return false;
            }
        }

        public static bool DeleteUser(int userId)
        {
            using (var context = new AppDbContext())
            {
                var userToDelete = context.Users.Find(userId);
                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool Login(string username, string password)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.AsEnumerable().FirstOrDefault(u => u.Name == username);
                if (user != null && VerifyPassword(password, user.PasswordHash))
                {
                    user.LastLogin = DateTime.Now;
                    context.SaveChanges();
                    SetCurrentUser(new UserDto(user.Id, user.Name, user.Email, user.PasswordHash));
                    return true;
                }
                return false;
            }
        }

        private const int WorkFactor = 10;
        public static string HashPassword(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword, WorkFactor);
        }

        public static bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }
    }
}
