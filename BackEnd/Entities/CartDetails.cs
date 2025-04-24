using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingAppDB.Entities
{
    public class CartDetails
    {
        [Column("ProductId")]
        public int ProductId { get; set; }
        //public Product Product { get; set; }
        [Column("ProductName")]
        public string ProductName { get; set; }

        [Column("Quentity")]
        public int Quentity { get; set; }
    }
}
