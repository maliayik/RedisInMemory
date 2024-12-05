using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private  IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue("zaman", out string? zamancache))
            {
                MemoryCacheEntryOptions options = new();
                
               options.SlidingExpiration = TimeSpan.FromSeconds(10);
               options.AbsoluteExpiration= DateTime.Now.AddMinutes(1);
               options.Priority=CacheItemPriority.High

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);
            }

            return View();
        }

        public IActionResult Get()
        {
            _memoryCache.TryGetValue("zaman", out string? zamancache);

            ViewBag.zaman = zamancache;

            return View();
        }

    }
}
