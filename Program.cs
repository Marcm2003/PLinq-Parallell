using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PLinq_Parallell.Models;

class Program
{
    static async Task Main(string[] args)
    {
        string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        string jsonFilePath = Path.Combine(projectDirectory, "Data", "people.json");
        string logFilePath = Path.Combine(projectDirectory, "Data", "log.txt");

        if (!File.Exists(jsonFilePath))
        {
            Console.WriteLine("The Json file does not exist.");
            Console.WriteLine($"Route: {jsonFilePath}");
            return;
        }

        List<Usuario> users = await LoadUsersAsync(jsonFilePath);

        var sequentialTime = await CheckUsers(users, logFilePath, isParallel: false);
        var parallelTime = await CheckUsers(users, logFilePath, isParallel: true);

        Console.WriteLine("Comparison of execution times:");
        Console.WriteLine($"Sequential time: {sequentialTime} ms");
        Console.WriteLine($"Parallel time: {parallelTime} ms");

        Console.ReadLine();
    }

    static async Task<List<Usuario>> LoadUsersAsync(string jsonFilePath)
    {
        string json = await File.ReadAllTextAsync(jsonFilePath);
        return JsonConvert.DeserializeObject<List<Usuario>>(json);
    }

    static async Task<long> CheckUsers(List<Usuario> users, string logFilePath, bool isParallel)
    {
        string mode = isParallel ? "parallel" : "sequential";
        long totalTime = 0;

        using (StreamWriter writer = new StreamWriter(logFilePath, append: true))
        {
            writer.WriteLine($"Checking users {mode}ly...");
            var watch = System.Diagnostics.Stopwatch.StartNew();

            foreach (var user in users)
            {
                await CheckUser(user, writer, isParallel);
            }

            watch.Stop();
            totalTime = watch.ElapsedMilliseconds;
            writer.WriteLine($"Total time: {totalTime} ms");
        }

        return totalTime;
    }

    static async Task CheckUser(Usuario user, StreamWriter writer, bool isParallel)
    {
        string mode = isParallel ? "parallel" : "sequential";
        string message = $"* Checking user {user.Name} {user.Surname}...";
        Console.WriteLine(message);
        writer?.WriteLine(message);

        bool dniValido = user.comprova_dni();
        bool mailValido = user.comprova_mail();
        bool nomValido = user.comprova_nom();

        message = $"[{mode}] DNI: {dniValido}, Mail: {mailValido}, Name: {nomValido}";
        Console.WriteLine(message);
        writer?.WriteLine(message);
    }
}
