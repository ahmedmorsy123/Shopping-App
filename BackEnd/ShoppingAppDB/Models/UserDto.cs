
namespace ShoppingAppDB.Models
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
}