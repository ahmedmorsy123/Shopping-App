﻿using ShoppingAppDB;
using static ShoppingAppDB.ProductData;

namespace ShoppingAppBussiness
{
    public class Products
    {
        public static List<ProductDto> GetProductsPaginated(int pageNumber)
        {
            return ProductData.GetProductsPaginated(pageNumber);
        }
    }

}
