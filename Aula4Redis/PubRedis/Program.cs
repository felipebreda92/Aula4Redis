using StackExchange.Redis;
using System;

namespace PubRedis
{
    class Program
    {
        static void Main(string[] args)
        {

            // Connect
            var redis = ConnectionMultiplexer.Connect("40.77.30.246");
            IDatabase db = redis.GetDatabase();
            var sub = redis.GetSubscriber();
            sub.Publish("FIAP14", "Teste Publish");
        }
    }
}
