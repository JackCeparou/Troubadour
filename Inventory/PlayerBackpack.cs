namespace T4.Plugins.Troubadour;

public sealed partial class PlayerBackpack : JackPlugin, IGameUserInterfacePainter
{
    public InventoryFeatures Equipment { get; private set; }
    public InventoryFeatures Consumables { get; private set; }
    public InventoryFeatures Aspects { get; private set; }

    public PlayerBackpack()
    {
        Order = 42;
        Group = PluginCategory.Inventory;
        Description = "displays information on items in player inventory.\ni.e. iLvl, BreakPoint, Dungeon tier, Aspect name, etc.";
    }

    public override void Load()
    {
        UserInterface.RegisterInventoryControls();

        Equipment = InventoryFeatures.Create(this, nameof(Equipment), "equipment",
                page: Inventory.Equipment,
                font: CreateDefaultFont(bold: false),
                errorFont: CreateDefaultErrorFont(bold: false))
            .AspectHunterIcon().AspectHunterHighlight(false)
            .TreasureHunterIcon().TreasureHunterHighlight(false).TreasureHunterFilterCount(false)
            .QualityLegendaryIcon(false)
            .QualityUniqueIcon()
            .NearBreakpointIcon()
            .ItemLevel()
            .ItemQualityModifier()
            .AspectName(false)
            .MalignantHeartIcon(false).MalignantHeartHighlight(false) // S01
            .ShowHint()
            .GreyOut(false)
            .Register();

        Consumables = InventoryFeatures.Create(this, nameof(Consumables), "consumables",
                page: Inventory.Consumables,
                font: CreateDefaultFont(bold: false),
                errorFont: CreateDefaultErrorFont(bold: false))
            .ElixirHunterHighlight(false)
            .ItemLevel()
            .MonsterLevel(false)
            .ElixirName(false) //.SigilName(false)
            .ShowHint()
            .GreyOut(false)
            .Register();

        Aspects = InventoryFeatures.Create(this, nameof(Aspects), "aspects",
                page: Inventory.Aspects,
                font: CreateDefaultFont(bold: false),
                errorFont: CreateDefaultErrorFont(bold: false))
            .AspectHunterIcon().AspectHunterHighlight(false)
            .AspectName()
            .ShowHint()
            .GreyOut(false)
            .Register();
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