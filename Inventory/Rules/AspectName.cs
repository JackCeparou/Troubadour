namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemTextLine CreateAspectName() => new()
    {
        IsName = true,
        Show = (item, features) => features.AspectNameEnabled && item.ItemSno.GemType == GemType.None,
        Text = (item, _) => item.GetFriendlyAffixName(),
        HasError = item => item.IsEquippedTemp() && DuplicateEquippedPowers.Contains(item.Affix1?.SnoId ?? 0),
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures AspectName(bool enabled = true)
    {
        AddTextLine(new BooleanFeatureResource
        {
            NameOf = nameof(AspectNameEnabled), DisplayText = Plugin.AspectName, Getter = () => AspectNameEnabled, Setter = v => AspectNameEnabled = v,
        });
        AspectNameEnabled = enabled;
        return this;
    }
}