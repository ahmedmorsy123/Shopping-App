using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingApp.Api.Models
{

    public class OrderDto
    {
        public int Id { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public List<ProductDto> OrderItems { get; set; }
    }
}