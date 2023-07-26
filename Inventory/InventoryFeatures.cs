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

    public bool ElixirNameEnabled { get; set; }

    // specifics
    public bool OnPaperDoll { get; private set; }
    public float MaxTextWidthRatio { get; private set; } = 1f;
    public bool HasIcons { get; private set; }
    public bool HasLines { get; private set; }
    public bool HasOverlays { get; private set; }
    public bool HasHints { get; private set; }

    public InventoryPage InventoryPage { get; private set; }
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

    public InventoryFeatures AddIcon(ItemIcon itemIcon)
    {
        if (!HasIcons)
        {
            HasIcons = true;
            AddFloatResource(nameof(IconSize), "icon size", 0f, 32.0f, () => IconSize, newValue => IconSize = newValue);
        }

        ItemIcons.Add(itemIcon);
        return this;
    }

    public InventoryFeatures AddTextLine(ItemTextLine itemTextLine)
    {
        if (!HasLines)
        {
            HasLines = true;
            AddFontResource(nameof(NormalFont), NormalFont, "normal font");
            AddFontResource(nameof(ErrorFont), ErrorFont, "error font");
        }

        ItemLines.Add(itemTextLine);
        return this;
    }

    public InventoryFeatures AddOverlay(ItemOverlay itemOverlay)
    {
        HasOverlays = true;
        ItemOverlays.Add(itemOverlay);
        return this;
    }

    #endregion

    #region Aspects

    public InventoryFeatures AspectHunterIcon(bool enabled = true)
    {
        AddIcon(new()
        {
            Scale = 0.8f,
            Show = (_, features) => features.AspectHunterIconEnabled,
            Texture = (item, _) => item.IsAspectHunted() ? Textures.AspectHunterIcon : null
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
            Show = (_, features) => features.AspectHunterHighlightEnabled,
            Style = (item, _) => item.IsAspectHunted() || Host.DebugEnabled ? AspectHunterStore.LineStyle : null,
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
            HasError = item =>
                item.IsEquippedTemp() && item.CurrentAffixes
                    .Where(x => x.MagicType is not MagicType.None)
                    .Any(x => Affixes.DuplicateEquippedLegendaryAffixes.Contains(x.SnoId)),
        });

        AspectNameEnabled = enabled;

        AddBooleanResource(nameof(AspectNameEnabled), "aspect name",
            () => AspectNameEnabled, v => AspectNameEnabled = v);

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
        AddOverlay(new()
        {
            Show = (_, features) => features.ElixirHunterHighlightEnabled,
            Style = (item, _) => item.IsElixirHunted() || Host.DebugEnabled ? Elixirs.LineStyle : null,
        });

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
            Show = ShouldGreyOut,
            Fill = (item, _) => item.MatchingFilterNames.Length == 0 ? TreasureHunterStore.GreyOutFillStyle : null,
        });

        GreyOutEnabled = enabled;

        AddBooleanResource(nameof(GreyOutEnabled), "grey out not hunted", () => GreyOutEnabled, v => GreyOutEnabled = v);

        return this;
    }

    public static bool ShouldGreyOut(IItem item, InventoryFeatures features)
    {
        if (!features.GreyOutEnabled)
            return false;
        if (features.GreyOutUpgradedItemsEnabled && item.UpgradeCount > 0)
            return false;
        if (features.GreyOutGemItemsEnabled && (item.ItemSno.GemType != GemType.None || item.ItemSno.SnoId == ItemSnoId.GamblingCurrency_Key))
            return false;
        if (features.GreyOutSigilItemsEnabled && item.ItemSno.ItemUseType == ItemUseType.DungeonKey)
            return false;
        if (item.IsMountCosmeticItem())
            return false;
        if (item.IsElixirItem())
            return !item.IsElixirHunted();

        if (Customization.InterestingAffixes.Any())
        {
            if (item.IsAspectItem())
                return !item.IsAspectHunted();

            //* S01
            if (item.IsMalignantInvoker())
                return false;
            if (item.IsMalignantHeart())
                return !item.IsMalignantHeartHunted();
            var heart = item.GetMalignantHeartLegendaryAffix();
            if (heart is not null)
                return !heart.SnoId.IsMalignantHeartHunted();
            // S01 */
        }

        return item.MatchingFilterNames.Length == 0 && !item.IsAspectHunted();
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

    public InventoryFeatures NearBreakpointIcon(bool enabled = true)
    {
        AddIcon(new()
        {
            Scale = 0.8f,
            Show = (item, features) => features.NearBreakpointIconEnabled && item.IsNearBreakpoint(),
            Texture = (_, _) => Textures.NearBreakpointIcon,
        });

        NearBreakpointIconEnabled = enabled;

        AddBooleanResource(nameof(NearBreakpointIconEnabled), "near breakpoint icon",
            () => NearBreakpointIconEnabled, v => NearBreakpointIconEnabled = v);

        return this;
    }

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
                    ItemQuality.Legendary => features.QualityLegendaryIconEnabled ? Textures.LegendaryIcon : null,
                    ItemQuality.Set or ItemQuality.Unique => features.QualityUniqueIconEnabled ? Textures.UniqueIcon : null,
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

    public InventoryFeatures TreasureHunterIcon(bool enabled = true)
    {
        AddIcon(new()
        {
            Scale = 1.2f,
            OffsetX = 6f,
            OffsetY = 3f,
            Show = (_, features) => features.TreasureHunterIconEnabled,
            Texture = (item, _) => item.MatchingFilterNames.Length > 0 || Host.DebugEnabled ? Textures.TreasureHunterGoldenOpenIcon : null,
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
            Show = (item, features) => features.TreasureHunterHighlightEnabled && item.MatchingFilterNames.Length > 0,
            Style = (item, _) => item.MatchingFilterNames.Length > 0 || Host.DebugEnabled ? TreasureHunterStore.LineStyle : null,
        });

        TreasureHunterHighlightEnabled = enabled;

        AddBooleanResource(nameof(TreasureHunterHighlightEnabled), "treasure hunter highlight",
            () => TreasureHunterHighlightEnabled, v => TreasureHunterHighlightEnabled = v);

        return this;
    }

    public InventoryFeatures TreasureHunterFilterCount(bool enabled = true)
    {
        AddOverlay(new()
        {
            Show = (item, features) => features.TreasureHunterMatchedFilterCountEnabled && item.MatchingFilterNames.Length > 0,
            Font = (item, _) => item.MatchingFilterNames.Length > 0 || Host.DebugEnabled ? TreasureHunterStore.MatchedFilterCounterFont : null,
        });

        TreasureHunterMatchedFilterCountEnabled = enabled;

        AddBooleanResource(nameof(TreasureHunterMatchedFilterCountEnabled),"treasure hunter matched filter count",
            () => TreasureHunterMatchedFilterCountEnabled, v => TreasureHunterMatchedFilterCountEnabled = v);

        return this;
    }

    #endregion
}