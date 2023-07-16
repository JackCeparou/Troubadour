namespace T4.Plugins.Troubadour;

public static class Textures
{
    public static ITexture LegendaryIcon { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_020);
    public static ITexture UniqueIcon { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_092);
    public static ITexture NearBreakpointIcon { get; } = Render.GetTexture(SupportedTextureId.UISkills_010);
    public static ITexture AspectHunterIcon { get; } = Render.GetTexture(SupportedTextureId.UITest_001);

    public static ITexture UiStash004 { get; } = Render.GetTexture(SupportedTextureId.UIStash_004);

    public static ITexture ShrineIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_007);

    // public static ITexture TreasureHunterIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_003);
    // public static ITexture TreasureHunterGoldenIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_001);
    public static ITexture TreasureHunterGoldenOpenIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_002);

    public static ITexture GlobeForeground { get; } = Render.GetTexture(SupportedTextureId.UIControls_103);
    // public static ITexture GlobeBackground { get; } = Render.GetTexture(SupportedTextureId.UISkills_001);

    public static ITexture HelltideChest { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_213);
}