namespace T4.Plugins.Troubadour;

public abstract class AuraGlobe : UiComponent
{
    public bool BackgroundEnabled { get; set; } = true;
    public IFillStyle BackgroundStyle { get; } = Render.GetFillStyle(240, 0, 0, 0);
    public abstract IFillStyle GetFillStyle();
    public abstract Func<float> GetCurrentValue { get; }
    public abstract Func<float> GetMaxValue { get; }
    public float Scale { get; set; } = 1f;
    public Func<float> GetScale { get; init; }

    public AuraGlobe()
    {
        GetScale = () => Scale;
    }

    // TODO: sanitize this because its janky AF
    protected override void DrawImpl()
    {
        var fill = GetFillStyle();
        var scale = GetScale.Invoke();
        var outerSize = Textures.GlobeForeground.Meta.Height * scale;
        var currentValue = GetCurrentValue.Invoke();
        var maxValue = GetMaxValue.Invoke();
        var percentage = currentValue / maxValue;
        var paddingTop = outerSize * 0.14f;
        var innerSize = outerSize - (paddingTop * 2);
        var innerTop = Placement.Top + paddingTop + innerSize - (innerSize * percentage);
        var o1 = outerSize * 0.2f;
        var o2 = outerSize * 0.13f;
        var x1 = Placement.Left + o1;
        var y1 = Placement.Top + paddingTop;
        var w1 = outerSize - (o1 * 2f);
        var h1 = innerSize * 0.2f;
        var x2 = Placement.Left + o2;
        var y2 = (float)Math.Floor(y1 + h1) - 1f;
        var w2 = outerSize - (o2 * 2f);
        var h2 = innerSize * 0.6f;
        var y3 = (float)Math.Floor(y2 + h2) - 1f;

        if (BackgroundEnabled)
        {
            var radius = innerSize * 0.5f;
            var centerX = Placement.Left + (outerSize * 0.5f);
            var centerY = Placement.Top + paddingTop + radius;
            BackgroundStyle.FillEllipse(centerX, centerY, radius, radius);
        }

        switch (percentage)
        {
            case > 0.8f:
            {
                var v1 = innerSize * (percentage - 0.8f);
                fill.FillRectangle(x1, innerTop, w1, v1);
                fill.FillRectangle(x2, y2, w2, h2);
                fill.FillRectangle(x1, y3, w1, h1);
                break;
            }
            case > 0.2f:
            {
                var v2 = innerSize * (percentage - 0.2f);
                fill.FillRectangle(x2, Math.Max(innerTop, y2), w2, v2);
                fill.FillRectangle(x1, y3, w1, h1);
                break;
            }
            case > 0.001f:
            {
                var v3 = innerSize * percentage;
                fill.FillRectangle(x1, Math.Max(innerTop, y3), w1, v3);
                break;
            }
        }

        if (!Host.DebugEnabled)
        {
            Textures.GlobeForeground.Draw(Placement.Left, Placement.Top, outerSize, outerSize);
            return;
        }
        DebugLineStyle.DrawRectangle(x1, y1, w1, h1);
        DebugLineStyle.DrawRectangle(x2, y2, w2, h2);
        DebugLineStyle.DrawRectangle(x1, y3, w1, h1);
    }
}