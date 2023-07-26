namespace T4.Plugins.Troubadour;

public static class AspectHunterStore
{
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
            return Customization.InterestingAffixes.FirstOrDefault(x => x.SnoId == affixSnoId) is not null;
        }

        return Customization.InterestingAffixes.FirstOrDefault(x => x.SnoId == item.AspectAffix?.SnoId) is not null;
    }
}