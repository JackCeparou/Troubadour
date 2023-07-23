﻿namespace T4.Plugins.Troubadour;

public static class SeasonOfTheMalignantStore
{
    public static Dictionary<AffixSnoId, bool> MalignantHeartAffixSnoIdEnabled { get; } = new();

    public static IAffixSno GetMalignantHeartLegendaryAffix(this IItem item)
    {
        return item.CurrentAffixes.FirstOrDefault(x => x.MagicType is not MagicType.None && x.SnoId.IsMalignantHeartAffix());
    }

    public static bool IsMalignantHeartHunted(this IItem item)
    {
        var affix = item.CurrentAffixes.FirstOrDefault(x => x.SnoId.IsMalignantHeartAffix());
        return affix is not null && affix.SnoId.IsMalignantHeartHunted();
    }

    public static bool IsMalignantHeartHunted(this AffixSnoId affixSnoId)
    {
        return MalignantHeartAffixSnoIdEnabled.TryGetValue(affixSnoId, out var enabled) && enabled;
    }

    public static bool IsMalignantHeart(this IItem item)
    {
        return MalignantHeartsSnoIds.Contains(item.ItemSno.SnoId);
    }

    public static bool IsMalignantHeartAffix(this AffixSnoId affixSnoId) => MalignantHeartAffixesSnoIdsSet.Contains(affixSnoId);

    public static readonly IEnumerable<ItemSnoId> MalignantHeartsSnoIds = new List<ItemSnoId>
    {
        ItemSnoId.S01_MalignantOrb_Defensive,
        ItemSnoId.S01_MalignantOrb_Offensive,
        ItemSnoId.S01_MalignantOrb_Utility,
        ItemSnoId.S01_MalignantOrb_Universal,
        ItemSnoId.S01_MalignantOrb_Universal_QST_S01_Main_04_VarshanHeart,
    };

    public static readonly IEnumerable<ItemSnoId> MalignantHeartsSnoIdsSet = new HashSet<ItemSnoId>(MalignantHeartsSnoIds);

    public static readonly IEnumerable<AffixSnoId> MalignantHeartAffixesSnoIds = new List<AffixSnoId>
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

    public static readonly IEnumerable<AffixSnoId> MalignantHeartAffixesSnoIdsSet = new HashSet<AffixSnoId>(MalignantHeartAffixesSnoIds);
}