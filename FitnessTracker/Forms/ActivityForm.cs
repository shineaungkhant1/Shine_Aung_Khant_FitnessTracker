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
        ClientSize = new Size(980, 620);

        BuildLayout();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1,
            Padding = new Padding(24),
            BackColor = AppTheme.Tertiary
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));

        var card = CreateCardPanel();
        card.Dock = DockStyle.Fill;

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 14
        };

        content.Controls.Add(UiStyles.CreateSectionTitle("Record activity"));
        content.Controls.Add(new Label
        {
            Text = $"Logged in as: {_username}",
            AutoSize = true,
            ForeColor = AppTheme.MutedText,
            Margin = new Padding(0, 0, 0, 14)
        });

        content.Controls.Add(UiStyles.CreateCaption("Select Activity"));
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
        content.Controls.Add(activityComboBox);

        _metric1Label.Text = _activities[0].Metric1Label;
        _metric1Label.ForeColor = AppTheme.MutedText;
        _metric1Label.AutoSize = true;
        _metric1Label.Margin = new Padding(0, 8, 0, 4);
        content.Controls.Add(_metric1Label);
        var metric1TextBox = new TextBox { Width = 360 };
        UiStyles.StyleTextBox(metric1TextBox);
        content.Controls.Add(metric1TextBox);

        _metric2Label.Text = _activities[0].Metric2Label;
        _metric2Label.ForeColor = AppTheme.MutedText;
        _metric2Label.AutoSize = true;
        _metric2Label.Margin = new Padding(0, 8, 0, 4);
        content.Controls.Add(_metric2Label);
        var metric2TextBox = new TextBox { Width = 360 };
        UiStyles.StyleTextBox(metric2TextBox);
        content.Controls.Add(metric2TextBox);

        _metric3Label.Text = _activities[0].Metric3Label;
        _metric3Label.ForeColor = AppTheme.MutedText;
        _metric3Label.AutoSize = true;
        _metric3Label.Margin = new Padding(0, 8, 0, 4);
        content.Controls.Add(_metric3Label);
        var metric3TextBox = new TextBox { Width = 360 };
        UiStyles.StyleTextBox(metric3TextBox);
        content.Controls.Add(metric3TextBox);

        var actionPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(0, 16, 0, 0)
        };

        var saveButton = new Button { Text = "Save Activity", Width = 145 };
        UiStyles.StylePrimaryButton(saveButton);
        saveButton.Click += (_, _) =>
        {
            MessageBox.Show(
                "Activity UI is ready. Save and calorie logic will be connected in implementation phase.",
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        };

        var backButton = new Button { Text = "Back to Dashboard", Width = 170 };
        UiStyles.StyleSecondaryButton(backButton);
        backButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _username);
            dashboard.Show();
            Hide();
        };

        actionPanel.Controls.Add(saveButton);
        actionPanel.Controls.Add(backButton);
        content.Controls.Add(actionPanel);

        card.Controls.Add(content);
        root.Controls.Add(card, 1, 0);
        Controls.Add(root);
    }
}
