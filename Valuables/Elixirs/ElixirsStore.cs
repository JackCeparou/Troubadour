namespace T4.Plugins.Troubadour;

public static class ElixirsStore
{
    public static bool InvertSelection { get; set; }
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);

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