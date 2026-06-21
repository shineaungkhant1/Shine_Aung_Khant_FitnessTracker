using FitnessTracker.Models;
using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class MainShellForm : BaseForm
{
    private readonly IAppService _appService;
    private readonly IReadOnlyList<ActivityDefinition> _activities;
    private readonly Panel _contentHost = new() { Dock = DockStyle.Fill };
    private readonly Label _headerTitle = new();
    private readonly Label _headerSubtitle = new();

    public MainShellForm(IAppService appService, IActivityDefinitionFactory activityDefinitionFactory)
    {
        _appService = appService;
        _activities = activityDefinitionFactory.CreateAll();

        Text = "Fitness Tracker";
        ClientSize = new Size(1240, 760);
        BuildLayout();
        ShowDashboard();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Tertiary,
            ColumnCount = 2,
            RowCount = 1
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        root.Controls.Add(BuildSidebar(), 0, 0);
        root.Controls.Add(BuildWorkspace(), 1, 0);
        Controls.Add(root);
    }

    private Control BuildSidebar()
    {
        var sidebar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Sidebar,
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 3
        };
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));

        var profile = new Panel { Dock = DockStyle.Fill, BackColor = AppTheme.Sidebar };
        profile.Controls.Add(new Label
        {
            Text = _appService.CurrentUsername ?? "Guest",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(56, 16)
        });
        profile.Controls.Add(new Label
        {
            Text = "ACTIVE SESSION",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 8.5F),
            AutoSize = true,
            Location = new Point(56, 40)
        });
        profile.Controls.Add(new Panel
        {
            BackColor = AppTheme.SurfaceSoft,
            Size = new Size(36, 36),
            Location = new Point(10, 14)
        });

        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };

        var dashboardButton = new Button { Text = "Dashboard", Width = 200 };
        UiStyles.StyleSidebarButton(dashboardButton, true);
        dashboardButton.Click += (_, _) => ShowDashboard();

        var goalsButton = new Button { Text = "Goals", Width = 200 };
        UiStyles.StyleSidebarButton(goalsButton);
        goalsButton.Click += (_, _) => ShowGoals();

        var logActivityButton = new Button { Text = "Log Activity", Width = 200 };
        UiStyles.StyleSidebarButton(logActivityButton);
        logActivityButton.Click += (_, _) => ShowActivityLogger();

        var helpButton = new Button { Text = "Help", Width = 200 };
        UiStyles.StyleSidebarButton(helpButton);
        helpButton.Click += (_, _) => ShowHelp();

        menu.Controls.Add(dashboardButton);
        menu.Controls.Add(goalsButton);
        menu.Controls.Add(logActivityButton);
        menu.Controls.Add(helpButton);

        var logoutButton = new Button { Text = "Logout", Dock = DockStyle.Fill };
        UiStyles.StyleSecondaryButton(logoutButton);
        logoutButton.Click += (_, _) =>
        {
            _appService.Logout();
            var auth = new AuthForm(_appService, new ActivityDefinitionFactory());
            auth.Show();
            Close();
        };

        sidebar.Controls.Add(profile, 0, 0);
        sidebar.Controls.Add(menu, 0, 1);
        sidebar.Controls.Add(logoutButton, 0, 2);
        return sidebar;
    }

    private Control BuildWorkspace()
    {
        var workspace = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Tertiary,
            Padding = new Padding(24),
            ColumnCount = 1,
            RowCount = 3
        };
        workspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
        workspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        workspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };

        _headerTitle.ForeColor = AppTheme.Neutral;
        _headerTitle.Font = new Font(AppTheme.FontFamily, 28F, FontStyle.Bold);
        _headerTitle.AutoSize = true;
        _headerSubtitle.ForeColor = AppTheme.MutedText;
        _headerSubtitle.Font = new Font(AppTheme.FontFamily, 11F);
        _headerSubtitle.AutoSize = true;
        header.Controls.Add(_headerTitle, 0, 0);
        header.Controls.Add(_headerSubtitle, 0, 1);

        var status = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Surface,
            ColumnCount = 2
        };
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        status.Controls.Add(new Label
        {
            Text = "System Online",
            ForeColor = AppTheme.Success,
            Anchor = AnchorStyles.Left,
            AutoSize = true
        }, 0, 0);
        status.Controls.Add(new Label
        {
            Text = $"Logged in: {_appService.CurrentUsername}",
            ForeColor = AppTheme.MutedText,
            Anchor = AnchorStyles.Right,
            AutoSize = true
        }, 1, 0);

        workspace.Controls.Add(header, 0, 0);
        workspace.Controls.Add(_contentHost, 0, 1);
        workspace.Controls.Add(status, 0, 2);
        return workspace;
    }

    private void ShowDashboard()
    {
        _headerTitle.Text = "Dashboard Overview";
        _headerSubtitle.Text = "Track totals, progress, and latest activities.";

        var totalCalories = _appService.GetTotalCalories();
        var goal = _appService.GetGoal();
        var goalStatus = _appService.IsGoalAchieved() ? "Achieved" : "In Progress";

        var page = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2
        };
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68));
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 220));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var summaryCard = CreateCardPanel();
        summaryCard.Dock = DockStyle.Fill;
        summaryCard.Controls.Add(new Label
        {
            Text = $"DAILY GOAL\n{totalCalories:F1} / {goal:F1} kcal\nStatus: {goalStatus}",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 18F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 24)
        });

        var right = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        var totalCard = UiStyles.CreateMetricCard("TOTAL CALORIES", $"{totalCalories:F1}", "kcal");
        totalCard.Dock = DockStyle.Fill;
        var goalCard = UiStyles.CreateMetricCard("GOAL TARGET", $"{goal:F1}", "kcal");
        goalCard.Dock = DockStyle.Fill;
        right.Controls.Add(totalCard, 0, 0);
        right.Controls.Add(goalCard, 0, 1);

        var recentCard = CreateCardPanel();
        recentCard.Dock = DockStyle.Fill;
        recentCard.Controls.Add(new Label
        {
            Text = "Recent Activities",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 14F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 16)
        });
        var recent = _appService.GetRecentActivities(6);
        var y = 52;
        foreach (var record in recent)
        {
            recentCard.Controls.Add(new Label
            {
                Text = $"{record.ActivityName} | {record.Calories:F1} kcal | {record.LoggedAt:g}",
                ForeColor = AppTheme.MutedText,
                Font = new Font(AppTheme.FontFamily, 9.5F),
                AutoSize = true,
                Location = new Point(24, y)
            });
            y += 26;
        }

        page.Controls.Add(summaryCard, 0, 0);
        page.Controls.Add(right, 1, 0);
        page.Controls.Add(recentCard, 0, 1);
        page.SetColumnSpan(recentCard, 2);

        RenderPage(page);
    }

    private void ShowGoals()
    {
        _headerTitle.Text = "Goal Management";
        _headerSubtitle.Text = "Set and save your personalized calorie target.";

        var page = CreateCardPanel();
        page.Dock = DockStyle.Fill;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 8
        };
        layout.Controls.Add(UiStyles.CreateSectionTitle("Target Calorie Goal"));

        var currentGoal = new Label
        {
            Text = $"Current Goal: {_appService.GetGoal():F1} kcal",
            ForeColor = AppTheme.Primary,
            AutoSize = true
        };
        layout.Controls.Add(currentGoal);

        layout.Controls.Add(UiStyles.CreateCaption("New Goal (kcal)"));
        var goalBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "e.g. 300" };
        UiStyles.StyleTextBox(goalBox);
        layout.Controls.Add(goalBox);

        var messageLabel = new Label { AutoSize = true, ForeColor = AppTheme.MutedText };
        layout.Controls.Add(messageLabel);

        var saveButton = new Button { Text = "Save Goal", Width = 160 };
        UiStyles.StylePrimaryButton(saveButton);
        saveButton.Click += (_, _) =>
        {
            if (!double.TryParse(goalBox.Text, out var goalValue))
            {
                messageLabel.ForeColor = AppTheme.Error;
                messageLabel.Text = "Enter a valid number.";
                return;
            }

            if (_appService.SetGoal(goalValue, out var message))
            {
                currentGoal.Text = $"Current Goal: {_appService.GetGoal():F1} kcal";
                messageLabel.ForeColor = AppTheme.Success;
                messageLabel.Text = message;
                return;
            }

            messageLabel.ForeColor = AppTheme.Error;
            messageLabel.Text = message;
        };

        layout.Controls.Add(saveButton);
        page.Controls.Add(layout);
        RenderPage(page);
    }

    private void ShowActivityLogger()
    {
        _headerTitle.Text = "Log Activity";
        _headerSubtitle.Text = "Record metrics for one activity and save calories.";

        var page = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 72));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 28));

        var formCard = CreateCardPanel();
        formCard.Dock = DockStyle.Fill;

        var formLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8
        };
        formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210));
        formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var activityCombo = new ComboBox
        {
            Dock = DockStyle.Top,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = AppTheme.SurfaceSoft,
            ForeColor = AppTheme.Neutral,
            FlatStyle = FlatStyle.Flat
        };
        activityCombo.Items.AddRange(_activities.Select(a => a.Name).ToArray());
        activityCombo.SelectedIndex = 0;

        var metric1Label = new Label { AutoSize = true, ForeColor = AppTheme.MutedText };
        var metric2Label = new Label { AutoSize = true, ForeColor = AppTheme.MutedText };
        var metric3Label = new Label { AutoSize = true, ForeColor = AppTheme.MutedText };
        var metric1Box = new TextBox { Dock = DockStyle.Top };
        var metric2Box = new TextBox { Dock = DockStyle.Top };
        var metric3Box = new TextBox { Dock = DockStyle.Top };
        UiStyles.StyleTextBox(metric1Box);
        UiStyles.StyleTextBox(metric2Box);
        UiStyles.StyleTextBox(metric3Box);

        void RefreshMetricLabels()
        {
            var selected = _activities[activityCombo.SelectedIndex];
            metric1Label.Text = selected.Metric1Label;
            metric2Label.Text = selected.Metric2Label;
            metric3Label.Text = selected.Metric3Label;
        }

        activityCombo.SelectedIndexChanged += (_, _) => RefreshMetricLabels();
        RefreshMetricLabels();

        var resultLabel = new Label { AutoSize = true, ForeColor = AppTheme.MutedText };
        var saveButton = new Button { Text = "Save Activity", Width = 170 };
        UiStyles.StylePrimaryButton(saveButton);
        saveButton.Click += (_, _) =>
        {
            if (!double.TryParse(metric1Box.Text, out var m1)
                || !double.TryParse(metric2Box.Text, out var m2)
                || !double.TryParse(metric3Box.Text, out var m3))
            {
                resultLabel.ForeColor = AppTheme.Error;
                resultLabel.Text = "Enter valid numeric values for all metrics.";
                return;
            }

            if (_appService.RecordActivity(activityCombo.Text, m1, m2, m3, out var calories, out var message))
            {
                resultLabel.ForeColor = AppTheme.Success;
                resultLabel.Text = $"{message} (Saved {calories:F1} kcal)";
                metric1Box.Clear();
                metric2Box.Clear();
                metric3Box.Clear();
                return;
            }

            resultLabel.ForeColor = AppTheme.Error;
            resultLabel.Text = message;
        };

        formLayout.Controls.Add(UiStyles.CreateCaption("Activity"), 0, 0);
        formLayout.Controls.Add(activityCombo, 1, 0);
        formLayout.Controls.Add(metric1Label, 0, 1);
        formLayout.Controls.Add(metric1Box, 1, 1);
        formLayout.Controls.Add(metric2Label, 0, 2);
        formLayout.Controls.Add(metric2Box, 1, 2);
        formLayout.Controls.Add(metric3Label, 0, 3);
        formLayout.Controls.Add(metric3Box, 1, 3);
        formLayout.Controls.Add(resultLabel, 1, 5);
        formLayout.Controls.Add(saveButton, 1, 6);
        formCard.Controls.Add(formLayout);

        var previewCard = CreateCardPanel();
        previewCard.Dock = DockStyle.Fill;
        previewCard.Controls.Add(new Label
        {
            Text = "Tip: save activity to instantly update dashboard totals and goal status.",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 24)
        });

        page.Controls.Add(formCard, 0, 0);
        page.Controls.Add(previewCard, 0, 1);
        RenderPage(page);
    }

    private void ShowHelp()
    {
        _headerTitle.Text = "Help & Guidance";
        _headerSubtitle.Text = "Quick guidance for using each feature.";

        var page = CreateCardPanel();
        page.Dock = DockStyle.Fill;
        page.Controls.Add(new Label
        {
            Text = "1) Register with letters/numbers username and 12-char password.\n" +
                   "2) Login. After 3 wrong attempts account is locked.\n" +
                   "3) Set your goal in Goals.\n" +
                   "4) Log activities with all 3 metrics.\n" +
                   "5) Check dashboard for total calories and goal achievement.",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F),
            AutoSize = true,
            Location = new Point(20, 24)
        });

        RenderPage(page);
    }

    private void RenderPage(Control page)
    {
        _contentHost.Controls.Clear();
        page.Dock = DockStyle.Fill;
        _contentHost.Controls.Add(page);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        Application.Exit();
    }
}
