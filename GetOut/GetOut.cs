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

    public GetOut()
    {
        EnabledByDefault = false;
        TroubadourExperiment = true;
        Group = PluginCategory.Fight;
        Description = "displays dangerous affixes on ground";
    }

    public override void Load()
    {
        Config = new Feature
        {
            Plugin = this,
            NameOf = nameof(Config),
            DisplayName = () => Translation.Translate(this, "config"),
            Resources = new List<AbstractFeatureResource>
            {
                // new FontFeatureResource { NameOf = nameof(TrapFont), DisplayText = () => nameof(TrapFont), Font = TrapFont, },
                new LineStyleFeatureResource { NameOf = nameof(LineStyle), DisplayText = () => Translation.Translate(this, "line style"), LineStyle = LineStyle, },
            }
        }.Register();

        Developer = new Feature
        {
            // Enabled = false,
            Plugin = this,
            NameOf = nameof(Developer),
            DisplayName = () => Translation.Translate(this, "developer"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnMonsters), DisplayText = () => nameof(OnMonsters), Getter = () => OnMonsters, Setter = value => OnMonsters = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnGizmos), DisplayText = () => nameof(OnGizmos), Getter = () => OnGizmos, Setter = value => OnGizmos = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnGenerics), DisplayText = () => nameof(OnGenerics), Getter = () => OnGenerics, Setter = value => OnGenerics = value,
                },
            }
        }.Register();
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
            // Render.WorldToScreenCoordinate(actor.Coordinate, out var x, out var y);
            // var tl = TrapFont.GetTextLayout(actor.ActorSno.NameLocalized);
            // tl.DrawText(x - (tl.Width / 2f), y - (tl.Height / 2f));
        }
    }
}