// ReSharper disable All

namespace T4.Plugins.Troubadour;

public static partial class Affixes
{
    public static List<AffixSnoId> DuplicateEquippedLegendaryAffixes { get; } = new();

    public static List<IItem> RefreshDuplicateLegendaryAffixes(this List<IItem> items)
    {
        if (DuplicateEquippedLegendaryAffixes.Any())
            DuplicateEquippedLegendaryAffixes.Clear();

        // that's awful 🤠
        var duplicatePowers = items
            .SelectMany(x => x.EquippedLegendaryAffixes)
            .Where(x => x is not null && x.MagicType is not MagicType.None)
            .Select(x => x.SnoId)
            .GroupBy(x => x)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();

        if (duplicatePowers.Length > 0)
            DuplicateEquippedLegendaryAffixes.AddRange(duplicatePowers);

        return items;
    }

    public static bool TryGetUniqueAffixSnoId(this ItemSnoId itemSnoId, out AffixSnoId snoId)
    {
        snoId = itemSnoId switch
        {
            ItemSnoId._1HAxe_Unique_Druid_100 => AffixSnoId._1HAxe_Unique_Druid_100,
            ItemSnoId._1HAxe_Unique_Generic_001 => AffixSnoId._1HAxe_Unique_Generic_001,
            ItemSnoId._1HDagger_Unique_Rogue_001 => AffixSnoId._1HDagger_Unique_Rogue_001,
            ItemSnoId._1HDagger_Unique_Rogue_002 => AffixSnoId._1HDagger_Unique_Rogue_002,
            ItemSnoId._1HScythe_Unique_Necro_100 => AffixSnoId._1HScythe_Unique_Necro_100,
            ItemSnoId._1HSword_Unique_Barb_101 => AffixSnoId._1HSword_Unique_Barb_101,
            ItemSnoId._1HSword_Unique_Generic_001 => AffixSnoId._1HSword_Unique_Generic_001,
            ItemSnoId._1HWand_Unique_Sorc_100 => AffixSnoId._1HWand_Unique_Sorc_100,
            ItemSnoId._2HAxe_Unique_Barb_001 => AffixSnoId._2HAxe_Unique_Barb_001,
            ItemSnoId._2HBow_Unique_Rogue_001 => AffixSnoId._2HBow_Unique_Rogue_001,
            ItemSnoId._2HBow_Unique_Rogue_006 => AffixSnoId._2HBow_Unique_Rogue_006,
            ItemSnoId._2HMace_Unique_Barb_001 => AffixSnoId._2HMace_Unique_Barb_001,
            ItemSnoId._2HMace_Unique_Barb_100 => AffixSnoId._2HMace_Unique_Barb_100,
            ItemSnoId._2HScythe_Unique_Necro_100 => AffixSnoId._2HScythe_Unique_Necro_100,
            ItemSnoId._2HStaff_Unique_Druid_001 => AffixSnoId._2HStaff_Unique_Druid_001,
            ItemSnoId._2HStaff_Unique_Sorc_002 => AffixSnoId._2HStaff_Unique_Sorc_002,
            ItemSnoId._2HStaff_Unique_Sorc_100 => AffixSnoId._2HStaff_Unique_Sorc_100,
            ItemSnoId._2HSword_Unique_Barb_002 => AffixSnoId._2HSword_Unique_Barb_002,
            ItemSnoId._2HSword_Unique_Generic_001 => AffixSnoId._2HSword_Unique_Generic_001,
            ItemSnoId.Amulet_Unique_Barb_100 => AffixSnoId.Amulet_Unique_Barb_100,
            ItemSnoId.Amulet_Unique_Generic_100 => AffixSnoId.Amulet_Unique_Generic_100,
            ItemSnoId.Amulet_Unique_Necro_100 => AffixSnoId.Amulet_Unique_Necro_100,
            ItemSnoId.Amulet_Unique_Rogue_100 => AffixSnoId.Amulet_Unique_Rogue_100,
            ItemSnoId.Amulet_Unique_Sorc_100 => AffixSnoId.Amulet_Unique_Sorc_100,
            ItemSnoId.Boots_Unique_Barb_100 => AffixSnoId.Boots_Unique_Barb_100,
            ItemSnoId.Boots_Unique_Generic_001 => AffixSnoId.Boots_Unique_Generic_001,
            ItemSnoId.Boots_Unique_Necro_100 => AffixSnoId.Boots_Unique_Necro_100,
            ItemSnoId.Boots_Unique_Sorc_100 => AffixSnoId.Boots_Unique_Sorc_100,
            ItemSnoId.Chest_Unique_Barb_100 => AffixSnoId.Chest_Unique_Barb_100,
            ItemSnoId.Chest_Unique_Druid_002 => AffixSnoId.Chest_Unique_Druid_002,
            ItemSnoId.Chest_Unique_Druid_003 => AffixSnoId.Chest_Unique_Druid_003,
            ItemSnoId.Chest_Unique_Generic_100 => AffixSnoId.Chest_Unique_Generic_100,
            ItemSnoId.Chest_Unique_Necro_101 => AffixSnoId.Chest_Unique_Necro_101,
            ItemSnoId.Chest_Unique_Sorc_002 => AffixSnoId.Chest_Unique_Sorc_002,
            ItemSnoId.Gloves_Unique_Barb_001 => AffixSnoId.Gloves_Unique_Barb_001,
            ItemSnoId.Gloves_Unique_Generic_002 => AffixSnoId.Gloves_Unique_Generic_002,
            ItemSnoId.Gloves_Unique_Generic_003 => AffixSnoId.Gloves_Unique_Generic_003,
            ItemSnoId.Gloves_Unique_Necro_100 => AffixSnoId.Gloves_Unique_Necro_100,
            ItemSnoId.Gloves_Unique_Rogue_100 => AffixSnoId.Gloves_Unique_Rogue_100,
            ItemSnoId.Gloves_Unique_Sorc_002 => AffixSnoId.Gloves_Unique_Sorc_002,
            ItemSnoId.Helm_Unique_Druid_100 => AffixSnoId.Helm_Unique_Druid_100,
            ItemSnoId.Helm_Unique_Druid_101 => AffixSnoId.Helm_Unique_Druid_101,
            ItemSnoId.Helm_Unique_Generic_001 => AffixSnoId.Helm_Unique_Generic_001,
            ItemSnoId.Helm_Unique_Generic_002 => AffixSnoId.Helm_Unique_Generic_002,
            ItemSnoId.Helm_Unique_Necro_100 => AffixSnoId.Helm_Unique_Necro_100,
            ItemSnoId.Helm_Unique_Rogue_001 => AffixSnoId.Helm_Unique_Rogue_001,
            ItemSnoId.Pants_Unique_Druid_001 => AffixSnoId.Pants_Unique_Druid_001,
            ItemSnoId.Pants_Unique_Generic_100 => AffixSnoId.Pants_Unique_Generic_100,
            ItemSnoId.Pants_Unique_Rogue_004 => AffixSnoId.Pants_Unique_Rogue_004,
            ItemSnoId.Pants_Unique_Sorc_100 => AffixSnoId.Pants_Unique_Sorc_100,
            ItemSnoId.Ring_Unique_Druid_003 => AffixSnoId.Ring_Unique_Druid_003,
            ItemSnoId.Ring_Unique_Generic_100 => AffixSnoId.Ring_Unique_Generic_100,
            ItemSnoId.Ring_Unique_Generic_101 => AffixSnoId.Ring_Unique_Generic_101,
            ItemSnoId.Ring_Unique_Necro_100 => AffixSnoId.Ring_Unique_Necro_100,
            // added in S01
            ItemSnoId._1HMace_Unique_Druid_001 => AffixSnoId._1HMace_Unique_Druid_001,
            ItemSnoId._1HShield_Unique_Necro_100 => AffixSnoId._1HShield_Unique_Necro_100,
            ItemSnoId._1HSword_Unique_Barb_102 => AffixSnoId._1HSword_Unique_Barb_102,
            ItemSnoId._1HWand_Unique_Sorc_101 => AffixSnoId._1HWand_Unique_Sorc_101,
            ItemSnoId._2HBow_Unique_Rogue_100 => AffixSnoId._2HBow_Unique_Rogue_100,
            ItemSnoId._2HStaff_Unique_Generic_100 => AffixSnoId._2HStaff_Unique_Generic_100,
            _ => AffixSnoId.Axe__Bad__Data,
        };

        return itemSnoId != ItemSnoId.Axe__Bad__Data;
    }
}