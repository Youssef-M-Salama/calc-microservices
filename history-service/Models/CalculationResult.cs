public class CalculationResult
{
    public int Id { get; set; }
    public double Result { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
