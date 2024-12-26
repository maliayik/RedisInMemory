using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "hashtypenames";
        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
           Dictionary<string,string> list = new Dictionary<string, string>();

            if (db.KeyExists(listKey))
            {
                db.HashGetAll(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }


            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            db.HashSet(listKey, name, value);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.HashDelete(listKey, name);
            return RedirectToAction("Index");
        }


    }
}
