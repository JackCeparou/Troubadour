namespace T4.Plugins.Troubadour;

public static partial class Elixirs
{
    public static bool InvertSelection { get; set; }
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);

    public static Dictionary<ItemSnoId, bool> ElixirSnoIdEnabled { get; } = new();

    public static bool IsElixirHunted(this IItem item)
        => ElixirSnoIdEnabled.TryGetValue(item.ItemSno.SnoId, out var enabled)
           && InvertSelection
            ? !enabled
            : enabled;
}

public sealed class ElixirsFeature : WorldFeature<IItem>
{
    private ElixirsFeature()
    {
        LineStyle = Elixirs.LineStyle;
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
    }

    public override IEnumerable<IItem> GetWorldActors()
    {
        return Game.Items.Where(item =>
        {
            if (item.Location != ItemLocation.None)
                return false;
            if (!item.IsElixirItem())
                return false;
            if (!Elixirs.ElixirSnoIdEnabled.TryGetValue(item.ItemSno.SnoId, out var enabled))
                return false;

            return Elixirs.InvertSelection ? !enabled : enabled;
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

        if (Elixirs.ElixirSnoIdEnabled.Count == 0)
        {
            foreach (var snoId in Elixirs.ElixirItemSnoIds)
            {
                Elixirs.ElixirSnoIdEnabled[snoId] = false;
                feature.AddBooleanResource(snoId.ToString(), GameData.GetItemSno(snoId)?.NameLocalized ?? snoId.ToString(),
                    () => Elixirs.ElixirSnoIdEnabled[snoId], newValue => Elixirs.ElixirSnoIdEnabled[snoId] = newValue);
            }
        }

        plugin.Features.Add(feature);
        return feature;
    }
}