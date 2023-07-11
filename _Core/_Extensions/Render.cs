namespace T4.Plugins.Troubadour;

public static class RenderExtensions
{
    public static IFont CreateDefaultFont(bool bold = false, bool italic = false, float size = 7, bool wordWrap = true)
        => Render.GetFont(240, 255, 255, 255, bold: bold, italic: italic, size: size, wordWrap: wordWrap);
    public static IFont CreateDefaultErrorFont(bool bold = false, bool italic = false, float size = 7, bool wordWrap = true)
        => Render.GetFont(240, 255, 0, 0, bold: bold, italic: italic, size: size, wordWrap: wordWrap);
}