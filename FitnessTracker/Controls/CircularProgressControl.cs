using System.Drawing.Drawing2D;
using FitnessTracker.Styling;

namespace FitnessTracker.Controls;

public sealed class CircularProgressControl : Control
{
    private int _progressPercent;

    public int ProgressPercent
    {
        get => _progressPercent;
        set
        {
            _progressPercent = Math.Max(0, Math.Min(100, value));
            Invalidate();
        }
    }

    public CircularProgressControl()
    {
        DoubleBuffered = true;
        Size = new Size(150, 150);
        BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var stroke = 12;
        var rect = new Rectangle(stroke, stroke, Width - (stroke * 2), Height - (stroke * 2));
        var startAngle = -90f;
        var sweep = (360f * _progressPercent) / 100f;

        using var trackPen = new Pen(AppTheme.Border, stroke);
        using var progressPen = new Pen(AppTheme.Primary, stroke)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };

        e.Graphics.DrawArc(trackPen, rect, 0, 360);
        if (_progressPercent > 0)
        {
            e.Graphics.DrawArc(progressPen, rect, startAngle, sweep);
        }
    }
}
