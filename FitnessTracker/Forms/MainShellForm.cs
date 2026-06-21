using FitnessTracker.Models;
using FitnessTracker.Services;
using FitnessTracker.Styling;
using FitnessTracker.Controls;

namespace FitnessTracker.Forms;

public sealed class MainShellForm : BaseForm
{
    private readonly IAppService _appService;
    private readonly IReadOnlyList<ActivityDefinition> _activities;
    private readonly Panel _contentHost = new() { Dock = DockStyle.Fill };
    private readonly Label _headerTitle = new();
    private readonly Label _headerSubtitle = new();
    private Button? _dashboardNav;
    private Button? _goalsNav;
    private Button? _activityNav;
    private Button? _helpNav;
    private Button? _quickLogButton;
    private bool _isLoggingOut;

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
            Text = "KINETIC",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 9F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(10, 2)
        });
        profile.Controls.Add(new Label
        {
            Text = _appService.CurrentUsername ?? "Guest",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(10, 24)
        });
        profile.Controls.Add(new Label
        {
            Text = "ACTIVE SESSION",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 8.5F),
            AutoSize = true,
            Location = new Point(10, 48)
        });

        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
        };

        _dashboardNav = new Button { Text = "◻ Dashboard", Width = 200 };
        UiStyles.StyleSidebarButton(_dashboardNav, true);
        _dashboardNav.Click += (_, _) =>
        {
            SetActiveNavigation(_dashboardNav);
            ShowDashboard();
        };

        _goalsNav = new Button { Text = "◎ Goals", Width = 200 };
        UiStyles.StyleSidebarButton(_goalsNav);
        _goalsNav.Click += (_, _) =>
        {
            SetActiveNavigation(_goalsNav);
            ShowGoals();
        };

        _activityNav = new Button { Text = "⊕ Log Activity", Width = 200 };
        UiStyles.StyleSidebarButton(_activityNav);
        _activityNav.Click += (_, _) =>
        {
            SetActiveNavigation(_activityNav);
            ShowActivityLogger();
        };

        _helpNav = new Button { Text = "? Help", Width = 200 };
        UiStyles.StyleSidebarButton(_helpNav);
        _helpNav.Click += (_, _) =>
        {
            SetActiveNavigation(_helpNav);
            ShowHelp();
        };

        menu.Controls.Add(_dashboardNav);
        menu.Controls.Add(_goalsNav);
        menu.Controls.Add(_activityNav);
        menu.Controls.Add(_helpNav);

        var logoutButton = new Button { Text = "Settings / Logout", Dock = DockStyle.Fill };
        UiStyles.StyleSecondaryButton(logoutButton);
        logoutButton.Click += (_, _) =>
        {
            _isLoggingOut = true;
            _appService.Logout();
            var login = new LoginScreenForm(_appService, new ActivityDefinitionFactory());
            login.Show();
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
            ColumnCount = 2,
            RowCount = 1
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));

        var titleBlock = new TableLayoutPanel
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
        titleBlock.Controls.Add(_headerTitle, 0, 0);
        titleBlock.Controls.Add(_headerSubtitle, 0, 1);
        header.Controls.Add(titleBlock, 0, 0);

        _quickLogButton = new Button
        {
            Text = "+  Log Workout",
            Dock = DockStyle.Top,
            Margin = new Padding(0, 14, 0, 0)
        };
        UiStyles.StylePrimaryButton(_quickLogButton);
        _quickLogButton.Click += (_, _) =>
        {
            SetActiveNavigation(_activityNav);
            ShowActivityLogger();
        };
        header.Controls.Add(_quickLogButton, 1, 0);

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
        SetActiveNavigation(_dashboardNav);
        _headerTitle.Text = "Dashboard Overview";
        _headerSubtitle.Text = $"Ready to crush your goals today, {_appService.CurrentUsername}?";

        var totalCalories = _appService.GetTotalCalories();
        var goal = _appService.GetGoal();
        var goalStatus = _appService.IsGoalAchieved() ? "Achieved" : "In Progress";
        var rawProgressPercent = goal <= 0 ? 0 : (totalCalories / goal) * 100d;
        var progressPercent = goal <= 0 ? 0 : Math.Max(0, (int)Math.Round(rawProgressPercent));
        var visualProgressPercent = Math.Min(100, progressPercent);
        var totalCaloriesText = FormatCompactNumber(totalCalories);
        var goalText = FormatCompactNumber(goal);
        var deltaCalories = totalCalories - goal;
        var progressMessage = deltaCalories >= 0
            ? $"Exceeded by {FormatCompactNumber(deltaCalories)} kcal"
            : $"Remaining {FormatCompactNumber(Math.Abs(deltaCalories))} kcal";

        var page = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2
        };
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68));
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
        page.RowStyles.Add(new RowStyle(SizeType.Absolute, 235));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var summaryCard = CreateCardPanel();
        summaryCard.Dock = DockStyle.Fill;
        var summaryLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
        summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));

        var leftBlock = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 7
        };
        leftBlock.Controls.Add(new Label
        {
            Text = "DAILY GOAL",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Bold),
            AutoSize = true
        }, 0, 0);
        leftBlock.Controls.Add(new Label
        {
            Text = $"{totalCaloriesText} / {goalText} kcal",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 30F, FontStyle.Bold),
            AutoSize = true
        }, 0, 1);
        leftBlock.Controls.Add(new Label
        {
            Text = "Active Calories",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true
        }, 0, 2);
        leftBlock.Controls.Add(new Label
        {
            Text = $"Goal Status: {goalStatus}",
            ForeColor = _appService.IsGoalAchieved() ? AppTheme.Success : AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 10.5F, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(0, 8, 0, 0)
        }, 0, 3);

        var progressTrack = new Panel
        {
            BackColor = AppTheme.Border,
            Dock = DockStyle.Top,
            Height = 10,
            Margin = new Padding(0, 16, 0, 0)
        };
        var progressFill = new Panel
        {
            BackColor = AppTheme.Primary,
            Height = 10,
            Dock = DockStyle.Left
        };
        progressFill.Width = Math.Max(0, Math.Min(320, (int)(visualProgressPercent * 3.2)));
        progressTrack.Controls.Add(progressFill);
        leftBlock.Controls.Add(progressTrack, 0, 4);
        leftBlock.Controls.Add(new Label
        {
            Text = $"Progress: {progressPercent}%",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Margin = new Padding(0, 8, 0, 0)
        }, 0, 5);
        leftBlock.Controls.Add(new Label
        {
            Text = progressMessage,
            ForeColor = deltaCalories >= 0 ? AppTheme.Success : AppTheme.MutedText,
            AutoSize = true,
            Margin = new Padding(0, 4, 0, 0)
        }, 0, 6);

        var rightBlock = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1
        };
        var ring = new CircularProgressControl
        {
            ProgressPercent = visualProgressPercent,
            Anchor = AnchorStyles.None,
            Size = new Size(150, 150)
        };
        rightBlock.Controls.Add(ring, 0, 0);

        summaryLayout.Controls.Add(leftBlock, 0, 0);
        summaryLayout.Controls.Add(rightBlock, 1, 0);
        summaryCard.Controls.Add(summaryLayout);

        var right = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        var totalCaloriesCard = UiStyles.CreateMetricCard("TOTAL CALORIES", totalCaloriesText, "kcal");
        totalCaloriesCard.Dock = DockStyle.Fill;
        var goalTargetCard = UiStyles.CreateMetricCard("GOAL TARGET", goalText, "kcal");
        goalTargetCard.Dock = DockStyle.Fill;
        right.Controls.Add(totalCaloriesCard, 0, 0);
        right.Controls.Add(goalTargetCard, 0, 1);

        var recentCard = CreateCardPanel();
        recentCard.Dock = DockStyle.Fill;
        var recentLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        recentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        recentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        recentLayout.Controls.Add(new Label
        {
            Text = "Recent Activities",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 14F, FontStyle.Bold),
            AutoSize = true
        }, 0, 0);

        var scrollHost = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = AppTheme.Surface
        };
        var activitiesList = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 0
        };
        scrollHost.Controls.Add(activitiesList);

        var recent = _appService.GetRecentActivities(6);
        if (recent.Count == 0)
        {
            activitiesList.Controls.Add(new Label
            {
                Text = "No activities logged yet. Use Log Activity to begin.",
                ForeColor = AppTheme.MutedText,
                Font = new Font(AppTheme.FontFamily, 9.5F),
                AutoSize = true,
                Margin = new Padding(0, 8, 0, 0)
            });
        }
        else
        {
            foreach (var record in _appService.GetRecentActivities(100))
            {
                var rowCard = new Panel
                {
                    BackColor = AppTheme.SurfaceSoft,
                    BorderStyle = BorderStyle.FixedSingle,
                    Height = 54,
                    Dock = DockStyle.Top,
                    Margin = new Padding(0, 8, 0, 0),
                    Padding = new Padding(12, 8, 12, 8)
                };

                var rowLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 3,
                    RowCount = 1
                };
                rowLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                rowLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                rowLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));

                rowLayout.Controls.Add(new Label
                {
                    Text = record.ActivityName,
                    ForeColor = AppTheme.Neutral,
                    Font = new Font(AppTheme.FontFamily, 10.5F, FontStyle.Bold),
                    AutoSize = true,
                    Anchor = AnchorStyles.Left
                }, 0, 0);

                rowLayout.Controls.Add(new Label
                {
                    Text = $"{record.Calories:F1} kcal",
                    ForeColor = AppTheme.Primary,
                    Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Bold),
                    AutoSize = true,
                    Anchor = AnchorStyles.Left
                }, 1, 0);

                rowLayout.Controls.Add(new Label
                {
                    Text = record.LoggedAt.ToString("g"),
                    ForeColor = AppTheme.MutedText,
                    Font = new Font(AppTheme.FontFamily, 9.5F, FontStyle.Regular),
                    AutoSize = true,
                    Anchor = AnchorStyles.Right
                }, 2, 0);

                rowCard.Controls.Add(rowLayout);
                activitiesList.Controls.Add(rowCard);
            }
        }
        recentLayout.Controls.Add(scrollHost, 0, 1);
        recentCard.Controls.Add(recentLayout);

        page.Controls.Add(summaryCard, 0, 0);
        page.Controls.Add(right, 1, 0);
        page.Controls.Add(recentCard, 0, 1);
        page.SetColumnSpan(recentCard, 2);

        RenderPage(page);
    }

    private void ShowGoals()
    {
        SetActiveNavigation(_goalsNav);
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
            Text = $"Current Goal: {FormatCompactNumber(_appService.GetGoal())} kcal",
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
                currentGoal.Text = $"Current Goal: {FormatCompactNumber(_appService.GetGoal())} kcal";
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
        SetActiveNavigation(_activityNav);
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
        SetActiveNavigation(_helpNav);
        _headerTitle.Text = "Help & Support";
        _headerSubtitle.Text = "Find guidance and resolve common user issues quickly.";

        var page = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2
        };
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
        page.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 72F));
        page.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));

        var faqCard = CreateCardPanel();
        faqCard.Dock = DockStyle.Fill;
        var faqLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 7
        };
        faqLayout.Controls.Add(UiStyles.CreateSectionTitle("Frequently Asked Questions"));
        faqLayout.Controls.Add(CreateHelpRow("How do I create an account?",
            "Use Register with letters/numbers username and exactly 12-char password."));
        faqLayout.Controls.Add(CreateHelpRow("Why am I blocked from login?",
            "After 3 failed attempts, login is temporarily locked for security."));
        faqLayout.Controls.Add(CreateHelpRow("How do I set my goal?",
            "Open Goals page, enter calorie target, and click Save Goal."));
        faqLayout.Controls.Add(CreateHelpRow("How do I record activities?",
            "Use Log Activity, select one of 6 activities, and enter all 3 metrics."));
        faqLayout.Controls.Add(CreateHelpRow("Where can I see progress?",
            "Dashboard shows total calories, goal target, progress %, and recent logs."));
        faqCard.Controls.Add(faqLayout);

        var docsCard = CreateCardPanel();
        docsCard.Dock = DockStyle.Fill;
        var docsLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6
        };
        docsLayout.Controls.Add(UiStyles.CreateSectionTitle("Quick Actions"));

        var toGoalsButton = new Button { Text = "Go to Goals", Width = 190 };
        UiStyles.StylePrimaryButton(toGoalsButton);
        toGoalsButton.Click += (_, _) =>
        {
            SetActiveNavigation(_goalsNav);
            ShowGoals();
        };

        var toActivityButton = new Button { Text = "Go to Log Activity", Width = 190 };
        UiStyles.StyleSecondaryButton(toActivityButton);
        toActivityButton.Click += (_, _) =>
        {
            SetActiveNavigation(_activityNav);
            ShowActivityLogger();
        };

        var toDashboardButton = new Button { Text = "Back to Dashboard", Width = 190 };
        UiStyles.StyleSecondaryButton(toDashboardButton);
        toDashboardButton.Click += (_, _) =>
        {
            SetActiveNavigation(_dashboardNav);
            ShowDashboard();
        };

        docsLayout.Controls.Add(new Label
        {
            Text = "Need to continue quickly?",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Margin = new Padding(0, 2, 0, 10)
        });
        docsLayout.Controls.Add(toGoalsButton);
        docsLayout.Controls.Add(toActivityButton);
        docsLayout.Controls.Add(toDashboardButton);
        docsLayout.Controls.Add(new Label
        {
            Text = "Tip: Test invalid inputs for Task 2 evidence.",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 9.5F, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(0, 12, 0, 0)
        });
        docsCard.Controls.Add(docsLayout);

        var supportCard = CreateCardPanel();
        supportCard.Dock = DockStyle.Fill;
        supportCard.Controls.Add(new Label
        {
            Text = "Support Note:\nIf something fails, check the on-screen message and retry with valid inputs.",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular),
            AutoSize = true,
            MaximumSize = new Size(900, 0),
            Location = new Point(20, 20)
        });

        page.Controls.Add(faqCard, 0, 0);
        page.Controls.Add(docsCard, 1, 0);
        page.Controls.Add(supportCard, 0, 1);
        page.SetColumnSpan(supportCard, 2);

        RenderPage(page);
    }

    private static Control CreateHelpRow(string question, string answer)
    {
        var wrapper = new Panel
        {
            Dock = DockStyle.Top,
            Height = 66,
            BackColor = AppTheme.SurfaceSoft,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(10, 8, 10, 8),
            Margin = new Padding(0, 0, 0, 8)
        };

        wrapper.Controls.Add(new Label
        {
            Text = question,
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(10, 8)
        });
        wrapper.Controls.Add(new Label
        {
            Text = answer,
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 9.2F, FontStyle.Regular),
            AutoSize = true,
            MaximumSize = new Size(700, 0),
            Location = new Point(10, 30)
        });

        return wrapper;
    }

    private void RenderPage(Control page)
    {
        _contentHost.Controls.Clear();
        page.Dock = DockStyle.Fill;
        _contentHost.Controls.Add(page);
    }

    private void SetActiveNavigation(Button? activeButton)
    {
        if (_dashboardNav is null || _goalsNav is null || _activityNav is null || _helpNav is null)
        {
            return;
        }

        UiStyles.StyleSidebarButton(_dashboardNav, activeButton == _dashboardNav);
        UiStyles.StyleSidebarButton(_goalsNav, activeButton == _goalsNav);
        UiStyles.StyleSidebarButton(_activityNav, activeButton == _activityNav);
        UiStyles.StyleSidebarButton(_helpNav, activeButton == _helpNav);
    }

    private static string FormatCompactNumber(double value)
    {
        var absolute = Math.Abs(value);
        var suffix = "";
        var scaled = value;

        if (absolute >= 1_000_000_000_000)
        {
            scaled = value / 1_000_000_000_000d;
            suffix = "T";
        }
        else if (absolute >= 1_000_000_000)
        {
            scaled = value / 1_000_000_000d;
            suffix = "B";
        }
        else if (absolute >= 1_000_000)
        {
            scaled = value / 1_000_000d;
            suffix = "M";
        }
        else if (absolute >= 1_000)
        {
            scaled = value / 1_000d;
            suffix = "K";
        }
        else
        {
            return value.ToString("N1");
        }

        return $"{scaled:0.0}{suffix}";
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        if (!_isLoggingOut)
        {
            Application.Exit();
        }
    }
}
