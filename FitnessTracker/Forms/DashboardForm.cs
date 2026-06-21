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
        var sidebar = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Sidebar,
            Padding = new Padding(12)
        };

        var profile = new Panel
        {
            Dock = DockStyle.Top,
            Height = 86,
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
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Height = 280
        };

        var dashboardButton = new Button { Text = "Dashboard", Width = 190 };
        UiStyles.StyleSidebarButton(dashboardButton, true);

        var goalsButton = new Button { Text = "Goals", Width = 190 };
        UiStyles.StyleSidebarButton(goalsButton);

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

        menu.Controls.Add(dashboardButton);
        menu.Controls.Add(goalsButton);
        menu.Controls.Add(logActivityButton);
        menu.Controls.Add(helpButton);

        var logoutButton = new Button { Text = "Settings / Logout", Width = 190 };
        UiStyles.StyleSecondaryButton(logoutButton);
        logoutButton.Location = new Point(12, 565);
        logoutButton.Click += (_, _) =>
        {
            var loginForm = new LoginForm(_activityDefinitionFactory);
            loginForm.Show();
            Hide();
        };

        sidebar.Controls.Add(logoutButton);
        sidebar.Controls.Add(menu);
        sidebar.Controls.Add(profile);
        return sidebar;
    }

    private Panel BuildContentArea()
    {
        var content = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Tertiary,
            Padding = new Padding(24)
        };

        var header = new Panel
        {
            Dock = DockStyle.Top,
            Height = 82
        };
        header.Controls.Add(new Label
        {
            Text = "Dashboard Overview",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 26F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(0, 0)
        });
        header.Controls.Add(new Label
        {
            Text = $"Ready to crush your goals today, {_displayName}?",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(2, 46)
        });

        var logButton = new Button { Text = "+  Log Workout", Width = 140, Location = new Point(690, 18) };
        UiStyles.StylePrimaryButton(logButton);
        logButton.Click += (_, _) =>
        {
            var activityForm = new ActivityForm(_activityDefinitionFactory, _displayName);
            activityForm.Show();
            Hide();
        };
        header.Controls.Add(logButton);

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

        var rightStats = new Panel { Dock = DockStyle.Fill };
        var activeMinutesCard = UiStyles.CreateMetricCard("ACTIVE MINUTES", "345", "minutes");
        activeMinutesCard.Dock = DockStyle.Top;
        var sleepCard = UiStyles.CreateMetricCard("AVG SLEEP", "7.2", "hours");
        sleepCard.Dock = DockStyle.Top;
        rightStats.Controls.Add(sleepCard);
        rightStats.Controls.Add(activeMinutesCard);

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

        var statusBar = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 26,
            BackColor = AppTheme.Surface
        };
        statusBar.Controls.Add(new Label
        {
            Text = "System Online",
            ForeColor = AppTheme.Success,
            AutoSize = true,
            Location = new Point(8, 5)
        });

        content.Controls.Add(mainGrid);
        content.Controls.Add(header);
        content.Controls.Add(statusBar);
        return content;
    }
}
