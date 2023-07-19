namespace T4.Plugins.Troubadour;

public static class Textures
{
    public static ITexture LegendaryIcon { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_2706340597);
    public static ITexture UniqueIcon { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_3759295089);
    public static ITexture NearBreakpointIcon { get; } = Render.GetTexture(SupportedTextureId.UISkills_4275309202);
    public static ITexture AspectHunterIcon { get; } = Render.GetTexture(SupportedTextureId.UITest_3915375899);

    public static ITexture UiStash004 { get; } = Render.GetTexture(SupportedTextureId.UIStash_360267698);

    public static ITexture ShrineIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_66845578);

    // public static ITexture TreasureHunterIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_003);
    // public static ITexture TreasureHunterGoldenIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_001);
    public static ITexture TreasureHunterGoldenOpenIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_339055441);

    public static ITexture GlobeForeground { get; } = Render.GetTexture(SupportedTextureId.UIControls_2475801208);
    // public static ITexture GlobeBackground { get; } = Render.GetTexture(SupportedTextureId.UISkills_001);
    public static ITexture CircleBackground { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_43109186);

    public static ITexture HelltideChest { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_43109186);

    public static ITexture BountyEventActive { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_503808014);
    // public static ITexture BountyEventInactive { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_094);
    // public static ITexture BountyEventDisabled { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_129);
    //
    // public static ITexture ChampionShield { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_041);
    // public static ITexture ChampionShield2 { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_042);
    // public static ITexture ChampionShield3 { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_043);

    // public static ITexture CrossedSwordsOnShield { get; } = Render.GetTexture(2089728434u);
    // public static ITexture TreasureBag { get; } = Render.GetTexture(2044228644u);
    // public static ITexture Amulet { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_050);
    // public static ITexture Boots { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_051);
    // public static ITexture Chest { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_052);
    // public static ITexture Gloves { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_053);
    // public static ITexture Helm { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_054);
    // public static ITexture Pants { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_055);
    // public static ITexture Offhand { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_056);
    // public static ITexture Ring { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_057);
    //
    // public static ITexture Sword_TBN { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_093);
    // public static ITexture Bow { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_095);
    // public static ITexture Offhand2 { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_097);
    //
    // public static ITexture Cinders_Whatever { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_026);
    // public static ITexture Cinders_Pvp { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_090);

    /*
    public static ITexture ExclamationMarkBright { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_108);

    public static ITexture Magic_Fire { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_059);
    public static ITexture Magic_Frost { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_060);
    public static ITexture Magic_Yellow_TBN { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_061);
    public static ITexture Magic_Lighting_TBC { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_062);
    public static ITexture Magic_Might_TBC { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_063);
    public static ITexture Magic_Poison { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_064);
    public static ITexture Magic_Fear { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_065);

    public static ITexture Aspect_Tab_All { get; } = Render.GetTexture(SupportedTextureId.UIAspectTabIcons_000);
    public static ITexture Aspect_Tab_Defensive { get; } = Render.GetTexture(SupportedTextureId.UIAspectTabIcons_001);
    public static ITexture Aspect_Tab_Mobility { get; } = Render.GetTexture(SupportedTextureId.UIAspectTabIcons_002);
    public static ITexture Aspect_Tab_Offensive { get; } = Render.GetTexture(SupportedTextureId.UIAspectTabIcons_003);
    public static ITexture Aspect_Tab_Resource { get; } = Render.GetTexture(SupportedTextureId.UIAspectTabIcons_004);
    public static ITexture Aspect_Tab_Utility { get; } = Render.GetTexture(SupportedTextureId.UIAspectTabIcons_005);
    public static ITexture Aspect_Defensive { get; } = Render.GetTexture(SupportedTextureId.UIAspectIcons_000);
    public static ITexture Aspect_Mobility { get; } = Render.GetTexture(SupportedTextureId.UIAspectIcons_001);
    public static ITexture Aspect_Offensive { get; } = Render.GetTexture(SupportedTextureId.UIAspectIcons_002);
    public static ITexture Aspect_Resource { get; } = Render.GetTexture(SupportedTextureId.UIAspectIcons_003);
    public static ITexture Aspect_Utility { get; } = Render.GetTexture(SupportedTextureId.UIAspectIcons_004);

    public static ITexture Aspect_Inventory_Offensive2_Ancestral { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_000);
    public static ITexture Aspect_Inventory_Offensive2_Normal { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_001);
    public static ITexture Aspect_Inventory_Offensive2_Sacred { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_002);

    public static ITexture Aspect_Inventory_Defensive_Ancestral { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_003);
    public static ITexture Aspect_Inventory_Defensive_Normal { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_004);
    public static ITexture Aspect_Inventory_Defensive_Sacred { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_005);

    public static ITexture Aspect_Inventory_Mobility_Ancestral { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_006);
    public static ITexture Aspect_Inventory_Mobility_Normal { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_007);
    public static ITexture Aspect_Inventory_Mobility_Sacred { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_008);

    public static ITexture Aspect_Inventory_Offensive_Ancestral { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_009);
    public static ITexture Aspect_Inventory_Offensive_Normal { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_010);
    public static ITexture Aspect_Inventory_Offensive_Sacred { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_011);

    public static ITexture Aspect_Inventory_Resource_Ancestral { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_012);
    public static ITexture Aspect_Inventory_Resource_Normal { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_013);
    public static ITexture Aspect_Inventory_Resource_Sacred { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_014);

    public static ITexture Aspect_Inventory_Utility_Ancestral { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_015);
    public static ITexture Aspect_Inventory_Utility_Normal { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_016);
    public static ITexture Aspect_Inventory_Utility_Sacred { get; } = Render.GetTexture(SupportedTextureId.InventoryAspectCrystalIcons_017);

    //*/
}