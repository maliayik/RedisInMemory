using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController(IDistributedCache _distributedCache) : Controller
    {
        // data kaydetme işlemi
        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            //1 dakikalık bir cache süresi belirledik
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name","Mehmet Ali", cacheEntryOptions);
            
            return View();
        }

        //okuma işlemi
        public IActionResult Show()
        {
            string name = _distributedCache.GetString("name");
            ViewBag.name = name;
            return View();
        }

        //silme işlemi
        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }

    }
}
