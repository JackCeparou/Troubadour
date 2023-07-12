namespace T4.Plugins.Troubadour;

public static class TranslationServiceExtensions
{
    public static Func<string> Translate(this IPlugin plugin, string text)
    {
        return () => Translation.Translate(plugin, text);
    }

    public static Func<string> TranslateFormat(this IPlugin plugin, string text, params object[] values)
    {
        return () => Translation.TranslateFormat(plugin, text, values);
    }

    public static string IconSize(this IPlugin plugin) => Translation.Translate(plugin, "icon size");
    public static string CircleSize(this IPlugin plugin) => Translation.Translate(plugin, "circle size");
    public static string Radius(this IPlugin plugin) => Translation.Translate(plugin, "radius");
    public static string Stroke(this IPlugin plugin) => Translation.Translate(plugin, "stroke");
    public static string MapRadius(this IPlugin plugin) => Translation.Translate(plugin, "map radius");
    public static string MapStroke(this IPlugin plugin) => Translation.Translate(plugin, "map stroke");
    public static string LineStroke(this IPlugin plugin) => Translation.Translate(plugin, "line stroke");
    public static string CircleStroke(this IPlugin plugin) => Translation.Translate(plugin, "circle stroke");
    public static string NormalFont(this IPlugin plugin) => Translation.Translate(plugin, "normal font");
    public static string ErrorFont(this IPlugin plugin) => Translation.Translate(plugin, "error font");
    public static string FillStyle(this IPlugin plugin) => Translation.Translate(plugin, "fill style");
    public static string GreyOut(this IPlugin plugin) => Translation.Translate(plugin, "grey out");
    public static string MatchedFilterCount(this IPlugin plugin) => Translation.Translate(plugin, "matched filter count");
    public static string LineStyle(this IPlugin plugin) => Translation.Translate(plugin, "line style");
    public static string MapLineStyle(this IPlugin plugin) => Translation.Translate(plugin, "map line style");
    public static string AspectName(this IPlugin plugin) => Translation.Translate(plugin, "aspect name");
    public static string AspectHunter(this IPlugin plugin) => Translation.Translate(plugin, "aspect hunter");
    public static string ElixirName(this IPlugin plugin) => Translation.Translate(plugin, "elixir name");
    public static string SigilName(this IPlugin plugin) => Translation.Translate(plugin, "sigil name");
    public static string BackgroundColor(this IPlugin plugin) => Translation.Translate(plugin, "background color");
    public static string TreasureHunterFilterNames(this IPlugin plugin) => Translation.Translate(plugin, "matched filter names");
}