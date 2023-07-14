using static T4.Plugins.Troubadour.TreasureHunterStore;

namespace T4.Plugins.Troubadour;

public sealed partial class Vendors : BasePlugin, IGameUserInterfacePainter, IItemDetector
{
    public InventoryFeatures OnShopList { get; private set; }

    public Vendors()
    {
        EnabledByDefault = true;
    }

    public override PluginCategory Category
        => PluginCategory.Inventory;

    public override string GetDescription()
        => Translation.Translate(this, "displays information about items on vendor panels.");

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.OverPanels)
            return;
        if (!OnShopList.Enabled || !OnShopList.TreasureHunterHighlightEnabled)
            return;

        var shopList = UserInterface.GetShopListControl();
        if (shopList is null || !shopList.Visible)
            return;

        var items = Game.Items.Where(x => x.Location == ItemLocation.Merchant).ToArray();

        if (items.Any(x => x.MatchingFilterNames.Length > 0))
        {
            var x = shopList.Left + (shopList.Width * 0.005f);
            var y = shopList.Top + (shopList.Height * 0.005f);
            var w = shopList.Width * 0.925f;
            var h = (shopList.Height / 8) * Math.Min(items.Length, 8);
            LineStyle.DrawRectangle(x, y, w, h, strokeWidthCorrection: 1.5f);
            var lines = items.SelectMany(item => item.MatchingFilterNames).Distinct().OrderBy(item => item);
            var tl = OnShopList.NormalFont.GetTextLayout(string.Join(Environment.NewLine, lines), shopList.Width);
            tl.DrawText(x - (tl.Width * 1.2f), y);
        }

        if (!Host.DebugEnabled)
            return;

        Inventory.ShopList.Draw(OnShopList);

        var vendorItems = Game.Items.Where(x => x.Location == ItemLocation.Merchant).OrderBy(x => x.InventoryX);
        var text = string.Join("\n", vendorItems.Select(x => $"{x.NameLocalized} {x.InventoryX} {x.InventoryY} {x.ItemPower} {x.Quality}"));
        DrawDevText(() => text);
        DrawDevOutline(UserInterface.GetShopListControl());
        foreach (var rect in UserInterfaceExtensions.ShopListControls)
        {
            DrawDevOutline(rect);
        }
    }

    public override void Load()
    {
        OnShopList = InventoryFeatures.Create(this, nameof(OnShopList), "shop list",
                font: CreateDefaultFont(bold: true),
                errorFont: CreateDefaultErrorFont(bold: true))
            // .AspectHunterIcon().AspectHunterHighlight(false)
            // .TreasureHunterIcon().TreasureHunterHighlight(false)
            //.GreyOut(false)
            .TreasureHunterHighlight();
        OnShopList.Resources.Add(new BooleanFeatureResource
        {
            NameOf = nameof(ShopNotificationEnabled),
            DisplayText = () => Translation.Translate(this, "treasure hunter notification"),
            Getter = () => ShopNotificationEnabled,
            Setter = newValue => ShopNotificationEnabled = newValue
        });
        OnShopList.Register();
    }

    public void OnItemDetected(IItem item)
    {
        if (!OnShopList.Enabled || !ShopNotificationEnabled)
            return;
        if (item.Location != ItemLocation.Merchant)
            return;
        if (item.MatchingFilterNames.Length == 0)
            return;

        item.NotifyMatchedFilters(this);
    }
}