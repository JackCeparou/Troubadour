namespace T4.Plugins.Troubadour;

public sealed partial class ItemOverlay
{
    public Func<IItem, InventoryFeatures, bool> Show { get; init; } = (_, _) => true;
    public Func<IItem, InventoryFeatures, ILineStyle> Style { get; init; }
    public Func<IItem, InventoryFeatures, IFillStyle> Fill { get; init; }
    public Func<IItem, InventoryFeatures, IFont> Font { get; init; }
    public Func<IItem, InventoryFeatures, float> StrokeWidthCorrection { get; init; } = (_, _) => 1f;
    public float OffsetLeft { get; init; } = 2f;
    public float OffsetTop { get; init; } = 0f;
    public float OffsetWidth { get; init; } = -4f;
    public float OffsetHeight { get; init; } = -4f;

    public void Draw(IItem item, InventoryFeatures features, IScreenRectangle rect)
    {
        var x = rect.Left + OffsetLeft;
        var y = rect.Top + OffsetTop;
        var w = rect.Width + OffsetWidth;
        var h = rect.Height + OffsetHeight;
        var stroke = StrokeWidthCorrection.Invoke(item, features);

        Style?.Invoke(item, features)?.DrawRectangle(x, y, w, h, strokeWidthCorrection: stroke);
        Fill?.Invoke(item, features)?.FillRectangle(x, y, w, h);
        var font = Font?.Invoke(item, features);
        if (font is null)
            return;
        // this will be limited to only one text overlay per item
        var tl = font.GetTextLayout(item.MatchingFilterNames.Length.ToString());
        y = rect.Top + rect.Height - tl.Height - (rect.Width * 0.06f);
        if (item.UpgradeCount > 0)
        {
            y -= rect.Height * 0.85f;
        }
        x += rect.Width * 0.06f;
        tl.DrawText(x, y);
    }
}