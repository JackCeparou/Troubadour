namespace T4.Plugins.Troubadour;

public static class CarryablesStore
{
    public static bool OnGroundEnabled { get; set; } = true;
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public static float WorldCircleSize { get; set; } = 0.5f;
    public static float WorldCircleStroke { get; set; } = 2f;
    public static bool OnMapEnabled { get; set; } = true;
    public static ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public static float MapCircleSize { get; set; } = 8f;
    public static float MapCircleStroke { get; set; } = 4f;

    public static Feature CreateCarryableFeature(this IPlugin plugin, string nameOf)
    {
        var feature = new Feature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = plugin.Translate("carryable items"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnGroundEnabled),
                    DisplayText = plugin.Translate("on ground"),
                    Getter = () => OnGroundEnabled,
                    Setter = newValue => OnGroundEnabled = newValue
                },
                new LineStyleFeatureResource { NameOf = nameof(LineStyle), DisplayText = plugin.LineStyle, LineStyle = LineStyle },
                new FloatFeatureResource
                {
                    NameOf = nameof(WorldCircleSize),
                    DisplayText = plugin.Radius,
                    Getter = () => WorldCircleSize,
                    Setter = newValue => WorldCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 2
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(WorldCircleStroke),
                    DisplayText = plugin.Stroke,
                    Getter = () => WorldCircleStroke,
                    Setter = newValue => WorldCircleStroke = newValue,
                    MinValue = 0,
                    MaxValue = 10
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnMapEnabled),
                    DisplayText = plugin.Translate("on map"),
                    Getter = () => OnMapEnabled,
                    Setter = newValue => OnMapEnabled = newValue
                },
                new LineStyleFeatureResource { NameOf = nameof(MapLineStyle), DisplayText = plugin.MapLineStyle, LineStyle = MapLineStyle },
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleSize),
                    DisplayText = plugin.MapRadius,
                    Getter = () => MapCircleSize,
                    Setter = newValue => MapCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 20
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleStroke),
                    DisplayText = plugin.MapStroke,
                    Getter = () => MapCircleStroke,
                    Setter = newValue => MapCircleStroke = newValue,
                    MinValue = 0,
                    MaxValue = 10
                }
            }
        };

        return feature.Register();
    }

    public static Func<IItem, bool> WorldItemPredicate { get; } = item =>
    {
        if (!OnGroundEnabled && !OnMapEnabled)
            return false;
        if (item.Location != ItemLocation.None)
            return false;

        return CarryableItemSnoIdsSet.Contains(item.ActorSno.SnoId);
    };

    public static void PaintGround()
    {
        if (!OnGroundEnabled)
            return;

        var items = Game.Items.Where(WorldItemPredicate);
        foreach (var item in items)
        {
            LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
        }
    }

    public static void PaintMap()
    {
        if (!OnMapEnabled)
            return;

        var items = Game.Items.Where(WorldItemPredicate);
        foreach (var item in items)
        {
            if (!Map.WorldToMapCoordinate(item.Coordinate, out var mapX, out var mapY))
                continue;

            MapLineStyle.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
        }
    }

    public static List<ActorSnoId> CarryableItemSnoIds { get; } = new()
    {
        ActorSnoId.Carryable_AncientsStatue,
        ActorSnoId.Carryable_Bloodstone,
        ActorSnoId.Carryable_CrusaderSkull,
        ActorSnoId.Carryable_DefacedShrine,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_01,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_02,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_03,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Purified,
        ActorSnoId.Carryable_Mechanical,
        ActorSnoId.Carryable_RunicStandingStone,
        ActorSnoId.Carryable_SightlessEye,
        ActorSnoId.Carryable_StoneCarving,
        ActorSnoId.Carryable_Winch,
    };

    public static HashSet<ActorSnoId> CarryableItemSnoIdsSet { get; } = new(CarryableItemSnoIds);
}