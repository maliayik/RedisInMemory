using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController(IDistributedCache _distributedCache) : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
