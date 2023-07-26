using static T4.Plugins.Troubadour.AspectHunterStore;

namespace T4.Plugins.Troubadour;

public sealed class AspectHunter : JackPlugin, IGameWorldPainter
{
    public Feature OnMap { get; private set; }
    public Feature OnGround { get; private set; }

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
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGroundEnabled:
                var groundItems = Game.Items.Where(WorldItemPredicate);
                foreach (var item in groundItems)
                {
                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (!OnGroundLineEnabled)
                        continue;

                    LineStyle.DrawWorldLine(item.Coordinate, Game.MyPlayerActor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                break;
            case GameWorldLayer.Map when OnMapEnabled:
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
}