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
            //memorydeki datanın var olup olmadığını kontrol ediyoruz 1. yol
            if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            //2. yol TryGetValue ile birlikte belirtilen keyi alıp objeye atamasını yapıyoruz
            if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            {
                //eğer memoryde yoksa memorye ekliyoruz
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            return View();
        }

        public IActionResult Get()
        {
            //Remove ile memorydeki verilen key'e göre data silinir
            _memoryCache.Remove("zaman");

            //GetOrCreate ile memoryde var sa alıyoruz yoksa oluşturuyoruz.
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });

            ViewBag.zaman = _memoryCache.Get<string>("zaman");

            return View();
        }

    }
}
