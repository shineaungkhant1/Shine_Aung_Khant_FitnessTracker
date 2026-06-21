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
            BackColor = AppTheme.Tertiary
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var sidebar = BuildSidebar();
        var content = BuildContentArea();

        root.Controls.Add(sidebar, 0, 0);
        root.Controls.Add(content, 1, 0);
        Controls.Add(root);
    }

    private Panel BuildSidebar()
    {
        var sidebar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Sidebar,
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 3
        };
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 94F));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

        var profile = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Sidebar
        };
        profile.Controls.Add(new Label
        {
            Text = _displayName,
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(56, 18)
        });
        profile.Controls.Add(new Label
        {
            Text = "PRO MEMBER",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 8.5F, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(56, 42)
        });

        var avatar = new Panel
        {
            BackColor = AppTheme.SurfaceSoft,
            Size = new Size(36, 36),
            Location = new Point(10, 16)
        };
        profile.Controls.Add(avatar);

        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(0, 6, 0, 6)
        };

        var dashboardButton = new Button { Text = "Dashboard", Width = 190 };
        UiStyles.StyleSidebarButton(dashboardButton, true);

        var goalsButton = new Button { Text = "Goals", Width = 190 };
        UiStyles.StyleSidebarButton(goalsButton);
        goalsButton.Click += (_, _) =>
        {
            var goals = new GoalsForm(_activityDefinitionFactory, _displayName);
            goals.Show();
            Hide();
        };

        var logActivityButton = new Button { Text = "Log Activity", Width = 190 };
        UiStyles.StyleSidebarButton(logActivityButton);
        logActivityButton.Click += (_, _) =>
        {
            var activityForm = new ActivityForm(_activityDefinitionFactory, _displayName);
            activityForm.Show();
            Hide();
        };

        var helpButton = new Button { Text = "Help", Width = 190 };
        UiStyles.StyleSidebarButton(helpButton);
        helpButton.Click += (_, _) =>
        {
            var help = new HelpForm(_activityDefinitionFactory, _displayName);
            help.Show();
            Hide();
        };

        menu.Controls.Add(dashboardButton);
        menu.Controls.Add(goalsButton);
        menu.Controls.Add(logActivityButton);
        menu.Controls.Add(helpButton);

        var logoutButton = new Button { Text = "Settings / Logout", Dock = DockStyle.Fill };
        UiStyles.StyleSecondaryButton(logoutButton);
        logoutButton.Click += (_, _) =>
        {
            var loginForm = new LoginForm(_activityDefinitionFactory);
            loginForm.Show();
            Hide();
        };

        sidebar.Controls.Add(profile, 0, 0);
        sidebar.Controls.Add(menu, 0, 1);
        sidebar.Controls.Add(logoutButton, 0, 2);
        return (Panel)sidebar;
    }

    private Panel BuildContentArea()
    {
        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Tertiary,
            Padding = new Padding(24),
            ColumnCount = 1,
            RowCount = 3
        };
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        content.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 2,
            RowCount = 1
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));

        var headerTextPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        headerTextPanel.Controls.Add(new Label
        {
            Text = "Dashboard Overview",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 26F, FontStyle.Bold),
            AutoSize = true
        }, 0, 0);
        headerTextPanel.Controls.Add(new Label
        {
            Text = $"Ready to crush your goals today, {_displayName}?",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true
        }, 0, 1);

        var logButton = new Button { Text = "+  Log Workout", Dock = DockStyle.Top, Margin = new Padding(0, 16, 0, 0) };
        UiStyles.StylePrimaryButton(logButton);
        logButton.Click += (_, _) =>
        {
            var activityForm = new ActivityForm(_activityDefinitionFactory, _displayName);
            activityForm.Show();
            Hide();
        };
        header.Controls.Add(headerTextPanel, 0, 0);
        header.Controls.Add(logButton, 1, 0);

        var mainGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2,
            Padding = new Padding(0, 12, 0, 0)
        };
        mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68));
        mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
        mainGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 230));
        mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var goalCard = CreateCardPanel();
        goalCard.Dock = DockStyle.Fill;
        goalCard.Controls.Add(new Label
        {
            Text = "DAILY GOAL",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 12F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 18)
        });
        goalCard.Controls.Add(new Label
        {
            Text = "120 / 300 kcal",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 33F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 58)
        });
        goalCard.Controls.Add(new Label
        {
            Text = "Active calories",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(24, 126)
        });
        goalCard.Controls.Add(new Label
        {
            Text = "Goal Status: In Progress",
            ForeColor = AppTheme.Success,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(24, 156)
        });

        var rightStats = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        rightStats.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        rightStats.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        var activeMinutesCard = UiStyles.CreateMetricCard("ACTIVE MINUTES", "345", "minutes");
        activeMinutesCard.Dock = DockStyle.Fill;
        var sleepCard = UiStyles.CreateMetricCard("AVG SLEEP", "7.2", "hours");
        sleepCard.Dock = DockStyle.Fill;
        rightStats.Controls.Add(activeMinutesCard, 0, 0);
        rightStats.Controls.Add(sleepCard, 0, 1);

        var activityListCard = CreateCardPanel();
        activityListCard.Dock = DockStyle.Fill;
        activityListCard.Controls.Add(new Label
        {
            Text = "Recent Activity",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 14F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 16)
        });
        activityListCard.Controls.Add(new Label
        {
            Text = "Morning Walk   |   3.2 km   |   250 kcal",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(24, 58)
        });
        activityListCard.Controls.Add(new Label
        {
            Text = "Laps Swimming  |   1.5 km   |   420 kcal",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(24, 90)
        });
        activityListCard.Controls.Add(new Label
        {
            Text = "Set your target in Goals and continue logging activities.",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 9.5F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(24, 130)
        });

        mainGrid.Controls.Add(goalCard, 0, 0);
        mainGrid.Controls.Add(rightStats, 1, 0);
        mainGrid.Controls.Add(activityListCard, 0, 1);
        mainGrid.SetColumnSpan(activityListCard, 2);

        var statusBar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Surface,
            ColumnCount = 2
        };
        statusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        statusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        statusBar.Controls.Add(new Label
        {
            Text = "System Online",
            ForeColor = AppTheme.Success,
            AutoSize = true,
            Anchor = AnchorStyles.Left
        }, 0, 0);
        statusBar.Controls.Add(new Label
        {
            Text = "Logged in",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Anchor = AnchorStyles.Right
        }, 1, 0);

        content.Controls.Add(header, 0, 0);
        content.Controls.Add(mainGrid, 0, 1);
        content.Controls.Add(statusBar, 0, 2);
        return (Panel)content;
    }
}
