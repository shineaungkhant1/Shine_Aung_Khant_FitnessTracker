using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class RegisterForm : BaseForm
{
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;

    public RegisterForm(IActivityDefinitionFactory activityDefinitionFactory)
    {
        _activityDefinitionFactory = activityDefinitionFactory;

        Text = "Fitness Tracker - Register";
        ClientSize = new Size(980, 680);

        BuildLayout();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            BackColor = AppTheme.Tertiary,
            ColumnCount = 3,
            RowCount = 3
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));

        var titlePanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 1
        };
        titlePanel.Controls.Add(new Label
        {
            Text = "KINETIC",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 20F, FontStyle.Bold),
            AutoSize = true,
            Anchor = AnchorStyles.None
        }, 0, 0);

        var card = CreateCardPanel();
        card.Dock = DockStyle.Fill;
        card.Margin = new Padding(0, 0, 0, 0);

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 12,
            AutoScroll = true
        };

        content.Controls.Add(UiStyles.CreateSectionTitle("Create your Kinetic account"));
        content.Controls.Add(new Label
        {
            Text = "Join the training workspace and track your progress.",
            AutoSize = true,
            ForeColor = AppTheme.MutedText,
            Margin = new Padding(0, 0, 0, 14)
        });
        content.Controls.Add(UiStyles.CreateCaption("Username"));
        var usernameTextBox = new TextBox { PlaceholderText = "Letters and numbers only", Dock = DockStyle.Top };
        UiStyles.StyleTextBox(usernameTextBox);
        content.Controls.Add(usernameTextBox);

        content.Controls.Add(UiStyles.CreateCaption("Password"));
        var passwordTextBox = new TextBox
        {
            PlaceholderText = "Exactly 12 characters with uppercase and lowercase",
            Dock = DockStyle.Top,
            PasswordChar = '*'
        };
        UiStyles.StyleTextBox(passwordTextBox);
        content.Controls.Add(passwordTextBox);

        content.Controls.Add(UiStyles.CreateCaption("Confirm Password"));
        var confirmPasswordTextBox = new TextBox
        {
            PlaceholderText = "Re-enter password",
            Dock = DockStyle.Top,
            PasswordChar = '*'
        };
        UiStyles.StyleTextBox(confirmPasswordTextBox);
        content.Controls.Add(confirmPasswordTextBox);

        var infoLabel = new Label
        {
            AutoSize = true,
            ForeColor = AppTheme.MutedText,
            Text = "Rules: Username letters/numbers, password exactly 12 chars with upper/lowercase.",
            Margin = new Padding(0, 12, 0, 14)
        };
        content.Controls.Add(infoLabel);

        var actionPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight
        };

        var createButton = new Button { Text = "Create Account", Width = 200 };
        UiStyles.StylePrimaryButton(createButton);
        createButton.Click += (_, _) =>
        {
            MessageBox.Show(
                "UI screen is ready. Registration logic will be implemented after design approval.",
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        };

        var backButton = new Button { Text = "Back to Login", Width = 200 };
        UiStyles.StyleSecondaryButton(backButton);
        backButton.Click += (_, _) =>
        {
            var loginForm = new LoginForm(_activityDefinitionFactory);
            loginForm.Show();
            Hide();
        };

        actionPanel.Controls.Add(createButton);
        actionPanel.Controls.Add(backButton);
        content.Controls.Add(actionPanel);

        card.Controls.Add(content);
        root.Controls.Add(titlePanel, 1, 0);
        root.Controls.Add(card, 1, 1);
        Controls.Add(root);
    }
}
