using FitnessTracker.Models;

namespace FitnessTracker.Services;

public interface IActivityDefinitionFactory
{
    IReadOnlyList<ActivityDefinition> CreateAll();
}
