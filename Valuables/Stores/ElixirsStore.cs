namespace T4.Plugins.Troubadour;

public static class ElixirsStore
{
    public static bool OnGroundEnabled { get; set; } = true;
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public static float WorldCircleSize { get; set; } = 0.5f;
    public static float WorldCircleStroke { get; set; } = 2f;
    public static bool OnMapEnabled { get; set; } = true;
    public static ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public static float MapCircleSize { get; set; } = 8f;
    public static float MapCircleStroke { get; set; } = 4f;

    public static bool InvertSelection { get; set; }

    public static Feature CreateElixirFeature(this IPlugin plugin, string nameOf)
    {
        var feature = new Feature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = plugin.Translate("elixirs"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(InvertSelection),
                    DisplayText = plugin.Translate("invert selection"),
                    Getter = () => InvertSelection,
                    Setter = newValue => InvertSelection = newValue
                },
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
        foreach (var snoId in ElixirItemSnoIds)
        {
            ElixirSnoIdEnabled[snoId] = false;
            feature.Resources.Add(new BooleanFeatureResource
            {
                DisplayText = () => GameData.GetItemSno(snoId)?.NameLocalized ?? snoId.ToString(),
                Getter = () => ElixirSnoIdEnabled.TryGetValue(snoId, out var enabled) && enabled,
                Setter = newValue => ElixirSnoIdEnabled[snoId] = newValue,
                NameOf = snoId.ToString()
            });
        }

        return feature.Register();
    }

    public static Func<IItem, bool> WorldItemPredicate { get; } = item =>
    {
        if (!OnGroundEnabled && !OnMapEnabled)
            return false;
        if (item.Location != ItemLocation.None)
            return false;
        if (!item.IsElixirItem())
            return false;
        if (!ElixirSnoIdEnabled.TryGetValue(item.ItemSno.SnoId, out var enabled))
            return false;

        return InvertSelection ? !enabled : enabled;
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

    public static Dictionary<ItemSnoId, bool> ElixirSnoIdEnabled { get; } = new();
    public static bool IsElixirHunted(this IItem item) 
        => ElixirSnoIdEnabled.TryGetValue(item.ItemSno.SnoId, out var enabled) && InvertSelection ? !enabled : enabled;
    public static bool IsElixirItem(this IItem item) 
        => ElixirItemSnoIdsSet.Contains(item.ItemSno.SnoId);

    public static List<ItemSnoId> ElixirItemSnoIds { get; } = new()
    {
        ItemSnoId.Elixir_Armor,
        ItemSnoId.Elixir_Curative,
        ItemSnoId.Elixir_Cheat_Death,
        ItemSnoId.Elixir_CritChance,
        ItemSnoId.Elixir_CritDamage,
        ItemSnoId.Elixir_HitEffect,
        ItemSnoId.Elixir_MaxLife,
        ItemSnoId.Elixir_MaxResource,
        ItemSnoId.Elixir_RCR,
        // offense
        ItemSnoId.Elixir_Assault_1,
        ItemSnoId.Elixir_Assault_2,
        ItemSnoId.Elixir_Assault_3,
        ItemSnoId.Elixir_Assault_4,
        ItemSnoId.Elixir_Assault_5,
        ItemSnoId.Elixir_Crushing_1,
        ItemSnoId.Elixir_Crushing_2,
        ItemSnoId.Elixir_Crushing_3,
        ItemSnoId.Elixir_Crushing_4,
        ItemSnoId.Elixir_Crushing_5,
        ItemSnoId.Elixir_Ironbarb_1,
        ItemSnoId.Elixir_Ironbarb_2,
        ItemSnoId.Elixir_Ironbarb_3,
        ItemSnoId.Elixir_Ironbarb_4,
        ItemSnoId.Elixir_Ironbarb_5,
        ItemSnoId.Elixir_Precision_1,
        ItemSnoId.Elixir_Precision_2,
        ItemSnoId.Elixir_Precision_3,
        ItemSnoId.Elixir_Precision_4,
        ItemSnoId.Elixir_Precision_5,
        ItemSnoId.Elixir_Family_Beasts,
        ItemSnoId.Elixir_Family_Demons,
        ItemSnoId.Elixir_Family_Humans,
        ItemSnoId.Elixir_Family_Undead,
        // utility
        ItemSnoId.Elixir_Acrobatics_1,
        ItemSnoId.Elixir_Acrobatics_2,
        ItemSnoId.Elixir_Acrobatics_3,
        ItemSnoId.Elixir_Acrobatics_4,
        ItemSnoId.Elixir_Acrobatics_5,
        ItemSnoId.Elixir_Vampirism_1,
        ItemSnoId.Elixir_Vampirism_2,
        ItemSnoId.Elixir_Vampirism_3,
        ItemSnoId.Elixir_Vampirism_4,
        ItemSnoId.Elixir_Vampirism_5,
        ItemSnoId.Elixir_ThirdEye_1,
        ItemSnoId.Elixir_ThirdEye_2,
        ItemSnoId.Elixir_ThirdEye_3,
        ItemSnoId.Elixir_ThirdEye_4,
        ItemSnoId.Elixir_ThirdEye_5,
        // resistances
        ItemSnoId.Elixir_MagicResist,
        ItemSnoId.Elixir_ColdResist_1,
        ItemSnoId.Elixir_ColdResist_2,
        ItemSnoId.Elixir_ColdResist_3,
        ItemSnoId.Elixir_ColdResist_4,
        ItemSnoId.Elixir_ColdResist_5,
        ItemSnoId.Elixir_FireResist_1,
        ItemSnoId.Elixir_FireResist_2,
        ItemSnoId.Elixir_FireResist_3,
        ItemSnoId.Elixir_FireResist_4,
        ItemSnoId.Elixir_FireResist_5,
        ItemSnoId.Elixir_LightningResist_1,
        ItemSnoId.Elixir_LightningResist_2,
        ItemSnoId.Elixir_LightningResist_3,
        ItemSnoId.Elixir_LightningResist_4,
        ItemSnoId.Elixir_LightningResist_5,
        ItemSnoId.Elixir_PoisonResist_1,
        ItemSnoId.Elixir_PoisonResist_2,
        ItemSnoId.Elixir_PoisonResist_3,
        ItemSnoId.Elixir_PoisonResist_4,
        ItemSnoId.Elixir_PoisonResist_5,
        ItemSnoId.Elixir_ShadowResist_1,
        ItemSnoId.Elixir_ShadowResist_2,
        ItemSnoId.Elixir_ShadowResist_3,
        ItemSnoId.Elixir_ShadowResist_4,
        ItemSnoId.Elixir_ShadowResist_5,
    };

    public static HashSet<ItemSnoId> ElixirItemSnoIdsSet { get; } = new(ElixirItemSnoIds);
}