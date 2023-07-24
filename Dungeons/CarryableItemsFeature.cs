namespace T4.Plugins.Troubadour;

public sealed class CarryableItemsFeature : WorldFeature<ICommonActor>
{
    private CarryableItemsFeature()
    {
        CarryableSnoIdsSet = new HashSet<ActorSnoId>(CarryableItemSnoIds);
        LineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
    }

    public override IEnumerable<ICommonActor> GetWorldActors()
    {
        foreach (var gizmo in Game.GizmoActors.Where(x => CarryableSnoIdsSet.Contains(x.ActorSno.SnoId)))
        {
            yield return gizmo;
        }
        foreach (var item in Game.Items.Where(x => x.Location == ItemLocation.None && CarryableSnoIdsSet.Contains(x.ActorSno.SnoId)))
        {
            yield return item;
        }
    }

    public static CarryableItemsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new CarryableItemsFeature
        {
            Plugin = plugin, NameOf = nameOf, DisplayName = () => Translation.Translate(plugin, "carryable items"), Resources = new List<AbstractFeatureResource>()
        };
        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();
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
        // keys
        ActorSnoId.Global_Flippy_Items_RustedIronKeys_01_Item,
        ActorSnoId.Global_Flippy_Items_RustedIronKeys_02_Item,
        ActorSnoId.Global_Flippy_Items_RustedIronKeys_03_Item,
    };

    public HashSet<ActorSnoId> CarryableSnoIdsSet { get; }
}