namespace T4.Plugins.Troubadour;

public sealed class ElixirsFeature : WorldFeature<IItem>
{
    private ElixirsFeature()
    {
        LineStyle = ElixirsStore.LineStyle;
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
    }

    public override IEnumerable<IItem> GetWorldObjects()
    {
        return Game.Items.Where(item =>
        {
            if (item.Location != ItemLocation.None)
                return false;
            if (!item.IsElixirItem())
                return false;
            if (!ElixirsStore.ElixirSnoIdEnabled.TryGetValue(item.ItemSno.SnoId, out var enabled))
                return false;

            return ElixirsStore.InvertSelection ? !enabled : enabled;
        });
    }

    public static ElixirsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new ElixirsFeature
        {
            Plugin = plugin, NameOf = nameOf, DisplayName = () => Translation.Translate(plugin, "elixirs"), Resources = new List<AbstractFeatureResource>()
        };

        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();

        if (ElixirsStore.ElixirSnoIdEnabled.Count == 0)
        {
            foreach (var snoId in ElixirsStore.ElixirItemSnoIds)
            {
                ElixirsStore.ElixirSnoIdEnabled[snoId] = false;
                feature.Resources.Add(new BooleanFeatureResource
                {
                    DisplayText = () => GameData.GetItemSno(snoId)?.NameLocalized ?? snoId.ToString(),
                    Getter = () => ElixirsStore.ElixirSnoIdEnabled.TryGetValue(snoId, out var enabled) && enabled,
                    Setter = newValue => ElixirsStore.ElixirSnoIdEnabled[snoId] = newValue,
                    NameOf = snoId.ToString()
                });
            }
        }

        return feature.Register();
    }
}