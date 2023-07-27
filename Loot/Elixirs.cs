namespace T4.Plugins.Troubadour;

public sealed class Elixirs : TroubadourPlugin, IGameWorldPainter
{
    public Feature OnGround { get; }
    public Feature OnMap { get; }
    public Feature Filters { get; }

    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float WorldCircleSize { get; set; } = 0.5f;
    public float WorldCircleStroke { get; set; } = 2f;

    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 4f;

    public static Dictionary<ItemSnoId, bool> SnoIdEnabled { get; } = new();
    public static bool IsHunted(IItem item) => SnoIdEnabled.TryGetValue(item.ItemSno.SnoId, out var enabled) && enabled;

    public Elixirs() : base(PluginCategory.Loot, "highlight most valuable elixirs")
    {
        OnGround = AddFeature(nameof(OnGround), "on ground")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style")
            .AddFloatResource(nameof(WorldCircleSize), "radius", 0, 2, () => WorldCircleSize, v => WorldCircleSize = v)
            .AddFloatResource(nameof(WorldCircleStroke), "stroke", 0, 10, () => WorldCircleStroke, v => WorldCircleStroke = v);
        OnMap = AddFeature(nameof(OnMap), "on map")
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFloatResource(nameof(MapCircleSize), "map radius", 0, 20, () => MapCircleSize, v => MapCircleSize = v)
            .AddFloatResource(nameof(MapCircleStroke), "map stroke", 0, 10, () => MapCircleStroke, v => MapCircleStroke = v);
        Filters = AddFeature(nameof(Filters), "filters");
        foreach (var snoId in ElixirItemSnoIds)
        {
            SnoIdEnabled[snoId] = false;
            Filters.AddBooleanResource(snoId.ToString(), GameData.GetItemSno(snoId)?.NameLocalized ?? snoId.ToString(),
                () => SnoIdEnabled[snoId], newValue => SnoIdEnabled[snoId] = newValue);
        }

        InventoryGreyOut.RegisterRule(100, item =>
        {
            if (!SnoIdEnabled.TryGetValue(item.ItemSno.SnoId, out _))
                return null;

            return !IsHunted(item);
        });
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (!OnGround.Enabled && !OnMap.Enabled)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                foreach (var actor in Game.Items.Where(item => item.Location == ItemLocation.None && IsHunted(item)))
                {
                    if (!actor.Coordinate.IsOnScreen)
                        continue;

                    LineStyle?.DrawWorldEllipse(WorldCircleSize, -1, actor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
                foreach (var actor in Game.Items.Where(item => item.Location == ItemLocation.None && IsHunted(item)))
                {
                    if (!actor.Coordinate.IsOnMap)
                        continue;

                    MapLineStyle?.DrawWorldEllipse(MapCircleSize, -1, actor.Coordinate, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }

    // GameData ItemUseType.ElixirScrollWhatever return way to much items
    private List<ItemSnoId> ElixirItemSnoIds { get; } = new()
    {
        ItemSnoId.Elixir_Armor,
        ItemSnoId.Elixir_Curative,
        ItemSnoId.Elixir_Cheat_Death,
        ItemSnoId.Elixir_HitEffect,
        ItemSnoId.Elixir_MaxLife,
        ItemSnoId.Elixir_MaxResource,
        ItemSnoId.Elixir_RCR,
        // offense
        ItemSnoId.Elixir_CritChance,
        ItemSnoId.Elixir_CritDamage,
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
}