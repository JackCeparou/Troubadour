namespace T4.Plugins.Troubadour;

public sealed class AspectHunter : TroubadourPlugin, IGameWorldPainter
{
    public Feature OnMap { get; }
    public Feature OnGround { get; }

    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 178, 0, 255);
    public float WorldCircleSize { get; set; } = 0.5f;
    public float WorldCircleStroke { get; set; } = 6f;
    public bool OnGroundLineEnabled { get; set; }

    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(255, 178, 0, 255);
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 4f;

    public AspectHunter() : base(PluginCategory.Loot, "Highlight most wanted aspects.")
    {
        Order = int.MaxValue;
        OnMap = AddFeature(nameof(OnMap), "items on map")
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFloatResource(nameof(MapCircleSize), "radius", 0, 42, () => MapCircleSize, v => MapCircleSize = v)
            .AddFloatResource(nameof(MapCircleStroke), "stroke", 0, 10, () => MapCircleStroke, v => MapCircleStroke = v);
        OnGround = AddFeature(nameof(OnGround), "items on ground")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style")
            .AddFloatResource(nameof(WorldCircleSize), "radius", 0, 42, () => WorldCircleSize, v => WorldCircleSize = v)
            .AddFloatResource(nameof(WorldCircleStroke), "stroke", 0, 10, () => WorldCircleStroke, v => WorldCircleStroke = v)
            .AddBooleanResource(nameof(OnGroundLineEnabled), "draw line to item", () => OnGroundLineEnabled, v => OnGroundLineEnabled = v);

        InventoryGreyOut.RegisterRule(1000, item =>
        {
            if (item.FilterMatches.Length == 0 && item.EquippedLegendaryAffixes.Any() && Customization.InterestingAffixes.Any())
            {
                return !IsHunted(item);
            }

            return null;
        });
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                var groundItems = Game.Items.Where(x => x.Location == ItemLocation.None && IsHunted(x));
                foreach (var item in groundItems)
                {
                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (item.IsSelected)
                        item.SetHint(this);
                    if (!OnGroundLineEnabled)
                        continue;

                    LineStyle.DrawWorldLine(item.Coordinate, Game.MyPlayerActor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
                var mapItems = Game.Items.Where(x => x.Location == ItemLocation.None && IsHunted(x));
                foreach (var item in mapItems)
                {
                    if (!item.Coordinate.IsOnMap)
                        continue;

                    MapLineStyle.DrawEllipse(item.Coordinate.MapX, item.Coordinate.MapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }

    public static bool IsHunted(IItem item)
    {
        if (item.Quality is not (ItemQuality.Legendary or ItemQuality.Unique or ItemQuality.Set))
            return false;
        if (item.ItemSno is null)
            return false;

        if (item.Quality is ItemQuality.Unique && item.ItemSno.SnoId.TryGetUniqueAffixSnoId(out var affixSnoId))
        {
            return Customization.InterestingAffixes.FirstOrDefault(x => x.SnoId == affixSnoId) is not null;
        }

        var snoId = item.AspectAffix?.SnoId ?? item.EquippedLegendaryAffixes.FirstOrDefault(x => x.IsSeasonal)?.SnoId;
        return snoId is not null && Customization.InterestingAffixes.FirstOrDefault(x => x.SnoId == snoId) is not null;
    }
}