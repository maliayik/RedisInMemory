using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            //redis servis üzerinde keylerimizi kaydediyoruz.
            db.StringSet("name", "Mehmet Ali AYIK");
            db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            var name = db.StringGet("name");

            //ziyaretci sayısını 1 arttırır.
            db.StringIncrement("ziyaretci", 1);

            if (name.HasValue)
            {
                ViewBag.name = name.ToString();
                ViewBag.ziyaretci = db.StringGet("ziyaretci");
            }

            return View();
        }
    }
}
