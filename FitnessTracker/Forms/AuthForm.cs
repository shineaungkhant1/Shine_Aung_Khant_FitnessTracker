using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class AuthForm : BaseForm
{
    private readonly IAppService _appService;
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;
    private readonly Label _loginMessage = new();
    private readonly Label _registerMessage = new();

    public AuthForm(IAppService appService, IActivityDefinitionFactory activityDefinitionFactory)
    {
        _appService = appService;
        _activityDefinitionFactory = activityDefinitionFactory;

        Text = "Fitness Tracker - Authentication";
        ClientSize = new Size(1080, 700);

        BuildLayout();
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Tertiary,
            Padding = new Padding(28),
            ColumnCount = 1,
            RowCount = 3
        };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        header.Controls.Add(new Label
        {
            Text = "KINETIC",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 34F, FontStyle.Bold),
            AutoSize = true,
            Anchor = AnchorStyles.None
        }, 0, 0);
        header.Controls.Add(new Label
        {
            Text = "Modern fitness tracker workspace",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F, FontStyle.Regular),
            AutoSize = true,
            Anchor = AnchorStyles.None
        }, 0, 1);

        var shell = CreateCardPanel();
        shell.Dock = DockStyle.Fill;
        shell.Padding = new Padding(24);

        var tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Appearance = TabAppearance.Normal,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Bold)
        };

        tabs.TabPages.Add(CreateLoginTab());
        tabs.TabPages.Add(CreateRegisterTab());
        shell.Controls.Add(tabs);

        var status = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Surface,
            ColumnCount = 2
        };
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        status.Controls.Add(new Label
        {
            Text = "System Online",
            ForeColor = AppTheme.Success,
            Anchor = AnchorStyles.Left,
            AutoSize = true
        }, 0, 0);
        status.Controls.Add(new Label
        {
            Text = "Secure Authentication Ready",
            ForeColor = AppTheme.MutedText,
            Anchor = AnchorStyles.Right,
            AutoSize = true
        }, 1, 0);

        root.Controls.Add(header, 0, 0);
        root.Controls.Add(shell, 0, 1);
        root.Controls.Add(status, 0, 2);
        Controls.Add(root);
    }

    private TabPage CreateLoginTab()
    {
        var page = new TabPage("Login")
        {
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Neutral
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 8,
            Padding = new Padding(16)
        };

        var usernameBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Username" };
        UiStyles.StyleTextBox(usernameBox);
        var passwordBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Password", PasswordChar = '*' };
        UiStyles.StyleTextBox(passwordBox);

        _loginMessage.ForeColor = AppTheme.MutedText;
        _loginMessage.AutoSize = true;

        var loginButton = new Button { Text = "Log In", Width = 170 };
        UiStyles.StylePrimaryButton(loginButton);
        loginButton.Click += (_, _) =>
        {
            if (_appService.Login(usernameBox.Text, passwordBox.Text, out var message))
            {
                var shell = new MainShellForm(_appService, _activityDefinitionFactory);
                shell.Show();
                Hide();
                _loginMessage.Text = string.Empty;
                return;
            }

            _loginMessage.ForeColor = AppTheme.Error;
            _loginMessage.Text = message;
        };

        layout.Controls.Add(UiStyles.CreateSectionTitle("Welcome Back"));
        layout.Controls.Add(UiStyles.CreateCaption("Username"));
        layout.Controls.Add(usernameBox);
        layout.Controls.Add(UiStyles.CreateCaption("Password"));
        layout.Controls.Add(passwordBox);
        layout.Controls.Add(_loginMessage);
        layout.Controls.Add(loginButton);
        page.Controls.Add(layout);
        return page;
    }

    private TabPage CreateRegisterTab()
    {
        var page = new TabPage("Register")
        {
            BackColor = AppTheme.Surface,
            ForeColor = AppTheme.Neutral
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 10,
            Padding = new Padding(16)
        };

        var usernameBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Username (letters and numbers)" };
        UiStyles.StyleTextBox(usernameBox);
        var passwordBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Password (exactly 12 chars)", PasswordChar = '*' };
        UiStyles.StyleTextBox(passwordBox);
        var confirmBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Confirm password", PasswordChar = '*' };
        UiStyles.StyleTextBox(confirmBox);

        _registerMessage.ForeColor = AppTheme.MutedText;
        _registerMessage.AutoSize = true;

        var registerButton = new Button { Text = "Create Account", Width = 190 };
        UiStyles.StylePrimaryButton(registerButton);
        registerButton.Click += (_, _) =>
        {
            if (!string.Equals(passwordBox.Text, confirmBox.Text, StringComparison.Ordinal))
            {
                _registerMessage.ForeColor = AppTheme.Error;
                _registerMessage.Text = "Passwords do not match.";
                return;
            }

            if (_appService.Register(usernameBox.Text, passwordBox.Text, out var message))
            {
                _registerMessage.ForeColor = AppTheme.Success;
                _registerMessage.Text = message;
                return;
            }

            _registerMessage.ForeColor = AppTheme.Error;
            _registerMessage.Text = message;
        };

        layout.Controls.Add(UiStyles.CreateSectionTitle("Create Account"));
        layout.Controls.Add(UiStyles.CreateCaption("Username"));
        layout.Controls.Add(usernameBox);
        layout.Controls.Add(UiStyles.CreateCaption("Password"));
        layout.Controls.Add(passwordBox);
        layout.Controls.Add(UiStyles.CreateCaption("Confirm Password"));
        layout.Controls.Add(confirmBox);
        layout.Controls.Add(_registerMessage);
        layout.Controls.Add(registerButton);
        page.Controls.Add(layout);
        return page;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        Application.Exit();
    }
}
