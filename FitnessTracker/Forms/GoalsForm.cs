using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class GoalsForm : BaseForm
{
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;
    private readonly string _username;

    public GoalsForm(IActivityDefinitionFactory activityDefinitionFactory, string username)
    {
        _activityDefinitionFactory = activityDefinitionFactory;
        _username = string.IsNullOrWhiteSpace(username) ? "Guest" : username;

        Text = "Fitness Tracker - Goals";
        ClientSize = new Size(1180, 720);

        BuildLayout();
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
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

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
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 94F));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));

        var profile = new Panel { Dock = DockStyle.Fill, BackColor = AppTheme.Sidebar };
        profile.Controls.Add(new Label
        {
            Text = _username,
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(56, 18)
        });
        profile.Controls.Add(new Label
        {
            Text = "ACTIVE SESSION",
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
        UiStyles.StyleSidebarButton(dashboardButton);
        dashboardButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _username);
            dashboard.Show();
            Hide();
        };

        var goalsButton = new Button { Text = "Goals", Width = 190 };
        UiStyles.StyleSidebarButton(goalsButton, true);

        var activityButton = new Button { Text = "Log Activity", Width = 190 };
        UiStyles.StyleSidebarButton(activityButton);
        activityButton.Click += (_, _) =>
        {
            var activity = new ActivityForm(_activityDefinitionFactory, _username);
            activity.Show();
            Hide();
        };

        var helpButton = new Button { Text = "Help", Width = 190 };
        UiStyles.StyleSidebarButton(helpButton);
        helpButton.Click += (_, _) =>
        {
            var help = new HelpForm(_activityDefinitionFactory, _username);
            help.Show();
            Hide();
        };

        menu.Controls.Add(dashboardButton);
        menu.Controls.Add(goalsButton);
        menu.Controls.Add(activityButton);
        menu.Controls.Add(helpButton);

        var logoutButton = new Button { Text = "Settings / Logout", Dock = DockStyle.Fill };
        UiStyles.StyleSecondaryButton(logoutButton);
        logoutButton.Click += (_, _) =>
        {
            var login = new LoginForm(_activityDefinitionFactory);
            login.Show();
            Hide();
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
        workspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
        workspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        workspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        header.Controls.Add(new Label
        {
            Text = "Goal Management",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 30F, FontStyle.Bold),
            AutoSize = true
        }, 0, 0);
        header.Controls.Add(new Label
        {
            Text = "Set your calorie target linked to your account.",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true
        }, 0, 1);

        var goalCard = CreateCardPanel();
        goalCard.Dock = DockStyle.Fill;

        var cardLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 7
        };
        cardLayout.Controls.Add(UiStyles.CreateCaption("TARGET CALORIE GOAL"));
        cardLayout.Controls.Add(new Label
        {
            Text = "Daily Limit Setpoint",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 13F, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(0, 10, 0, 6)
        });

        var targetTextBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "e.g. 300" };
        UiStyles.StyleTextBox(targetTextBox);
        targetTextBox.Font = new Font(AppTheme.FontFamily, 16F, FontStyle.Bold);
        cardLayout.Controls.Add(targetTextBox);

        cardLayout.Controls.Add(new Label
        {
            Text = "kcal",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true,
            Margin = new Padding(0, 6, 0, 10)
        });

        cardLayout.Controls.Add(new Label
        {
            Text = "Your goal is stored per user account after save.",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular),
            AutoSize = true,
            Margin = new Padding(0, 10, 0, 16)
        });

        var saveGoalButton = new Button { Text = "Save Goal Configuration", Width = 250 };
        UiStyles.StylePrimaryButton(saveGoalButton);
        saveGoalButton.Click += (_, _) =>
        {
            MessageBox.Show(
                "Goal screen UI is ready. SQLite goal persistence will be connected next.",
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        };
        cardLayout.Controls.Add(saveGoalButton);

        var goDashboardButton = new Button { Text = "Back to Dashboard", Width = 250 };
        UiStyles.StyleSecondaryButton(goDashboardButton);
        goDashboardButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _username);
            dashboard.Show();
            Hide();
        };
        cardLayout.Controls.Add(goDashboardButton);

        goalCard.Controls.Add(cardLayout);

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
            Text = "Goal service ready",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Anchor = AnchorStyles.Right
        }, 1, 0);

        workspace.Controls.Add(header, 0, 0);
        workspace.Controls.Add(goalCard, 0, 1);
        workspace.Controls.Add(statusBar, 0, 2);
        return workspace;
    }
}
