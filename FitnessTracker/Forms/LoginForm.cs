using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class LoginForm : BaseForm
{
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;
    private readonly TextBox _usernameTextBox = new();
    private readonly TextBox _passwordTextBox = new();
    private readonly Label _messageLabel = new();

    public LoginForm(IActivityDefinitionFactory activityDefinitionFactory)
    {
        _activityDefinitionFactory = activityDefinitionFactory;

        Text = "Fitness Tracker - Login";
        ClientSize = new Size(920, 560);

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
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

        var card = CreateCardPanel();
        card.Dock = DockStyle.Fill;

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 9
        };

        content.Controls.Add(UiStyles.CreateSectionTitle("Welcome back"));
        content.Controls.Add(UiStyles.CreateCaption("Username"));
        UiStyles.StyleTextBox(_usernameTextBox);
        _usernameTextBox.PlaceholderText = "Enter username";
        _usernameTextBox.Width = 440;
        content.Controls.Add(_usernameTextBox);

        content.Controls.Add(UiStyles.CreateCaption("Password"));
        UiStyles.StyleTextBox(_passwordTextBox);
        _passwordTextBox.PlaceholderText = "Enter password";
        _passwordTextBox.PasswordChar = '*';
        _passwordTextBox.Width = 440;
        content.Controls.Add(_passwordTextBox);

        _messageLabel.AutoSize = true;
        _messageLabel.ForeColor = AppTheme.Secondary;
        _messageLabel.Text = "UI setup phase: authentication logic will be connected next.";
        _messageLabel.Margin = new Padding(0, 12, 0, 14);
        content.Controls.Add(_messageLabel);

        var actionPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight
        };

        var loginButton = new Button { Text = "Log In", Width = 145 };
        UiStyles.StylePrimaryButton(loginButton);
        loginButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _usernameTextBox.Text.Trim());
            dashboard.Show();
            Hide();
        };

        var registerButton = new Button { Text = "Create Account", Width = 145 };
        UiStyles.StyleSecondaryButton(registerButton);
        registerButton.Click += (_, _) =>
        {
            var registerForm = new RegisterForm(_activityDefinitionFactory);
            registerForm.Show();
            Hide();
        };

        actionPanel.Controls.Add(loginButton);
        actionPanel.Controls.Add(registerButton);
        content.Controls.Add(actionPanel);

        card.Controls.Add(content);
        root.Controls.Add(card, 1, 0);
        Controls.Add(root);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        Application.Exit();
    }
}
