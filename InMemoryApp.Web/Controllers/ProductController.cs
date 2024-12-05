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

               // options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

               //10 saniye içinde dataya erişilmezse memoryden silinir.
               options.SlidingExpiration = TimeSpan.FromSeconds(10);

                //best practice açısından bir slidind expiration  tanımlarken güncel olmayan bir data ile karşılaşmamak için absolute expiration da tanımlanmalıdır.
                //yani en fazla 1 dakika sonra memoryden silinir.
                options.AbsoluteExpiration= DateTime.Now.AddMinutes(1);

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
