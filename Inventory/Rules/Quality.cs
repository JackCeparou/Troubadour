namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemIcon CreateQuality() => new()
    {
        Scale = 1.2f,
        OffsetX = 6f,
        Show = (_, features) => features.QualityLegendaryIconEnabled || features.QualityUniqueIconEnabled,
        Texture = (item, features) =>
        {
            if (Host.DebugEnabled)
                return Textures.LegendaryIcon;

            return item.Quality switch
            {
                ItemQuality.Legendary => features.QualityLegendaryIconEnabled ? Textures.LegendaryIcon : null,
                ItemQuality.Set or ItemQuality.Unique => features.QualityUniqueIconEnabled ? Textures.UniqueIcon : null,
                _ => null
            };
        }
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures QualityLegendaryIcon(bool enabled = true)
    {
        AddIcon(new BooleanFeatureResource
        {
            NameOf = nameof(QualityLegendaryIconEnabled),
            DisplayText = () => Translation.Translate(Plugin, "quality icon (legendary)"),
            Getter = () => QualityLegendaryIconEnabled,
            Setter = v => QualityLegendaryIconEnabled = v,
        });
        QualityLegendaryIconEnabled = enabled;
        return this;
    }

    public InventoryFeatures QualityUniqueIcon(bool enabled = true)
    {
        AddIcon(new BooleanFeatureResource
        {
            NameOf = nameof(QualityUniqueIconEnabled),
            DisplayText = () => Translation.Translate(Plugin, "quality icon (unique)"),
            Getter = () => QualityUniqueIconEnabled,
            Setter = v => QualityUniqueIconEnabled = v,
        });
        QualityUniqueIconEnabled = enabled;
        return this;
    }
}