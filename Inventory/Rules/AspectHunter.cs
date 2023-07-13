namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemIcon CreateAspectHunterIcon() => new()
    {
        Scale = 0.8f,
        Show = (_, features) => features.AspectHunterIconEnabled,
        Texture = (item, _) => item.IsAspectHunted() ? Textures.AspectHunterIcon : null
    };

    private static ItemOverlay CreateAspectHunterHighlight() => new()
    {
        StrokeWidthCorrection = (_, _) => 1f,
        Show = (_, features) => features.AspectHunterHighlightEnabled,
        Style = (item, _) => item.IsAspectHunted() || Host.DebugEnabled ? AspectHunterStore.LineStyle : null,
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures AspectHunterIcon(bool enabled = true)
    {
        AddIcon(new BooleanFeatureResource
        {
            NameOf = nameof(AspectHunterIconEnabled),
            DisplayText = () => Translation.Translate(Plugin, "aspect hunter icon"),
            Getter = () => AspectHunterIconEnabled,
            Setter = v => AspectHunterIconEnabled = v,
        });
        AspectHunterIconEnabled = enabled;
        return this;
    }

    public InventoryFeatures AspectHunterHighlight(bool enabled = true)
    {
        AddOverlay(new BooleanFeatureResource
        {
            NameOf = nameof(AspectHunterHighlightEnabled),
            DisplayText = () => Translation.Translate(Plugin, "aspect hunter highlight"),
            Getter = () => AspectHunterHighlightEnabled,
            Setter = v => AspectHunterHighlightEnabled = v,
        });
        AspectHunterHighlightEnabled = enabled;
        return this;
    }
}