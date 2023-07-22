namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemTextLine CreateAspectName() => new()
    {
        IsName = true,
        Show = (item, features) => features.AspectNameEnabled && item.ItemSno.GemType == GemType.None,
        Text = (item, _) => item.GetFriendlyAffixName(),
        HasError = item =>
            item.IsEquippedTemp() && item.MainAffixes
                .Where(x => x.MagicType is not MagicType.None)
                .Any(x => DuplicateEquippedPowers.Contains(x.SnoId)),
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures AspectName(bool enabled = true)
    {
        AddTextLine(new BooleanFeatureResource
        {
            NameOf = nameof(AspectNameEnabled),
            DisplayText = () => Translation.Translate(Plugin, "aspect name"),
            Getter = () => AspectNameEnabled,
            Setter = v => AspectNameEnabled = v,
        });
        AspectNameEnabled = enabled;
        return this;
    }
}