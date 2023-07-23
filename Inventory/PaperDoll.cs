using static T4.Plugins.Troubadour.Inventory;

namespace T4.Plugins.Troubadour;

public sealed partial class PaperDoll : JackPlugin, IGameUserInterfacePainter
{
    public InventoryFeatures Equipped { get; private set; }
    private readonly Throttler _throttler = Throttler.Create(100);

    public PaperDoll()
    {
        Group = PluginCategory.Inventory;
        Description = "displays information about items on paper doll.\ni.e. iLvl, BreakPoint, Aspect name, etc.";
    }

    public override void Load()
    {
        Equipped = InventoryFeatures.Create(this, nameof(Equipped), "equipped items",
                font: CreateDefaultFont(wordWrap: true),
                errorFont: CreateDefaultErrorFont(wordWrap: true))
            .QualityLegendaryIcon(false)
            .QualityUniqueIcon()
            .AspectHunterIcon(false)
            .TreasureHunterIcon(false)//.TreasureHunterHighlight(false)
            .NearBreakpointIcon()
            .ItemLevel()
            .ItemQualityModifier()
            .AspectName()
            .MalignantHeartIcon(false).MalignantHeartName() // S01
            .PaperDoll()
            .Register();
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.OverPanels)
            return;
        if (!UserInterface.InventoryControl.Visible)
            return;
        if (!Equipped.Enabled)
            return;

        var items = Game.GetEquippedItems();
        _throttler.RunWhenElapsed(() => items.RefreshDuplicatePowers());
        foreach (var item in items)
        {
            var uiControl = UserInterface.GetEquippedItemLocationControl(item.Location);
            if (uiControl is null)
                continue;

            GetPlacementAndOrientation(uiControl, item, out var x, out var y, out var expandUpwards, out var alignRight);

            y = DrawIconsLine(item, x, y, expandUpwards, alignRight);

            DrawTextLines(item, uiControl, x, y, expandUpwards, alignRight);
            if (uiControl.CoordinateInside(Game.CursorX, Game.CursorY))
            {
                item.SetHint(this);
            }
        }

        /*
        var control = UserInterface.GetRegisteredControlByPath("inventory_dialog_mainPage.InventoryContainer.stats");
        if (control is null || !control.Visible || !Game.CursorInsideRect(control.Left, control.Top, control.Width, control.Height))
            return;

        var timeSpan = TimeSpan.FromSeconds(Game.MyPlayer.SecondsPlayed);
        var text = timeSpan switch
        {
            {Days: > 0} => $"{timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s",
            {Hours: > 0} => $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s",
            {Minutes: > 0} => $"{timeSpan.Minutes}m {timeSpan.Seconds}s",
            {Seconds: > 0} => $"{timeSpan.Seconds}s",
            _ => "0s"
        };
        var tl = DebugFont.GetTextLayout(text);
        tl.DrawText(control.Left + (control.Width * 0.48f) - (tl.Width / 2), control.Top + (control.Width * 0.125f) - (tl.Height / 2));
        //*/
    }

    private float DrawIconsLine(IItem item, float x, float y, bool expandUpwards, bool alignRight)
    {
        var height = 0f;
        foreach (var icon in ItemIcons)
        {
            var size = icon.Draw(item, Equipped, x, y, expandUpwards, alignRight);
            if (size == 0)
                continue;

            x += alignRight ? -size : size;
            height = size;
        }

        if (height == 0)
        {
            return y + (Equipped.IconSize / 2);
        }

        return expandUpwards
            ? y - (height / 2)
            : y + height;
    }

    private void DrawTextLines(IItem item, IScreenRectangle uiControl, float xPos, float yPos, bool expandUpwards, bool alignRight)
    {
        foreach (var line in ItemLines)
        {
            if (!line.Show.Invoke(item, Equipped))
                continue;

            line.Draw(item, Equipped, uiControl, xPos, yPos, expandUpwards, alignRight, out var height);
            yPos += height;
        }
    }

    private void GetPlacementAndOrientation(IScreenRectangle uiControl, IItem item,
        out float x, out float y, out bool expandUpwards, out bool alignRight)
    {
        alignRight = false;
        expandUpwards = true;
        x = uiControl.Left + uiControl.Width;
        y = uiControl.Top + (uiControl.Height * 0.8f);
        switch (item.Location)
        {
            // on the right
            case ItemLocation.PlayerNeck:
            case ItemLocation.PlayerRightFinger:
            case ItemLocation.PlayerLeftFinger:
                alignRight = true;
                x = uiControl.Left;
                y = uiControl.Top + (uiControl.Height * 0.825f);
                break;
            // main hand
            case ItemLocation.PlayerMainHand:
                y = uiControl.Top + (uiControl.Height * 0.8375f);
                break;
            // off hands
            case ItemLocation.PlayerOffHand:
            case ItemLocation.Player2HSlash:
                x = uiControl.Left;
                y = uiControl.Top + (uiControl.Height * 0.8f);
                alignRight = true;
                break;
            // barbarian or rogue
            case ItemLocation.Player1HMain:
                x = uiControl.Left;
                alignRight = true;

                var isBarbarianMainHand = Game.MyPlayer?.Actor?.PlayerClassSno?.SnoId == PlayerClassSnoId.Barbarian;
                if (isBarbarianMainHand)
                {
                    x = uiControl.Left + uiControl.Width - (uiControl.Width * 0.35f);
                    expandUpwards = true;
                    y = uiControl.Top - (Equipped.IconSize / 2);
                }

                break;
            case ItemLocation.Player1HOff:
                x = uiControl.Left + uiControl.Width;
                y = uiControl.Top - (Equipped.IconSize / 2);
                expandUpwards = true;
                alignRight = true;

                var isBarbarianOffHand = Game.MyPlayer?.Actor?.PlayerClassSno?.SnoId == PlayerClassSnoId.Barbarian;
                if (isBarbarianOffHand)
                {
                    x = uiControl.Left + (uiControl.Width * 0.35f);
                    alignRight = false;
                }

                break;
        }
    }
}