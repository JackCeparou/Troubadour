namespace T4.Plugins.Troubadour;

public static class PlacementExtensions
{
    public static bool HitTest(this IComponentPlacement placement, float x, float y) =>
        x >= placement.Left && y >= placement.Top && x < placement.Left + placement.Width && y < placement.Top + placement.Height;
}