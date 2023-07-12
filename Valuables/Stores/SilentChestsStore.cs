namespace T4.Plugins.Troubadour;

public static class SilentChestsStore
{
    public static bool OnGroundEnabled { get; set; } = true;
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public static float WorldCircleSize { get; set; } = 0.5f;
    public static float WorldCircleStroke { get; set; } = 2f;
    public static bool OnMapEnabled { get; set; } = true;
    public static ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public static float MapCircleSize { get; set; } = 8f;
    public static float MapCircleStroke { get; set; } = 4f;

    public static Feature CreateSilentChestFeature(this IPlugin plugin, string nameOf)
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

        return SilentChestSnoIdsSet.Contains(item.ActorSno.SnoId);
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

    public static List<ActorSnoId> SilentChestSnoIds { get; } = new()
    {
        ActorSnoId.Generic_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Goatman_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.HaweHU_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.HaweHU_Smuggler_Rare_Chest_Locked_GamblingCurrency,
        ActorSnoId.HaweHU_Witch_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Hell_Prop_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.MageHalls_Prop_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.RedChurch_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Scos_Forest_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.ScosglenHU_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.ScosglenHU_Druid_Chest_Rare_Bloody_Locked_GamblingCurrency,
        ActorSnoId.ScosglenHU_Druid_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.SoDun_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.SoDun_Chest_Rare_Locked_GamblingCurrency_BloodRain,
        ActorSnoId.Spider_Chest_Rare_Locked_GamblingCurrency,
    };

    public static HashSet<ActorSnoId> SilentChestSnoIdsSet { get; } = new(SilentChestSnoIds);
}