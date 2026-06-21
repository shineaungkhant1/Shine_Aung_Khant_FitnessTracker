namespace FitnessTracker.Models;

public sealed class ActivityDefinition
{
    public required string Name { get; init; }
    public required string Metric1Label { get; init; }
    public required string Metric2Label { get; init; }
    public required string Metric3Label { get; init; }
}
