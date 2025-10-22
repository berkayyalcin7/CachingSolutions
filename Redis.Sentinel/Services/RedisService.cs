using StackExchange.Redis;

namespace Redis.Sentinel.Services
{
    public class RedisService
    {
        static ConfigurationOptions sentinelOptions => new()
        {
            EndPoints =
            {
                { "localhost",26379 },
                { "localhost",26380 },
                { "localhost",26381 }
            },
            CommandMap = CommandMap.Sentinel,
            AbortOnConnectFail = false
        };

        static ConfigurationOptions masterOptions => new()
        {
            AbortOnConnectFail = false
        };

        public void RedisMasterDatabase()
        {
            var sentinelConnection = ConnectionMultiplexer.Connect(sentinelOptions);
            System.Net.EndPoint masterEndpoint = null;
            foreach (var endpoint in sentinelConnection.GetEndPoints())
            {
                IServer server = sentinelConnection.GetServer(endpoint);

                if (!server.IsConnected)
                {
                    continue;
                }
                masterEndpoint = server.SentinelGetMasterAddressByName("mymaster");
                break;
            }

            // Docker konfigürasyonu
            var localMasterIp = masterEndpoint.ToString() switch
            {
                "172.20.0.10:6379"=>"localhost:6379",
                "172.20.0.11:6379" => "localhost:6380",
                "172.20.0.12:6379" => "localhost:6381",
                "172.20.0.13:6379" => "localhost:6382",
            };
        }

    }
}
