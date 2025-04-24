namespace ShoppingAppDB.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }


}
