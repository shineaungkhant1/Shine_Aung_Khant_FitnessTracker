using FitnessTracker.Models;

namespace FitnessTracker.Services;

public interface IAppService
{
    bool Register(string username, string password, out string message);
    bool Login(string username, string password, out string message);
    void Logout();
    string? CurrentUsername { get; }

    bool SetGoal(double calories, out string message);
    double GetGoal();
    double GetTotalCalories();
    bool IsGoalAchieved();

    bool RecordActivity(string activityName, double metric1, double metric2, double metric3, out double calories, out string message);
    IReadOnlyList<ActivityRecord> GetRecentActivities(int count = 5);
}
