namespace T4.Plugins.Troubadour;

public sealed partial class PlayerStash : BasePlugin, IGameUserInterfacePainter
{
    public InventoryFeatures Stash { get; private set; }

    public PlayerStash()
    {
        Order = 43;
        EnabledByDefault = true;
    }

    public override void Load()
    {
        Stash = InventoryFeatures.Create(this, nameof(Stash), "items",
                CreateDefaultFont(bold: true),
                CreateDefaultErrorFont(bold: true))
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
            .ShowHint()
            .GreyOut(false)
            .Register();
    }

    public override PluginCategory Category
        => PluginCategory.Inventory;

    public override string GetDescription()
        => Translation.Translate(this, "displays information on items in player stash.\ni.e. iLvl, BreakPoint, Dungeon tier, Aspect name, etc.");

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.OverPanels)
            return;
        if (!UserInterface.InventoryControl.Visible) // extra check, stash cannot be open if inventory is not open
            return;

        Inventory.Stash.Draw(Stash);
    }
}