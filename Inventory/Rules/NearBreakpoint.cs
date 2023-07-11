namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemIcon CreateNearBreakpoint() => new()
    {
        Scale = 0.8f,
        Show = (_, features) => features.NearBreakpointIconEnabled,
        Texture = (item, _) =>
        {
            if (Host.DebugEnabled)
                return Textures.NearBreakpointIcon;

            if (item.Quality is ItemQuality.None or ItemQuality.White or ItemQuality.Common or ItemQuality.Magic)
            {
                return null;
            }

            var isNearBreakPoint = item.IsNearBreakpoint();
            return isNearBreakPoint ? Textures.NearBreakpointIcon : null;
        }
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures NearBreakpointIcon(bool enabled = true)
    {
        AddIcon(new BooleanFeatureResource
        {
            NameOf = nameof(NearBreakpointIconEnabled),
            DisplayText = Plugin.Translate("near breakpoint icon"),
            Getter = () => NearBreakpointIconEnabled,
            Setter = v => NearBreakpointIconEnabled = v,
        });
        NearBreakpointIconEnabled = enabled;
        return this;
    }
}