namespace T4.Plugins.Troubadour;

public sealed partial class PlayerBackpack : JackPlugin, IGameUserInterfacePainter
{
    public InventoryFeatures Equipment { get; }
    public InventoryFeatures Consumables { get; }
    public InventoryFeatures Aspects { get; }

    public PlayerBackpack() : base(PluginCategory.Inventory, "displays information on items in player inventory.\ni.e. iLvl, BreakPoint, Dungeon tier, Aspect name, etc.")
    {
        Order = 42;
        UserInterface.RegisterInventoryControls();

        Equipment = InventoryFeatures.Create(this, nameof(Equipment), "equipment",
                page: new(3, 11)
                {
                    ItemPredicate = x => x.Location == ItemLocation.PlayerBackpack,
                    GetUiContainer = () => UserInterface.GetInventoryBackpackControl(),
                    GetItemControl = item => UserInterface.GetInventorySlotControl(0, item.InventoryX, item.InventoryY),
                },
                font: CreateDefaultFont(bold: false),
                errorFont: CreateDefaultErrorFont(bold: false))
            .AspectHunterIcon().AspectHunterHighlight(false)
            .TreasureHunterIcon().TreasureHunterHighlight(false).TreasureHunterFilterCount(false)
            .QualityIcon(false)
            .NearBreakpointIcon()
            .ItemLevel()
            .AspectName(false)
            .ShowHint()
            .GreyOut(false);

        Consumables = InventoryFeatures.Create(this, nameof(Consumables), "consumables",
                page: new(3, 11)
                {
                    ItemPredicate = x => x.Location == ItemLocation.PlayerConsumables,
                    GetUiContainer = () => UserInterface.GetInventoryConsumablesControl(),
                    GetItemControl = item => UserInterface.GetInventorySlotControl(1, item.InventoryX, item.InventoryY),
                },
                font: CreateDefaultFont(bold: false),
                errorFont: CreateDefaultErrorFont(bold: false))
            .ElixirHunterHighlight(false)
            .ItemLevel()
            .MonsterLevel(false)
            .ElixirName(false)
            .ShowHint()
            .GreyOut(false);

        Aspects = InventoryFeatures.Create(this, nameof(Aspects), "aspects",
                page: new(2, 11)
                {
                    ItemPredicate = x => x.Location == ItemLocation.PlayerMaterials,
                    GetUiContainer = () => UserInterface.GetInventoryAspectsControl(),
                    GetItemControl = item => UserInterface.GetInventorySlotControl(3, item.InventoryX, item.InventoryY),
                },
                font: CreateDefaultFont(bold: false),
                errorFont: CreateDefaultErrorFont(bold: false))
            .AspectHunterIcon().AspectHunterHighlight(false)
            .AspectName()
            .ShowHint()
            .GreyOut(false);
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.OverPanels)
            return;
        if (!UserInterface.InventoryControl.Visible)
            return;

        Equipment.Draw();
        Consumables.Draw();
        Aspects.Draw();
    }
}