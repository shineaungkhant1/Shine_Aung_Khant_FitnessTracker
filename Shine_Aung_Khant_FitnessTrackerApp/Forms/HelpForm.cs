using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class HelpForm : BaseForm
{
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;
    private readonly string _username;

    public HelpForm(IActivityDefinitionFactory activityDefinitionFactory, string username)
    {
        _activityDefinitionFactory = activityDefinitionFactory;
        _username = string.IsNullOrWhiteSpace(username) ? "Guest" : username;

        Text = "Fitness Tracker - Help & Support";
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
            Text = "CONNECTED",
            ForeColor = AppTheme.Success,
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
        UiStyles.StyleSidebarButton(goalsButton);
        goalsButton.Click += (_, _) =>
        {
            var goals = new GoalsForm(_activityDefinitionFactory, _username);
            goals.Show();
            Hide();
        };

        var activityButton = new Button { Text = "Log Activity", Width = 190 };
        UiStyles.StyleSidebarButton(activityButton);
        activityButton.Click += (_, _) =>
        {
            var activity = new ActivityForm(_activityDefinitionFactory, _username);
            activity.Show();
            Hide();
        };

        var helpButton = new Button { Text = "Help", Width = 190 };
        UiStyles.StyleSidebarButton(helpButton, true);

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
            ColumnCount = 2,
            RowCount = 3
        };
        workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
        workspace.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
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
            Text = "Help & Support",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 30F, FontStyle.Bold),
            AutoSize = true
        }, 0, 0);
        header.Controls.Add(new Label
        {
            Text = "Find answers and guidance for using Fitness Tracker.",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true
        }, 0, 1);

        var faqCard = CreateCardPanel();
        faqCard.Dock = DockStyle.Fill;
        var faqLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6
        };
        faqLayout.Controls.Add(UiStyles.CreateSectionTitle("Frequently Asked Questions"));
        faqLayout.Controls.Add(CreateFaqLabel("How do I set my calorie goal?"));
        faqLayout.Controls.Add(CreateFaqLabel("How is calories burned calculated?"));
        faqLayout.Controls.Add(CreateFaqLabel("Why does login fail after many attempts?"));
        faqLayout.Controls.Add(CreateFaqLabel("How do I save activity data per user?"));
        faqLayout.Controls.Add(new Label
        {
            Text = "Tip: errors and guidance messages will appear near each input field.",
            ForeColor = AppTheme.Primary,
            AutoSize = true,
            Margin = new Padding(0, 16, 0, 0)
        });
        faqCard.Controls.Add(faqLayout);

        var docCard = CreateCardPanel();
        docCard.Dock = DockStyle.Fill;
        var docLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5
        };
        docLayout.Controls.Add(UiStyles.CreateSectionTitle("Documentation"));
        docLayout.Controls.Add(CreateFaqLabel("Getting Started"));
        docLayout.Controls.Add(CreateFaqLabel("Account and Data"));
        docLayout.Controls.Add(CreateFaqLabel("Activity Metric Inputs"));
        var backButton = new Button { Text = "Back to Dashboard", Width = 220 };
        UiStyles.StylePrimaryButton(backButton);
        backButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _username);
            dashboard.Show();
            Hide();
        };
        docLayout.Controls.Add(backButton);
        docCard.Controls.Add(docLayout);

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
            Text = "Support center active",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Anchor = AnchorStyles.Right
        }, 1, 0);

        workspace.Controls.Add(header, 0, 0);
        workspace.SetColumnSpan(header, 2);
        workspace.Controls.Add(faqCard, 0, 1);
        workspace.Controls.Add(docCard, 1, 1);
        workspace.Controls.Add(statusBar, 0, 2);
        workspace.SetColumnSpan(statusBar, 2);
        return workspace;
    }

    private static Label CreateFaqLabel(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular),
            AutoSize = true,
            Margin = new Padding(0, 4, 0, 10)
        };
    }
}
