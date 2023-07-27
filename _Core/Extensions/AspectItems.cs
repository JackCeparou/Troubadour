namespace T4.Plugins.Troubadour;

public static class AspectItems
{
    public static bool IsAspectItem(this IItem item) => AspectItemSnoIdsSet.Contains(item.ItemSno.SnoId);

    private static readonly HashSet<ItemSnoId> AspectItemSnoIdsSet = new()
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
}