namespace T4.Plugins.Troubadour;

public sealed class SilentChestsFeature : WorldFeature<IItem>
{
    private SilentChestsFeature()
    {
        LineStyle = SilentChestsStore.LineStyle;
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
    }

    public override IEnumerable<IItem> GetWorldObjects()
    {
        return Game.Items.Where(item =>
        {
            if (!OnGroundEnabled && !OnMapEnabled)
                return false;

            return item.Location == ItemLocation.None && SilentChestsStore.SilentChestSnoIdsSet.Contains(item.ActorSno.SnoId);
        });
    }

    public static SilentChestsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new SilentChestsFeature
        {
            Plugin = plugin, NameOf = nameOf, DisplayName = () => Translation.Translate(plugin, "silent chests"), Resources = new List<AbstractFeatureResource>()
        };

        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();

        return feature.Register();
    }
}