namespace T4.Plugins.Troubadour;

public static class ItemExtensions
{
    public static bool IsEquippedTemp(this IItem item) => EquippedItemLocationsSet.Contains(item.Location);

    public static IEnumerable<ItemLocation> EquippedItemLocations { get; } = new List<ItemLocation>
    {
        ItemLocation.PlayerHead,
        ItemLocation.PlayerTorso,
        ItemLocation.PlayerHands,
        ItemLocation.PlayerLegs,
        ItemLocation.PlayerFeet,
        // jewelry
        ItemLocation.PlayerNeck,
        ItemLocation.PlayerRightFinger,
        ItemLocation.PlayerLeftFinger,
        // weapons
        ItemLocation.PlayerOffHand,
        ItemLocation.PlayerMainHand,
        ItemLocation.Player2HBlunt,
        ItemLocation.Player2HSlash,
        ItemLocation.Player2HRanged,
        ItemLocation.Player1HMain,
        ItemLocation.Player1HOff,
    };

    public static readonly HashSet<ItemLocation> EquippedItemLocationsSet = new(EquippedItemLocations);

    public static List<IItem> GetEquippedItems(this IGameService game)
        => game.Items.Where(x => EquippedItemLocationsSet.Contains(x.Location)).ToList();

    public static int GetMaxUpgradeCount(this IItem item) => item.ItemPower switch
    {
        >= 600 => 5,
        // TODO: this is wrong, but I don't know the correct values
        // >= 400 => 4, // tested wrong values: 300
        // _ => 3
        // so, we support only iLvl >= 600 for now
        _ => 0
    };

    public static bool IsNearBreakpoint(this IItem item)
    {
        // only apply to gear
        if (item.ItemSno.ItemUseType != ItemUseType.None)
            return false;
        if (item.ItemPowerTotal < 600 || item.Quality < ItemQuality.Rare)
            return false;
        // there is no breakpoint over 725 in the game atm
        if (item.ItemPowerTotal >= 725)
            return false;
        // only apply to ancestral, because normal and sacred gear has other rules we don't know yet
        if (item.QualityModifier is ItemQualityModifier.None)
            return false;
        if (item.QualityModifier is ItemQualityModifier.Sacred && item.ItemPowerTotal < 600)
            return false;
        var upgradesLeft = item.GetMaxUpgradeCount() - item.UpgradeCount;
        if (upgradesLeft <= 0)
            return false;

        var maxItemPower = item.ItemPowerTotal + (upgradesLeft * 5);
        return item.ItemPower switch
        {
            // we support only iLvl >= 600 for now
            // >= 135 and <= 149 => item.ItemPowerTotal < 150 && maxItemPower >= 150,
            // >= 320 and <= 339 => item.ItemPowerTotal < 340 && maxItemPower >= 340,
            // >= 440 and <= 459 => item.ItemPowerTotal < 460 && maxItemPower >= 460,
            >= 600 and <= 624 => item.ItemPowerTotal < 625 && maxItemPower >= 625,
            >= 700 and <= 724 => item.ItemPowerTotal < 725 && maxItemPower >= 725,
            _ => false
        };
    }

    public static int GetNextReachableBreakpoint(this IItem item)
    {
        return item.ItemPower switch
        {
            // we support only iLvl >= 600 for now
            // >= 135 and <= 149 => 150,
            // >= 320 and <= 339 => 340,
            // >= 440 and <= 459 => 460,
            >= 600 and <= 624 => 625,
            >= 700 and <= 724 => 725,
            _ => 0
        };
    }

    public static string GetFormattedItemPower(this IItem item, bool qualityModifierEnabled, bool upgradeSuffixEnabled)
    {
        if (item.ItemSno.ItemUseType == ItemUseType.DungeonKey)
            return $"T{item.SigilLevel}";

        var prefix = item.QualityModifier switch
        {
            ItemQualityModifier.Sacred when qualityModifierEnabled => "s",
            ItemQualityModifier.Ancestral when qualityModifierEnabled => "a",
            _ => "i",
        };
        var itemPower = upgradeSuffixEnabled
            ? item.ItemPower
            : item.ItemPowerTotal;
        var suffix = upgradeSuffixEnabled && item.UpgradeCount > 0
            ? $"+{item.UpgradeCount * 5}"
            : string.Empty;

        return $"{prefix}{itemPower}{suffix}";
    }

    public static string GetFriendlyAffixName(this IItem item)
    {
        switch (item.Quality)
        {
            case ItemQuality.Legendary:
                return item.AspectAffix?.GetFriendlyName()
                       ?? item.EquippedLegendaryAffixes.FirstOrDefault(x => x.IsSeasonal)?.GetFriendlyName()
                       ?? string.Empty;
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

    public static void SetHint(this IItem item, IPlugin plugin)
    {
        if (item.FilterMatches.Length == 0 && !AspectHunter.IsHunted(item) && !item.IsNearBreakpoint())
            return;

        var lines = item.GetHintLines(plugin).ToArray();
        if (lines.Length == 0)
            return;
        var hint = string.Join(Environment.NewLine, lines);
        Hint.SetHint(hint);
    }

    private static IEnumerable<string> GetHintLines(this IItem item, IPlugin plugin)
    {
        if (!item.IsEquippedTemp() && AspectHunter.IsHunted(item))
            yield return $"{Translation.Translate(plugin, "aspect hunter")}: {item.GetFriendlyAffixName()}";

        if (item.IsNearBreakpoint())
            yield return Translation.TranslateFormat(plugin, "near breakpoint ({0})", item.GetNextReachableBreakpoint());

        foreach (var match in item.FilterMatches)
        {
            yield return match.AsString();
        }
    }

    public static string AsString(this ItemFilterMatch x) => $"{x.FilterName} [{x.OptionalMatchCount}/{x.OptionalLimit}]";

    public static void NotifyMatchedFilters(this IItem item, IPlugin plugin)
    {
        var message = string.Join(", ", item.FilterMatches.Select(x => x.AsString()));
        Log.WriteLine(Verbosity.Info, () => Translation.TranslateFormat(plugin, "matched filters: {0}", message), 10);
    }
}