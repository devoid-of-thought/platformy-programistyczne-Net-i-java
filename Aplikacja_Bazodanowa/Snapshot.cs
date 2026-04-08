public class Snapshot
{
    public int Id { get; set; }
    public required long Timestamp { get; set; }
    public List<Rate>? Rates { get; set; }
}

public class Rate
{
    public int Id { get; set; }
    public required string Currency { get; set; }
    public required decimal Value { get; set; }
    public int SnapshotId { get; set; }
    public Snapshot? Snapshot { get; set; }
}