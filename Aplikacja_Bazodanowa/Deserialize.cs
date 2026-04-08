
using System.Text.Json.Serialization;
public class Deserialized
{
    [JsonPropertyName("disclaimer")]
    public string? Disclaimer { get; set; }

    [JsonPropertyName("license")]
    public string? License { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    [JsonPropertyName("base")]
    public string? Base { get; set; }

    [JsonPropertyName("rates")]
public Dictionary<string, decimal>? Rates { get; set; }

    public override string ToString()
    {
        return $"Disclaimer: {Disclaimer}, License: {License}, Timestamp: {Timestamp}, Base: {Base}, Rates: {string.Join(", ", Rates?.Select(kv => $"{kv.Key}: {kv.Value}") ?? new List<string>())}";
    }
}
