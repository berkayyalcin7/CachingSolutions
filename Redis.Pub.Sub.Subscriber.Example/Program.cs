using StackExchange.Redis;

namespace Redis.Pub.Sub.Subscriber.Example
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Connecting to Redis...");
                
                var connectionRedis = await ConnectionMultiplexer.ConnectAsync("localhost:6379", options =>
                {
                    options.AbortOnConnectFail = false;
                    options.ConnectTimeout = 10000; // 10 saniye
                    options.SyncTimeout = 5000; // 5 saniye
                    options.AsyncTimeout = 5000; // 5 saniye
                    options.ConnectRetry = 3; // 3 kez dene
                    options.ReconnectRetryPolicy = new ExponentialRetry(1000); // Exponential backoff
                });

                if (connectionRedis.IsConnected)
                {
                    Console.WriteLine("✅ Successfully connected to Redis!");
                }
                else
                {
                    Console.WriteLine("❌ Failed to connect to Redis!");
                    return;
                }

                var subscriber = connectionRedis.GetSubscriber();

                // dinle- 
                await subscriber.SubscribeAsync("mychannel.*", (channel, message) =>
                {
                    Console.WriteLine($"📨 Received message: {message}");
                });

                Console.WriteLine("🎧 Listening for messages on 'mychannel'...");
                Console.WriteLine("Press any key to stop listening...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine("Make sure Redis is running on localhost:6379");
            }
            finally
            {
                Console.WriteLine("Subscriber stopped.");
            }
        }
    }
}
