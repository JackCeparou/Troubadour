//*// all credits to swarm
namespace T4.Plugins.Troubadour;

public class DungeonAspectNameOnMap : BasePlugin, IGameWorldPainter
{
    public IFont NameFont { get; } = Services.Render.GetFont(255, 0, 128, 0, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered).SetShadowColor(255, 190, 178, 155);
    public float MaxMapZoomLevel = 7;
    public bool OnlyInteresting { get; set; } = true;

    public Feature NameOnMap { get; private set; }

    public override string GetDescription() => Services.Translation.Translate(this, "display aspect names for dungeons on the map");
    public override PluginCategory Category => PluginCategory.Marker;

    private readonly IGlobalMarker[] Markers = Services.Game.GlobalMarkers
        .Where(x => x.WorldSno?.SnoId == WorldSnoId.Sanctuary_Eastern_Continent && x.GizmoType == GizmoType.Portal && x.TrackedRewardSno?.AspectSno != null)
        .ToArray();

    public override void Load()
    {
        base.Load();

        NameOnMap = new Feature()
        {
            Plugin = this,
            NameOf = nameof(NameOnMap),
            DisplayName = () => Services.Translation.Translate(this, "dungeon aspect name on map"),
            Resources = new()
            {
                new FontFeatureResource(nameof(NameFont), NameFont, () => Services.Translation.Translate(this, "name font")),
                new FloatFeatureResource()
                {
                    NameOf = nameof(MaxMapZoomLevel),
                    DisplayText = () => Services.Translation.Translate(this, "maximum zoom level"),
                    MinValue = 1.0f,
                    MaxValue = 10.0f,
                    Getter = () => MaxMapZoomLevel,
                    Setter = newValue => MaxMapZoomLevel = newValue,
                },
            },
        };

        Services.Customization.RegisterFeature(NameOnMap);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (layer != GameWorldLayer.Map)
            return;

        if (Services.Map.MapZoomLevel > MaxMapZoomLevel)
            return;

        if (!NameOnMap.Enabled)
            return;

        if (Services.Map.MapWorldSno.SnoId != WorldSnoId.Sanctuary_Eastern_Continent)
            return;

        var maxWidth = Services.Game.WindowHeight / 100f * 8f;

        foreach (var marker in Markers)
        {
            if (OnlyInteresting && !Services.Customization.InterestingAffixes.Contains(marker.TrackedRewardSno.AspectSno.AffixSno))
                continue;

            var isOnMap = Services.Map.WorldToMapCoordinate(marker.WorldCoordinate, out var mapX, out var mapY);
            if (!isOnMap)
                continue;

            var tlName = NameFont.GetTextLayout(marker.TrackedRewardSno.AspectSno.AffixSno.CombineWithLocalized(null), maxWidth);
            tlName.DrawText(mapX - (maxWidth / 2), mapY + tlName.Height, false);
        }
    }
}
//*/