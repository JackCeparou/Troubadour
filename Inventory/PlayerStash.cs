namespace T4.Plugins.Troubadour;

public sealed partial class PlayerStash : JackPlugin, IGameUserInterfacePainter
{
    public InventoryFeatures Stash { get; private set; }

    public PlayerStash()
    {
        Order = 43;
        Group = PluginCategory.Inventory;
        Description = "displays information on items in player stash.\ni.e. iLvl, BreakPoint, Dungeon tier, Aspect name, etc.";
    }

    public override void Load()
    {
        Stash = InventoryFeatures.Create(this, nameof(Stash), "items",
                page: Inventory.Stash,
                font: CreateDefaultFont(bold: false),
                errorFont: CreateDefaultErrorFont(bold: false))
            .AspectHunterIcon().AspectHunterHighlight(false)
            .TreasureHunterIcon().TreasureHunterHighlight(false).TreasureHunterFilterCount(false)
            .ElixirHunterHighlight(false)
            .QualityLegendaryIcon(false)
            .QualityUniqueIcon(false)
            .NearBreakpointIcon(false)
            .ItemLevel()
            .MonsterLevel()
            .ItemQualityModifier()
            .AspectName(false).ElixirName(false) //.SigilName(false)
            .MalignantHeartIcon(false).MalignantHeartHighlight(false) // S01
            .ShowHint()
            .GreyOut(false)
            .Register();
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.OverPanels)
            return;
        if (!UserInterface.InventoryControl.Visible) // extra check, stash cannot be open if inventory is not open
            return;

        Stash.Draw();
    }
}