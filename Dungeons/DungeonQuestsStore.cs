namespace T4.Plugins.Troubadour;

public static class DungeonQuestsStore
{
    public static IEnumerable<ActorSnoId> ActorSnoIds { get; } = new List<ActorSnoId>
    {
        ActorSnoId.Symbol_Quest_Proxy, // probably not needed/wanted 
        ActorSnoId.DGN_Standard_Sitting_Skeleton_Switch,
        // add more here (don't forget the comma) 
    };

    public static HashSet<ActorSnoId> ActorSnoIdSet { get; } = new(ActorSnoIds);
}