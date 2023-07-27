namespace T4.Plugins.Troubadour;

public sealed class SeasonOfTheMalignant : TroubadourPlugin, IGameWorldPainter
{
    public Feature HeartsOnGround { get; }
    public ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float WorldCircleSize { get; set; } = 0.5f;
    public float WorldCircleStroke { get; set; } = 1f;

    public Feature HeartsOnMap { get; }
    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 2f;

    public SeasonOfTheMalignant() : base(PluginCategory.Loot,"Season 1 companion")
    {
        HeartsOnGround = AddFeature(nameof(HeartsOnGround), "malignant hearts on ground")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style")
            .AddFloatResource(nameof(WorldCircleSize), "radius", 0, 2, () => WorldCircleSize, v => WorldCircleSize = v)
            .AddFloatResource(nameof(WorldCircleStroke), "stroke", 0, 10, () => WorldCircleStroke, v => WorldCircleStroke = v);
        HeartsOnMap = AddFeature(nameof(HeartsOnMap), "malignant hearts on map")
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFloatResource(nameof(MapCircleSize), "radius", 0, 20, () => MapCircleSize, v => MapCircleSize = v)
            .AddFloatResource(nameof(MapCircleStroke), "stroke", 0, 10, () => MapCircleStroke, v => MapCircleStroke = v);
        // no need to register GreyOut because there is no inventory feature
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
                    if (!actor.Coordinate.IsOnMap)
                        continue;

                    MapLineStyle?.DrawEllipse(actor.Coordinate.MapX, actor.Coordinate.MapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }

    private HashSet<ActorSnoId> SnoIdsSet { get; } = new()
    {
        ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Pink_Dyn, ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Blue_Dyn,
        ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Black_Dyn, ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Orange_Dyn,
        // ActorSnoId.S01_MalignantHeart_CaptureSequence_Main_Dyn, // <-- not sure about this one.
    };
}
/*
public static class SeasonOfTheMalignantExtensions
{
    public static bool IsMalignantHeart(this IItem item) => MalignantHeartsSnoIdsSet.Contains(item.ItemSno.SnoId);
    public static bool IsMalignantInvoker(this IItem item) => MalignantInvokersSnoIdsSet.Contains(item.ItemSno.SnoId);

    private static readonly IEnumerable<ItemSnoId> MalignantHeartsSnoIdsSet = new HashSet<ItemSnoId>
    {
        ItemSnoId.S01_MalignantOrb_Defensive,
        ItemSnoId.S01_MalignantOrb_Offensive,
        ItemSnoId.S01_MalignantOrb_Utility,
        ItemSnoId.S01_MalignantOrb_Universal,
        ItemSnoId.S01_MalignantOrb_Universal_QST_S01_Main_04_VarshanHeart,
    };

    private static readonly IEnumerable<ItemSnoId> MalignantInvokersSnoIdsSet = new HashSet<ItemSnoId>
    {
        ItemSnoId.S01_Malignant_Key_Defensive,
        ItemSnoId.S01_Malignant_Key_Offensive,
        ItemSnoId.S01_Malignant_Key_Utility,
        ItemSnoId.S01_Malignant_Key_Varshan,
        ItemSnoId.S01_Malignant_Key_Universal,
        ItemSnoId.S01_Malignant_Key_Varshan_T3,
        ItemSnoId.S01_Malignant_Key_Varshan_T4,
    };
}
//*/