namespace T4.Plugins.Troubadour;

public sealed class SilentChestsFeature : WorldFeature<ICommonActor>
{
    private SilentChestsFeature()
    {
        WorldCircleSize = 1f;
        LineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapIconSize = 30f;
        MapIconTexture = Render.GetTexture(SupportedTextureId.UIMinimapIcons_503639160, 196);
        SilentChestSnoIdsSet = _silentChestSnoIds.ToHashSet();
    }

    public override IEnumerable<ICommonActor> GetWorldActors()
    {
        return Game.GizmoActors.Where(x => SilentChestSnoIdsSet.Contains(x.ActorSno.SnoId));
    }

    public static SilentChestsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new SilentChestsFeature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = () => Translation.Translate(plugin, "silent chests"),
            Resources = new List<AbstractFeatureResource>()
        };

        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();

        return feature.Register();
    }

    private HashSet<ActorSnoId> SilentChestSnoIdsSet { get; }

    private readonly IEnumerable<ActorSnoId> _silentChestSnoIds = new[]
    {
        ActorSnoId.Bandit_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.Cannibals_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Cultist_Triune_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.Drown_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.DrySteppesHU_Chest_Rare_Buried_Locked_GamblingCurrency, ActorSnoId.DrySteppesHU_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.DrySteppesHU_Chest_Rare_Salt_Locked_GamblingCurrency, ActorSnoId.Fallen_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.FracturedPeaksHU_Pale_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.FracturedPeaksHU_Town_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Generic_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.Goatman_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.HaweHU_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.HaweHU_Smuggler_Rare_Chest_Locked_GamblingCurrency,
        ActorSnoId.HaweHU_Witch_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.Hell_Prop_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.MageHalls_Prop_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.RedChurch_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.Scos_Forest_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.ScosglenHU_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.ScosglenHU_Druid_Chest_Rare_Bloody_Locked_GamblingCurrency, ActorSnoId.ScosglenHU_Druid_Chest_Rare_Locked_GamblingCurrency,
        ActorSnoId.SoDun_Chest_Rare_Locked_GamblingCurrency, ActorSnoId.SoDun_Chest_Rare_Locked_GamblingCurrency_BloodRain,
        ActorSnoId.Spider_Chest_Rare_Locked_GamblingCurrency,
    };
}