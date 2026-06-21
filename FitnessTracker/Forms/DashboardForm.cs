using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class DashboardForm : BaseForm
{
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;
    private readonly string _displayName;

    public DashboardForm(IActivityDefinitionFactory activityDefinitionFactory, string username)
    {
        _activityDefinitionFactory = activityDefinitionFactory;
        _displayName = string.IsNullOrWhiteSpace(username) ? "Guest" : username;

        Text = "Fitness Tracker - Dashboard";
        ClientSize = new Size(1020, 620);

        BuildLayout();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(24),
            BackColor = AppTheme.Tertiary
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));

        var leftCard = CreateCardPanel();
        leftCard.Dock = DockStyle.Fill;

        var leftContent = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 12
        };

        leftContent.Controls.Add(UiStyles.CreateSectionTitle($"Welcome, {_displayName}"));

        var introLabel = new Label
        {
            Text = "Set your calories target and monitor progress.",
            AutoSize = true,
            ForeColor = AppTheme.MutedText,
            Margin = new Padding(0, 0, 0, 16)
        };
        leftContent.Controls.Add(introLabel);

        leftContent.Controls.Add(UiStyles.CreateCaption("Calories Goal"));
        var goalTextBox = new TextBox { Width = 320, PlaceholderText = "e.g. 300" };
        UiStyles.StyleTextBox(goalTextBox);
        leftContent.Controls.Add(goalTextBox);

        var setGoalButton = new Button { Text = "Set Goal", Width = 120 };
        UiStyles.StylePrimaryButton(setGoalButton);
        leftContent.Controls.Add(setGoalButton);

        var statPanel = new Panel
        {
            BackColor = AppTheme.SurfaceSoft,
            BorderStyle = BorderStyle.FixedSingle,
            Width = 520,
            Height = 140,
            Padding = new Padding(16),
            Margin = new Padding(0, 18, 0, 18)
        };

        var stats = new Label
        {
            Text = "Total Calories Burned: 0\r\nGoal Status: Not started",
            AutoSize = true,
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 10.5F, FontStyle.Regular)
        };
        statPanel.Controls.Add(stats);
        leftContent.Controls.Add(statPanel);

        var recordButton = new Button { Text = "Record Activity", Width = 170 };
        UiStyles.StylePrimaryButton(recordButton);
        recordButton.Click += (_, _) =>
        {
            var activityForm = new ActivityForm(_activityDefinitionFactory, _displayName);
            activityForm.Show();
            Hide();
        };
        leftContent.Controls.Add(recordButton);

        leftCard.Controls.Add(leftContent);

        var rightCard = CreateCardPanel();
        rightCard.Dock = DockStyle.Fill;

        var rightContent = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4
        };

        rightContent.Controls.Add(UiStyles.CreateSectionTitle("Quick Actions"));

        var logoutButton = new Button { Text = "Logout", Width = 130 };
        UiStyles.StyleSecondaryButton(logoutButton);
        logoutButton.Click += (_, _) =>
        {
            var loginForm = new LoginForm(_activityDefinitionFactory);
            loginForm.Show();
            Hide();
        };

        var tipLabel = new Label
        {
            Text = "This dashboard UI is set. Next step is wiring database and business rules.",
            AutoSize = true,
            ForeColor = AppTheme.MutedText,
            MaximumSize = new Size(280, 0),
            Margin = new Padding(0, 0, 0, 18)
        };

        rightContent.Controls.Add(tipLabel);
        rightContent.Controls.Add(logoutButton);
        rightCard.Controls.Add(rightContent);

        root.Controls.Add(leftCard, 0, 0);
        root.Controls.Add(rightCard, 1, 0);
        Controls.Add(root);
    }
}
