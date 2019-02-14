using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SubRedis
{
    class Program
    {
        static List<string> lstOqueRespostas = new List<string>();
        static List<string> lstQualRespostas = new List<string>();
        static List<string> lstPorqueRespostas = new List<string>();
        static List<string> lstOndeRespostas = new List<string>();
        static List<string> lstQuandoRespostas = new List<string>();
        static List<string> lstQuemRespostas = new List<string>();
        static List<string> lstQuantoRespostas = new List<string>();
        static List<string> lstComplemento = new List<string>();

        static void Main(string[] args)
        {
            lstOqueRespostas.Add("O que? ");
            lstQualRespostas.Add("Qual? ");
            lstPorqueRespostas.Add("Porque ");
            lstOndeRespostas.Add("Onde o que?  ");
            lstQuandoRespostas.Add("Quando? ");
            lstQuemRespostas.Add("Quem? ");
            lstQuantoRespostas.Add("Quanto? ");

            lstComplemento.Add("talvez eu tenha entendido errado, mas a resposta da vida sempre foi 42.");
            lstComplemento.Add("minha preferência nunca foi relogio de pulso, acho que é uma perda de tempo.");
            lstComplemento.Add("a vida é simples, mas a pergunta é complicada.");
            lstComplemento.Add("juro que não consegue desenvolver uma linha de raciocinio com essa pergunta.");
            lstComplemento.Add("talvez ele, mas ai teriamos que realmente confirmar com ele ok?");
            lstComplemento.Add("poxa nesses milisegundos acabei de ler barça inteira e não encontrei.");
            lstComplemento.Add("Deus QUIS!");
            lstComplemento.Add("sério não consegui entender sua pergunta.");
            lstComplemento.Add("entrei em contato com a Cortana no caso minha mãe e a Siri, no caso minha tia e ninguém soube me dizer.");
            lstComplemento.Add("Um sábio uma vez me ensinou que o silêncio é a melhor resposta.");
            lstComplemento.Add("Só sei que nada sei");
            lstComplemento.Add("Você é a doença eu sou a cura.");
            lstComplemento.Add("Em espanhol talvez fique melhor, \"Hasta la vista Baby.\"");
            lstComplemento.Add("Em inglês talvez fique melhor, \"Be cool.\"");
            lstComplemento.Add("Posso cantar uma música curtinha. \"Não tinha medo tal João do Santo Cristo era o que todos diziam quando ele se perdeu...\"");
            lstComplemento.Add("Essa eu não sei, mas o Pedro ta doente.");
            lstComplemento.Add("Antes tava ruim, agora parece que piorou.");
            lstComplemento.Add("Estão me censurando 3,1415926535897932.");
            lstComplemento.Add("Não sei contar.");

            var redis = ConnectionMultiplexer.Connect("40.122.106.36");
            //var redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();

            var sub = redis.GetSubscriber();

            sub.Subscribe("perguntas", (ch, msg) =>
            {
                db.HashSet(ObterIdPergunta(msg), "PedroCriadoPelaVó", ObterResposta(msg));
            });

            Console.ReadKey();
        }

        public static string ObterPergunta(string msg)
        {
            Regex obterPergunta = new Regex(@"(?<=:\s*).*\s*$");
            Match resultado = obterPergunta.Match(msg);

            return resultado.Value;
        }

        public static string ObterResposta(string msg)
        {
            string pergunta = ObterPergunta(msg);

            if (msg.ToUpper().Contains("O QUE"))
                return lstOqueRespostas[0].ToString() + lstComplemento[new Random().Next(lstComplemento.Count + 1)];
            else if (msg.ToUpper().Contains("QUAL"))
                return lstQualRespostas[0] + lstComplemento[new Random().Next(lstComplemento.Count + 1)];
            else if (msg.ToUpper().Contains("POR QUE"))
                return lstPorqueRespostas[0] + lstComplemento[new Random().Next(lstComplemento.Count + 1)];
            else if (msg.ToUpper().Contains("ONDE"))
                return lstOndeRespostas[0] + lstComplemento[new Random().Next(lstComplemento.Count + 1)];
            else if (msg.ToUpper().Contains("QUANDO"))
                return lstQuandoRespostas[0] + lstComplemento[new Random().Next(lstComplemento.Count + 1)];
            else if (msg.ToUpper().Contains("QUEM"))
                return lstQuemRespostas[0] + lstComplemento[new Random().Next(lstComplemento.Count + 1)];
            else if (msg.ToUpper().Contains("QUANTO"))
            {
                int numero1 = 0;
                int numero2 = 0;

                numero1 = Convert.ToInt32(Regex.Match(msg, @"\d{1,3}(?=\+)").Value);
                numero2 = Convert.ToInt32(Regex.Match(msg, @"(?<=\+)\d{1,3}").Value);

                string soma = (numero1 + numero2).ToString();
                string multiplicacao = (numero1 * numero2).ToString();
                string divisao = (numero1 / numero2).ToString();
                string subtracao = (numero1 - numero2).ToString();

                return $"A soma é {soma} ";
                    //, mas se você quiser a multiplicação é : {multiplicacao} a divisão é: {divisao} e a subtração é: {subtracao} e uma frase pra apaziguar: {lstComplemento[new Random().Next(lstComplemento.Count)]}";

            }                
            else
                return "Sei não pai, só sei que foi assim.";
        }

        public static string ObterIdPergunta(string msg)
        {
            Regex obterIdPergunta = new Regex(@".*(?=:)");
            Match resultado = obterIdPergunta.Match(msg);
            return resultado.Value;
        }
    }

}
