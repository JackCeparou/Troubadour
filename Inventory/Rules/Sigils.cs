namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemTextLine CreateSigilName() => new()
    {
        Show = (item, features) => features.SigilNameEnabled && item.ItemSno.ItemUseType == ItemUseType.DungeonKey, Text = (item, _) => item.NameLocalized,
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures SigilName(bool enabled = true)
    {
        AddTextLine(new BooleanFeatureResource
        {
            NameOf = nameof(SigilNameEnabled),
            DisplayText = () => Translation.Translate(Plugin, "sigil name"),
            Getter = () => SigilNameEnabled,
            Setter = v => SigilNameEnabled = v,
        });
        SigilNameEnabled = enabled;
        return this;
    }
}