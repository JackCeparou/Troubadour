namespace T4.Plugins.Troubadour;

public sealed class CellarsOnMap : JackPlugin, IGameWorldPainter
{
    public static float IconSize { get; set; } = 24f;
    public static Feature OnMap { get; private set; }
    public static float MaxMapZoomLevel { get; set; } = 10;

    public CellarsOnMap()
    {
        EnabledByDefault = false;
        Group = PluginCategory.Map;
        Description = "displays cellars on map.";
    }

    public override void Load()
    {
        OnMap = new Feature
        {
            Plugin = this,
            NameOf = nameof(CellarsOnMap),
            DisplayName = () => Translation.Translate(this, "on map"),
            Resources = new List<AbstractFeatureResource>
            {
                new FloatFeatureResource
                {
                    NameOf = nameof(IconSize),
                    DisplayText = () => Translation.Translate(this, "icon size"),
                    MinValue = 10,
                    MaxValue = 42,
                    Getter = () => IconSize,
                    Setter = newValue => IconSize = newValue
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(MaxMapZoomLevel),
                    DisplayText = () => Translation.Translate(this, "maximum zoom level"),
                    MinValue = 1,
                    MaxValue = 10,
                    Getter = () => MaxMapZoomLevel,
                    Setter = newValue => MaxMapZoomLevel = newValue,
                },
            }
        }.Register();
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