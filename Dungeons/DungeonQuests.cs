using static T4.Plugins.Troubadour.DungeonQuestsStore;

namespace T4.Plugins.Troubadour;

public sealed class DungeonQuests : BasePlugin, IGameWorldPainter
{
    public Feature Config { get; private set; }
    public CarryableItemsFeature CarryableItems { get; private set; }

    public static IFont Font { get; } = Render.GetFont(240, 255, 255, 255, "consolas");
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 255, 255, 0, DashStyle.Dash);

    public bool OnMonsters { get; set; }
    public bool OnGyzmos { get; set; }
    public bool OnGenerics { get; set; }

    public DungeonQuests()
    {
        EnabledByDefault = false;
    }

    //TODO: translations when ready to enable by default
    public override string GetDescription() => "Displays dungeon quest objectives on map and ground:\nEXPERIMENTAL / IN DEVELOPMENT";

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

        CarryableItems = CarryableItemsFeature.Create(this, nameof(CarryableItems));
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground:
                CarryableItems.PaintGround();

                DrawGroundActors(true, Game.Monsters.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));
                DrawGroundActors(true, Game.GizmoActors.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));
                DrawGroundActors(true, Game.GenericActors.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));

                DrawDebugActors(OnMonsters, Game.Monsters.Where(x => DebugActorSnoIdSet.Contains(x.ActorSno.SnoId)));
                DrawDebugActors(OnGyzmos, Game.GizmoActors.Where(x => DebugActorSnoIdSet.Contains(x.ActorSno.SnoId)));
                DrawDebugActors(OnGenerics, Game.GenericActors.Where(x => DebugActorSnoIdSet.Contains(x.ActorSno.SnoId)));
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
            LineStyle.DrawWorldEllipse(1.3f, -1, actor.Coordinate, false, strokeWidthCorrection: 2f);
            Render.WorldToScreenCoordinate(actor.Coordinate, out var x, out var y);
            var tl = Font.GetTextLayout(actor.ActorSno.NameLocalized);
            tl.DrawText(x - (tl.Width / 2f), y - (tl.Height / 2f));
        }
    }
}