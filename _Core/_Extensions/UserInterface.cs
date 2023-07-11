namespace T4.Plugins.Troubadour;

public static class UserInterfaceExtensions
{
    const string StashPath = "stash_dialog_mainPage.StashContainer_Default.stash_container";

    const string BackpackPath = "inventory_dialog_mainPage.InventoryContainer.inventory_button_backpack";
    const string ConsumablesPath = "inventory_dialog_mainPage.InventoryContainer.inventory_button_consumables";
    const string QuestItemsPath = "inventory_dialog_mainPage.InventoryContainer.inventory_button_questItems";

    const string AspectsPath = "inventory_dialog_mainPage.InventoryContainer.inventory_button_materials";

    // const string HeroNamePath = "inventory_dialog_mainPage.InventoryContainer.Hero_Name";
    const string HeroNamePath = "inventory_dialog_mainPage.InventoryContainer.HeroInfo";
    const string ShopListPath = "shop_dialog_mainPage.ShopContainer_Default.Shop_list";

    public static void RegisterInventoryControls(this IUserInterfaceService userInterface)
    {
        userInterface.RegisterControl(StashPath);
        userInterface.RegisterControl(BackpackPath);
        userInterface.RegisterControl(ConsumablesPath);
        userInterface.RegisterControl(QuestItemsPath);
        userInterface.RegisterControl(AspectsPath);
        userInterface.RegisterControl(HeroNamePath);
        userInterface.RegisterControl(ShopListPath);
    }

    public static IUIControl GetInventoryStashControl(this IUserInterfaceService userInterface)
        => userInterface.GetRegisteredControlByPath(StashPath);

    public static IUIControl GetInventoryBackpackControl(this IUserInterfaceService userInterface)
        => userInterface.GetRegisteredControlByPath(BackpackPath);

    public static IUIControl GetInventoryConsumablesControl(this IUserInterfaceService userInterface)
        => userInterface.GetRegisteredControlByPath(ConsumablesPath);

    public static IUIControl GetInventoryQuestItemsControl(this IUserInterfaceService userInterface)
        => userInterface.GetRegisteredControlByPath(QuestItemsPath);

    public static IUIControl GetInventoryAspectsControl(this IUserInterfaceService userInterface)
        => userInterface.GetRegisteredControlByPath(AspectsPath);

    public static IUIControl GetShopListControl(this IUserInterfaceService userInterface)
        => userInterface.GetRegisteredControlByPath(ShopListPath);

    public static IUIControl GetInventoryHeroNameControl(this IUserInterfaceService userInterface)
        => userInterface.GetRegisteredControlByPath(HeroNamePath);

    // temporary solution while we get the controls from hud
    private static float _shopListWidth;
    private static float _shopListHeight;
    public static readonly ShopListSlot[] ShopListControls = {
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
        new(),
    };

    public static IScreenRectangle GetShopListSlotControl(this IUserInterfaceService userInterface, int inventoryX, int inventoryY)
    {
        if (inventoryY < 0 || inventoryY >= ShopListControls.Length)
            return null;

        var container = userInterface.GetShopListControl();
        if (container is null)
            return null;
        if (Math.Abs(_shopListWidth - container.Width) > 0.001f || Math.Abs(_shopListHeight - container.Height) > 0.001f)
        {
            _shopListWidth = container.Width;
            _shopListHeight = container.Height;
            var left = container.Left + (container.Width * 0.025f);
            var width = container.Width * 0.125f;
            var spacer = container.Height * 0.0175f;
            var height = (container.Height / ShopListControls.Length) - spacer; // - container.Height * 0.0;
            var top = container.Top + (container.Height * 0.01f); // + spacer;
            foreach (var slot in ShopListControls)
            {
                slot.Left = left;
                slot.Top = top;
                slot.Width = width;
                slot.Height = height;
                top += height + spacer;
            }
        }

        return ShopListControls[inventoryX];
    }
}

public class ShopListSlot : IScreenRectangle
{
    public float Left { get; set; }
    public float Top { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public bool CoordinateInside(float x, float y)
    {
        return x >= Left && x <= Left + Width &&
               y >= Top && y <= Top + Height;
    }
}