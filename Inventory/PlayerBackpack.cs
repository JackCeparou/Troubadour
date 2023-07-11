namespace T4.Plugins.Troubadour;

public sealed partial class PlayerBackpack : BasePlugin,
    IGameUserInterfacePainter,
    ITroubadourPlugin
{
    public InventoryFeatures Equipment { get; private set; }
    public InventoryFeatures Consumables { get; private set; }
    public InventoryFeatures Aspects { get; private set; }

    public PlayerBackpack()
    {
        Order = 42;
        EnabledByDefault = true;
    }

    public override void Load()
    {
        UserInterface.RegisterInventoryControls();

        Equipment = InventoryFeatures.Create(this, nameof(Equipment), "equipment",
                font: CreateDefaultFont(bold: true),
                errorFont: CreateDefaultErrorFont(bold: true))
            .AspectHunterIcon().AspectHunterHighlight(false)
            .TreasureHunterIcon().TreasureHunterHighlight(false).TreasureHunterFilterCount(false)
            .QualityLegendaryIcon(false)
            .QualityUniqueIcon()
            .NearBreakpointIcon()
            .ItemLevel()
            .ItemQualityModifier()
            .AspectName(false)
            .ShowHint()
            .GreyOut(false)
            .Register();

        Consumables = InventoryFeatures.Create(this, nameof(Consumables), "consumables",
                font: CreateDefaultFont(bold: true),
                errorFont: CreateDefaultErrorFont(bold: true))
            .ElixirHunterHighlight(false)
            .ItemLevel()
            .MonsterLevel(false)
            .ElixirName(false) //.SigilName(false)
            .ShowHint()
            .GreyOut(false)
            .Register();

        Aspects = InventoryFeatures.Create(this, nameof(Aspects), "aspects",
                font: CreateDefaultFont(bold: true),
                errorFont: CreateDefaultErrorFont(bold: true))
            .AspectHunterIcon().AspectHunterHighlight(false)
            .AspectName()
            .ShowHint()
            .GreyOut(false)
            .Register();
    }

    public void OnScreenResize()
    {
        // noop
    }

    public override string GetDescription()
        => "Display information on items in player inventory.\ni.e. iLvl, BreakPoint, Dungeon tier, Aspect name, etc.";

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.OverPanels)
            return;
        if (!UserInterface.InventoryControl.Visible)
            return;

        Inventory.Equipment.Draw(Equipment);
        Inventory.Consumables.Draw(Consumables);
        Inventory.Aspects.Draw(Aspects);
    }
}