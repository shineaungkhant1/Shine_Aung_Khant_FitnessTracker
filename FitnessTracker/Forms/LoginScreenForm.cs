using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class LoginScreenForm : BaseForm
{
    private readonly IAppService _appService;
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;

    public LoginScreenForm(IAppService appService, IActivityDefinitionFactory activityDefinitionFactory)
    {
        _appService = appService;
        _activityDefinitionFactory = activityDefinitionFactory;

        Text = "Fitness Tracker - Login";
        ClientSize = new Size(980, 620);
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
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
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
            Text = "Sign in to your fitness workspace",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F),
            AutoSize = true,
            Anchor = AnchorStyles.None
        }, 0, 1);

        var cardWrap = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = 1
        };
        cardWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        cardWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
        cardWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        cardWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));

        var card = CreateCardPanel();
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(22);

        var form = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 9
        };

        var usernameBox = new TextBox { Dock = DockStyle.Top, PlaceholderText = "Username" };
        UiStyles.StyleTextBox(usernameBox);
        var passwordBox = new TextBox
        {
            Dock = DockStyle.Top,
            PlaceholderText = "Password",
            UseSystemPasswordChar = true,
            PasswordChar = '*'
        };
        UiStyles.StyleTextBox(passwordBox);

        var loginButton = new Button { Text = "Log In", Width = 190 };
        UiStyles.StylePrimaryButton(loginButton);
        loginButton.Click += (_, _) =>
        {
            if (_appService.Login(usernameBox.Text, passwordBox.Text, out var message))
            {
                MessageBox.Show(message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var shell = new MainShellForm(_appService, _activityDefinitionFactory);
                shell.Show();
                Hide();
                return;
            }

            MessageBox.Show(message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        };

        var registerButton = new Button { Text = "Create New Account", Width = 220 };
        UiStyles.StyleSecondaryButton(registerButton);
        registerButton.Click += (_, _) =>
        {
            var registerForm = new RegisterScreenForm(_appService, _activityDefinitionFactory);
            registerForm.Show();
            Hide();
        };

        form.Controls.Add(UiStyles.CreateSectionTitle("User Login"));
        form.Controls.Add(UiStyles.CreateCaption("Username"));
        form.Controls.Add(usernameBox);
        form.Controls.Add(UiStyles.CreateCaption("Password"));
        form.Controls.Add(passwordBox);
        form.Controls.Add(new Label
        {
            Text = "Account locks after 3 failed attempts.",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Margin = new Padding(0, 8, 0, 12)
        });
        form.Controls.Add(loginButton);
        form.Controls.Add(registerButton);

        card.Controls.Add(form);
        cardWrap.Controls.Add(card, 1, 0);
        cardWrap.Controls.Add(UiStyles.CreateInfoPanel(
            "Why Kinetic?",
            "Track calories, set goals, and monitor progress from one workspace.\n\nYour account auto-locks temporarily after repeated failed login attempts for safety."),
            2, 0);

        var status = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = AppTheme.Surface,
            ColumnCount = 2
        };
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        status.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
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
        root.Controls.Add(cardWrap, 0, 1);
        root.Controls.Add(status, 0, 2);
        Controls.Add(root);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        base.OnFormClosed(e);
        Application.Exit();
    }
}
