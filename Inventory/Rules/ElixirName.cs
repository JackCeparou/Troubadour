namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemTextLine CreateElixirName() => new()
    {
        IsName = true,
        Show = (item, features) => features.ElixirNameEnabled && item.ItemSno.ItemUseType == ItemUseType.ElixirScrollWhatever,
        Text = (item, _) =>
        {
            var name = item.NameLocalized ?? string.Empty;
            switch (Translation.Language)
            {
                case Language.enUS:
                    if (name.Contains("Elixir of "))
                        name = name.Replace("Elixir of ", " ");
                    else if (name.EndsWith(" Elixir"))
                        name = name.Substring(0, name.Length - 7);
                    break;
            }

            return name;
        }
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures ElixirName(bool enabled = true)
    {
        AddTextLine(new BooleanFeatureResource
        {
            NameOf = nameof(ElixirNameEnabled),
            DisplayText = () => Translation.Translate(Plugin, "elixir name"),
            Getter = () => ElixirNameEnabled,
            Setter = v => ElixirNameEnabled = v,
        });
        ElixirNameEnabled = enabled;
        return this;
    }
}