namespace T4.Plugins.Troubadour;

public sealed partial class ItemTextLine
{
    public bool IsName { get; init; }
    public Func<IItem, InventoryFeatures, bool> Show { get; init; } = (_, _) => true;
    public Func<IItem, InventoryFeatures, string> Text { get; init; }
    public Func<IItem, bool> HasError { get; init; } = _ => false;

    public void Draw(IItem item, InventoryFeatures features, IScreenRectangle rect,
        float x, float y, bool expandUpwards, bool alignRight, out float height)
    {
        height = 0;
        var text = Text.Invoke(item, features);
        if (string.IsNullOrEmpty(text))
            return;

        var font = features.GetFont(HasError.Invoke(item));
        if (IsName && !features.OnPaperDoll)
        {
            var padding = rect.Width * 0.1f;
            var tlName = font.GetTextLayout(text, rect.Width - (2 * padding), rect.Height * 0.75f);
            tlName.DrawText(rect.Left + padding, rect.Top + (rect.Height * 0.2f));
            return;
        }

        var tl = font.GetTextLayout(text, rect.Width * features.MaxTextWidthRatio);
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
}