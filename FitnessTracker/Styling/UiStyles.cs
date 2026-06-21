namespace FitnessTracker.Styling;

public static class UiStyles
{
    public static void StylePrimaryButton(Button button)
    {
        button.BackColor = AppTheme.Primary;
        button.ForeColor = AppTheme.Tertiary;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Bold);
        button.Height = 42;
        button.Cursor = Cursors.Hand;
    }

    public static void StyleSecondaryButton(Button button)
    {
        button.BackColor = AppTheme.SurfaceSoft;
        button.ForeColor = AppTheme.Neutral;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = AppTheme.Border;
        button.Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular);
        button.Height = 38;
        button.Cursor = Cursors.Hand;
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
        textBox.Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular);
        textBox.Height = 34;
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

        var titleLabel = new Label
        {
            Text = title,
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Font = new Font(AppTheme.FontFamily, 9F, FontStyle.Bold)
        };
        var valueLabel = new Label
        {
            Text = value,
            ForeColor = AppTheme.Neutral,
            AutoSize = true,
            Font = new Font(AppTheme.FontFamily, 14F, FontStyle.Bold),
            Margin = new Padding(0, 8, 0, 6)
        };
        var subtitleLabel = new Label
        {
            Text = subtitle,
            ForeColor = AppTheme.MutedText,
            AutoSize = true,
            Font = new Font(AppTheme.FontFamily, 9F, FontStyle.Regular)
        };

        panel.Controls.Add(subtitleLabel);
        panel.Controls.Add(valueLabel);
        panel.Controls.Add(titleLabel);

        subtitleLabel.Location = new Point(14, 68);
        valueLabel.Location = new Point(14, 34);
        titleLabel.Location = new Point(14, 14);
        panel.Height = 106;
        return panel;
    }
}
