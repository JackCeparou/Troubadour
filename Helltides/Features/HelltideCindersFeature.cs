namespace T4.Plugins.Troubadour;

public sealed class HelltideCindersFeature : WorldFeature<ICommonActor>
{
    private HelltideCindersFeature()
    {
        _cinderCachesSnoIdsSet = _cinderCachesSnoIds.ToHashSet();
        LineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
    }

    public override IEnumerable<ICommonActor> GetWorldActors()
    {
        return Game.GizmoActors.Where(x => _cinderCachesSnoIdsSet.Contains(x.ActorSno.SnoId));
    }

    public static HelltideCindersFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new HelltideCindersFeature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = () => Translation.Translate(plugin, "cinders caches"),
            Resources = new List<AbstractFeatureResource>()
        };
        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();
        return feature.Register();
    }

    private readonly IEnumerable<ActorSnoId> _cinderCachesSnoIds = new List<ActorSnoId>()
    {
        ActorSnoId.HelltideBonus_BreakableContainer_Any_1x1_01_Arrangement,
        ActorSnoId.HelltideBonus_BreakableContainer_Any_1x1_02_Arrangement,
        ActorSnoId.HellTideBonus_BreakableContainer_Any_1x1_03_Arrangement,
        ActorSnoId.HelltideBonus_BreakableContainer_Any_1x1_04_Arrangement,
        ActorSnoId.HelltideBonus_CommonClicky_01_Dyn,
        ActorSnoId.HelltideBonus_CommonClicky_02_Dyn,
        ActorSnoId.HelltideBonus_CommonClicky_03_Dyn,
        ActorSnoId.HelltideBonus_ThematicClicky_01_Dyn,
        ActorSnoId.HelltideBonus_ThematicClicky_02_Dyn,
        ActorSnoId.HelltideBonus_ThematicClicky_03_Dyn,
        // plant and ore
        ActorSnoId.HarvestNode_Herb_Rare_FiendRose,
        ActorSnoId.USZ_HarvestNode_Ore_UberSubzone_001_Dyn,
        // ???
        // ActorSnoId.Helltide_Firestorm_Elite_Spawner,
        // ActorSnoId.helltide_necropolis_guardian_staff,
        // ActorSnoId.Helltide_Tornado_Crater_A,
        // ActorSnoId.helltideShard,
        // ActorSnoId.USZ_HarvestNode_Ore_UberSubzone_001_Pieces_Dyn,
    };

    private readonly HashSet<ActorSnoId> _cinderCachesSnoIdsSet;
}