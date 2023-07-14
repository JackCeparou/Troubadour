namespace T4.Plugins.Troubadour;

public static class TreasureHunterStore
{
    public static bool DropNotificationEnabled { get; set; }
    public static bool ShopNotificationEnabled { get; set; }
    public static bool OnGroundLineEnabled { get; set; }
    public static bool ShowFilterNamesOnGround { get; set; }
    public static IFont FilterNamesOnGroundFont { get; } = Render.GetFont(255, 178, 0, 255);
    public static IFillStyle FilterNamesOnGroundBackground { get; } = Render.GetFillStyle(255, 0, 0, 0);
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 178, 0, 255, DashStyle.Dash);
    public static IFillStyle GreyOutFillStyle { get; } = Render.GetFillStyle(180, 0, 0, 0);
    public static IFont MatchedFilterCounterFont { get; } = Render.GetFont(255, 255, 255, 0, shadowMode: FontShadowMode.Heavy);
    public static float WorldCircleSize { get; set; } = 0.4f;
    public static float WorldCircleStroke { get; set; } = 8f;
    public static ILineStyle MapLineStyle { get; } = Render.GetLineStyle(255, 178, 0, 255);
    public static float MapCircleSize { get; set; } = 8f;
    public static float MapCircleStroke { get; set; } = 4f;

    public static Func<IItem, bool> WorldItemPredicate { get; } = item =>
    {
        if (item.Location != ItemLocation.None)
            return false;

        return item.MatchingFilterNames is not null && item.MatchingFilterNames.Length > 0;
    };

    public static void NotifyMatchedFilters(this IItem item, IPlugin plugin)
    {
        var message = string.Join(", ", item.MatchingFilterNames ?? Array.Empty<string>());
        Log.WriteLine(Verbosity.Info, () => Translation.TranslateFormat(plugin, "matched filters: {0}", message), 10);
    }
}