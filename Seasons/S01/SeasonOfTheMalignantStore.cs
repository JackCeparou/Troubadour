namespace T4.Plugins.Troubadour;

public static class SeasonOfTheMalignantStore
{
    public static bool IsMalignantHeartAffix(this AffixSnoId affixSnoId) => MalignantHeartsSnoIdsSet.Contains(affixSnoId);
    
    public static readonly IEnumerable<AffixSnoId> MalignantHeartsSnoIds = new List<AffixSnoId>
    {
        AffixSnoId.s01_orb_defensive_barbarian_001,
        AffixSnoId.s01_orb_defensive_druid_001,
        AffixSnoId.s01_orb_defensive_generic_001,
        AffixSnoId.s01_orb_defensive_generic_002,
        AffixSnoId.s01_orb_defensive_generic_003,
        AffixSnoId.s01_orb_defensive_necromancer_001,
        AffixSnoId.s01_orb_defensive_rogue_001,
        AffixSnoId.s01_orb_defensive_sorcerer_001,
        AffixSnoId.s01_orb_offensive_barbarian_001,
        AffixSnoId.s01_orb_offensive_druid_001,
        AffixSnoId.s01_orb_offensive_generic_001,
        AffixSnoId.s01_orb_offensive_generic_002,
        AffixSnoId.s01_orb_offensive_generic_003,
        AffixSnoId.s01_orb_offensive_necromancer_001,
        AffixSnoId.s01_orb_offensive_rogue_001,
        AffixSnoId.s01_orb_offensive_sorcerer_001,
        AffixSnoId.s01_orb_rare_barbarian_001,
        AffixSnoId.s01_orb_rare_druid_001,
        AffixSnoId.s01_orb_rare_generic_001,
        AffixSnoId.s01_orb_rare_generic_002,
        AffixSnoId.s01_orb_rare_generic_003,
        AffixSnoId.s01_orb_rare_necromancer_001,
        AffixSnoId.s01_orb_rare_rogue_001,
        AffixSnoId.s01_orb_rare_sorcerer_001,
        AffixSnoId.s01_orb_utility_barbarian_001,
        AffixSnoId.s01_orb_utility_druid_001,
        AffixSnoId.s01_orb_utility_generic_001,
        AffixSnoId.s01_orb_utility_generic_002,
        AffixSnoId.s01_orb_utility_generic_003,
        AffixSnoId.s01_orb_utility_necromancer_001,
        AffixSnoId.s01_orb_utility_rogue_001,
        AffixSnoId.s01_orb_utility_sorcerer_001,
    };
    public static IEnumerable<AffixSnoId> MalignantHeartsSnoIdsSet = new HashSet<AffixSnoId>(MalignantHeartsSnoIds);
}