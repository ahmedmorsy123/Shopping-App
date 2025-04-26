using Microsoft.AspNetCore.Mvc;
using ShoppingAppDB.Data;
using ShoppingAppDB.Data.Seeder;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrderssAPI : ControllerBase
    {
        [HttpGet]
        public async Task Get() 
        {
            var context = new AppDbContext();
            var seeder = new DatabaseSeeder(context);
            seeder.Seed(userCount: 500, categoryCount: 50, productCount: 20000);
        }
    }
}
