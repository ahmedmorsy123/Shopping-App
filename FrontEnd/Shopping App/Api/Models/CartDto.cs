using System.Collections.Generic;

namespace ShoppingApp.Api.Models
{
    public class CartDto
    {
        public int UserId { get; set; }
        public int CartId { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}