using FitnessTracker.Services;
using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public sealed class RegisterScreenForm : BaseForm
{
    private readonly IAppService _appService;
    private readonly IActivityDefinitionFactory _activityDefinitionFactory;

    public RegisterScreenForm(IAppService appService, IActivityDefinitionFactory activityDefinitionFactory)
    {
        _appService = appService;
        _activityDefinitionFactory = activityDefinitionFactory;

        Text = "Fitness Tracker - Register";
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
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 2
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));

        var backArrowButton = new Button
        {
            Text = "← Back",
            Width = 78,
            Height = 34,
            Anchor = AnchorStyles.Left | AnchorStyles.Top,
            Margin = new Padding(0, 8, 0, 0)
        };
        UiStyles.StyleSecondaryButton(backArrowButton);
        backArrowButton.Click += (_, _) =>
        {
            var loginForm = new LoginScreenForm(_appService, _activityDefinitionFactory);
            loginForm.Show();
            Hide();
        };
        header.Controls.Add(backArrowButton, 0, 0);

        header.Controls.Add(new Label
        {
            Text = "KINETIC",
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 34F, FontStyle.Bold),
            AutoSize = true,
            Anchor = AnchorStyles.None
        }, 1, 0);
        header.Controls.Add(new Label
        {
            Text = "Create your fitness account",
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 11F),
            AutoSize = true,
            Anchor = AnchorStyles.None
        }, 1, 1);

        var cardWrap = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 1
        };
        cardWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
        cardWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
        cardWrap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));

        var card = CreateCardPanel();
        card.Dock = DockStyle.Fill;
        card.Padding = new Padding(22);

        var form = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 12
        };
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        form.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        form.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));

        var usernameBox = new TextBox { Dock = DockStyle.Fill, PlaceholderText = "Username (letters and numbers only)" };
        UiStyles.StyleTextBox(usernameBox);
        var passwordBox = new TextBox
        {
            Dock = DockStyle.Fill,
            PlaceholderText = "Password",
            UseSystemPasswordChar = true,
            PasswordChar = '*'
        };
        UiStyles.StyleTextBox(passwordBox);
        var confirmBox = new TextBox
        {
            Dock = DockStyle.Fill,
            PlaceholderText = "Confirm password",
            UseSystemPasswordChar = true,
            PasswordChar = '*'
        };
        UiStyles.StyleTextBox(confirmBox);

        var registerButton = new Button { Text = "Create Account", Dock = DockStyle.Fill };
        UiStyles.StylePrimaryButton(registerButton);
        registerButton.Click += (_, _) =>
        {
            if (!string.Equals(passwordBox.Text, confirmBox.Text, StringComparison.Ordinal))
            {
                MessageBox.Show("Passwords do not match.", "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_appService.Register(usernameBox.Text, passwordBox.Text, out var message))
            {
                MessageBox.Show(message, "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var loginForm = new LoginScreenForm(_appService, _activityDefinitionFactory);
                loginForm.Show();
                Hide();
                return;
            }

            MessageBox.Show(message, "Register Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        };

        var backButton = new Button { Text = "Back to Login", Dock = DockStyle.Fill };
        UiStyles.StyleSecondaryButton(backButton);
        backButton.Click += (_, _) =>
        {
            var loginForm = new LoginScreenForm(_appService, _activityDefinitionFactory);
            loginForm.Show();
            Hide();
        };

        form.Controls.Add(UiStyles.CreateSectionTitle("Create Account"));
        form.Controls.Add(UiStyles.CreateCaption("Username"));
        form.Controls.Add(usernameBox);
        form.Controls.Add(UiStyles.CreateCaption("Password"));
        form.Controls.Add(passwordBox);
        form.Controls.Add(UiStyles.CreateCaption("Confirm Password"));
        form.Controls.Add(confirmBox);
        form.Controls.Add(new Label
        {
            Text = "Password must include upper and lower case letters.",
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Margin = new Padding(0, 8, 0, 8)
        });
        form.Controls.Add(registerButton);
        form.Controls.Add(backButton);

        card.Controls.Add(form);
        cardWrap.Controls.Add(card, 1, 0);

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
            Text = "Registration Ready",
            ForeColor = AppTheme.MutedText,
            Anchor = AnchorStyles.Right,
            AutoSize = true
        }, 1, 0);

        root.Controls.Add(header, 0, 0);
        root.Controls.Add(cardWrap, 0, 1);
        root.Controls.Add(status, 0, 2);
        Controls.Add(root);
    }
}
