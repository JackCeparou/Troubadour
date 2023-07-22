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

    public override IEnumerable<IItem> GetWorldObjects()
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

        if (Elixirs.ElixirSnoIdEnabled.Count != 0) 
            return feature.Register();

        foreach (var snoId in Elixirs.ElixirItemSnoIds)
        {
            Elixirs.ElixirSnoIdEnabled[snoId] = false;
            feature.Resources.Add(new BooleanFeatureResource
            {
                DisplayText = () => GameData.GetItemSno(snoId)?.NameLocalized ?? snoId.ToString(),
                Getter = () => Elixirs.ElixirSnoIdEnabled.TryGetValue(snoId, out var enabled) && enabled,
                Setter = newValue => Elixirs.ElixirSnoIdEnabled[snoId] = newValue,
                NameOf = snoId.ToString()
            });
        }

        return feature.Register();
    }
}