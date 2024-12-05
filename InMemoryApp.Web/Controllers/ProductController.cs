using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;
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
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                options.Priority = CacheItemPriority.High;

                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");

                });

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

                Product p = new()
                {
                    Id = 1,
                    Name = "Kalem",
                    Price = 100
                };

                //IMemoryCache serialize işlemini kendisi  yaptığından complex type'ı direk ekleyebiliriz.
                _memoryCache.Set<Product>("product:1", p);

            }

            return View();
        }

        public IActionResult Get()
        {
            _memoryCache.TryGetValue("zaman", out string? zamancache);
            _memoryCache.TryGetValue("callback", out string? callback);

            ViewBag.callback = callback;
            ViewBag.zaman = zamancache;
            ViewBag.product = _memoryCache.Get<Product>("product:1");

            return View();
        }

    }
}
