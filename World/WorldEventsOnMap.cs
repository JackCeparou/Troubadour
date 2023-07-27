namespace T4.Plugins.Troubadour;

public sealed class WorldEventsOnMap : TroubadourPlugin, IGameWorldPainter
{
    public static Feature OnMap { get; private set; }

    public static float IconSize { get; set; } = 24f;
    public static float MaxMapZoomLevel { get; set; } = 10;

    private readonly ITexture _bountyEventActive = Render.GetTexture(SupportedTextureId.UIMinimapIcons_503808014);

    public WorldEventsOnMap() : base(PluginCategory.Marker, "displays world events on map.")
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
        foreach (var quest in GameData.GetBountyEvents())
        {
            if (quest.WorldSno != currentWorldSno || quest.WorldCoordinate.IsZero)
                continue;
            if (!Map.WorldToMapCoordinate(quest.WorldCoordinate, out var mapX, out var mapY))
                continue;

            _bountyEventActive.Draw(mapX - (IconSize / 2), mapY - (IconSize / 2), IconSize, IconSize);
        }
    }
}