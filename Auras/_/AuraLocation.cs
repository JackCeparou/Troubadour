namespace T4.Plugins.Troubadour;

public abstract class AuraCollection
{
    public abstract AuraLocation Location { get; }
}

public enum AuraLocation
{
    None = 0,
    Free = 1,
    // Screen
    ScreenCenter = 10,
    ScreenTopLeft = 11,
    ScreenTopCenter = 12,
    ScreenTopRight = 13,
    ScreenLeftCenter = 14,
    ScreenRightCenter = 15,
    ScreenBottomLeft = 16,
    ScreenBottomCenter = 17,
    ScreenBottomRight = 18,
    // Character
    BelowCharacter = 100,
    LeftOfCharacter = 101,
    RightOfCharacter = 102,
    TopOfCharacter = 103,
    // Chat
    ChatTopCenter = 200,
    ChatHorizontalTop = 201,
    ChatHorizontalCenter = 202,
    ChatHorizontalBottom = 203,
    ChatBottomCenter = 204,
    // Minimap
    MinimapTopLeft = 300,
    MinimapCenterLeft = 301,
    MinimapBottomLeft = 302,
    BelowMinimap = 303,
}