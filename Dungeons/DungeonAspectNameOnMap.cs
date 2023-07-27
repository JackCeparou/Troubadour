//*// all credits to swarm

namespace T4.Plugins.Troubadour;

public class DungeonAspectNameOnMap : BasePlugin, IGameWorldPainter
{
    public IFont NameFont { get; } =
        Render.GetFont(255, 0, 128, 0, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered)
            .SetShadowColor(255, 190, 178, 155);

    public float MaxMapZoomLevel = 7;
    public bool OnlyInteresting { get; private set; } = true;
    public bool HideUnlocked { get; private set; } = true;

    public Feature NameOnMap { get; }

    private readonly IGlobalMarker[] _markers = Game.GlobalMarkers
        .Where(x => x.WorldSno?.SnoId == WorldSnoId.Sanctuary_Eastern_Continent && x.GizmoType == GizmoType.Portal && x.TrackedRewardSno?.AspectSno != null)
        .ToArray();

    public DungeonAspectNameOnMap() : base(PluginCategory.Marker, "display aspect names for dungeons on the map")
    {
        NameOnMap = AddFeature(nameof(NameOnMap), "name font")
            .AddFontResource(nameof(NameFont), NameFont, "name font")
            .AddFloatResource(nameof(MaxMapZoomLevel), "maximum zoom level",
                1, 10, () => MaxMapZoomLevel, v => MaxMapZoomLevel = v)
            .AddBooleanResource(nameof(OnlyInteresting), "only interesting aspects",
                () => OnlyInteresting, v => OnlyInteresting = v)
            .AddBooleanResource(nameof(HideUnlocked), "hide earned aspects",
                () => HideUnlocked, v => HideUnlocked = v);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (layer != GameWorldLayer.Map)
            return;
        if (Map.MapZoomLevel > MaxMapZoomLevel)
            return;
        if (!NameOnMap.Enabled)
            return;
        if (Map.MapWorldSno.SnoId != WorldSnoId.Sanctuary_Eastern_Continent)
            return;

        var maxWidth = Game.WindowHeight / 100f * 8f;

        foreach (var marker in _markers)
        {
            if (OnlyInteresting && !Customization.InterestingAffixes.Contains(marker.TrackedRewardSno.AspectSno.AffixSno))
                continue;
            if (HideUnlocked && Game.UnlockedAspects.Any(x => x.SnoId == marker.TrackedRewardSno.AspectSno.SnoId))
                continue;

            var isOnMap = Map.WorldToMapCoordinate(marker.WorldCoordinate, out var mapX, out var mapY);
            if (!isOnMap)
                continue;

            var tlName = NameFont.GetTextLayout(marker.TrackedRewardSno.AspectSno.AffixSno.CombineWithLocalized(null), maxWidth);
            tlName.DrawText(mapX - (maxWidth / 2), mapY + tlName.Height, false);
        }
    }
}