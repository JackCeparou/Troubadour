namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemIcon CreateTreasureHunter() => new()
    {
        Scale = 1.2f,
        OffsetX = 6f,
        OffsetY = 3f,
        Show = (_, features) => features.TreasureHunterIconEnabled,
        Texture = (item, _) => item.MatchingFilterNames.Length > 0 || Host.DebugEnabled ? Textures.TreasureHunterGoldenOpenIcon : null,
        BackgroundTexture = (item, _) => item.MatchingFilterNames.Length > 0 || Host.DebugEnabled ? Textures.UiStash004 : null,
    };

    private static ItemOverlay CreateTreasureHunterHighlight() => new()
    {
        StrokeWidthCorrection = (_, _) => 2f,
        Show = (item, features) => features.TreasureHunterHighlightEnabled && item.MatchingFilterNames.Length > 0,
        Style = (item, _) => item.MatchingFilterNames.Length > 0 || Host.DebugEnabled ? TreasureHunterStore.LineStyle : null,
    };

    private static ItemOverlay CreateTreasureHunterMatchedFilterCount() => new()
    {
        Show = (item, features) => features.TreasureHunterMatchedFilterCountEnabled && item.MatchingFilterNames.Length > 0,
        Font = (item, _) => item.MatchingFilterNames.Length > 0 || Host.DebugEnabled ? TreasureHunterStore.MatchedFilterCounterFont : null,
    };
}

public sealed partial class InventoryFeatures
{
    public InventoryFeatures TreasureHunterIcon(bool enabled = true)
    {
        AddIcon(new BooleanFeatureResource
        {
            NameOf = nameof(TreasureHunterIconEnabled),
            DisplayText = () => Translation.Translate(Plugin, "treasure hunter icon"),
            Getter = () => TreasureHunterIconEnabled,
            Setter = v => TreasureHunterIconEnabled = v,
        });
        TreasureHunterIconEnabled = enabled;
        return this;
    }

    public InventoryFeatures TreasureHunterHighlight(bool enabled = true)
    {
        AddOverlay(new BooleanFeatureResource
        {
            NameOf = nameof(TreasureHunterHighlightEnabled),
            DisplayText = () => Translation.Translate(Plugin, "treasure hunter highlight"),
            Getter = () => TreasureHunterHighlightEnabled,
            Setter = v => TreasureHunterHighlightEnabled = v,
        });
        TreasureHunterHighlightEnabled = enabled;
        return this;
    }

    public InventoryFeatures TreasureHunterFilterCount(bool enabled = true)
    {
        AddOverlay(new BooleanFeatureResource
        {
            NameOf = nameof(TreasureHunterMatchedFilterCountEnabled),
            DisplayText = () => Translation.Translate(Plugin, "treasure hunter matched filter count"),
            Getter = () => TreasureHunterMatchedFilterCountEnabled,
            Setter = v => TreasureHunterMatchedFilterCountEnabled = v,
        });
        TreasureHunterMatchedFilterCountEnabled = enabled;
        return this;
    }
}