using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PollyHttpExtensions
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var httpCliente = new HttpClient() { Timeout = TimeSpan.FromMinutes(1) };


            var retryConfiguration = HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TaskCanceledException>()
                    .WaitAndRetryAsync(new TimeSpan[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(12), TimeSpan.FromMinutes(1) } );

            int i = 3;

            var teste = await retryConfiguration.ExecuteAndCaptureAsync(async () => await httpCliente.GetAsync("https://localhost:44320/testes-espera?minutos=1"));
            if (teste.Outcome == OutcomeType.Successful)
                Console.WriteLine(await teste.Result.Content.ReadAsStringAsync());
            else
                Console.WriteLine("Erro");
            Console.ReadLine();
        }
    }
}
