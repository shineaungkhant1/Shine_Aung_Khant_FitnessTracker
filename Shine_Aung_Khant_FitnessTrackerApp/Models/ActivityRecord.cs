namespace FitnessTracker.Models;

public sealed class ActivityRecord
{
    public required string Username { get; init; }
    public required string ActivityName { get; init; }
    public required double Metric1 { get; init; }
    public required double Metric2 { get; init; }
    public required double Metric3 { get; init; }
    public required double Calories { get; init; }
    public required DateTime LoggedAt { get; init; }
}
