namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemTextLine CreateElixirName() => new()
    {
        IsName = true,
        Show = (item, features) => features.ElixirNameEnabled && item.ItemSno.ItemUseType == ItemUseType.ElixirScrollWhatever,
        Text = (item, _) => item.NameLocalized ?? item.NameEnglish ?? "42",
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures ElixirName(bool enabled = true)
    {
        AddTextLine(new BooleanFeatureResource
        {
            NameOf = nameof(ElixirNameEnabled), DisplayText = Plugin.ElixirName, Getter = () => ElixirNameEnabled, Setter = v => ElixirNameEnabled = v,
        });
        ElixirNameEnabled = enabled;
        return this;
    }
}