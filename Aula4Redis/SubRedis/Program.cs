using StackExchange.Redis;
using System;
using System.Text.RegularExpressions;

namespace SubRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            //var redis = ConnectionMultiplexer.Connect("40.77.30.246");
            var redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();
                        
            var sub = redis.GetSubscriber();

            sub.Subscribe("PERGUNTAS", (ch, msg) =>
            {
                db.HashSet(ObterIdPergunta(msg), "EQUIPE2", ObterPergunta(msg));
            });
            Console.ReadKey();
        }

        public static string ObterPergunta(string msg)
        {
            Regex obterPergunta = new Regex(@"(?<=:\s*).*\s*$");
            Match resultado = obterPergunta.Match(msg);
            return resultado.Value;
        }

        public static string ObterIdPergunta(string msg)
        {
            Regex obterIdPergunta = new Regex(@".*(?=:)");
            Match resultado = obterIdPergunta.Match(msg);
            return resultado.Value;            
        }
    }

}
