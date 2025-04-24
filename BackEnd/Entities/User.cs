using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppDB.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }

        public Cart? Cart { get; set; } 
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
