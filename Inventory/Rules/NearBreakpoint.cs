namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemIcon CreateNearBreakpoint() => new()
    {
        Scale = 0.8f,
        Show = (item, features) => features.NearBreakpointIconEnabled && item.IsNearBreakpoint(),
        Texture = (_, _) => Textures.NearBreakpointIcon,
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures NearBreakpointIcon(bool enabled = true)
    {
        AddIcon(new BooleanFeatureResource
        {
            NameOf = nameof(NearBreakpointIconEnabled),
            DisplayText = () => Translation.Translate(Plugin, "near breakpoint icon"),
            Getter = () => NearBreakpointIconEnabled,
            Setter = v => NearBreakpointIconEnabled = v,
        });
        NearBreakpointIconEnabled = enabled;
        return this;
    }
}