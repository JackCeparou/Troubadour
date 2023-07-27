namespace T4.Plugins.Troubadour;

public sealed class CellarsOnMap : TroubadourPlugin, IGameWorldPainter
{
    public Feature OnMap { get; }

    public float IconSize { get; set; } = 24f;
    public float MaxMapZoomLevel { get; set; } = 10;
    private readonly ITexture _caveEntranceTexture = Render.GetTexture(SupportedTextureId.UIMinimapIcons_3564463460);

    public CellarsOnMap() : base(PluginCategory.Marker, "displays cellars on map.")
    {
        EnabledByDefault = false;
        OnMap = AddFeature(nameof(OnMap), "on map")
            .AddFloatResource(nameof(IconSize), "icon size",
                10, 42, () => IconSize, v => IconSize = v)
            .AddFloatResource(nameof(MaxMapZoomLevel), "maximum zoom level",
                1, 10, () => MaxMapZoomLevel, v => MaxMapZoomLevel = v);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (layer != GameWorldLayer.Map)
            return;
        if (!OnMap.Enabled)
            return;
        if (Map.CurrentMode == MapMode.Map && Map.MapZoomLevel > MaxMapZoomLevel)
            return;

        var currentWorldSno = Map.MapWorldSno;
        var markers = Game.GlobalMarkers.Where(x => x.ActorSno?.SnoId is ActorSnoId.Portal_Cellar_Flat or ActorSnoId.Portal_Cellar_Generic);
        foreach (var marker in markers)
        {
            if (marker.WorldSno != currentWorldSno || marker.WorldCoordinate.IsZero)
                continue;
            if (!Map.WorldToMapCoordinate(marker.WorldCoordinate, out var mapX, out var mapY))
                continue;

            _caveEntranceTexture.Draw(mapX - (IconSize / 2), mapY - (IconSize / 2), IconSize, IconSize);
        }
    }
}