namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemOverlay CreateElixirHunterHighlight() => new()
    {
        Show = (_, features) => features.ElixirHunterHighlightEnabled,
        Style = (item, _) => item.IsElixirHunted() || Host.DebugEnabled ? ElixirsStore.LineStyle : null,
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures ElixirHunterHighlight(bool enabled = true)
    {
        AddOverlay(new BooleanFeatureResource
        {
            NameOf = nameof(ElixirHunterHighlightEnabled),
            DisplayText = () => Translation.Translate(Plugin, "elixir hunter highlight"),
            Getter = () => ElixirHunterHighlightEnabled,
            Setter = v => ElixirHunterHighlightEnabled = v,
        });
        ElixirHunterHighlightEnabled = enabled;
        return this;
    }
}