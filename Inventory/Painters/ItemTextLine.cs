namespace T4.Plugins.Troubadour;

public sealed partial class ItemTextLine
{
    public bool IsName { get; init; }
    public Func<IItem, InventoryFeatures, bool> Show { get; init; } = (_, _) => true;
    public Func<IItem, InventoryFeatures, string> Text { get; init; }
    public Func<IItem, bool> HasError { get; init; } = _ => false;

    public void Draw(IItem item, InventoryFeatures features, IScreenRectangle screenRectangle,
        float x, float y, bool expandUpwards, bool alignRight, out float height)
    {
        height = 0;
        var text = Text.Invoke(item, features);
        if (string.IsNullOrEmpty(text))
            return;

        if (IsName && !features.OnPaperDoll)
        {
            DrawName(item, features, text, screenRectangle);
            return;
        }

        var font = features.GetFont(HasError.Invoke(item));
        var tl = font.GetTextLayout(text, screenRectangle.Width * features.MaxTextWidthRatio);
        if (expandUpwards)
        {
            y -= tl.Height;
        }

        if (alignRight)
        {
            x -= tl.Width;
        }

        tl.DrawText(x, y);
        height = expandUpwards ? -tl.Height : tl.Height;
    }

    private void DrawName(IItem item, InventoryFeatures features, string name, IScreenRectangle rect)
    {
        var font = features.GetFont(HasError.Invoke(item));
        var padding = rect.Width * 0.1f;

        var tl = font.GetTextLayout(name, rect.Width - (2 * padding), rect.Height * 0.75f);
        tl.DrawText(rect.Left + padding, rect.Top + (rect.Height * 0.2f));
    }
}