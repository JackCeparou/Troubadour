namespace T4.Plugins.Troubadour;

public sealed class CellarsOnMap : JackPlugin, IGameWorldPainter
{
    public static float IconSize { get; set; } = 24f;
    public static Feature OnMap { get; private set; }
    public static float MaxMapZoomLevel { get; set; } = 10;

    public CellarsOnMap() : base(PluginCategory.Map, "displays cellars on map.")
    {
        EnabledByDefault = false;
        OnMap = AddFeature(nameof(OnMap), "on map")
            .AddFloatResource(nameof(IconSize), "icon size",
                10, 42,
                () => IconSize,
                v => IconSize = v)
            .AddFloatResource(nameof(MaxMapZoomLevel), "maximum zoom level",
                1, 10,
                () => MaxMapZoomLevel,
                v => MaxMapZoomLevel = v);
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

            Textures.CaveEntrance?.Draw(mapX - (IconSize / 2), mapY - (IconSize / 2), IconSize, IconSize);

            if (!Host.DebugEnabled)
                continue;

            var tl = DebugFont.GetTextLayout($"{marker.ActorSno.SnoId}\n{marker.GizmoType}", Game.WindowWidth);
            tl.DrawText(mapX - (tl.Width / 2), mapY - (tl.Height / 2));
        }
    }
}