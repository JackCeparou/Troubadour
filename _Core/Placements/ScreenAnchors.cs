using System.Drawing;

namespace T4.Plugins.Troubadour;

public static class ScreenAnchors
{
    public static IComponentPlacement TopLeft { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(0, 0, 0 , 0) 
    };
    public static IComponentPlacement TopCenter { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(Game.WindowWidth / 2f, 0, 0 , 0) 
    };
    public static IComponentPlacement TopRight { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(Game.WindowWidth, 0, 0 , 0) 
    };
    public static IComponentPlacement LeftCenter { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(0, Game.WindowHeight / 2f, 0 , 0) 
    };
    public static IComponentPlacement Center { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(Game.WindowWidth / 2f, Game.WindowHeight / 2f, 0 , 0) 
    };
    public static IComponentPlacement PlayerFeet { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(Game.MyPlayerActor?.Coordinate?.ScreenX ?? Game.WindowWidth / 2f, Game.MyPlayerActor?.Coordinate?.ScreenY ?? Game.WindowHeight / 2f , 0 , 0) 
    };
    public static IComponentPlacement RightCenter { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(Game.WindowWidth, Game.WindowHeight / 2f, 0 , 0) 
    };
    public static IComponentPlacement BottomLeft { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(0, Game.WindowHeight, 0 , 0) 
    };
    public static IComponentPlacement BottomCenter { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(Game.WindowWidth / 2f, Game.WindowHeight, 0 , 0) 
    };
    public static IComponentPlacement BottomRight { get; } = new DynamicComponentPlacement
    {
        Calculator = () => new RectangleF(Game.WindowWidth, Game.WindowHeight, 0 , 0) 
    };

    public static void Update()
    {
        TopCenter.Update();
        PlayerFeet.Update();
        TopRight.Update();
        LeftCenter.Update();
        Center.Update();
        RightCenter.Update();
        BottomLeft.Update();
        BottomCenter.Update();
        BottomRight.Update();
    }
}