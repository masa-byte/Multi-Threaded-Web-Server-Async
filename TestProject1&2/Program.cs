using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.ReadKey();
        int req = 4;

        string baseUrl = "http://localhost:5000/?"; 
        string[] r = new string[req];
        r[0] = "q=Taylor Swift&type=track&limit=10";
        r[1] = "q=track:Sunflower&type=track&limit=5";
        r[2] = "q=Selena Gomez&type=track&limit=15";
        r[3] = "q=Harry Styles&type=artist&limit=10";


        int numRequests = 30;
        Console.WriteLine("Round1");

        var client = new HttpClient();
        var tasks = new List<Task>();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < numRequests; i++)
        {
            int local = i;
            tasks.Add(Task.Run(async () =>
            {
                var response = await client.GetAsync(baseUrl + r[local % req]);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(response.IsSuccessStatusCode);
                Console.WriteLine(response.StatusCode);
            }));
        }

        await Task.WhenAll(tasks);

        stopwatch.Stop();
        Console.WriteLine($"Non cached requests time {stopwatch.Elapsed.TotalMilliseconds}ms");

        Thread.Sleep(3000);

        Console.WriteLine("Round2");

        client = new HttpClient();
        tasks = new List<Task>();

        stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < numRequests; i++)
        {
            int local = i;
            tasks.Add(Task.Run(async () =>
            {
                var response = await client.GetAsync(baseUrl + r[local % req]);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(response.IsSuccessStatusCode);
                Console.WriteLine(response.StatusCode);
            }));
        }

        await Task.WhenAll(tasks);

        stopwatch.Stop();
        Console.WriteLine($"Cached requests time {stopwatch.Elapsed.TotalMilliseconds}ms");

        var response = await client.GetAsync("http://localhost:5000/v1/search?q=Taylor Swift&limit=10");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response.IsSuccessStatusCode);
        Console.WriteLine(response.StatusCode);
        Console.WriteLine(await response.Content.ReadAsStringAsync());

        response = await client.GetAsync("http://localhost:5000/v1/search?q=iqhksdohuaweba&type=track&limit=10");
        content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response.IsSuccessStatusCode);
        Console.WriteLine(response.StatusCode);
        Console.WriteLine(await response.Content.ReadAsStringAsync());

    }
}
