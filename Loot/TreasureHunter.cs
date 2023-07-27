namespace T4.Plugins.Troubadour;

public sealed class TreasureHunter : TroubadourPlugin, IGameWorldPainter, IItemDetector
{
    public Feature OnInventory { get; }
    public Feature OnDrop { get; }
    public Feature OnMap { get; }
    public Feature OnGround { get; }

    public bool DropNotificationEnabled { get; set; }
    public bool OnGroundLineEnabled { get; set; }
    public bool ShowFilterNamesOnGround { get; set; }

    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 178, 0, 255, DashStyle.Dash);
    public float WorldCircleSize { get; set; } = 0.4f;
    public float WorldCircleStroke { get; set; } = 8f;

    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(255, 178, 0, 255);
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 4f;

    public static IFont MatchedFilterCounterFont { get; } = Render.GetFont(255, 255, 255, 0, shadowMode: FontShadowMode.Heavy);
    public IFont FilterNamesOnGroundFont { get; } = Render.GetFont(255, 178, 0, 255);
    public IFillStyle FilterNamesOnGroundBackground { get; } = Render.GetFillStyle(255, 0, 0, 0);

    public TreasureHunter() : base(PluginCategory.Loot, "Highlight most wanted items.\nClose this window and press F5 to configure rules.")
    {
        Order = int.MaxValue;
        OnInventory = AddFeature(nameof(OnInventory), "on inventory")
            .AddFillStyleResource("GreyOutFillStyle", InventoryGreyOut.FillStyle, "grey out")
            .AddFontResource(nameof(MatchedFilterCounterFont), MatchedFilterCounterFont, "matched filter count");
        OnDrop = AddFeature(nameof(OnDrop), "on item drop")
            .AddBooleanResource(nameof(DropNotificationEnabled), "notify",
                () => DropNotificationEnabled, v => DropNotificationEnabled = v);
        OnMap = AddFeature(nameof(OnMap), "items on map")
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFloatResource(nameof(MapCircleSize), "radius",
                0, 20, () => MapCircleSize, v => MapCircleSize = v)
            .AddFloatResource(nameof(MapCircleStroke), "stroke",
                0, 10, () => MapCircleStroke, v => MapCircleStroke = v);
        OnGround = AddFeature(nameof(OnGround), "items on ground")
            .AddBooleanResource(nameof(ShowFilterNamesOnGround), "matched filter names",
                () => ShowFilterNamesOnGround, v => ShowFilterNamesOnGround = v)
            .AddFillStyleResource(nameof(FilterNamesOnGroundBackground), FilterNamesOnGroundBackground, "background color")
            .AddFontResource(nameof(FilterNamesOnGroundFont), FilterNamesOnGroundFont, "normal font")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style")
            .AddFloatResource(nameof(WorldCircleSize), "radius",
                0, 2, () => WorldCircleSize, v => WorldCircleSize = v)
            .AddFloatResource(nameof(WorldCircleStroke), "stroke",
                0, 10, () => WorldCircleStroke, v => WorldCircleStroke = v)
            .AddBooleanResource(nameof(OnGroundLineEnabled), "draw line to item",
                () => OnGroundLineEnabled, v => OnGroundLineEnabled = v);

        InventoryGreyOut.RegisterRule(int.MaxValue, item => item.FilterMatches.Length == 0);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                var groundItems = Game.Items.Where(x => x.Location == ItemLocation.None && x.FilterMatches.Length > 0);
                foreach (var item in groundItems)
                {
                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (OnGroundLineEnabled)
                        LineStyle.DrawWorldLine(item.Coordinate, Game.MyPlayerActor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (item.IsSelected)
                        item.SetHint(this);
                    if (!ShowFilterNamesOnGround || item.FilterMatches.Length == 0)
                        continue;

                    var names = string.Join(Environment.NewLine, item.FilterMatches.Select(x => x.AsString()));
                    var tl = FilterNamesOnGroundFont.GetTextLayout(names);
                    var x = item.Coordinate.ScreenX - (tl.Width / 2);
                    var y = item.Coordinate.ScreenY - (tl.Height / 2);
                    const float padding = 2f;
                    FilterNamesOnGroundBackground.FillRectangle(x - padding, y - padding, tl.Width + (padding * 2), tl.Height + (padding * 2));
                    tl.DrawText(x, y);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
                var mapItems = Game.Items.Where(x => x.Location == ItemLocation.None && x.FilterMatches.Length > 0);
                foreach (var item in mapItems)
                {
                    if (!item.Coordinate.IsOnMap)
                        continue;

                    MapLineStyle.DrawEllipse(item.Coordinate.MapX, item.Coordinate.MapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }

    public void OnItemDetected(IItem item)
    {
        if (!OnDrop.Enabled || !DropNotificationEnabled)
            return;
        if (item.Location != ItemLocation.None)
            return;
        if (item.FilterMatches.Length == 0)
            return;

        item.NotifyMatchedFilters(this);
    }
}