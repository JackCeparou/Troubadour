using static T4.Plugins.Troubadour.GetOutStore;

namespace T4.Plugins.Troubadour;

public sealed class GetOut : JackPlugin, IGameWorldPainter
{
    public Feature Config { get; private set; }
    public Feature Developer { get; private set; }

    public static IFont TrapFont { get; } = Render.GetFont(240, 255, 255, 255, "consolas");
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 255, 0, 0, DashStyle.Dash);

    public bool OnMonsters { get; set; }
    public bool OnGizmos { get; set; }
    public bool OnGenerics { get; set; }

    public GetOut() : base(PluginCategory.Fight, "displays dangerous affixes on ground.")
    {
        EnabledByDefault = false;
        Config = AddFeature(nameof(Config), "config")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style");
        Developer = AddFeature(nameof(Developer), "developer")
            .AddBooleanResource(nameof(OnMonsters), "`on monsters", () => OnMonsters, v => OnMonsters = v)
            .AddBooleanResource(nameof(OnGizmos), "`on gizmos", () => OnGizmos, v => OnGizmos = v)
            .AddBooleanResource(nameof(OnGenerics), "`on generics", () => OnGenerics, v => OnGenerics = v);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (layer != GameWorldLayer.Ground)
            return;

        DrawActors(true, Game.Monsters.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));
        DrawActors(true, Game.GizmoActors.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));
        DrawActors(true, Game.GenericActors.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));

        if (!Developer.Enabled)
            return;

        DrawDevActors(x => DevActorSnoIdSet.Contains(x.ActorSno.SnoId), OnMonsters, OnGizmos, OnGenerics);
    }

    public static void DrawActors(bool enabled, IEnumerable<ICommonActor> actors)
    {
        if (!enabled)
            return;

        foreach (var actor in actors)
        {
            var radius = actor.GetWorldRadius();
            LineStyle.DrawWorldEllipse(radius, -1, actor.Coordinate, false, strokeWidthCorrection: 2f);
        }
    }
}