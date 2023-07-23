namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    public static List<ItemTextLine> ItemLines { get; } = new()
    {
        CreateItemLevel(),
        CreateMonsterLevel(),
        CreateMalignantHeartName(), // S01
        CreateAspectName(),
        CreateSigilName(),
        CreateElixirName(),
    };

    public static List<ItemIcon> ItemIcons { get; } = new()
    {
        CreateTreasureHunter(),
        CreateAspectHunterIcon(),
        CreateMalignantHeartHunterIcon(), // S01
        CreateQuality(),
        CreateNearBreakpoint(),
    };

    public static List<ItemOverlay> ItemOverlays { get; } = new()
    {
        CreateGreyOut(),
        CreateAspectHunterHighlight(),
        CreateTreasureHunterHighlight(),
        CreateTreasureHunterMatchedFilterCount(),
        CreateElixirHunterHighlight(),
        CreateMalignantHeartHunterHighlight(), // S01
    };

    public static List<AffixSnoId> DuplicateEquippedPowers { get; } = new();

    public static readonly InventoryPage Stash = new(5, 10, 4)
    {
        ItemPredicate = x => x.Location == ItemLocation.Stash,
        GetUiContainer = () => UserInterface.GetInventoryStashControl(),
        GetItemControl = item => UserInterface.GetItemUIControlIfVisible(item),
    };

    public static readonly InventoryPage Equipment = new(3, 11)
    {
        ItemPredicate = x => x.Location == ItemLocation.PlayerBackpack,
        GetUiContainer = () => UserInterface.GetInventoryBackpackControl(),
        GetItemControl = item => UserInterface.GetInventorySlotControl(0, item.InventoryX, item.InventoryY),
    };

    public static readonly InventoryPage ShopList = new(8, 1) // TODO: fix it for scroll
    {
        ItemPredicate = x => x.Location == ItemLocation.Merchant,
        GetUiContainer = () => UserInterface.GetShopListControl(),
        GetItemControl = item => UserInterface.GetShopListSlotControl(item.InventoryX, item.InventoryY),
    };

    public static readonly InventoryPage Consumables = new(3, 11)
    {
        ItemPredicate = x => x.Location == ItemLocation.PlayerConsumables,
        GetUiContainer = () => UserInterface.GetInventoryConsumablesControl(),
        GetItemControl = item => UserInterface.GetInventorySlotControl(1, item.InventoryX, item.InventoryY),
    };

    public static readonly InventoryPage QuestItems = new(2, 11)
    {
        ItemPredicate = x => x.Location == ItemLocation.PlayerQuestItems,
        GetUiContainer = () => UserInterface.GetInventoryQuestItemsControl(),
        GetItemControl = item => UserInterface.GetInventorySlotControl(2, item.InventoryX, item.InventoryY),
    };

    public static readonly InventoryPage Aspects = new(2, 11)
    {
        ItemPredicate = x => x.Location == ItemLocation.PlayerMaterials,
        GetUiContainer = () => UserInterface.GetInventoryAspectsControl(),
        GetItemControl = item => UserInterface.GetInventorySlotControl(3, item.InventoryX, item.InventoryY),
    };

    public static List<IItem> RefreshDuplicatePowers(this List<IItem> items)
    {
        if (DuplicateEquippedPowers.Any())
            DuplicateEquippedPowers.Clear();

        // that's awful 🤠
        var duplicatePowers = items
            .Select(x => x.GetEternalLegendaryAffix())
            .Where(x => x is not null && x.MagicType is not MagicType.None)
            .Select(x => x.SnoId)
            .GroupBy(x => x)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();

        if (duplicatePowers.Length > 0)
            DuplicateEquippedPowers.AddRange(duplicatePowers);

        return items;
    }
}