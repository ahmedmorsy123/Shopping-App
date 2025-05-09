namespace ShoppingAppDB.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public string? productCategory { get; set; }
        public string? productDescription { get; set; }
        public int quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal price { get; set; }
    }
}