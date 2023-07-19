namespace T4.Plugins.Troubadour;

public static class BountyEvents
{
    public static IEnumerable<IQuestSno> GetBountyEvents(this IGameDataService gameData)
        => gameData.AllQuestSno.Where(x => x.BountyType is BountyType.Event && !EventBlacklist.Contains(x.SnoId));
    public static IEnumerable<IQuestSno> GetBountyEvents(this IGameDataService gameData, Func<IQuestSno, bool> predicate)
        => gameData.AllQuestSno.Where(x => x.BountyType is BountyType.Event && !EventBlacklist.Contains(x.SnoId)).Where(predicate);

    public static readonly HashSet<QuestSnoId> EventBlacklist = new()
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