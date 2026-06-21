using FitnessTracker.Models;

namespace FitnessTracker.Services;

public sealed class ActivityDefinitionFactory : IActivityDefinitionFactory
{
    public IReadOnlyList<ActivityDefinition> CreateAll()
    {
        return new List<ActivityDefinition>
        {
            new()
            {
                Name = "Walking",
                Metric1Label = "Steps",
                Metric2Label = "Distance (km)",
                Metric3Label = "Time Taken (minutes)"
            },
            new()
            {
                Name = "Swimming",
                Metric1Label = "Number of Laps",
                Metric2Label = "Time Taken (minutes)",
                Metric3Label = "Average Heart Rate (bpm)"
            },
            new()
            {
                Name = "Running",
                Metric1Label = "Distance (km)",
                Metric2Label = "Time Taken (minutes)",
                Metric3Label = "Average Heart Rate (bpm)"
            },
            new()
            {
                Name = "Cycling",
                Metric1Label = "Distance (km)",
                Metric2Label = "Time Taken (minutes)",
                Metric3Label = "Average Speed (km/h)"
            },
            new()
            {
                Name = "Weight Training",
                Metric1Label = "Sets",
                Metric2Label = "Total Repetitions",
                Metric3Label = "Time Taken (minutes)"
            },
            new()
            {
                Name = "Yoga",
                Metric1Label = "Duration (minutes)",
                Metric2Label = "Average Heart Rate (bpm)",
                Metric3Label = "Difficulty Level (1-10)"
            }
        };
    }
}
