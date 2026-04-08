using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
public class ApiTest
{
    private static readonly HttpClient Client = new HttpClient();
    public async Task<string> GetData(string accessToken)
    {

        string call = $"https://openexchangerates.org/api/historical/{DateTime.UtcNow:yyyy-MM-dd}.json?app_id={accessToken}";
        var response = await Client.GetStringAsync(call);

        return response;
    }
}



internal class Program
{
    
    private static async Task Main()
    {
        using var db = new CurrencyRates();
        var accessToken = new ApiKey().GetKey();
        ApiTest apiTest = new ApiTest();
        string json = await apiTest.GetData(accessToken);
        Deserialized? deserialized = JsonSerializer.Deserialize<Deserialized>(json);

        if (deserialized != null)
        {
            var latestSnapshot = db.Snapshots.OrderByDescending(s => s.Timestamp).FirstOrDefault();

            if (latestSnapshot == null || latestSnapshot.Timestamp != deserialized.Timestamp)
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

                Console.WriteLine("Pobrano nowe dane. Zaktualizowano baze.");
            }
            else
            {
                Console.WriteLine("Dane w bazie sa juz aktualne. Pominiento zapis.");
            }
        }
        var savedSnapshot = db.Snapshots.Include(s => s.Rates).OrderByDescending(s => s.Timestamp).FirstOrDefault();

        if (savedSnapshot != null)
        {
            Console.WriteLine($"Czas pobrania (Timestamp): {savedSnapshot.Timestamp}");

            
            var sortedRates = savedSnapshot.Rates
                .OrderBy(r => r.Currency)
                .ToList();

            foreach (var rate in sortedRates)
            {
                Console.WriteLine($"Waluta: {rate.Currency}, Kurs: {rate.Value}");
            }
        }
    }
}