namespace T4.Plugins.Troubadour;

public static class AspectHunterStore
{
    public static bool OnGroundEnabled { get; set; } = true;
    public static bool OnGroundLineEnabled { get; set; } = false;
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 178, 0, 255);
    public static float WorldCircleSize { get; set; } = 0.5f;
    public static float WorldCircleStroke { get; set; } = 6f;
    public static bool OnMapEnabled { get; set; } = true;
    public static ILineStyle MapLineStyle { get; } = Render.GetLineStyle(255, 178, 0, 255);
    public static float MapCircleSize { get; set; } = 8f;
    public static float MapCircleStroke { get; set; } = 4f;

    public static bool OnlyMyCurrentClass { get; set; }

    public static Dictionary<AffixSnoId, bool> AffixSnoIdEnabled { get; } = new();

    public static Func<IItem, bool> WorldItemPredicate { get; } = item =>
    {
        if (item.Location != ItemLocation.None)
            return false;

        switch (item.Quality)
        {
            case ItemQuality.Set:
            case ItemQuality.Unique:
            case ItemQuality.Legendary:
                return item.IsAspectHunted();

            default:
                return false;
        }
    };

    public static IEnumerable<ItemSnoId> AspectItemSnoIds { get; } = new List<ItemSnoId>
    {
        ItemSnoId.ItemAspect_Defensive_01,
        ItemSnoId.ItemAspect_Defensive_02,
        ItemSnoId.ItemAspect_Defensive_03,
        ItemSnoId.ItemAspect_Mobility_01,
        ItemSnoId.ItemAspect_Mobility_02,
        ItemSnoId.ItemAspect_Mobility_03,
        ItemSnoId.ItemAspect_Offensive_01,
        ItemSnoId.ItemAspect_Offensive_02,
        ItemSnoId.ItemAspect_Offensive_03,
        ItemSnoId.ItemAspect_Resource_01,
        ItemSnoId.ItemAspect_Resource_02,
        ItemSnoId.ItemAspect_Resource_03,
        ItemSnoId.ItemAspect_Utility_01,
        ItemSnoId.ItemAspect_Utility_02,
        ItemSnoId.ItemAspect_Utility_03,
        ItemSnoId.ItemAspect_Weapon_01,
        ItemSnoId.ItemAspect_Weapon_02,
        ItemSnoId.ItemAspect_Weapon_03,
    };

    public static readonly HashSet<ItemSnoId> AspectItemSnoIdsSet = new(AspectItemSnoIds);
    public static bool IsAspectItem(this IItem item) => AspectItemSnoIdsSet.Contains(item.ItemSno.SnoId);

    public static AspectItemType GetAspectItemType(this IItem item) => item.ItemSno.SnoId switch
    {
        ItemSnoId.ItemAspect_Defensive_01 or ItemSnoId.ItemAspect_Defensive_02 or ItemSnoId.ItemAspect_Defensive_03 => AspectItemType.Defensive,
        ItemSnoId.ItemAspect_Mobility_01 or ItemSnoId.ItemAspect_Mobility_02 or ItemSnoId.ItemAspect_Mobility_03 => AspectItemType.Mobility,
        ItemSnoId.ItemAspect_Offensive_01 or ItemSnoId.ItemAspect_Offensive_02 or ItemSnoId.ItemAspect_Offensive_03 => AspectItemType.Offensive,
        ItemSnoId.ItemAspect_Resource_01 or ItemSnoId.ItemAspect_Resource_02 or ItemSnoId.ItemAspect_Resource_03 => AspectItemType.Resource,
        ItemSnoId.ItemAspect_Utility_01 or ItemSnoId.ItemAspect_Utility_02 or ItemSnoId.ItemAspect_Utility_03 => AspectItemType.Utility,
        ItemSnoId.ItemAspect_Weapon_01 or ItemSnoId.ItemAspect_Weapon_02 or ItemSnoId.ItemAspect_Weapon_03 => AspectItemType.Weapon,
        _ => AspectItemType.None,
    };

    public static bool IsAspectHunted(this IItem item)
    {
        if (item.Quality is not (ItemQuality.Legendary or ItemQuality.Unique or ItemQuality.Set))
            return false;
        if (item.ItemSno is null)
            return false;

        if (item.Quality is ItemQuality.Unique && item.ItemSno.SnoId.TryGetUniqueAffixSnoId(out var affixSnoId))
        {
            return affixSnoId.IsAspectHunted();
        }

        return item.MainAffixes
            .Where(x => x.MagicType is not MagicType.None)
            .Any(affix => affix.SnoId.IsAspectHunted());
    }

    public static bool IsAspectHunted(this AffixSnoId affixSnoId)
    {
        if (!OnlyMyCurrentClass)
            return AffixSnoIdEnabled.TryGetValue(affixSnoId, out var enabled) && enabled;

        var isGeneric = AffixSnoIds.GenericSet.Contains(affixSnoId) || AffixSnoIds.GenericUniqueSet.Contains(affixSnoId);
        if (isGeneric)
            return AffixSnoIdEnabled.TryGetValue(affixSnoId, out var genericEnabled) && genericEnabled;

        var isMyClass = Game.MyPlayerActor.PlayerClassSno.SnoId switch
        {
            PlayerClassSnoId.Barbarian => AffixSnoIds.BarbarianSet.Contains(affixSnoId) || AffixSnoIds.BarbarianUniqueSet.Contains(affixSnoId),
            PlayerClassSnoId.Druid => AffixSnoIds.DruidSet.Contains(affixSnoId) || AffixSnoIds.DruidUniqueSet.Contains(affixSnoId),
            PlayerClassSnoId.Necromancer => AffixSnoIds.NecromancerSet.Contains(affixSnoId) || AffixSnoIds.NecromancerUniqueSet.Contains(affixSnoId),
            PlayerClassSnoId.Rogue => AffixSnoIds.RogueSet.Contains(affixSnoId) || AffixSnoIds.RogueUniqueSet.Contains(affixSnoId),
            PlayerClassSnoId.Sorcerer => AffixSnoIds.SorcererSet.Contains(affixSnoId) || AffixSnoIds.SorcererUniqueSet.Contains(affixSnoId),
            _ => false,
        };
        if (!isMyClass)
            return false;

        return AffixSnoIdEnabled.TryGetValue(affixSnoId, out var affixEnabled) && affixEnabled;
    }
}