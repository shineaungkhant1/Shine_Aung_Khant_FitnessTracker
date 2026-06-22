namespace FitnessTracker.Models;

public sealed class UserAccount
{
    public required string Username { get; init; }
    public required string Password { get; set; }
    public int FailedLoginAttempts { get; set; }
    public bool IsLocked { get; set; }
    public double GoalCalories { get; set; }
}
