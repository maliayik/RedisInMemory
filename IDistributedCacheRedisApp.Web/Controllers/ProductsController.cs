using System.Text;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController(IDistributedCache _distributedCache) : Controller
    {
        // data kaydetme işlemi
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 5000
            };

            //Veri kaydetme yöntemimizi ister json ister binary formatta yapabiliriz.
            //Json serialize işlemi 
            string jsonProduct = JsonConvert.SerializeObject(product);
            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            //Byte dönüştürme işlemi
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("product:1",byteProduct);

            return View();
        }

        //okuma işlemi
        public IActionResult Show()
        {
            //Json Deserialize işlemi
            string jsonproduct = _distributedCache.GetString("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);

            //Byte okuma işlemi
            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            ViewBag.product = p;
            return View();
        }

        //silme işlemi
        public IActionResult Remove()
        {
            _distributedCache.Remove("product:1");
            return View();
        }

        //Resim cacheleme işlemi
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images/winter.jpg");

            Byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image", imageByte);

            return View();
        }

        public IActionResult ImageUrl()
        {
            Byte[] imageByte = _distributedCache.Get("image");

            //byte dizisini image olarak döndürme işlemi
            return File(imageByte, "image/jpg");
        }

    }
}
