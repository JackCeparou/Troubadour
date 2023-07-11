namespace T4.Plugins.Troubadour;

public static class ItemExtensions
{
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

        var maxItemPower = item.ItemPowerTotal + (upgradesLeft * 5);
        return item.ItemPower switch
        {
            >= 135 and <= 149 => maxItemPower >= 150,
            >= 320 and <= 339 => maxItemPower >= 340,
            >= 440 and <= 459 => maxItemPower >= 460,
            >= 600 and <= 624 => maxItemPower >= 625,
            >= 700 and <= 724 => maxItemPower >= 725,
            _ => false
        };
    }

    public static int GetNextBreakpoint(this IItem item)
    {
        return item.ItemPower switch
        {
            >= 135 and <= 149 => 150,
            >= 320 and <= 339 => 340,
            >= 440 and <= 459 => 460,
            >= 600 and <= 624 => 625,
            >= 700 and <= 724 => 725,
            _ => 0
        };
    }

    public static string GetFriendlyAffixName(this IItem item)
    {
        switch (item.Quality)
        {
            case ItemQuality.Legendary:
                var affix = item.GetAffix();
                var name = affix?.NameLocalized ?? affix?.NameEnglish;
                if (string.IsNullOrWhiteSpace(name))
                    name = item.NameLocalized ?? item.NameEnglish;
                if (!string.IsNullOrWhiteSpace(name))
                    return GetFriendlyName(name);

                return name ?? string.Empty;

            case ItemQuality.Set:
            case ItemQuality.Unique:
                var uniqueName = item.GetAffix()?.NameLocalized;
                if (!string.IsNullOrWhiteSpace(uniqueName))
                    return uniqueName;
                return item.ItemSno.NameLocalized ?? item.ItemSno.NameEnglish ?? $"{item.Location}";

            default:
                return string.Empty;
        }
    }

    public static string GetFriendlyName(this AffixSnoId affixSnoId)
    {
        var affix = GameData.GetAffixSno(affixSnoId);
        if (Host.DebugEnabled)
        {
            return $"{affixSnoId}: {affix.GetFriendlyName()}";
        }

        return affix.GetFriendlyName();
    }

    public static string GetFriendlyName(this IAffixSno affixSno)
    {
        var name = affixSno.NameLocalized ?? affixSno.NameEnglish;
        if (!string.IsNullOrWhiteSpace(name) || !affixSno.SnoId.TryGetUniqueItemSnoId(out var itemSnoId))
            return GetFriendlyName(name);

        var item = GameData.GetItemSno(itemSnoId);
        return item?.NameLocalized ?? item?.NameEnglish ?? $"{itemSnoId}";
    }

    public static string GetFriendlyName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        if (!name.Contains(' '))
            return name;
        if (FriendlyNameCache.TryGetValue(name, out var friendlyName))
            return friendlyName;

        // very naive way to get a 'clean' name, not sure how it will react in i18n scenarios
        var nameParts = name.Split(' ').Where(x => x.Length > 3).ToArray();
        friendlyName = nameParts.Length > 1
            ? string.Join(' ', nameParts)
            : nameParts.FirstOrDefault() ?? name;

        FriendlyNameCache[name] = friendlyName;
        return friendlyName;
    }

    private static Dictionary<string, string> FriendlyNameCache { get; } = new();

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
FriendlyName: {item.GetAffix()?.GetFriendlyName()}
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