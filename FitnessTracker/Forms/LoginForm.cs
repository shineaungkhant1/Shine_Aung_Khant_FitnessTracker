using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class LoginForm : BaseForm
{
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;
    private readonly TextBox _usernameTextBox = new();
    private readonly TextBox _passwordTextBox = new();
    private readonly TextBox _registerUsernameTextBox = new();
    private readonly TextBox _registerPasswordTextBox = new();
    private readonly Label _messageLabel = new();

    public LoginForm(IActivityDefinitionFactory activityDefinitionFactory)
    {
        _activityDefinitionFactory = activityDefinitionFactory;

        Text = "Fitness Tracker - Login";
        ClientSize = new Size(1180, 720);

        BuildLayout();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Tertiary,
            Padding = new Padding(24),
            ColumnCount = 1,
            RowCount = 3
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));

        var headerPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        headerPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        headerPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

        var appTitle = new Label
        {
            Text = "KINETIC",
            Font = new Font(AppTheme.FontFamily, 34F, FontStyle.Bold),
            ForeColor = AppTheme.Neutral,
            AutoSize = true,
            Anchor = AnchorStyles.None
        };
        var appTagline = new Label
        {
            Text = "Focus. Push. Progress.",
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Anchor = AnchorStyles.None
        };
        headerPanel.Controls.Add(appTitle, 0, 0);
        headerPanel.Controls.Add(appTagline, 0, 1);

        var cardsPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(24, 8, 24, 8)
        };
        cardsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        cardsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        var loginCard = CreateCardPanel();
        loginCard.Dock = DockStyle.Fill;
        loginCard.Padding = new Padding(26);

        var loginContent = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 10
        };

        loginContent.Controls.Add(UiStyles.CreateSectionTitle("User Login"));
        loginContent.Controls.Add(UiStyles.CreateCaption("Username"));
        UiStyles.StyleTextBox(_usernameTextBox);
        _usernameTextBox.PlaceholderText = "athlete@kinetic.com";
        _usernameTextBox.Dock = DockStyle.Top;
        loginContent.Controls.Add(_usernameTextBox);

        loginContent.Controls.Add(UiStyles.CreateCaption("Password"));
        UiStyles.StyleTextBox(_passwordTextBox);
        _passwordTextBox.PlaceholderText = "Enter secure password";
        _passwordTextBox.PasswordChar = '*';
        _passwordTextBox.Dock = DockStyle.Top;
        loginContent.Controls.Add(_passwordTextBox);

        _messageLabel.AutoSize = true;
        _messageLabel.ForeColor = AppTheme.Error;
        _messageLabel.Text = "Validation and lockout logic will be wired in next phase.";
        _messageLabel.Margin = new Padding(0, 12, 0, 14);
        loginContent.Controls.Add(_messageLabel);

        var loginButton = new Button { Text = "Log In   ->", Dock = DockStyle.Top };
        UiStyles.StylePrimaryButton(loginButton);
        loginButton.Click += (_, _) =>
        {
            var dashboard = new DashboardForm(_activityDefinitionFactory, _usernameTextBox.Text.Trim());
            dashboard.Show();
            Hide();
        };
        loginContent.Controls.Add(loginButton);
        loginCard.Controls.Add(loginContent);

        var registerCard = CreateCardPanel();
        registerCard.Dock = DockStyle.Fill;
        registerCard.Padding = new Padding(26);

        var registerContent = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 10
        };
        registerContent.Controls.Add(UiStyles.CreateSectionTitle("User Registration"));

        registerContent.Controls.Add(UiStyles.CreateCaption("Username"));
        UiStyles.StyleTextBox(_registerUsernameTextBox);
        _registerUsernameTextBox.PlaceholderText = "Choose a username";
        _registerUsernameTextBox.Dock = DockStyle.Top;
        registerContent.Controls.Add(_registerUsernameTextBox);

        registerContent.Controls.Add(UiStyles.CreateCaption("Password"));
        UiStyles.StyleTextBox(_registerPasswordTextBox);
        _registerPasswordTextBox.PlaceholderText = "Exactly 12 chars";
        _registerPasswordTextBox.PasswordChar = '*';
        _registerPasswordTextBox.Dock = DockStyle.Top;
        registerContent.Controls.Add(_registerPasswordTextBox);

        var registerHint = new Label
        {
            Text = "Complete registration page for full validation rules.",
            AutoSize = true,
            ForeColor = AppTheme.MutedText,
            Margin = new Padding(0, 12, 0, 14)
        };
        registerContent.Controls.Add(registerHint);

        var registerButton = new Button { Text = "Open Full Registration +", Dock = DockStyle.Top };
        UiStyles.StylePrimaryButton(registerButton);
        registerButton.Click += (_, _) =>
        {
            var registerForm = new RegisterForm(_activityDefinitionFactory);
            registerForm.Show();
            Hide();
        };
        registerContent.Controls.Add(registerButton);
        registerCard.Controls.Add(registerContent);

        cardsPanel.Controls.Add(loginCard, 0, 0);
        cardsPanel.Controls.Add(registerCard, 1, 0);

        var statusBar = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Surface,
            ColumnCount = 2,
            RowCount = 1
        };
        statusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        statusBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        statusBar.Controls.Add(new Label
        {
            Text = "System Online",
            ForeColor = AppTheme.Success,
            AutoSize = true,
            Anchor = AnchorStyles.Left
        }, 0, 0);
        statusBar.Controls.Add(new Label
        {
            Text = "v1.2.0",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Anchor = AnchorStyles.Right
        }, 1, 0);

        root.Controls.Add(headerPanel, 0, 0);
        root.Controls.Add(cardsPanel, 0, 1);
        root.Controls.Add(statusBar, 0, 2);
        Controls.Add(root);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        Application.Exit();
    }
}
