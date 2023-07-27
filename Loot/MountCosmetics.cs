namespace T4.Plugins.Troubadour;

public sealed class MountCosmetics : TroubadourPlugin, IGameWorldPainter
{
    public Feature OnGround { get; }
    public Feature OnMap { get; }

    public ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float WorldCircleSize { get; set; } = 0.5f;
    public float WorldCircleStroke { get; set; } = 2f;

    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 4f;

    private static HashSet<ItemSnoId> _snoIdsSet;
    private static bool IsMountCosmetic(IItem item) => _snoIdsSet.Contains(item.ItemSno.SnoId);

    public MountCosmetics() : base(PluginCategory.Loot, "highlight mount cosmetics")
    {
        OnGround = AddFeature(nameof(OnGround), "on ground")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style")
            .AddFloatResource(nameof(WorldCircleSize), "radius", 0, 2, () => WorldCircleSize, v => WorldCircleSize = v)
            .AddFloatResource(nameof(WorldCircleStroke), "stroke", 0, 10, () => WorldCircleStroke, v => WorldCircleStroke = v);
        OnMap = AddFeature(nameof(OnMap), "on map")
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFloatResource(nameof(MapCircleSize), "map radius", 0, 20, () => MapCircleSize, v => MapCircleSize = v)
            .AddFloatResource(nameof(MapCircleStroke), "map stroke", 0, 10, () => MapCircleStroke, v => MapCircleStroke = v);
        _snoIdsSet = GameData.AllItemSno
            .Where(x => x.ItemUseType is ItemUseType.Mount or ItemUseType.Test70)
            .Select(x => x.SnoId)
            .ToHashSet();

        InventoryGreyOut.RegisterRule(100, item =>
        {
            if (!IsMountCosmetic(item))
                return null;

            return false;
        });
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (!OnGround.Enabled && !OnMap.Enabled)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                foreach (var actor in Game.Items.Where(item => item.Location == ItemLocation.None && IsMountCosmetic(item)))
                {
                    if (!actor.Coordinate.IsOnScreen)
                        continue;

                    LineStyle?.DrawWorldEllipse(WorldCircleSize, -1, actor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
                foreach (var actor in Game.Items.Where(item => item.Location == ItemLocation.None && IsMountCosmetic(item)))
                {
                    if (!actor.Coordinate.IsOnMap)
                        continue;

                    MapLineStyle?.DrawWorldEllipse(MapCircleSize, -1, actor.Coordinate, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }
}