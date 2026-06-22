using FitnessTracker.Styling;

namespace FitnessTracker.Forms;

public class BaseForm : Form
{
    protected BaseForm()
    {
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = AppTheme.Tertiary;
        ForeColor = AppTheme.Neutral;
        Font = new Font(AppTheme.FontFamily, 10F, FontStyle.Regular);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = true;
        MinimizeBox = true;
    }

    protected Panel CreateCardPanel()
    {
        return new Panel
        {
            BackColor = AppTheme.Surface,
            BorderStyle = BorderStyle.FixedSingle,
            Padding = new Padding(24),
            Margin = new Padding(0)
        };
    }
}
