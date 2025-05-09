namespace ShoppingAppDB.Models
{
    public class CartDto
    {
        public int UserId { get; set; }
        public int CartId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}