namespace T4.Plugins.Troubadour;

public sealed partial class InventoryFeatures : Feature
{
    // icons
    public float IconSize { get; set; } = 24f;
    public bool QualityLegendaryIconEnabled { get; set; }
    public bool QualityUniqueIconEnabled { get; set; }
    public bool NearBreakpointIconEnabled { get; set; }
    public bool AspectHunterIconEnabled { get; set; }

    public bool TreasureHunterIconEnabled { get; set; }

    // overlays
    public bool AspectHunterHighlightEnabled { get; set; }
    public bool TreasureHunterHighlightEnabled { get; set; }
    public bool TreasureHunterMatchedFilterCountEnabled { get; set; }
    public bool TreasureHunterMatchedFilterNamesEnabled { get; set; }

    public bool ElixirHunterHighlightEnabled { get; set; } = true;

    // grey out
    public bool GreyOutEnabled { get; set; }
    public bool GreyOutUpgradedItemsEnabled { get; set; } = true;
    public bool GreyOutGemItemsEnabled { get; set; } = true;

    public bool GreyOutSigilItemsEnabled { get; set; } = true;

    // lines
    public IFont ErrorFont { get; init; }
    public IFont NormalFont { get; init; }
    public bool ItemLevelEnabled { get; set; }
    public bool ItemLevelUpgradeSuffixEnabled { get; set; }
    public bool ItemQualityModifierEnabled { get; set; }
    public bool MonsterLevelEnabled { get; set; }
    public bool AspectNameEnabled { get; set; }
    public bool AspectNameSeasonalEnabled { get; set; }

    public bool ElixirNameEnabled { get; set; }

    // specifics
    public bool OnPaperDoll { get; private set; }
    public float MaxTextWidthRatio { get; private set; } = 1f;
    public bool HasIcons { get; private set; }
    public bool HasLines { get; private set; }
    public bool HasOverlays { get; private set; }
    public bool HasHints { get; private set; }

    private InventoryPage InventoryPage { get; init; }
    public List<ItemTextLine> ItemLines { get; } = new();
    public List<ItemIcon> ItemIcons { get; } = new();
    public List<ItemOverlay> ItemOverlays { get; } = new();

    public IFont GetFont(bool hasError)
        => hasError ? ErrorFont : NormalFont;

    #region Common

    public static InventoryFeatures Create(IPlugin plugin, string name, string displayName,
        IFont font = null,
        IFont errorFont = null,
        InventoryPage page = null)
    {
        font ??= CreateDefaultFont();
        errorFont ??= CreateDefaultErrorFont();
        var feature = new InventoryFeatures
        {
            Plugin = plugin,
            NameOf = name,
            DisplayName = () => Translation.Translate(plugin, displayName),
            Resources = new(),
            NormalFont = font,
            ErrorFont = errorFont,
            InventoryPage = page,
        };
        plugin.Features.Add(feature);
        return feature;
    }

    public void Draw()
    {
        InventoryPage?.Draw(this);
    }

    public InventoryFeatures PaperDoll()
    {
        OnPaperDoll = true;
        MaxTextWidthRatio = 3f;
        AddFloatResource(nameof(MaxTextWidthRatio), "max text width ratio",
            1f, 5.0f,
            () => MaxTextWidthRatio, v => MaxTextWidthRatio = v);
        return this;
    }

    public InventoryFeatures ShowHint()
    {
        HasHints = true;
        AddBooleanResource(nameof(HasHints), "hints", () => HasHints, newValue => HasHints = newValue);
        return this;
    }

    private void AddIcon(ItemIcon itemIcon)
    {
        if (!HasIcons)
        {
            HasIcons = true;
            AddFloatResource(nameof(IconSize), "icon size", 0f, 32.0f, () => IconSize, newValue => IconSize = newValue);
        }

        ItemIcons.Add(itemIcon);
    }

    private void AddTextLine(ItemTextLine itemTextLine)
    {
        if (!HasLines)
        {
            HasLines = true;
            AddFontResource(nameof(NormalFont), NormalFont, "normal font");
            AddFontResource(nameof(ErrorFont), ErrorFont, "error font");
        }

        ItemLines.Add(itemTextLine);
    }

    private void AddOverlay(ItemOverlay itemOverlay)
    {
        HasOverlays = true;
        ItemOverlays.Add(itemOverlay);
    }

