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
        var root = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            BackColor = AppTheme.Tertiary
        };

        var headerPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 140
        };
        var appTitle = new Label
        {
            Text = "KINETIC",
            Font = new Font(AppTheme.FontFamily, 34F, FontStyle.Bold),
            ForeColor = AppTheme.Neutral,
            AutoSize = true,
            Location = new Point(500, 14)
        };
        var appTagline = new Label
        {
            Text = "Focus. Push. Progress.",
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Location = new Point(506, 84)
        };
        headerPanel.Controls.Add(appTitle);
        headerPanel.Controls.Add(appTagline);

        var cardsPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(24, 12, 24, 12)
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
        _usernameTextBox.Width = 430;
        loginContent.Controls.Add(_usernameTextBox);

        loginContent.Controls.Add(UiStyles.CreateCaption("Password"));
        UiStyles.StyleTextBox(_passwordTextBox);
        _passwordTextBox.PlaceholderText = "Enter secure password";
        _passwordTextBox.PasswordChar = '*';
        _passwordTextBox.Width = 430;
        loginContent.Controls.Add(_passwordTextBox);

        _messageLabel.AutoSize = true;
        _messageLabel.ForeColor = AppTheme.Error;
        _messageLabel.Text = "Validation and lockout logic will be wired in next phase.";
        _messageLabel.Margin = new Padding(0, 12, 0, 14);
        loginContent.Controls.Add(_messageLabel);

        var loginButton = new Button { Text = "Log In   ->", Width = 430 };
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
        _registerUsernameTextBox.Width = 430;
        registerContent.Controls.Add(_registerUsernameTextBox);

        registerContent.Controls.Add(UiStyles.CreateCaption("Password"));
        UiStyles.StyleTextBox(_registerPasswordTextBox);
        _registerPasswordTextBox.PlaceholderText = "Exactly 12 chars";
        _registerPasswordTextBox.PasswordChar = '*';
        _registerPasswordTextBox.Width = 430;
        registerContent.Controls.Add(_registerPasswordTextBox);

        var registerHint = new Label
        {
            Text = "Complete registration page for full validation rules.",
            AutoSize = true,
            ForeColor = AppTheme.MutedText,
            Margin = new Padding(0, 12, 0, 14)
        };
        registerContent.Controls.Add(registerHint);

        var registerButton = new Button { Text = "Open Full Registration +", Width = 430 };
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

        var statusBar = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 28,
            BackColor = AppTheme.Surface
        };
        statusBar.Controls.Add(new Label
        {
            Text = "System Online",
            ForeColor = AppTheme.Success,
            AutoSize = true,
            Location = new Point(10, 6)
        });
        statusBar.Controls.Add(new Label
        {
            Text = "v1.2.0",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Location = new Point(1115, 6)
        });

        root.Controls.Add(cardsPanel);
        root.Controls.Add(headerPanel);
        root.Controls.Add(statusBar);
        Controls.Add(root);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        Application.Exit();
    }
}
