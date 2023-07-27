namespace T4.Plugins.Troubadour;

public sealed class Cursed : TroubadourPlugin, IGameWorldPainter
{
    public Feature OnMap { get; }
    public Feature OnGround { get; }

    public float MapCircleSize { get; set; } = 32f;
    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(255, 0, 0, 0);
    public IFillStyle MapFillStyle { get; } = Render.GetFillStyle(128, 255, 0, 0);

    public float GroundIconSize { get; set; } = 100f;
    private ITexture GroundIcon { get; } = Render.GetTexture(SupportedTextureId.UIBuffDebuff_3845477221);
    private ITexture ShrineIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_66845578);

    public Cursed() : base(PluginCategory.Fight, "Displays cursed chests and shrines on the map and ground.")
    {
        Order = -1; // draw before default chests plugin
        OnMap = AddFeature(nameof(OnMap), "on map")
            .AddFloatResource(nameof(MapCircleSize), "radius", 0, 100, () => MapCircleSize, v => MapCircleSize = v)
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFillStyleResource(nameof(MapFillStyle), MapFillStyle, "fill style");
        OnGround = AddFeature(nameof(OnGround), "icon on ground")
            .AddFloatResource(nameof(GroundIconSize), "icon size", 0, 200, () => GroundIconSize, v => GroundIconSize = v);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                foreach (var actor in GetCursedActors())
                {
                    var offset = GroundIconSize / 2f;
                    GroundIcon.Draw(actor.Coordinate.ScreenX - offset, actor.Coordinate.ScreenY - offset, GroundIconSize, GroundIconSize);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
                foreach (var actor in GetCursedActors())
                {
                    if (!actor.Coordinate.IsOnMap)
                        continue;

                    var size = MapCircleSize * 1.25f;
                    var offset = size / 2f;
                    var mapX = actor.Coordinate.MapX;
                    var mapY = actor.Coordinate.MapY;
                    if (actor.ActorSno.SnoId == ActorSnoId.DE_CursedShrine_Debuff_Trigger)
                    {
                        ShrineIcon.Draw(mapX - offset, mapY - offset, size, size);
                    }

                    MapFillStyle.FillEllipse(mapX, mapY, offset, offset, false);
                    MapLineStyle.DrawEllipse(mapX, mapY, offset, offset, false);
                }

                break;
        }
    }

    private static HashSet<ActorSnoId> CursedActorIdsSet { get; } = new()
    {
        // ActorSnoId.CursedEventChest,
        // ActorSnoId.CursedEventChestRare,
        // ActorSnoId.DE_Spawner_Cursed_Event_Reward_Chest,
        ActorSnoId.DE_CursedChest_Standard_AffixCasterMonster,
        // ActorSnoId.DE_CursedShrine_AffixUser,
        ActorSnoId.DE_CursedShrine_Debuff_Trigger,
        ActorSnoId.Healing_Well_Cursed_MapIcon,
        // ActorSnoId.pvp_OpenWorld_Cursed_Chest,
        ActorSnoId.Shrine_DRLG_Cursed_MapIcon,

        // ActorSnoId.DE_Universal_Rare_Chest,
        ActorSnoId.DGN_Standard_NoBoss_ChestBlocker,
        ActorSnoId.DGN_Standard_NoBoss_ChestGuardian,
        ActorSnoId.DGN_Standard_NoBoss_LootChest,
    };

    public static IEnumerable<ICommonActor> GetCursedActors()
    {
        var actors = Game.GenericActors
            .Where(x => CursedActorIdsSet.Contains(x.ActorSno.SnoId));
        foreach (var actor in actors)
        {
            yield return actor;
        }

        var gizmos = Game.GizmoActors
            .Where(x => (x.GizmoType == GizmoType.Chest && x.IsDisabledByScript) || CursedActorIdsSet.Contains(x.ActorSno.SnoId));
        foreach (var actor in gizmos)
        {
            yield return actor;
        }

        var monsters = Game.Monsters
            .Where(x => CursedActorIdsSet.Contains(x.ActorSno.SnoId));
        foreach (var monster in monsters)
        {
            yield return monster;
        }
    }
}