    #endregion

    #region Aspects

    private static ITexture AspectHunterTexture { get; } = Render.GetTexture(SupportedTextureId.UITest_3915375899);

    public InventoryFeatures AspectHunterIcon(bool enabled = true)
    {
        AddIcon(new()
        {
            Scale = 0.8f,
            Show = (item, features) => features.AspectHunterIconEnabled && AspectHunter.IsHunted(item),
            Texture = (_, _) => AspectHunterTexture
        });

        AspectHunterIconEnabled = enabled;

        AddBooleanResource(nameof(AspectHunterIconEnabled), "aspect hunter icon",
            () => AspectHunterIconEnabled, v => AspectHunterIconEnabled = v);

        return this;
    }

    public InventoryFeatures AspectHunterHighlight(bool enabled = true)
    {
        AddOverlay(new()
        {
            StrokeWidthCorrection = (_, _) => 1f,
            Show = (item, features) => features.AspectHunterHighlightEnabled && AspectHunter.IsHunted(item),
            Style = (_, _) => AspectHunter.LineStyle,
        });

        AspectHunterHighlightEnabled = enabled;

        AddBooleanResource(nameof(AspectHunterHighlightEnabled), "aspect hunter highlight",
            () => AspectHunterHighlightEnabled, v => AspectHunterHighlightEnabled = v);

        return this;
    }

    public InventoryFeatures AspectName(bool enabled = true)
    {
        AddTextLine(new()
        {
            IsName = true,
            Show = (item, features) => features.AspectNameEnabled && item.ItemSno.GemType == GemType.None,
            Text = (item, _) => item.GetFriendlyAffixName(),
            HasError = item => item.IsEquippedTemp()
                               && item.AspectAffix is not null
                               && Affixes.DuplicateEquippedLegendaryAffixes.Contains(item.AspectAffix.SnoId),
        });

        AspectNameEnabled = enabled;

        AddBooleanResource(nameof(AspectNameEnabled), "aspect name",
            () => AspectNameEnabled, v => AspectNameEnabled = v);

        return this;
    }

    public InventoryFeatures SeasonalAspectName(bool enabled = true)
    {
        AddTextLine(new()
        {
            IsName = true,
            Show = (item, features) => features.AspectNameSeasonalEnabled && item.ItemSno.GemType == GemType.None,
            Text = (item, _) => item.EquippedLegendaryAffixes.FirstOrDefault(x => x.IsSeasonal)?.GetFriendlyName(),
            HasError = item =>
            {
                if (!item.IsEquippedTemp())
                    return false;

                var seasonalAffix = item.EquippedLegendaryAffixes.FirstOrDefault(x => x.IsSeasonal);

                return seasonalAffix is not null && Affixes.DuplicateEquippedLegendaryAffixes.Contains(seasonalAffix.SnoId);
            },
        });

        AspectNameSeasonalEnabled = enabled;

        AddBooleanResource(nameof(AspectNameSeasonalEnabled), "aspect name (seasonal)",
            () => AspectNameSeasonalEnabled, v => AspectNameSeasonalEnabled = v);

        return this;
    }

    #endregion

    #region Elixirs

