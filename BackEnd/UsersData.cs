using Microsoft.EntityFrameworkCore;
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
    public class UsersData
    {
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

        public static async Task<bool> Login(string username, string password)
        {
            string passwordHash = PasswordService.HashPassword(password);

            using (var context = new AppDbContext())
            {
                var user = context.Users.Where(u => u.Name == username && u.PasswordHash == passwordHash).FirstOrDefault();
                if (user != null)
                {
                    user.LastLogin = DateTime.Now;
                    await context.SaveChangesAsync();
                }

                return user != null;
            }
        }

        public static async Task<bool> AddUser(UserDto user)
        {
            using (var context = new AppDbContext())
            {
                User userToAdd = new User();

                userToAdd.Name = user.Name;
                userToAdd.Email = user.Email;
                userToAdd.PasswordHash = PasswordService.HashPassword(user.Password);
                userToAdd.CreatedAt = DateTime.Now;
                userToAdd.LastLogin = DateTime.Now;
                context.Users.Add(userToAdd);
                await context.SaveChangesAsync();

                return true;

            }
        }
    }
}
