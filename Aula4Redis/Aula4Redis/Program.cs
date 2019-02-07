using System;
using StackExchange.Redis;

namespace Aula4Redis
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect
            var redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();

            Console.WriteLine(db.StringSet("A", "1"));
            Console.WriteLine(db.StringGet("A"));            
            Console.WriteLine(db.StringIncrement("A"));
            Console.WriteLine(db.StringGet("A"));
            Console.WriteLine((db.SetAdd("TECH", "SQL")));
            Console.WriteLine((db.HashSet("CLI", "AA", "1")));
            Console.WriteLine(db.ListLeftPush("L1", "A"));
            Console.WriteLine(db.ListLeftPush("L1", "B"));
            Console.ReadKey();
            
        }
    }
}
