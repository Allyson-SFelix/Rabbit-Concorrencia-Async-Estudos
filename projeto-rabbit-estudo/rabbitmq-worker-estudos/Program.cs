using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using rabbitMQaprendendo;

namespace rabbitMQaprendendo
{
    internal class Program
    {
        static async Task  Main(string[] args)
        {
            // recebendo chamada da API
            List<Job> listaJob = Job.CriandoSituacao();
            Console.WriteLine("[PROGRAM] CRIANDO JOBS");
            await Teste(listaJob);

            Console.WriteLine("[PROGRAM] PÓS ESCALONAR");

            Console.ReadLine();
        }

        public static async Task Teste(List<Job> listaJob)
        {
            Console.WriteLine("[PROGRAM] ENTRANDO NO TESTE");


            Scheduler scheduler = new Scheduler(); //configurado como singleton no program.cs do ASP.NET Core

            _ = scheduler.Escalonar(); //deixa rodar sem precisar esperar o resultado
            
            foreach (Job job in listaJob)
            {
                scheduler.AddJob(job);
            }



            Console.WriteLine("[PROGRAM] SAINDO DO TESTE");
        }
    }
}