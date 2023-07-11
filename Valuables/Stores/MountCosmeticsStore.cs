namespace T4.Plugins.Troubadour;

public static class MountCosmeticsStore
{
    public static bool OnGroundEnabled { get; set; } = true;
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public static float WorldCircleSize { get; set; } = 0.5f;
    public static float WorldCircleStroke { get; set; } = 2f;
    public static bool OnMapEnabled { get; set; } = true;
    public static ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public static float MapCircleSize { get; set; } = 8f;
    public static float MapCircleStroke { get; set; } = 4f;

    public static bool IsMountCosmeticItem(this IItem item) => MountCosmeticItemSnoIdsSet.Contains(item.ItemSno.SnoId);

    public static Feature CreateMountCosmeticFeature(this IPlugin plugin, string nameOf)
    {
        var feature = new Feature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = plugin.Translate("mount cosmetics"),
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
                },
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

        return item.IsMountCosmeticItem();
    };

    public static void PaintGround()
    {
        if (!OnGroundEnabled)
            return;

        var groundItems = Game.Items.Where(WorldItemPredicate);
        foreach (var item in groundItems)
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

    public static List<ItemSnoId> MountCosmeticItemSnoIds { get; } = new()
    {
        ItemSnoId.mnt_amor00_horse,
        ItemSnoId.mnt_amor001_horse_stor,
        ItemSnoId.mnt_amor002_horse_stor,
        ItemSnoId.mnt_amor01_horse,
        ItemSnoId.mnt_amor02_horse,
        ItemSnoId.mnt_amor03_horse,
        ItemSnoId.mnt_amor04_horse,
        ItemSnoId.mnt_amor05_horse,
        ItemSnoId.mnt_amor06_horse,
        ItemSnoId.mnt_amor07_horse,
        ItemSnoId.mnt_amor08_horse,
        ItemSnoId.mnt_amor09_horse,
        ItemSnoId.mnt_amor10_horse,
        ItemSnoId.mnt_amor100_horse_dlux,
        ItemSnoId.mnt_amor101_horse,
        ItemSnoId.mnt_amor102_horse,
        ItemSnoId.mnt_amor103_horse_stor,
        ItemSnoId.mnt_amor107_horse,
        ItemSnoId.mnt_amor108_horse,
        ItemSnoId.mnt_amor109_horse,
        ItemSnoId.mnt_amor11_horse,
        ItemSnoId.mnt_amor110_horse,
        ItemSnoId.mnt_amor110_horse_stor,
        ItemSnoId.mnt_amor111_horse,
        ItemSnoId.mnt_amor112_horse_stor,
        ItemSnoId.mnt_amor113_horse_stor,
        ItemSnoId.mnt_amor114_horse_stor,
        ItemSnoId.mnt_amor12_horse,
        ItemSnoId.mnt_amor124_horse_stor,
        ItemSnoId.mnt_amor125_horse_stor,
        ItemSnoId.mnt_amor126_horse_stor,
        ItemSnoId.mnt_amor127_horse_stor,
        ItemSnoId.mnt_amor130_horse_stor,
        ItemSnoId.mnt_amor19_horse,
        ItemSnoId.mnt_amor20_horse_pvp,
        ItemSnoId.mnt_amor23_horse_pvp,
        ItemSnoId.mnt_amor24_horse_pvp,
        ItemSnoId.mnt_amor25_horse,
        ItemSnoId.mnt_amor26_horse,
        ItemSnoId.mnt_amor27_horse,
        ItemSnoId.mnt_amor28_horse,
        ItemSnoId.mnt_amor29_horse,
        ItemSnoId.mnt_amor30_horse,
        ItemSnoId.mnt_amor31_horse,
        ItemSnoId.mnt_amor50_horse,
        ItemSnoId.mnt_amor51_horse_pvp,
        ItemSnoId.mnt_amor52_horse_pvp,
        ItemSnoId.mnt_amor53_horse,
        ItemSnoId.mnt_amor54_horse,
        ItemSnoId.mnt_amor57_horse,
        ItemSnoId.mnt_amor61_horse,
        ItemSnoId.mnt_amor62_horse,
        ItemSnoId.mnt_amor63_horse,
        ItemSnoId.mnt_amor64_horse,
        ItemSnoId.mnt_amor65_horse,
        ItemSnoId.mnt_base00_horse30,
        ItemSnoId.mnt_dlux001_horse,
        ItemSnoId.mnt_dlux100_horse,
        ItemSnoId.mnt_stor001_horse,
        ItemSnoId.mnt_stor001_trophy,
        ItemSnoId.mnt_stor002_trophy,
        ItemSnoId.mnt_stor003_trophy,
        ItemSnoId.mnt_stor004_horse,
        ItemSnoId.mnt_stor004_trophy,
        ItemSnoId.mnt_stor005_horse,
        ItemSnoId.mnt_stor006_horse,
        ItemSnoId.mnt_stor009_horse,
        ItemSnoId.mnt_stor013_horse,
        ItemSnoId.mnt_stor014_horse,
        ItemSnoId.mnt_stor014_trophy,
        ItemSnoId.mnt_stor015_trophy,
        ItemSnoId.mnt_stor016_trophy,
        ItemSnoId.mnt_stor017_trophy,
        ItemSnoId.mnt_stor018_trophy,
        ItemSnoId.mnt_stor019_trophy,
        ItemSnoId.mnt_stor020_trophy,
        ItemSnoId.mnt_stor021_trophy,
        ItemSnoId.mnt_stor022_trophy,
        ItemSnoId.mnt_stor023_trophy,
        ItemSnoId.mnt_stor024_trophy,
        ItemSnoId.mnt_stor025_trophy,
        ItemSnoId.mnt_stor026_trophy,
        ItemSnoId.mnt_stor027_trophy,
        ItemSnoId.mnt_stor028_trophy,
        ItemSnoId.mnt_stor047_trophy,
        ItemSnoId.mnt_stor049_trophy,
        ItemSnoId.mnt_stor050_trophy,
        ItemSnoId.mnt_stor051_trophy,
        ItemSnoId.mnt_stor052_trophy,
        ItemSnoId.mnt_stor053_trophy,
        ItemSnoId.mnt_stor059_trophy,
        ItemSnoId.mnt_stor060_trophy,
        ItemSnoId.mnt_stor125_trophy,
        ItemSnoId.mnt_uniq01_trophy,
        ItemSnoId.mnt_uniq02_trophy,
        ItemSnoId.mnt_uniq03_trophy,
        ItemSnoId.mnt_uniq04_trophy,
        ItemSnoId.mnt_uniq05_trophy,
        ItemSnoId.mnt_uniq06_trophy,
        ItemSnoId.mnt_uniq07_trophy,
        ItemSnoId.mnt_uniq08_trophy,
        ItemSnoId.mnt_uniq09_trophy,
        ItemSnoId.mnt_uniq14_trophy,
        ItemSnoId.mnt_uniq15_trophy_pvp,
        ItemSnoId.mnt_uniq18_trophy_pvp,
        ItemSnoId.mnt_uniq19_trophy,
        ItemSnoId.mnt_uniq20_trophy_pvp,
        ItemSnoId.mnt_uniq24_trophy,
        ItemSnoId.mnt_uniq26_trophy,
        ItemSnoId.mnt_uniq27_trophy,
        ItemSnoId.mnt_uniq51_trophy_pvp,
        ItemSnoId.mnt_uniq52_trophy_pvp,
        ItemSnoId.mnt_uniq53_trophy,
        ItemSnoId.mnt_uniq54_trophy,
        ItemSnoId.mnt_uniq55_trophy,
        ItemSnoId.mnt_uniq56_trophy,
        ItemSnoId.mnt_uniq61_trophy,
        ItemSnoId.mnt_uniq62_trophy,
        ItemSnoId.mnt_uniq63_trophy,
        ItemSnoId.mnt_uniq64_trophy,
        ItemSnoId.mnt_uniq65_trophy,
        ItemSnoId.MountReins_BloodyHorse,
        ItemSnoId.MountReins_BloodyLiquidMount,
        ItemSnoId.MountReins_CaldeumHorse,
        ItemSnoId.MountReins_CavalierKnightHorse,
        ItemSnoId.MountReins_DarkHorse,
        ItemSnoId.MountReins_DecayHorse,
        ItemSnoId.MountReins_ExecutionerHorse,
        ItemSnoId.MountReins_Frac_Normal,
        ItemSnoId.MountReins_Frac_Rare,
        ItemSnoId.MountReins_GraveRobberHorse,
        ItemSnoId.MountReins_Hawezar_Normal,
        ItemSnoId.MountReins_Hawezar_Rare,
        ItemSnoId.MountReins_Kehji_Normal,
        ItemSnoId.MountReins_Kehji_Rare,
        ItemSnoId.MountReins_MottledHorse,
        ItemSnoId.MountReins_OldNellHorse,
        ItemSnoId.MountReins_Scos_Normal,
        ItemSnoId.MountReins_Scos_Rare,
        ItemSnoId.MountReins_SpectralHorse,
        ItemSnoId.MountReins_Step_Normal,
        ItemSnoId.MountReins_Step_Rare,
        ItemSnoId.MountReins_TrapperHorse,
        ItemSnoId.MountReins_WhiteHorse,
    };
    public static HashSet<ItemSnoId> MountCosmeticItemSnoIdsSet { get; } = new(MountCosmeticItemSnoIds);

}