namespace T4.Plugins.Troubadour;

public static partial class Inventory
{
    private static ItemIcon CreateMalignantHeartHunterIcon() => new()
    {
        Scale = 0.8f, Show = (_, features) => features.HeartHunterIconEnabled, Texture = (item, _) => item.IsMalignantHeartHunted() ? Textures.HeartHunterIcon : null
    };

    private static ItemOverlay CreateMalignantHeartHunterHighlight() => new()
    {
        StrokeWidthCorrection = (_, _) => 1f,
        Show = (_, features) => features.HeartHunterHighlightEnabled,
        Style = (item, _) => item.IsMalignantHeartHunted() || Host.DebugEnabled ? MalignantHeartsFeature.SharedLineStyle : null,
    };

    private static ItemTextLine CreateMalignantHeartName() => new()
    {
        IsName = true,
        Show = (item, features) => features.HeartNameEnabled && item.ItemSno.GemType == GemType.None,
        Text = (item, _) => item.GetMalignantHeartLegendaryAffix()?.GetFriendlyName() ?? string.Empty,
    };
}

public sealed partial class InventoryFeatures
{
    public bool HeartNameEnabled { get; set; }
    public bool HeartHunterIconEnabled { get; set; }
    public bool HeartHunterHighlightEnabled { get; set; }

    public InventoryFeatures MalignantHeartName(bool enabled = true)
    {
        AddTextLine(new BooleanFeatureResource
        {
            NameOf = nameof(HeartNameEnabled),
            DisplayText = () => Translation.Translate(Plugin, "heart name"),
            Getter = () => HeartNameEnabled,
            Setter = v => HeartNameEnabled = v,
        });
        HeartNameEnabled = enabled;
        return this;
    }

    public InventoryFeatures MalignantHeartIcon(bool enabled = true)
    {
        AddIcon(new BooleanFeatureResource
        {
            NameOf = nameof(HeartHunterIconEnabled),
            DisplayText = () => Translation.Translate(Plugin, "caged heart hunter icon"),
            Getter = () => HeartHunterIconEnabled,
            Setter = v => HeartHunterIconEnabled = v,
        });
        HeartHunterIconEnabled = enabled;
        return this;
    }

    public InventoryFeatures MalignantHeartHighlight(bool enabled = true)
    {
        AddOverlay(new BooleanFeatureResource
        {
            NameOf = nameof(HeartHunterHighlightEnabled),
            DisplayText = () => Translation.Translate(Plugin, "caged heart hunter highlight"),
            Getter = () => HeartHunterHighlightEnabled,
            Setter = v => HeartHunterHighlightEnabled = v,
        });
        HeartHunterHighlightEnabled = enabled;
        return this;
    }
}