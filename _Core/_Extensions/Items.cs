namespace T4.Plugins.Troubadour;

public static class ItemExtensions
{
    private static int[] ItemBreakpoints = new[] { 150, 340, 460, 625, 725 };
    public static bool IsEquippedTemp(this IItem item) => EquippedItemLocationsSet.Contains(item.Location);
    public static IAffixSno GetAffix(this IItem item) => item.LegendaryAffixEquipped ?? item.Affix1 ?? item.Affix2; // ?? item.EnchantedAffix;

    public static IEnumerable<ItemLocation> EquippedItemLocations { get; } = new List<ItemLocation>
    {
        ItemLocation.PlayerHead,
        ItemLocation.PlayerTorso,
        ItemLocation.PlayerOffHand,
        ItemLocation.PlayerMainHand,
        ItemLocation.Player2HBlunt,
        ItemLocation.Player2HSlash,
        ItemLocation.Player2HRanged,
        ItemLocation.Player1HMain,
        ItemLocation.Player1HOff,
        ItemLocation.PlayerHands,
        ItemLocation.PlayerLegs,
        ItemLocation.PlayerFeet,
        ItemLocation.PlayerRightFinger,
        ItemLocation.PlayerLeftFinger,
        ItemLocation.PlayerNeck,
    };

    public static readonly HashSet<ItemLocation> EquippedItemLocationsSet = new(EquippedItemLocations);

    public static List<IItem> GetEquippedItems(this IGameService game)
        => game.Items.Where(x => EquippedItemLocationsSet.Contains(x.Location)).ToList();

    public static int GetMaxUpgradeCount(this IItem item) => item.ItemPower switch
    {
        // TODO: this is wrong, but I don't know the correct values
        >= 600 => 5,
        >= 400 => 4, // tested wrong values: 300
        _ => 3
    };

    public static bool IsNearBreakpoint(this IItem item)
    {
        var upgradesLeft = item.GetMaxUpgradeCount() - item.UpgradeCount;
        if (upgradesLeft <= 0)
            return false;
        if (item.ItemPowerTotal >= 725)
            return false;

        var maxItemPower = item.ItemPower + (item.GetMaxUpgradeCount() * 5);
        return ItemBreakpoints.Any(i => item.ItemPowerTotal < i && maxItemPower > i);
    }

    public static int GetNextBreakpoint(this IItem item)
    {
        return ItemBreakpoints.First(i => i > item.ItemPowerTotal);
    }

    public static string GetFriendlyAffixName(this IItem item)
    {
        switch (item.Quality)
        {
            case ItemQuality.Legendary:
                return item.GetAffix()?.GetFriendlyName();
            case ItemQuality.Set:
            case ItemQuality.Unique:
                var name = item.ItemSno.NameLocalized ?? string.Empty;
                if (name.StartsWith("[fp]") || name.StartsWith("[mp]"))
                    return name.Substring(4);

                return name;

            default:
                return string.Empty;
        }
    }

    public static string GetFriendlyName(this IAffixSno affix)
    {
        var combinedName = affix.CombineWithLocalized(null);
        if (Host.DebugEnabled)
        {
            return $"{affix.SnoId}: {combinedName}";
        }

        return combinedName;
    }

    public static string GetFriendlyName(this AffixSnoId affixSnoId)
        => GameData.GetAffixSno(affixSnoId)?.GetFriendlyName() ?? string.Empty;

    public static void SetHint(this IItem item, IPlugin plugin)
    {
        if (Host.DebugEnabled)
        {
            var text = $"""


Id: {item.ItemSno.SnoId}
Name: {item.ItemSno.NameEnglish}
Affix1: {item.Affix1?.SnoId}
Affix2: {item.Affix2?.SnoId}
Affix3: {item.LegendaryAffixEquipped?.SnoId}
Affix4: {item.EnchantedAffix?.SnoId}
AffixFallback: {item.GetAffix()?.SnoId}
FriendlyName: {item.GetAffix()?.CombineWithLocalized(null)}
Values: {item.Affix1Value.ToString(CultureInfo.InvariantCulture)} {item.Affix1FlatValue.ToString(CultureInfo.InvariantCulture)} {item.Affix2Value.ToString(CultureInfo.InvariantCulture)} {item.Affix2FlatValue.ToString(CultureInfo.InvariantCulture)}
ItemLevel: {item.ItemPowerTotal}
Filters: {string.Join(", ", item.MatchingFilterNames)}
AspectHunted: {item.IsAspectHunted()}
NearBreakpoint: {item.IsNearBreakpoint()}
Elixir: {item.IsElixirItem()} {item.IsElixirHunted()}
Texture: {item.ActorSno?.ItemTextureMeta?.TextureSnoId} {item.ActorSno?.ItemTextureMeta?.TextureId}


""";

            // Textures: {string.Join(", ", item.ItemSno.ClassTextureDefault?.Select(x => x.TextureSnoId.ToString()) ?? Array.Empty<string>())}
            // Textures: {string.Join(", ", item.ItemSno.ClassTextureDefault?.Select(x => x.TextureId.ToString()) ?? Array.Empty<string>())}
            // Textures: {string.Join(", ", item.ItemSno.ClassTextureFemale?.Select(x => x.TextureSnoId.ToString()) ?? Array.Empty<string>())}
            // Textures: {string.Join(", ", item.ItemSno.ClassTextureFemale?.Select(x => x.TextureId.ToString()) ?? Array.Empty<string>())}
            Hint.SetHint(text);
            return;
        }

        var hasMatchingFilters = item.MatchingFilterNames.Length > 0;
        var isAspectHunted = item.IsAspectHunted();
        var isNearBreakpoint = item.IsNearBreakpoint();
        if (!hasMatchingFilters && !isAspectHunted && !isNearBreakpoint)
            return;

        var lines = item.GetHintLines(plugin).ToArray();
        if (lines.Length == 0)
            return;
        var hint = string.Join(Environment.NewLine, lines);
        Hint.SetHint(hint);
    }

    public static IEnumerable<string> GetHintLines(this IItem item, IPlugin plugin)
    {
        if (!item.IsEquippedTemp() && item.IsAspectHunted())
            yield return plugin.AspectHunter();

        if (item.IsNearBreakpoint())
            yield return Translation.TranslateFormat(plugin, "near breakpoint ({0})", item.GetNextBreakpoint());

        foreach (var filterName in item.MatchingFilterNames)
        {
            yield return filterName;
        }
    }
}