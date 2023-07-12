namespace T4.Plugins.Troubadour;

public static partial class DebugService
{
    public static IFont DebugFont { get; } = Render.GetFont(240, 255, 255, 255, "consolas");
    public static IFillStyle DebugFillStyle { get; } = Render.GetFillStyle(128, 0, 0, 0);
    public static ILineStyle DebugLineStyle { get; } = Render.GetLineStyle(255, 255, 255, 255);
    public static ILineStyle DebugActorLineStyle { get; } = Render.GetLineStyle(255, 255, 255, 255, DashStyle.Dash);

    public static void DrawDebugText(Func<string> text, float? x = null, float? y = null)
    {
        if (!Host.DebugEnabled || text == null)
            return;

        DrawDevText(text, x, y);
    }

    public static void DrawDevText(Func<string> text, float? x = null, float? y = null)
    {
        x ??= Game.WindowWidth / 2f;
        y ??= Game.WindowHeight / 2f;
        var tl = DebugFont.GetTextLayout(text.Invoke());
        tl.DrawText(x.Value, y.Value);
    }

    public static void DrawDebugOutline(IScreenRectangle rect)
    {
        if (!Host.DebugEnabled || rect == null)
            return;

        DrawDevOutline(rect);
    }

    public static void DrawDebugOutline(float x, float y, float width, float height)
    {
        if (!Host.DebugEnabled)
            return;

        DrawDevOutline(x, y, width, height);
    }

    public static void DrawDevOutline(IScreenRectangle rect)
    {
        DebugLineStyle.DrawRectangle(rect.Left, rect.Top, rect.Width, rect.Height);
    }

    public static void DrawDevOutline(float x, float y, float width, float height)
    {
        DebugLineStyle.DrawRectangle(x, y, width, height);
    }

    public static void DrawDebugFrame(IEnumerable<string> lines, float x, float y, bool leftOf = false)
    {
        var tl = DebugFont.GetTextLayout(string.Join(Environment.NewLine, lines));
        if (leftOf)
        {
            x -= tl.Width;
        }

        DebugFillStyle.FillRectangle(x, y, tl.Width, tl.Height);
        tl.DrawText(x, y);
    }

    public static void DrawDebugActors(bool enabled, IEnumerable<ICommonActor> actors)
    {
        if (!enabled)
            return;

        foreach (var actor in actors)
        {
            DebugActorLineStyle.DrawWorldEllipse(1f, -1, actor.Coordinate, false, strokeWidthCorrection: 2f);
            Render.WorldToScreenCoordinate(actor.Coordinate, out var x, out var y);
            if (actor.ActorSno.SnoId == ActorSnoId.Generic_Proxy)
            {
                var tl1 = DebugFont.GetTextLayout($"GenericProxy {actor.AcdId} {actor.AnnId}");
                tl1.DrawText(x - (tl1.Width / 2f), y - (tl1.Height / 2f));
                continue;
            }
            var tl = DebugFont.GetTextLayout(actor.ActorSno.SnoId.ToString());
            tl.DrawText(x - (tl.Width / 2f), y - (tl.Height / 2f));
        }
    }

    public static void Try(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception e)
        {
            Log.WriteLine(Verbosity.Error, () => e.Message, 60);
        }
    }
}