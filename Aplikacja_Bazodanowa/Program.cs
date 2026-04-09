using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

public class ApiTest
{
    private static readonly HttpClient Client = new HttpClient();
    public async Task<string> GetData(string accessToken, string date)
    {
        string call = $"https://openexchangerates.org/api/historical/{date}.json?app_id={accessToken}";
        var response = await Client.GetStringAsync(call);
        return response;
    }
}

internal class Program
{
    private static async Task Main()
    {
        using var db = new CurrencyRates();
        var apiKey = new ApiKey().GetKey();
        ApiTest apiTest = new ApiTest();

        Console.WriteLine("Podaj date dla kursu (format YYYY-MM-DD, np. 2026-04-06):");
        string? date = Console.ReadLine();

        if (string.IsNullOrEmpty(date))
        {
            Console.WriteLine("Bledna data.");
            return;
        }

        try 
        {
            string json = await apiTest.GetData(apiKey, date);
            Deserialized? deserialized = JsonSerializer.Deserialize<Deserialized>(json);

            if (deserialized != null)
            {
                var exists = db.Snapshots.Any(s => s.Timestamp == deserialized.Timestamp);

                if (!exists)
                {
                    var snapshot = new Snapshot
                    {
                        Timestamp = deserialized.Timestamp,
                        Rates = deserialized.Rates?.Select(pair => new Rate
                        {
                            Currency = pair.Key,
                            Value = pair.Value
                        }).ToList() ?? new List<Rate>()
                    };

                    db.Snapshots.Add(snapshot);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Pobrano i zapisano nowe dane.");
                }
                else
                {
                    Console.WriteLine("Dane dla tej daty juz sa w bazie.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Blad: {ex.Message}");
        }

        var savedSnapshot = db.Snapshots.Include(s => s.Rates).OrderByDescending(s => s.Timestamp).FirstOrDefault();
        if (savedSnapshot != null && savedSnapshot.Rates != null)
        {
            Console.WriteLine($"\nKursy z dnia (Timestamp): {savedSnapshot.Timestamp}");
            foreach (var rate in savedSnapshot.Rates.OrderBy(r => r.Currency))
            {
                Console.WriteLine($"{rate.Currency}: {rate.Value}");
            }
        }
    }
}