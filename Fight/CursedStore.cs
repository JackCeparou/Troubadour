namespace T4.Plugins.Troubadour;

public static class CursedStore
{
    public static List<ActorSnoId> SnoIds { get; } = new()
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

    private static HashSet<ActorSnoId> CursedActorIdsSet { get; } = new(SnoIds);

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