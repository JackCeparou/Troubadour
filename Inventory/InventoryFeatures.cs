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
    public bool ItemQualityModifierEnabled { get; set; }
    public bool MonsterLevelEnabled { get; set; }
    public bool AspectNameEnabled { get; set; }
    public bool SigilNameEnabled { get; set; }
    public bool ElixirNameEnabled { get; set; }
    // specifics
    public bool OnPaperDoll { get; private set; }
    public float MaxTextWidthRatio { get; private set; } = 1f;
    public bool HasIcons { get; private set; }
    public bool HasLines { get; private set; }
    public bool HasOverlays { get; private set; }
    public bool HasHints { get; private set; }

    public IFont GetFont(bool hasError)
        => hasError ? ErrorFont : NormalFont;

    public static InventoryFeatures Create(IPlugin plugin, string name, string displayName, IFont font = null, IFont errorFont = null)
    {
        font ??= CreateDefaultFont();
        errorFont ??= CreateDefaultErrorFont();
        return new InventoryFeatures
        {
            Plugin = plugin,
            NameOf = name,
            DisplayName = plugin.Translate(displayName),
            Resources = new(),
            NormalFont = font,
            ErrorFont = errorFont,
        };
    }

    public InventoryFeatures PaperDoll()
    {
        Resources.Add(new FloatFeatureResource
            {
                NameOf = nameof(MaxTextWidthRatio),
                DisplayText = Plugin.Translate("max text width ratio"),
                MinValue = 1f,
                MaxValue = 5.0f,
                Getter = () => MaxTextWidthRatio,
                Setter = newValue => MaxTextWidthRatio = newValue,
            }
        );
        OnPaperDoll = true;
        MaxTextWidthRatio = 3f;
        return this;
    }

    public InventoryFeatures ShowHint()
    {
        Resources.Add(new BooleanFeatureResource()
            {
                NameOf = "Hints",
                DisplayText = Plugin.Translate("hints"),
                Getter = () => HasHints,
                Setter = newValue => HasHints = newValue,
            }
        );
        HasHints = true;
        return this;
    }

    public void AddIcon(AbstractFeatureResource resource)
    {
        if (!HasIcons)
        {
            Resources.Add(new FloatFeatureResource
                {
                    NameOf = nameof(IconSize),
                    DisplayText = Plugin.IconSize,
                    MinValue = 0f,
                    MaxValue = 32.0f,
                    Getter = () => IconSize,
                    Setter = newValue => IconSize = newValue,
                }
            );
            HasIcons = true;
        }

        Resources.Add(resource);
    }

    public void AddTextLine(AbstractFeatureResource resource)
    {
        if (!HasLines)
        {
            Resources.Add(new FontFeatureResource { NameOf = nameof(NormalFont), DisplayText = Plugin.NormalFont, Font = NormalFont, });
            Resources.Add(new FontFeatureResource { NameOf = nameof(ErrorFont), DisplayText = Plugin.ErrorFont, Font = ErrorFont, });
            HasLines = true;
        }

        Resources.Add(resource);
    }

    public void AddOverlay(AbstractFeatureResource resource)
    {
        HasOverlays = true;
        Resources.Add(resource);
    }
}