using System.Globalization;
using System.Text.RegularExpressions;
using FitnessTracker.Models;
using Microsoft.Data.Sqlite;

namespace FitnessTracker.Services;

public sealed class AppService : IAppService
{
    private readonly string _connectionString;
    private const int MaxFailedAttempts = 3;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(5);

    public AppService()
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "fitness_tracker.db");
        _connectionString = $"Data Source={dbPath}";
        InitializeDatabase();
    }

    public string? CurrentUsername { get; private set; }

    public bool Register(string username, string password, out string message)
    {
        username = username.Trim();

        if (!Regex.IsMatch(username, "^[A-Za-z0-9]+$"))
        {
            message = "Username must contain only letters and numbers.";
            return false;
        }

        if (!TryValidatePassword(password, out var passwordMessage))
        {
            message = passwordMessage;
            return false;
        }

        using var connection = OpenConnection();
        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT COUNT(1) FROM Users WHERE Username = $username;";
        checkCommand.Parameters.AddWithValue("$username", username);
        var exists = Convert.ToInt32(checkCommand.ExecuteScalar(), CultureInfo.InvariantCulture) > 0;
        if (exists)
        {
            message = "Username already exists.";
            return false;
        }

        using var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = """
            INSERT INTO Users (Username, Password, FailedLoginAttempts, IsLocked, GoalCalories)
            VALUES ($username, $password, 0, 0, 300);
            """;
        insertCommand.Parameters.AddWithValue("$username", username);
        insertCommand.Parameters.AddWithValue("$password", password);
        insertCommand.ExecuteNonQuery();

        message = "Registration successful. You can now log in.";
        return true;
    }

    public bool Login(string username, string password, out string message)
    {
        username = username.Trim();
        using var connection = OpenConnection();
        using var selectCommand = connection.CreateCommand();
        selectCommand.CommandText = """
            SELECT Password, FailedLoginAttempts, IsLocked, LockedUntilUtc
            FROM Users
            WHERE Username = $username;
            """;
        selectCommand.Parameters.AddWithValue("$username", username);

        using var reader = selectCommand.ExecuteReader();
        if (!reader.Read())
        {
            message = "Invalid username or password.";
            return false;
        }

        var storedPassword = reader.GetString(0);
        var failedAttempts = reader.GetInt32(1);
        var isLocked = reader.GetInt32(2) == 1;
        var lockedUntilRaw = reader.IsDBNull(3) ? null : reader.GetString(3);

        if (isLocked)
        {
            if (TryParseUtc(lockedUntilRaw, out var lockedUntilUtc) && DateTime.UtcNow < lockedUntilUtc)
            {
                var remaining = lockedUntilUtc - DateTime.UtcNow;
                var remainingMinutes = Math.Max(1, (int)Math.Ceiling(remaining.TotalMinutes));
                message = $"Account is locked. Try again in about {remainingMinutes} minute(s).";
                return false;
            }

            // Lock duration expired, unlock automatically.
            using var unlockCommand = connection.CreateCommand();
            unlockCommand.CommandText = """
                UPDATE Users
                SET FailedLoginAttempts = 0,
                    IsLocked = 0,
                    LockedUntilUtc = NULL
                WHERE Username = $username;
                """;
            unlockCommand.Parameters.AddWithValue("$username", username);
            unlockCommand.ExecuteNonQuery();

            failedAttempts = 0;
            isLocked = false;
        }

        if (!string.Equals(storedPassword, password, StringComparison.Ordinal))
        {
            failedAttempts++;
            var lockAccount = failedAttempts >= MaxFailedAttempts;

            using var updateCommand = connection.CreateCommand();
            updateCommand.CommandText = """
                UPDATE Users
                SET FailedLoginAttempts = $failedAttempts,
                    IsLocked = $isLocked,
                    LockedUntilUtc = $lockedUntil
                WHERE Username = $username;
                """;
            updateCommand.Parameters.AddWithValue("$failedAttempts", failedAttempts);
            updateCommand.Parameters.AddWithValue("$isLocked", lockAccount ? 1 : 0);
            updateCommand.Parameters.AddWithValue("$lockedUntil",
                lockAccount ? DateTime.UtcNow.Add(LockoutDuration).ToString("O", CultureInfo.InvariantCulture) : DBNull.Value);
            updateCommand.Parameters.AddWithValue("$username", username);
            updateCommand.ExecuteNonQuery();

            if (lockAccount)
            {
                message = $"Account locked after {MaxFailedAttempts} failed attempts. Try again in about {(int)LockoutDuration.TotalMinutes} minute(s).";
                return false;
            }

            message = $"Invalid username or password. Attempts left: {MaxFailedAttempts - failedAttempts}";
            return false;
        }

        using var resetCommand = connection.CreateCommand();
        resetCommand.CommandText = """
            UPDATE Users
            SET FailedLoginAttempts = 0,
                IsLocked = 0,
                LockedUntilUtc = NULL
            WHERE Username = $username;
            """;
        resetCommand.Parameters.AddWithValue("$username", username);
        resetCommand.ExecuteNonQuery();

        CurrentUsername = username;
        message = "Login successful.";
        return true;
    }

    public void Logout()
    {
        CurrentUsername = null;
    }

    public bool SetGoal(double calories, out string message)
    {
        if (CurrentUsername is null)
        {
            message = "Please log in first.";
            return false;
        }

        if (calories <= 0)
        {
            message = "Goal must be greater than 0.";
            return false;
        }

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Users
            SET GoalCalories = $goal
            WHERE Username = $username;
            """;
        command.Parameters.AddWithValue("$goal", calories);
        command.Parameters.AddWithValue("$username", CurrentUsername);
        command.ExecuteNonQuery();

        message = "Goal saved successfully.";
        return true;
    }

    public double GetGoal()
    {
        if (CurrentUsername is null)
        {
            return 0;
        }

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT GoalCalories FROM Users WHERE Username = $username;";
        command.Parameters.AddWithValue("$username", CurrentUsername);
        var result = command.ExecuteScalar();
        return result is null || result == DBNull.Value
            ? 0
            : Convert.ToDouble(result, CultureInfo.InvariantCulture);
    }

    public double GetTotalCalories()
    {
        if (CurrentUsername is null)
        {
            return 0;
        }

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COALESCE(SUM(Calories), 0) FROM ActivityRecords WHERE Username = $username;";
        command.Parameters.AddWithValue("$username", CurrentUsername);
        var result = command.ExecuteScalar();
        return Convert.ToDouble(result, CultureInfo.InvariantCulture);
    }

    public bool IsGoalAchieved()
    {
        var goal = GetGoal();
        if (goal <= 0)
        {
            return false;
        }

        return GetTotalCalories() >= goal;
    }

    public bool RecordActivity(string activityName, double metric1, double metric2, double metric3, out double calories, out string message)
    {
        calories = 0;
        if (CurrentUsername is null)
        {
            message = "Please log in first.";
            return false;
        }

        if (metric1 < 0 || metric2 < 0 || metric3 < 0)
        {
            message = "Metrics must be valid positive values.";
            return false;
        }

        calories = CalculateCalories(activityName, metric1, metric2, metric3);
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO ActivityRecords
            (Username, ActivityName, Metric1, Metric2, Metric3, Calories, LoggedAt)
            VALUES
            ($username, $activityName, $metric1, $metric2, $metric3, $calories, $loggedAt);
            """;
        command.Parameters.AddWithValue("$username", CurrentUsername);
        command.Parameters.AddWithValue("$activityName", activityName);
        command.Parameters.AddWithValue("$metric1", metric1);
        command.Parameters.AddWithValue("$metric2", metric2);
        command.Parameters.AddWithValue("$metric3", metric3);
        command.Parameters.AddWithValue("$calories", calories);
        command.Parameters.AddWithValue("$loggedAt", DateTime.Now.ToString("O", CultureInfo.InvariantCulture));
        command.ExecuteNonQuery();

        message = $"Activity saved. Calories burned: {calories:F1}";
        return true;
    }

    public IReadOnlyList<ActivityRecord> GetRecentActivities(int count = 5)
    {
        if (CurrentUsername is null)
        {
            return [];
        }

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Username, ActivityName, Metric1, Metric2, Metric3, Calories, LoggedAt
            FROM ActivityRecords
            WHERE Username = $username
            ORDER BY LoggedAt DESC
            LIMIT $count;
            """;
        command.Parameters.AddWithValue("$username", CurrentUsername);
        command.Parameters.AddWithValue("$count", count);

        using var reader = command.ExecuteReader();
        var records = new List<ActivityRecord>();
        while (reader.Read())
        {
            records.Add(new ActivityRecord
            {
                Username = reader.GetString(0),
                ActivityName = reader.GetString(1),
                Metric1 = reader.GetDouble(2),
                Metric2 = reader.GetDouble(3),
                Metric3 = reader.GetDouble(4),
                Calories = reader.GetDouble(5),
                LoggedAt = DateTime.Parse(reader.GetString(6), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
            });
        }

        return records;
    }

    private static bool TryValidatePassword(string password, out string message)
    {
        if (!password.Any(char.IsUpper))
        {
            message = "Password must contain at least one uppercase letter.";
            return false;
        }

        if (!password.Any(char.IsLower))
        {
            message = "Password must contain at least one lowercase letter.";
            return false;
        }

        message = string.Empty;
        return true;
    }

    private static double CalculateCalories(string activityName, double metric1, double metric2, double metric3)
    {
        return activityName switch
        {
            "Walking" => (metric1 * 0.04) + (metric2 * 35) + (metric3 * 1.5),
            "Swimming" => (metric1 * 8) + (metric2 * 6) + (metric3 * 0.3),
            "Running" => (metric1 * 50) + (metric2 * 5) + (metric3 * 0.25),
            "Cycling" => (metric1 * 30) + (metric2 * 4) + (metric3 * 2),
            "Weight Training" => (metric1 * 12) + (metric2 * 0.5) + (metric3 * 3),
            "Yoga" => (metric1 * 3) + (metric2 * 0.2) + (metric3 * 8),
            _ => (metric1 + metric2 + metric3) * 2
        };
    }

    private SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private void InitializeDatabase()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Users (
                Username TEXT PRIMARY KEY,
                Password TEXT NOT NULL,
                FailedLoginAttempts INTEGER NOT NULL DEFAULT 0,
                IsLocked INTEGER NOT NULL DEFAULT 0,
                LockedUntilUtc TEXT NULL,
                GoalCalories REAL NOT NULL DEFAULT 300
            );

            CREATE TABLE IF NOT EXISTS ActivityRecords (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                ActivityName TEXT NOT NULL,
                Metric1 REAL NOT NULL,
                Metric2 REAL NOT NULL,
                Metric3 REAL NOT NULL,
                Calories REAL NOT NULL,
                LoggedAt TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();

        EnsureLockedUntilColumnExists(connection);
    }

    private static bool TryParseUtc(string? value, out DateTime utcValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            utcValue = default;
            return false;
        }

        if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed))
        {
            utcValue = default;
            return false;
        }

        utcValue = parsed.Kind == DateTimeKind.Utc ? parsed : parsed.ToUniversalTime();
        return true;
    }

    private static void EnsureLockedUntilColumnExists(SqliteConnection connection)
    {
        using var pragmaCommand = connection.CreateCommand();
        pragmaCommand.CommandText = "PRAGMA table_info(Users);";

        using var reader = pragmaCommand.ExecuteReader();
        var hasColumn = false;
        while (reader.Read())
        {
            var columnName = reader.GetString(1);
            if (string.Equals(columnName, "LockedUntilUtc", StringComparison.OrdinalIgnoreCase))
            {
                hasColumn = true;
                break;
            }
        }

        if (hasColumn)
        {
            return;
        }

        using var alterCommand = connection.CreateCommand();
        alterCommand.CommandText = "ALTER TABLE Users ADD COLUMN LockedUntilUtc TEXT NULL;";
        alterCommand.ExecuteNonQuery();
    }
}
