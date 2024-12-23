using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        
        //kaydedilen datayı okumak için.
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();

            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }

        //data kaydetme (ekleme metodu)
        [HttpPost]
        public IActionResult Add(string name)
        {
            //listenin sonuna eleman ekler.
            db.ListRightPush(listKey, name);
           
            return RedirectToAction("Index");
        }
        
        //silme işlemi
        public  IActionResult DeleteItem(string name)
        {
             db.ListRemoveAsync(listKey, name).Wait();

            return RedirectToAction("Index");
        }

        //listenin solundaki ilk elamnı silen metot
        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey);
            return RedirectToAction("Index");
        }
    }
}
