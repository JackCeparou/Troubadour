namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemOverlay CreateGreyOut() => new()
    {
        OffsetLeft = 0f,
        OffsetTop = 0f,
        OffsetWidth = 0f,
        OffsetHeight = 0f,
        Show = ShouldGreyOut,
        Fill = (item, _) => item.MatchingFilterNames.Length == 0 ? TreasureHunterStore.GreyOutFillStyle : null,
    };

    private static bool ShouldGreyOut(IItem item, InventoryFeatures features)
    {
        if (!features.GreyOutEnabled)
            return false;
        if (features.GreyOutUpgradedItemsEnabled && item.UpgradeCount > 0)
            return false;
        if (features.GreyOutGemItemsEnabled && (item.ItemSno.GemType != GemType.None || item.ItemSno.SnoId == ItemSnoId.GamblingCurrency_Key))
            return false;
        if (features.GreyOutSigilItemsEnabled && item.ItemSno.ItemUseType == ItemUseType.DungeonKey)
            return false;
        if (item.IsMountCosmeticItem())
            return false;
        if (item.IsElixirItem())
            return !item.IsElixirHunted();
        if (item.IsAspectItem() && AspectHunterStore.AffixSnoIdEnabled.Any(x => x.Value))
            return !item.IsAspectHunted();

        // S01 //*
        if (item.IsMalignantInvoker())
            return false;
        if (SeasonOfTheMalignantStore.MalignantHeartAffixSnoIdEnabled.Any(x => x.Value))
        {
            if (item.IsMalignantHeart())
                return !item.IsMalignantHeartHunted();
            var heart = item.GetMalignantHeartLegendaryAffix();
            if (heart is not null)
                return !heart.SnoId.IsMalignantHeartHunted();
        }
        // S01 */

        return item.MatchingFilterNames.Length == 0 && !item.IsAspectHunted();
    }
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures GreyOut(bool enabled = true)
    {
        AddOverlay(new BooleanFeatureResource
        {
            NameOf = nameof(GreyOutEnabled),
            DisplayText = () => Translation.Translate(Plugin, "grey out not hunted"),
            Getter = () => GreyOutEnabled,
            Setter = v => GreyOutEnabled = v,
        });
        GreyOutEnabled = enabled;
        return this;
    }
}