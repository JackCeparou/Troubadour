namespace T4.Plugins.Troubadour;

public class InventoryPage
{
    public Func<IItem, bool> ItemPredicate { get; init; }
    public Func<IUIControl> GetUiContainer { get; init; }
    public Func<IItem, IScreenRectangle> GetItemControl { get; init; }

    public int LineCount { get; }
    public int ColumnCount { get; }
    public int PageCount { get; }

    public int SlotCount { get; }
    public bool Visible => GetUiContainer?.Invoke()?.Visible ?? false;

    public InventoryPage(int lines, int columns, int pageCount = 1)
    {
        LineCount = lines;
        ColumnCount = columns;
        PageCount = pageCount;
        SlotCount = lines * columns;
    }

    public void Draw(InventoryFeatures features)
    {
        if (!Visible || !features.Enabled)
            return;

        var items = Game.Items.Where(ItemPredicate);
        foreach (var item in items)
        {
            var control = GetItemControl.Invoke(item);
            if (control is null)
                continue;

            DrawIcons(item, control, features);
            DrawLines(item, control, features, control.Width * 0.06f);
            DrawOverlays(item, control, features);

            SetHint(item, control, features);
        }
    }

    private void SetHint(IItem item, IScreenRectangle control, InventoryFeatures features)
    {
        if (!features.HasHints)
            return;
        if (!Game.CursorInsideRect(control.Left, control.Top, control.Width, control.Height))
            return;

        item.SetHint(features.Plugin);
    }

    private static void DrawOverlays(IItem item, IScreenRectangle control, InventoryFeatures features)
    {
        if (!features.HasOverlays)
            return;

        foreach (var highlight in features.ItemOverlays)
        {
            if (!highlight.Show.Invoke(item, features))
                continue;

            highlight.Draw(item, features, control);
        }
    }

    private static void DrawIcons(IItem item, IScreenRectangle control, InventoryFeatures features)
    {
        if (!features.HasIcons)
            return;

        var x = control.Left;
        var y = control.Top;
        foreach (var icon in features.ItemIcons)
        {
            x += icon.Draw(item, features, x, y, false, false);
        }
    }

    private static void DrawLines(IItem item, IScreenRectangle control, InventoryFeatures features, float offset)
    {
        if (!features.HasLines)
            return;

        var x = control.Left + control.Width - (offset * 1.25f);
        var y = control.Top + control.Height - offset;
        var first = true;
        foreach (var line in features.ItemLines)
        {
            if (!line.Show.Invoke(item, features))
                continue;

            if (first && item.ItemPower != item.ItemPowerTotal)
            {
                y -= control.Height * 0.2f;
                first = false;
            }

            line.Draw(item, features, control, x, y, true, true, out var height);
            y += height;
        }
    }
}