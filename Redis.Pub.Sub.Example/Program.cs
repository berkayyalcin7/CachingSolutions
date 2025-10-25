using StackExchange.Redis;

namespace Redis.Pub.Sub.Example
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
                    options.ConnectTimeout = 20000; // 10 saniye
                    options.SyncTimeout = 10000; // 5 saniye
                    options.AsyncTimeout = 10000; // 5 saniye
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

                // Paylaş
                while (true)
                {
                    Console.WriteLine("Type a message to publish (or 'exit' to quit): ");
                    var message = Console.ReadLine();

                    if (message?.ToLower() == "exit")
                    {
                        break;
                    }

                    if (!string.IsNullOrEmpty(message))
                    {
                        var subscribersCount = await subscriber.PublishAsync("mychannel", message);
                        Console.WriteLine($"📤 Message published to {subscribersCount} subscribers");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine("Make sure Redis is running on localhost:6379");
            }
            finally
            {
                Console.WriteLine("Publisher stopped.");
            }
        }
    }
}
