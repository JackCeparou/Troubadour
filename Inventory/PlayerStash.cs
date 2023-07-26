namespace T4.Plugins.Troubadour;

public sealed partial class PlayerStash : JackPlugin, IGameUserInterfacePainter
{
    public InventoryFeatures Stash { get; private set; }

    public PlayerStash() : base(PluginCategory.Inventory, "displays information on items in player stash.\ni.e. iLvl, BreakPoint, Dungeon tier, Aspect name, etc.")
    {
        Order = 43;
        Stash = InventoryFeatures.Create(this, nameof(Stash), "items",
                page: new(5, 10, 4)
                {
                    ItemPredicate = x => x.Location == ItemLocation.Stash,
                    GetUiContainer = () => UserInterface.GetInventoryStashControl(),
                    GetItemControl = item => UserInterface.GetItemUIControlIfVisible(item),
                },
                font: CreateDefaultFont(bold: false),
                errorFont: CreateDefaultErrorFont(bold: false))
            .AspectHunterIcon().AspectHunterHighlight(false)
            .TreasureHunterIcon().TreasureHunterHighlight(false).TreasureHunterFilterCount(false)
            .ElixirHunterHighlight(false)
            .QualityIcon(false, false)
            .NearBreakpointIcon(false)
            .ItemLevel()
            .MonsterLevel()
            .AspectName(false).ElixirName(false)
            .ShowHint()
            .GreyOut(false);
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