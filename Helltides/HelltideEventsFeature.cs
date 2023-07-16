namespace T4.Plugins.Troubadour;

public sealed class HelltideEventsFeature : WorldFeature<ICommonActor>
{
    private HelltideEventsFeature()
    {
        OnGroundEnabled = false;
        MapIconSize = 24f;
        MapIconTexture = Textures.BountyEvent;
    }

    public override IEnumerable<ICommonActor> GetWorldObjects()
    {
        yield break;
    }

    public static HelltideEventsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new HelltideEventsFeature
        {
            Plugin = plugin, NameOf = nameOf, DisplayName = () => Translation.Translate(plugin, "events"), Resources = new List<AbstractFeatureResource>()
        };

        return feature.Register();
    }

    public override void PaintMap()
    {
        if (!Enabled || !OnMapEnabled)
            return;
        var helltide = Game.HelltideEventMarkers.FirstOrDefault(x => x.EndsInMilliseconds > 0);
        if (helltide is null)
            return;

        foreach (var quest in GetZoneEvents(helltide.SubzoneSno.SnoId))
        {
            if (!Map.WorldToMapCoordinate(quest.WorldCoordinate, out var mapX, out var mapY))
                continue;

            MapLineStyle?.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
            MapIconTexture?.Draw(mapX - (MapIconSize / 2), mapY - (MapIconSize / 2), MapIconSize, MapIconSize);
        }
    }

    public IEnumerable<IQuestSno> GetZoneEvents(SubzoneSnoId snoId)
    {
        switch (snoId)
        {
            case SubzoneSnoId.Frac_Tundra_N:
            case SubzoneSnoId.Frac_Tundra_S:
                return GameData.AllQuestSno
                    .Where(x => x.BountyType is BountyType.Event && !EventBlacklist.Contains(x.SnoId))
                    .Where(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Frac_Tundra_N or SubzoneSnoId.Frac_Tundra_S);

            case SubzoneSnoId.Scos_Coast:
            case SubzoneSnoId.Scos_Deep_Forest:
            case SubzoneSnoId.Scos_ZoneEvent:
                return GameData.AllQuestSno
                    .Where(x => x.BountyType is BountyType.Event && !EventBlacklist.Contains(x.SnoId))
                    .Where(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Scos_Coast or SubzoneSnoId.Scos_Deep_Forest);

            case SubzoneSnoId.Step_South:
            case SubzoneSnoId.Step_Central:
            case SubzoneSnoId.Step_TempleOfRot:
            case SubzoneSnoId.Step_ZoneEvent:
                return GameData.AllQuestSno
                    .Where(x => x.BountyType is BountyType.Event && !EventBlacklist.Contains(x.SnoId))
                    .Where(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Step_South or SubzoneSnoId.Step_Central);

            case SubzoneSnoId.Kehj_Oasis:
            case SubzoneSnoId.Kehj_LowDesert:
            case SubzoneSnoId.Kehj_HighDesert:
            case SubzoneSnoId.Kehj_ZoneEvent:
                return GameData.AllQuestSno
                    .Where(x => x.BountyType is BountyType.Event && !EventBlacklist.Contains(x.SnoId))
                    .Where(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Kehj_Oasis or SubzoneSnoId.Kehj_LowDesert or SubzoneSnoId.Kehj_HighDesert);

            case SubzoneSnoId.Hawe_Verge:
            case SubzoneSnoId.Hawe_Wetland:
            case SubzoneSnoId.Hawe_ZoneEvent:
                return GameData.AllQuestSno
                    .Where(x => x.BountyType is BountyType.Event && !EventBlacklist.Contains(x.SnoId))
                    .Where(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Hawe_Verge or SubzoneSnoId.Hawe_Wetland);

            default:
                return Array.Empty<IQuestSno>();
        }
    }

    public readonly HashSet<QuestSnoId> EventBlacklist = new()
    {
        QuestSnoId.Bounty_LE_Tier1_Step_PvP_CompleteAny1, //963800u, // 0xEB4D8
        QuestSnoId.Bounty_LE_Tier2_Frac_TundraN_CompleteAny3, //1292295u, // 0x13B807
        QuestSnoId.Bounty_LE_Tier2_Frac_TundraS_CompleteAny3, //608337u, // 0x94851
        QuestSnoId.Bounty_LE_Tier2_Hawe_Crossway_CompleteAny3, //1241683u, // 0x12F253
        QuestSnoId.Bounty_LE_Tier2_Hawe_Delta_CompleteAny3, //1241805u, // 0x12F2CD
        QuestSnoId.Bounty_LE_Tier2_Hawe_Fens_CompleteAny3, //1241962u, // 0x12F36A
        QuestSnoId.Bounty_LE_Tier2_Hawe_Marsh_CompleteAny3, //1241974u, // 0x12F376
        QuestSnoId.Bounty_LE_Tier2_Hawe_Verge_CompleteAny3, //1241982u, // 0x12F37E
        QuestSnoId.Bounty_LE_Tier2_Hawe_Wetland_CompleteAny3, //1241991u, // 0x12F387
        QuestSnoId.Bounty_LE_Tier2_Kehj_LowDesert_CompleteAny3, //1076318u, // 0x106C5E
        QuestSnoId.Bounty_LE_Tier2_Kehj_Ridge_CompleteAny3, //1290329u, // 0x13B059
        QuestSnoId.Bounty_LE_Tier2_Scos_Coast_CompleteAny3, //884844u, // 0xD806C
        QuestSnoId.Bounty_LE_Tier2_Scos_Highlands_CompleteAny3, //1219060u, // 0x1299F4
        QuestSnoId.Bounty_LE_Tier2_Scos_Lowlands_CompleteAny3, //884842u, // 0xD806A
        QuestSnoId.Bounty_LE_Tier2_Scos_Moors_CompleteAny3, //1219035u, // 0x1299DB
        QuestSnoId.Bounty_LE_Tier2_Step_Central_CompleteAny3, //920784u, // 0xE0CD0
        QuestSnoId.Bounty_LE_Tier2_Step_South_CompleteAny3, //920704u, // 0xE0C80
    };
}