namespace T4.Plugins.Troubadour;

public sealed class SeasonOfTheMalignant : JackPlugin, IGameWorldPainter
{
    public Feature HeartsOnGround { get; private set; }
    public ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float WorldCircleSize { get; set; } = 0.5f;
    public float WorldCircleStroke { get; set; } = 1f;

    public Feature HeartsOnMap { get; private set; }
    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 2f;

    public SeasonOfTheMalignant() : base(PluginCategory.Loot,"Season 1 companion")
    {
        Order = int.MaxValue;
        SnoIdsSet = _snoIds.ToHashSet();
        HeartsOnGround = AddFeature(nameof(HeartsOnGround), "malignant hearts on ground")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style")
            .AddFloatResource(nameof(WorldCircleSize), "radius", 0, 2, () => WorldCircleSize, v => WorldCircleSize = v)
            .AddFloatResource(nameof(WorldCircleStroke), "stroke", 0, 10, () => WorldCircleStroke, v => WorldCircleStroke = v);
        HeartsOnMap = AddFeature(nameof(HeartsOnMap), "malignant hearts on map")
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFloatResource(nameof(MapCircleSize), "radius", 0, 20, () => MapCircleSize, v => MapCircleSize = v)
            .AddFloatResource(nameof(MapCircleStroke), "stroke", 0, 10, () => MapCircleStroke, v => MapCircleStroke = v);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (!HeartsOnGround.Enabled && !HeartsOnMap.Enabled)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground when HeartsOnGround.Enabled:
                foreach (var actor in Game.GizmoActors.Where(item => SnoIdsSet.Contains(item.ActorSno.SnoId)))
                {
                    LineStyle?.DrawWorldEllipse(WorldCircleSize, -1, actor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                break;
            case GameWorldLayer.Map when HeartsOnMap.Enabled:
                foreach (var actor in Game.GizmoActors.Where(item => SnoIdsSet.Contains(item.ActorSno.SnoId)))
                {
                    if (!Map.WorldToMapCoordinate(actor.Coordinate, out var mapX, out var mapY))
                        continue;

                    MapLineStyle?.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }

    private HashSet<ActorSnoId> SnoIdsSet { get; }

    private readonly IEnumerable<ActorSnoId> _snoIds = new[]
    {
        ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Pink_Dyn, ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Blue_Dyn,
        ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Black_Dyn, ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Orange_Dyn,
        // ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Dyn, // <-- not sure about this one.
    };
}