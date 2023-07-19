using static T4.Plugins.Troubadour.DungeonQuestsStore;

namespace T4.Plugins.Troubadour;

public sealed class DungeonQuests : JackPlugin, IGameWorldPainter
{
    public Feature Developer { get; private set; }
    public CarryableItemsFeature CarryableItems { get; private set; }

    public static IFont Font { get; } = Render.GetFont(240, 255, 255, 255, "consolas");
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 255, 255, 0, DashStyle.Dash);

    public bool OnMonsters { get; set; }
    public bool OnGizmos { get; set; }
    public bool OnGenerics { get; set; }

    public DungeonQuests()
    {
        Group = PluginCategory.Dungeon;
        Description = "displays dungeon quest objectives on map and ground.";
    }

    public override void Load()
    {
        CarryableItems = CarryableItemsFeature.Create(this, nameof(CarryableItems));

        Developer = new Feature
        {
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
        if (Game.MyPlayer.LevelAreaSno.IsTown)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground:
                CarryableItems.PaintGround();

                DrawGroundActors(true, Game.Monsters.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));
                DrawGroundActors(true, Game.GizmoActors.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));
                DrawGroundActors(true, Game.GenericActors.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));

                if (!Developer.Enabled)
                    return;
                DrawDevActors(x => DebugActorSnoIdSet.Contains(x.ActorSno.SnoId), OnMonsters, OnGizmos, OnGenerics);
                break;
            case GameWorldLayer.Map:
                CarryableItems.PaintMap();
                break;
        }
    }

    public static void DrawGroundActors(bool enabled, IEnumerable<ICommonActor> actors)
    {
        if (!enabled)
            return;

        foreach (var actor in actors)
        {
            LineStyle.DrawWorldEllipse(0.8f, -1, actor.Coordinate, false, strokeWidthCorrection: 2f);
            Render.WorldToScreenCoordinate(actor.Coordinate, out var x, out var y);
            var tl = Font.GetTextLayout(actor.ActorSno.NameLocalized);
            tl.DrawText(x - (tl.Width / 2f), y - (tl.Height / 2f));
        }
    }
}