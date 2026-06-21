namespace FitnessTracker.Styling;

public static class UiStyles
{
    public static void StylePrimaryButton(Button button)
    {
        button.BackColor = AppTheme.Primary;
        button.ForeColor = AppTheme.Tertiary;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#7EA8F0");
        button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#B3D0FF");
        button.Font = new Font(AppTheme.FontFamily, 10.5F, FontStyle.Bold);
        button.Height = 44;
        button.Cursor = Cursors.Hand;
        button.Padding = new Padding(10, 0, 10, 0);
    }

    public static void StyleSecondaryButton(Button button)
    {
        button.BackColor = AppTheme.SurfaceSoft;
        button.ForeColor = AppTheme.Neutral;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = AppTheme.Border;
        button.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#2A3442");
        button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2A3442");
        button.Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular);
        button.Height = 40;
        button.Cursor = Cursors.Hand;
        button.Padding = new Padding(10, 0, 10, 0);
    }

    public static void StyleSidebarButton(Button button, bool active = false)
    {
        button.BackColor = active ? AppTheme.SurfaceSoft : AppTheme.Sidebar;
        button.ForeColor = active ? AppTheme.Primary : AppTheme.MutedText;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = active ? AppTheme.Primary : AppTheme.Border;
        button.Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular);
        button.Height = 42;
        button.TextAlign = ContentAlignment.MiddleLeft;
        button.Padding = new Padding(14, 0, 0, 0);
        button.Cursor = Cursors.Hand;
    }

    public static void StyleTextBox(TextBox textBox)
    {
        textBox.BackColor = AppTheme.SurfaceSoft;
        textBox.ForeColor = AppTheme.Neutral;
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.Font = new Font(AppTheme.FontFamily, 10.5F, FontStyle.Regular);
        textBox.Height = 36;
    }

    public static Label CreateSectionTitle(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = AppTheme.Neutral,
            Font = new Font(AppTheme.FontFamily, 13F, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 16)
        };
    }

    public static Label CreateCaption(string text)
    {
        return new Label
        {
            Text = text,
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 9.5F, FontStyle.Regular),
            AutoSize = true,
            Margin = new Padding(0, 8, 0, 4)
        };
    }

    public static Panel CreateMetricCard(string title, string value, string subtitle)
    {
        var panel = new Panel
        {
            BackColor = AppTheme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(14),
            Margin = new Padding(0, 0, 0, 12)
        };

        var content = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3
        };
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));

        content.Controls.Add(new Label
        {
            Text = title,
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Font = new Font(AppTheme.FontFamily, 9F, FontStyle.Bold)
        }, 0, 0);
        content.Controls.Add(new Label
        {
            Text = value,
            ForeColor = AppTheme.Neutral,
            AutoSize = true,
            Font = new Font(AppTheme.FontFamily, 14F, FontStyle.Bold),
            Margin = new Padding(0, 8, 0, 4)
        }, 0, 1);
        content.Controls.Add(new Label
        {
            Text = subtitle,
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Font = new Font(AppTheme.FontFamily, 9F, FontStyle.Regular)
        }, 0, 2);

        panel.Controls.Add(content);
        return panel;
    }

    public static Panel CreateInfoPanel(string title, string body)
    {
        var panel = new Panel
        {
            BackColor = AppTheme.SurfaceSoft,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(18)
        };

        panel.Controls.Add(new Label
        {
            Text = title,
            ForeColor = AppTheme.Primary,
            Font = new Font(AppTheme.FontFamily, 12F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(16, 16)
        });
        panel.Controls.Add(new Label
        {
            Text = body,
            ForeColor = AppTheme.MutedText,
            Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular),
            AutoSize = true,
            MaximumSize = new Size(260, 0),
            Location = new Point(16, 48)
        });
        return panel;
    }
}
