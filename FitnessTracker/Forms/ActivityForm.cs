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
        var workspace = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Tertiary,
            Padding = new Padding(24)
        };

        workspace.Controls.Add(new Label
        {
            Text = "Log Activity",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 34F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(8, 6)
        });
        workspace.Controls.Add(new Label
        {
            Text = "Track your progress and analyze your performance.",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true,
            Location = new Point(11, 64)
        });

        var activityCard = CreateCardPanel();
        activityCard.Location = new Point(8, 112);
        activityCard.Size = new Size(900, 290);

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 10
        };
        content.Controls.Add(UiStyles.CreateCaption("ACTIVITY TYPE"));

        var chipPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 58
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
        content.Controls.Add(UiStyles.CreateCaption("METRICS"));

        _metric1Label.Text = _activities[0].Metric1Label;
        _metric1Label.ForeColor = AppTheme.Primary;
        _metric1Label.AutoSize = true;
        _metric1Label.Margin = new Padding(0, 8, 0, 4);
        var metricsGrid = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            ColumnCount = 3,
            RowCount = 2,
            Height = 120
        };
        metricsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
        metricsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
        metricsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));

        var metric1Panel = new Panel { Dock = DockStyle.Fill };
        var metric1TextBox = new TextBox { Width = 250 };
        UiStyles.StyleTextBox(metric1TextBox);
        metric1Panel.Controls.Add(_metric1Label);
        metric1Panel.Controls.Add(metric1TextBox);
        _metric1Label.Location = new Point(0, 0);
        metric1TextBox.Location = new Point(0, 26);

        _metric2Label.Text = _activities[0].Metric2Label;
        _metric2Label.ForeColor = AppTheme.Primary;
        _metric2Label.AutoSize = true;
        _metric2Label.Margin = new Padding(0, 8, 0, 4);
        var metric2Panel = new Panel { Dock = DockStyle.Fill };
        var metric2TextBox = new TextBox { Width = 250 };
        UiStyles.StyleTextBox(metric2TextBox);
        metric2Panel.Controls.Add(_metric2Label);
        metric2Panel.Controls.Add(metric2TextBox);
        _metric2Label.Location = new Point(0, 0);
        metric2TextBox.Location = new Point(0, 26);

        _metric3Label.Text = _activities[0].Metric3Label;
        _metric3Label.ForeColor = AppTheme.Primary;
        _metric3Label.AutoSize = true;
        _metric3Label.Margin = new Padding(0, 8, 0, 4);
        var metric3Panel = new Panel { Dock = DockStyle.Fill };
        var metric3TextBox = new TextBox { Width = 250 };
        UiStyles.StyleTextBox(metric3TextBox);
        metric3Panel.Controls.Add(_metric3Label);
        metric3Panel.Controls.Add(metric3TextBox);
        _metric3Label.Location = new Point(0, 0);
        metric3TextBox.Location = new Point(0, 26);

        metricsGrid.Controls.Add(metric1Panel, 0, 0);
        metricsGrid.Controls.Add(metric2Panel, 1, 0);
        metricsGrid.Controls.Add(metric3Panel, 2, 0);
        content.Controls.Add(metricsGrid);

        activityCard.Controls.Add(content);

        var bottomPanel = new TableLayoutPanel
        {
            Location = new Point(8, 430),
            Size = new Size(900, 150),
            ColumnCount = 2
        };
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
        bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

        var estimatedCard = CreateCardPanel();
        estimatedCard.Dock = DockStyle.Fill;
        estimatedCard.Controls.Add(new Label
        {
            Text = "ESTIMATED BURN",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(18, 14)
        });
        estimatedCard.Controls.Add(new Label
        {
            Text = "150 kcal",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 36F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(18, 36)
        });

        var saveCard = CreateCardPanel();
        saveCard.Dock = DockStyle.Fill;
        var saveButton = new Button { Text = "Save Activity Log", Width = 280, Height = 58, Location = new Point(40, 42) };
        UiStyles.StylePrimaryButton(saveButton);
        saveButton.Click += (_, _) =>
        {
            MessageBox.Show(
                "Activity UI is ready. Save and calorie logic will be connected in implementation phase.",
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        };

        var backButton = new Button { Text = "Back to Dashboard", Width = 180, Height = 36, Location = new Point(90, 104) };
        UiStyles.StyleSecondaryButton(backButton);
        backButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _username);
            dashboard.Show();
            Hide();
        };
        saveCard.Controls.Add(saveButton);
        saveCard.Controls.Add(backButton);

        bottomPanel.Controls.Add(estimatedCard, 0, 0);
        bottomPanel.Controls.Add(saveCard, 1, 0);

        workspace.Controls.Add(activityCard);
        workspace.Controls.Add(bottomPanel);

        root.Controls.Add(sidebar, 0, 0);
        root.Controls.Add(workspace, 1, 0);
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
        sidebar.Controls.Add(new Label
        {
            Text = "KINETIC",
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 15F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(10, 10)
        });
        sidebar.Controls.Add(new Label
        {
            Text = _username,
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 10.5F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(12, 56)
        });

        var dashboardButton = new Button { Text = "Dashboard", Width = 190, Location = new Point(12, 120) };
        UiStyles.StyleSidebarButton(dashboardButton);
        dashboardButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _username);
            dashboard.Show();
            Hide();
        };

        var goalsButton = new Button { Text = "Goals", Width = 190, Location = new Point(12, 168) };
        UiStyles.StyleSidebarButton(goalsButton);

        var activityButton = new Button { Text = "Log Activity", Width = 190, Location = new Point(12, 216) };
        UiStyles.StyleSidebarButton(activityButton, true);

        var helpButton = new Button { Text = "Help", Width = 190, Location = new Point(12, 264) };
        UiStyles.StyleSidebarButton(helpButton);

        sidebar.Controls.Add(dashboardButton);
        sidebar.Controls.Add(goalsButton);
        sidebar.Controls.Add(activityButton);
        sidebar.Controls.Add(helpButton);
        return sidebar;
    }
}
