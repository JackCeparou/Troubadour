using static T4.Plugins.Troubadour.AspectHunterStore;

namespace T4.Plugins.Troubadour;

public sealed class AspectHunter : JackPlugin, IGameWorldPainter
{
    public Feature OnMap { get; private set; }
    public Feature OnGround { get; private set; }

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
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                var groundItems = Game.Items.Where(x => x.Location == ItemLocation.None && x.IsAspectHunted());
                foreach (var item in groundItems)
                {
                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                    if (!OnGroundLineEnabled)
                        continue;

                    LineStyle.DrawWorldLine(item.Coordinate, Game.MyPlayerActor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
                var mapItems = Game.Items.Where(x => x.Location == ItemLocation.None && x.IsAspectHunted());
                foreach (var item in mapItems)
                {
                    if (!item.Coordinate.IsOnMap)
                        continue;

                    MapLineStyle.DrawEllipse(item.Coordinate.MapX, item.Coordinate.MapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }
}