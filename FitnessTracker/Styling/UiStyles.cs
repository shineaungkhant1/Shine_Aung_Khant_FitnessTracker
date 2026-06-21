namespace FitnessTracker.Styling;

public static class UiStyles
{
    public static void StylePrimaryButton(Button button)
    {
        button.BackColor = AppTheme.Primary;
        button.ForeColor = AppTheme.Neutral;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Bold);
        button.Height = 38;
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

    public static void StyleTextBox(TextBox textBox)
    {
        textBox.BackColor = AppTheme.SurfaceSoft;
        textBox.ForeColor = AppTheme.Neutral;
        textBox.BorderStyle = BorderStyle.FixedSingle;
        textBox.Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular);
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
}
