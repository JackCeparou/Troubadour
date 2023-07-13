namespace T4.Plugins.Troubadour;

public static class SilentChestsStore
{
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);

    public static List<ActorSnoId> SilentChestSnoIds { get; } = new()
    {
        ActorSnoId.Generic_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Goatman_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.HaweHU_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.HaweHU_Smuggler_Rare_Chest_Locked_GamblingCurrency,
        ActorSnoId.HaweHU_Witch_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Hell_Prop_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.MageHalls_Prop_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.RedChurch_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Scos_Forest_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.ScosglenHU_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.ScosglenHU_Druid_Chest_Rare_Bloody_Locked_GamblingCurrency,
        ActorSnoId.ScosglenHU_Druid_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.SoDun_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.SoDun_Chest_Rare_Locked_GamblingCurrency_BloodRain,
        ActorSnoId.Spider_Chest_Rare_Locked_GamblingCurrency,
    };

    public static HashSet<ActorSnoId> SilentChestSnoIdsSet { get; } = new(SilentChestSnoIds);
}