namespace T4.Plugins.Troubadour;

public sealed class CarryableItemsFeature : WorldFeature<IItem>
{
    private CarryableItemsFeature()
    {
        CarryableItemSnoIdsSet = new HashSet<ActorSnoId>(CarryableItemSnoIds);
        LineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
    }

    public override IEnumerable<IItem> GetObjects()
    {
        return Game.Items.Where(x => x.Location == ItemLocation.None && CarryableItemSnoIdsSet.Contains(x.ActorSno.SnoId));
    }

    public static CarryableItemsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new CarryableItemsFeature
        {
            Plugin = plugin, NameOf = nameOf, DisplayName = plugin.Translate("carryable items"), Resources = new List<AbstractFeatureResource>()
        };
        feature.AddDefaultGroundResources(plugin);
        feature.AddDefaultMapResources(plugin);
        return feature.Register();
    }

    public List<ActorSnoId> CarryableItemSnoIds { get; } = new()
    {
        ActorSnoId.Carryable_AncientsStatue,
        ActorSnoId.Carryable_Bloodstone,
        ActorSnoId.Carryable_CrusaderSkull,
        ActorSnoId.Carryable_DefacedShrine,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_01,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_02,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_03,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Purified,
        ActorSnoId.Carryable_Mechanical,
        ActorSnoId.Carryable_RunicStandingStone,
        ActorSnoId.Carryable_SightlessEye,
        ActorSnoId.Carryable_StoneCarving,
        ActorSnoId.Carryable_Winch,
    };

    public HashSet<ActorSnoId> CarryableItemSnoIdsSet { get; }
}