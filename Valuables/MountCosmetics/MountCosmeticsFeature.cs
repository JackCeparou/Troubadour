namespace T4.Plugins.Troubadour;

public sealed class MountCosmeticsFeature : WorldFeature<IItem>
{
    private MountCosmeticsFeature()
    {
        LineStyle = MountCosmeticsStore.LineStyle;
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
    }

    public override IEnumerable<IItem> GetWorldObjects()
    {
        return Game.Items.Where(item =>
        {
            if (!OnGroundEnabled && !OnMapEnabled)
                return false;

            return item.Location == ItemLocation.None && item.IsMountCosmeticItem();
        });
    }

    public static MountCosmeticsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new MountCosmeticsFeature
        {
            Plugin = plugin, NameOf = nameOf, DisplayName = () => Translation.Translate(plugin, "mount cosmetics"), Resources = new List<AbstractFeatureResource>()
        };

        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();

        return feature.Register();
    }
}