using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        private ConnectionMultiplexer _redis;
        public IDatabase db { get; set; }


        //appsettings.json dosyasındaki Redis bilgilerini almak için IConfiguration nesnesini kullanıyoruz.
        public RedisService(IConfiguration configuration)
        {
             _redisHost= configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        //ConnectioMultiplexer ile Redis bağlantısını gerçekleştiriyoruz.
        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";
            _redis = ConnectionMultiplexer.Connect(configString);
        }

        //redis server üzerindeki veritabanlarına erişmek için kullanılır.
        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
        
    }
}
