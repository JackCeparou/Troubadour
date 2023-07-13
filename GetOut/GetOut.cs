using static T4.Plugins.Troubadour.GetOutStore;

namespace T4.Plugins.Troubadour;

public sealed class GetOut : BasePlugin, IGameWorldPainter
{
    public Feature Config { get; private set; }

    public static IFont TrapFont { get; } = Render.GetFont(240, 255, 255, 255, "consolas");
    public static ILineStyle TrapLineStyle { get; } = Render.GetLineStyle(255, 255, 0, 0, DashStyle.Dash);

    public bool OnMonsters { get; set; }
    public bool OnGyzmos { get; set; }
    public bool OnGenerics { get; set; }

    public GetOut()
    {
        EnabledByDefault = false;
    }

    //TODO: translations when ready to enable by default
    public override string GetDescription() => "Displays monster affixes on ground:\nEXPERIMENTAL / IN DEVELOPMENT";

    public override void Load()
    {
        Config = new Feature
        {
            // Enabled = false,
            Plugin = this,
            NameOf = nameof(Config),
            DisplayName = () => Translation.Translate(this, "debug"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnMonsters), DisplayText = () => nameof(OnMonsters), Getter = () => OnMonsters, Setter = value => OnMonsters = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(OnGyzmos), DisplayText = () => nameof(OnGyzmos), Getter = () => OnGyzmos, Setter = value => OnGyzmos = value,
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

        DrawDebugActors(OnMonsters, Game.Monsters.Where(x => DebugActorSnoIdSet.Contains(x.ActorSno.SnoId)));
        DrawDebugActors(OnGyzmos, Game.GizmoActors.Where(x => DebugActorSnoIdSet.Contains(x.ActorSno.SnoId)));
        DrawDebugActors(OnGenerics, Game.GenericActors.Where(x => DebugActorSnoIdSet.Contains(x.ActorSno.SnoId)));
    }

    public static void DrawActors(bool enabled, IEnumerable<ICommonActor> actors)
    {
        if (!enabled)
            return;

        foreach (var actor in actors)
        {
            TrapLineStyle.DrawWorldEllipse(1.3f, -1, actor.Coordinate, false, strokeWidthCorrection: 2f);
            Render.WorldToScreenCoordinate(actor.Coordinate, out var x, out var y);
            var tl = TrapFont.GetTextLayout(actor.ActorSno.NameLocalized);
            tl.DrawText(x - (tl.Width / 2f), y - (tl.Height / 2f));
        }
    }
}