namespace ShoppingAppDB.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; }

        public Cart Cart { get; set; } = new Cart();
    }


}
