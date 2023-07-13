namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemTextLine CreateMonsterLevel() => new()
    {
        Show = (item, features) =>
        {
            if (!features.MonsterLevelEnabled)
                return false;
            return item.ItemSno.ItemUseType == ItemUseType.DungeonKey;
        },
        Text = (item, _) => $"M{item.SigilLevel + 54}",
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures MonsterLevel(bool enabled = true)
    {
        AddTextLine(new BooleanFeatureResource
        {
            NameOf = nameof(MonsterLevelEnabled),
            DisplayText = () => Translation.Translate(Plugin, "monster level"),
            Getter = () => MonsterLevelEnabled,
            Setter = v => MonsterLevelEnabled = v,
        });
        MonsterLevelEnabled = enabled;
        return this;
    }
}