    public InventoryFeatures ElixirName(bool enabled = true)
    {
        AddTextLine(new()
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
                            name = name.Replace("Elixir of ", string.Empty);
                        else if (name.EndsWith(" Elixir"))
                            name = name.Substring(0, name.Length - 7);
                        break;
                }

                return name;
            }
        });

        ElixirNameEnabled = enabled;

        AddBooleanResource(nameof(ElixirNameEnabled), "elixir name",
            () => ElixirNameEnabled, v => ElixirNameEnabled = v);

        return this;
    }

    public InventoryFeatures ElixirHunterHighlight(bool enabled = true)
    {
        AddOverlay(new() { Show = (item, features) => features.ElixirHunterHighlightEnabled && Elixirs.IsHunted(item), Style = (_, _) => Elixirs.LineStyle, });

        ElixirHunterHighlightEnabled = enabled;

        AddBooleanResource(nameof(ElixirHunterHighlightEnabled), "elixir hunter highlight",
            () => ElixirHunterHighlightEnabled, v => ElixirHunterHighlightEnabled = v);

        return this;
    }

    #endregion

    #region GreyOut

    public InventoryFeatures GreyOut(bool enabled = true)
    {
        AddOverlay(new()
        {
            OffsetLeft = 0f,
            OffsetTop = 0f,
            OffsetWidth = 0f,
            OffsetHeight = 0f,
            Show = (item, features) =>
            {
                if (!features.GreyOutEnabled)
                    return false;
                if (features.GreyOutUpgradedItemsEnabled && item.UpgradeCount > 0)
                    return false;
                if (features.GreyOutGemItemsEnabled && (item.ItemSno.GemType != GemType.None || item.ItemSno.SnoId == ItemSnoId.GamblingCurrency_Key))
                    return false;
                if (features.GreyOutSigilItemsEnabled && item.ItemSno.ItemUseType == ItemUseType.DungeonKey)
                    return false;

                return InventoryGreyOut.Evaluate(item);
            },
            Fill = (_, _) => InventoryGreyOut.FillStyle,
        });

        GreyOutEnabled = enabled;

        AddBooleanResource(nameof(GreyOutEnabled), "grey out not hunted", () => GreyOutEnabled, v => GreyOutEnabled = v);

        return this;
    }

    #endregion

    #region ItemPower

    public InventoryFeatures ItemLevel(bool enabled = true, bool enabledQualityPrefix = true, bool enabledSuffix = false)
    {
        AddTextLine(new()
        {
            Show = (item, features) =>
            {
                if (!features.ItemLevelEnabled)
                    return false;
                if (item.ItemPower == 0)
                    return false;
                if (item.IsAspectItem())
                    return false;
                if (item.ItemSno.GemType != GemType.None)
                    return false;
                if (item.Location is ItemLocation.PlayerBackpack or ItemLocation.Stash)
                    return true;
                if (item.ItemSno.ItemUseType == ItemUseType.DungeonKey)
                    return true;

                return item.IsEquippedTemp();
            },
            Text = (item, features) => item.GetFormattedItemPower(features.ItemQualityModifierEnabled, features.ItemLevelUpgradeSuffixEnabled),
        });

        ItemLevelEnabled = enabled;
        ItemQualityModifierEnabled = enabledQualityPrefix;
        ItemLevelUpgradeSuffixEnabled = enabledSuffix;

        AddBooleanResource(nameof(ItemLevelEnabled), "iLvl",
            () => ItemLevelEnabled, v => ItemLevelEnabled = v);
        AddBooleanResource(nameof(ItemQualityModifierEnabled), "iLvl quality prefix: s = sacred, a = ancestral",
            () => ItemQualityModifierEnabled, v => ItemQualityModifierEnabled = v);
        AddBooleanResource(nameof(ItemLevelUpgradeSuffixEnabled), "iLvl upgrades",
            () => ItemLevelUpgradeSuffixEnabled, v => ItemLevelUpgradeSuffixEnabled = v);

        return this;
    }

    public InventoryFeatures MonsterLevel(bool enabled = true)
    {
        AddTextLine(new()
        {
            Show = (item, features) =>
            {
                if (!features.MonsterLevelEnabled)
                    return false;
                return item.ItemSno.ItemUseType == ItemUseType.DungeonKey;
            },
            Text = (item, _) => $"M{item.SigilLevel + 54}",
        });

        MonsterLevelEnabled = enabled;

        AddBooleanResource(nameof(MonsterLevelEnabled), "monster level",
            () => MonsterLevelEnabled, v => MonsterLevelEnabled = v);

        return this;
    }

    private static ITexture NearBreakpointTexture { get; } = Render.GetTexture(SupportedTextureId.UISkills_4275309202);

    public InventoryFeatures NearBreakpointIcon(bool enabled = true)
    {
        AddIcon(new()
        {
            Scale = 0.8f,
            Show = (item, features) => features.NearBreakpointIconEnabled && item.IsNearBreakpoint(),
            Texture = (_, _) => NearBreakpointTexture,
        });

        NearBreakpointIconEnabled = enabled;

        AddBooleanResource(nameof(NearBreakpointIconEnabled), "near breakpoint icon",
            () => NearBreakpointIconEnabled, v => NearBreakpointIconEnabled = v);

        return this;
    }

    private static ITexture LegendaryIcon { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_2706340597);
    private static ITexture UniqueIcon { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_3759295089);

    public InventoryFeatures QualityIcon(bool legendary = true, bool unique = true)
    {
        AddIcon(new()
        {
            Scale = 1.2f,
            OffsetX = 6f,
            Show = (_, features) => features.QualityLegendaryIconEnabled || features.QualityUniqueIconEnabled,
            Texture = (item, features) =>
            {
                return item.Quality switch
                {
                    ItemQuality.Legendary => features.QualityLegendaryIconEnabled ? LegendaryIcon : null,
                    ItemQuality.Set or ItemQuality.Unique => features.QualityUniqueIconEnabled ? UniqueIcon : null,
                    _ => null
                };
            }
        });

        QualityLegendaryIconEnabled = legendary;
        QualityUniqueIconEnabled = unique;

        AddBooleanResource(nameof(QualityLegendaryIconEnabled), "quality icon (legendary)",
            () => QualityLegendaryIconEnabled, v => QualityLegendaryIconEnabled = v);
        AddBooleanResource(nameof(QualityUniqueIconEnabled), "quality icon (unique)",
            () => QualityUniqueIconEnabled, v => QualityUniqueIconEnabled = v);

        return this;
    }

    #endregion

    #region TreasureHunter

    // public static ITexture TreasureHunterIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_003);
    // public static ITexture TreasureHunterGoldenIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_001);
    private static ITexture TreasureHunterGoldenOpenIcon { get; } = Render.GetTexture(SupportedTextureId.UIStash_339055441);

    public InventoryFeatures TreasureHunterIcon(bool enabled = true)
    {
        AddIcon(new()
        {
            Scale = 1.2f,
            OffsetX = 6f,
            OffsetY = 3f,
            Show = (item, features) => features.TreasureHunterIconEnabled && item.FilterMatches.Length > 0,
            Texture = (_, _) => TreasureHunterGoldenOpenIcon,
        });

        TreasureHunterIconEnabled = enabled;

        AddBooleanResource(nameof(TreasureHunterIconEnabled), "treasure hunter icon",
            () => TreasureHunterIconEnabled, v => TreasureHunterIconEnabled = v);

        return this;
    }

    public InventoryFeatures TreasureHunterHighlight(bool enabled = true)
    {
        AddOverlay(new()
        {
            StrokeWidthCorrection = (_, _) => 2f,
            Show = (item, features) => features.TreasureHunterHighlightEnabled && item.FilterMatches.Length > 0,
            Style = (_, _) => TreasureHunter.LineStyle,
        });

        TreasureHunterHighlightEnabled = enabled;

        AddBooleanResource(nameof(TreasureHunterHighlightEnabled), "treasure hunter highlight",
            () => TreasureHunterHighlightEnabled, v => TreasureHunterHighlightEnabled = v);

        return this;
    }

    public InventoryFeatures TreasureHunterFilterNames(bool enabled = true)
    {
        AddTextLine(new()
        {
            Show = (item, features) => features.TreasureHunterMatchedFilterNamesEnabled && item.FilterMatches.Length > 0,
            Text = (item, _) => string.Join(Environment.NewLine, item.FilterMatches.Take(3).Select(x => x.AsString())),
        });

        TreasureHunterMatchedFilterNamesEnabled = enabled;

        AddBooleanResource(nameof(TreasureHunterMatchedFilterNamesEnabled), "treasure hunter matched filter names",
            () => TreasureHunterMatchedFilterNamesEnabled, v => TreasureHunterMatchedFilterNamesEnabled = v);

        return this;
    }

    public InventoryFeatures TreasureHunterFilterCount(bool enabled = true)
    {
        AddOverlay(new()
        {
            Show = (item, features) => features.TreasureHunterMatchedFilterCountEnabled && item.FilterMatches.Length > 0,
            Font = (_, _) => TreasureHunter.MatchedFilterCounterFont,
        });

        TreasureHunterMatchedFilterCountEnabled = enabled;

        AddBooleanResource(nameof(TreasureHunterMatchedFilterCountEnabled), "treasure hunter matched filter count",
            () => TreasureHunterMatchedFilterCountEnabled, v => TreasureHunterMatchedFilterCountEnabled = v);

        return this;
    }

    #endregion
}