using FitnessTracker.Models;
using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class ActivityForm : BaseForm
{
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;
    private readonly string _username;
    private readonly IReadOnlyList<ActivityDefinition> _activities;
    private readonly Label _metric1Label = new();
    private readonly Label _metric2Label = new();
    private readonly Label _metric3Label = new();

    public ActivityForm(IActivityDefinitionFactory activityDefinitionFactory, string username)
    {
        _activityDefinitionFactory = activityDefinitionFactory;
        _username = username;
        _activities = _activityDefinitionFactory.CreateAll();

        Text = "Fitness Tracker - Record Activity";
        ClientSize = new Size(1180, 720);

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
        var workspace = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Tertiary,
            Padding = new Padding(24),
            ColumnCount = 1,
            RowCount = 4
        };
        workspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 94F));
        workspace.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        workspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
        workspace.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

        var headerPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        headerPanel.Controls.Add(new Label
        {
            Text = "Log Activity",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 34F, FontStyle.Bold),
            AutoSize = true
        }, 0, 0);
        headerPanel.Controls.Add(new Label
        {
            Text = "Track your progress and analyze your performance.",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true
        }, 0, 1);

        var activityCard = CreateCardPanel();
        activityCard.Dock = DockStyle.Fill;

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6
        };
        content.Controls.Add(UiStyles.CreateCaption("ACTIVITY TYPE"));

        var chipPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            WrapContents = true
        };

        var activityComboBox = new ComboBox
        {
            Width = 360,
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = AppTheme.SurfaceSoft,
            ForeColor = AppTheme.Neutral,
            FlatStyle = FlatStyle.Flat
        };
        activityComboBox.Items.AddRange(_activities.Select(a => a.Name).ToArray());
        activityComboBox.SelectedIndexChanged += (_, _) =>
        {
            var selected = _activities[activityComboBox.SelectedIndex];
            _metric1Label.Text = selected.Metric1Label;
            _metric2Label.Text = selected.Metric2Label;
            _metric3Label.Text = selected.Metric3Label;
        };
        activityComboBox.SelectedIndex = 0;

        foreach (var activity in _activities.Take(4))
        {
            var chip = new Button
            {
                Text = activity.Name,
                Width = 120
            };
            UiStyles.StyleSecondaryButton(chip);
            chip.Click += (_, _) => activityComboBox.SelectedItem = activity.Name;
            chipPanel.Controls.Add(chip);
        }
        content.Controls.Add(chipPanel);
        content.Controls.Add(UiStyles.CreateCaption("Or choose from all 6 activities"));
        activityComboBox.Dock = DockStyle.Top;
        content.Controls.Add(activityComboBox);
        content.Controls.Add(UiStyles.CreateCaption("METRICS"));

        _metric1Label.Text = _activities[0].Metric1Label;
        _metric1Label.ForeColor = AppTheme.Primary;
        _metric1Label.AutoSize = true;
        _metric1Label.Margin = new Padding(0, 8, 0, 4);
        var metricsGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 3,
            RowCount = 1,
            AutoSize = true
        };
        metricsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
        metricsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
        metricsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));

        var metric1Panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        var metric1TextBox = new TextBox { Dock = DockStyle.Top };
        UiStyles.StyleTextBox(metric1TextBox);
        metric1Panel.Controls.Add(_metric1Label, 0, 0);
        metric1Panel.Controls.Add(metric1TextBox, 0, 1);

        _metric2Label.Text = _activities[0].Metric2Label;
        _metric2Label.ForeColor = AppTheme.Primary;
        _metric2Label.AutoSize = true;
        _metric2Label.Margin = new Padding(0, 8, 0, 4);
        var metric2Panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        var metric2TextBox = new TextBox { Dock = DockStyle.Top };
        UiStyles.StyleTextBox(metric2TextBox);
        metric2Panel.Controls.Add(_metric2Label, 0, 0);
        metric2Panel.Controls.Add(metric2TextBox, 0, 1);

        _metric3Label.Text = _activities[0].Metric3Label;
        _metric3Label.ForeColor = AppTheme.Primary;
        _metric3Label.AutoSize = true;
        _metric3Label.Margin = new Padding(0, 8, 0, 4);
        var metric3Panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        var metric3TextBox = new TextBox { Dock = DockStyle.Top };
        UiStyles.StyleTextBox(metric3TextBox);
        metric3Panel.Controls.Add(_metric3Label, 0, 0);
        metric3Panel.Controls.Add(metric3TextBox, 0, 1);

        metricsGrid.Controls.Add(metric1Panel, 0, 0);
        metricsGrid.Controls.Add(metric2Panel, 1, 0);
        metricsGrid.Controls.Add(metric3Panel, 2, 0);
        content.Controls.Add(metricsGrid);

        activityCard.Controls.Add(content);

        var bottomPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

        var estimatedCard = CreateCardPanel();
        estimatedCard.Dock = DockStyle.Fill;
        var estimatedLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        estimatedLayout.Controls.Add(new Label
        {
            Text = "ESTIMATED BURN",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Bold),
            AutoSize = true
        }, 0, 0);
        estimatedLayout.Controls.Add(new Label
        {
            Text = "150 kcal",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 36F, FontStyle.Bold),
            AutoSize = true
        }, 0, 1);
        estimatedCard.Controls.Add(estimatedLayout);

        var saveCard = CreateCardPanel();
        saveCard.Dock = DockStyle.Fill;
        var saveLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        var saveButton = new Button { Text = "Save Activity Log", Dock = DockStyle.Top };
        UiStyles.StylePrimaryButton(saveButton);
        saveButton.Click += (_, _) =>
        {
            MessageBox.Show(
                "Activity UI is ready. Save and calorie logic will be connected in implementation phase.",
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        };

        var backButton = new Button { Text = "Back to Dashboard", Dock = DockStyle.Top };
        UiStyles.StyleSecondaryButton(backButton);
        backButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _username);
            dashboard.Show();
            Hide();
        };
        saveLayout.Controls.Add(saveButton, 0, 0);
        saveLayout.Controls.Add(backButton, 0, 1);
        saveCard.Controls.Add(saveLayout);

        bottomPanel.Controls.Add(estimatedCard, 0, 0);
        bottomPanel.Controls.Add(saveCard, 1, 0);

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
            Text = "Sync enabled",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Anchor = AnchorStyles.Right
        }, 1, 0);

        workspace.Controls.Add(headerPanel, 0, 0);
        workspace.Controls.Add(activityCard, 0, 1);
        workspace.Controls.Add(bottomPanel, 0, 2);
        workspace.Controls.Add(statusBar, 0, 3);

        root.Controls.Add(sidebar, 0, 0);
        root.Controls.Add(workspace, 1, 0);
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
            RowCount = 2
        };
        sidebar.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
        sidebar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        header.Controls.Add(new Label
        {
            Text = "KINETIC",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 15F, FontStyle.Bold),
            AutoSize = true
        }, 0, 0);
        header.Controls.Add(new Label
        {
            Text = _username,
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 10.5F, FontStyle.Bold),
            AutoSize = true
        }, 0, 1);

        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false
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
        UiStyles.StyleSidebarButton(activityButton, true);

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

        sidebar.Controls.Add(header, 0, 0);
        sidebar.Controls.Add(menu, 0, 1);
        return (Panel)sidebar;
    }
}
