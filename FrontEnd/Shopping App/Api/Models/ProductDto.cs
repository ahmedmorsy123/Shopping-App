namespace ShoppingApp.Api.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
    }
}