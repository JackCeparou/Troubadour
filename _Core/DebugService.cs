namespace T4.Plugins.Troubadour;

public static partial class DebugService
{
    public static IFont DebugFont { get; } = Render.GetFont(240, 255, 255, 255, "consolas");
    public static IFillStyle DebugFillStyle { get; } = Render.GetFillStyle(128, 0, 0, 0);
    public static ILineStyle DebugLineStyle { get; } = Render.GetLineStyle(255, 255, 255, 255);
    public static ILineStyle DebugActorLineStyle { get; } = Render.GetLineStyle(255, 255, 0, 0, DashStyle.Dot);

    public static bool IsDebugSession => SessionType == SessionType.Debug || Host.DebugEnabled;
    public static bool IsDevSession => SessionType == SessionType.Development;

    public static SessionType SessionType { get; private set; }

    static DebugService()
    {
        SessionType = Environment.GetEnvironmentVariable("T4_TROUBADOUR_DEBUG") switch
        {
            "debug" => SessionType.Debug,
            "dev" => SessionType.Development,
            _ => SessionType.Release,
        };
        if (SessionType != SessionType.Release)
            return;

        // replace translation service to avoid spamming hud service with WIP translations
        Translation = new TranslationDevService();
    }

    public static void ChangeSessionType(SessionType sessionType)
    {
        SessionType = sessionType;
    }

    public static void DrawDebugText(Func<string> text, float? x = null, float? y = null)
    {
        if (!IsDebugSession || text == null)
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
        if (!IsDebugSession || rect == null)
            return;

        DrawDevOutline(rect);
    }

    public static void DrawDebugOutline(float x, float y, float width, float height)
    {
        if (!IsDebugSession)
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
            DebugActorLineStyle.DrawWorldEllipse(0.5f, -1, actor.Coordinate, false);
            Render.WorldToScreenCoordinate(actor.Coordinate, out var x, out var y);
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