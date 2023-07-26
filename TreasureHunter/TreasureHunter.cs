using static T4.Plugins.Troubadour.TreasureHunterStore;

namespace T4.Plugins.Troubadour;

public sealed class TreasureHunter : JackPlugin, IGameWorldPainter, IItemDetector
{
    public Feature OnInventory { get; }
    public Feature OnDrop { get; }
    public Feature OnMap { get; }
    public Feature OnGround { get; }

    public TreasureHunter() : base(PluginCategory.Loot, "Highlight most wanted items.\nClose this window and press F5 to configure rules.")
    {
        Order = int.MaxValue;
        OnInventory = AddFeature(nameof(OnInventory), "on inventory")
            .AddFillStyleResource(nameof(GreyOutFillStyle), GreyOutFillStyle, "grey out")
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
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                var groundItems = Game.Items.Where(WorldItemPredicate);
                foreach (var item in groundItems)
                {
                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (OnGroundLineEnabled)
                        LineStyle.DrawWorldLine(item.Coordinate, Game.MyPlayerActor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (!ShowFilterNamesOnGround || item.MatchingFilterNames.Length == 0)
                        continue;

                    var names = string.Join(Environment.NewLine, item.MatchingFilterNames);
                    var tl = FilterNamesOnGroundFont.GetTextLayout(names);
                    var x = item.Coordinate.ScreenX - (tl.Width / 2);
                    var y = item.Coordinate.ScreenY - (tl.Height / 2);
                    const float padding = 2f;
                    FilterNamesOnGroundBackground.FillRectangle(x - padding, y - padding, tl.Width + (padding * 2), tl.Height + (padding * 2));
                    tl.DrawText(x, y);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
                var mapItems = Game.Items.Where(WorldItemPredicate);
                foreach (var item in mapItems)
                {
                    if (!Map.WorldToMapCoordinate(item.Coordinate, out var mapX, out var mapY))
                        continue;

                    MapLineStyle.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
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
        if (item.MatchingFilterNames.Length == 0)
            return;

        item.NotifyMatchedFilters(this);
    }
}