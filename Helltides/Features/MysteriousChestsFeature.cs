namespace T4.Plugins.Troubadour;

public sealed class MysteriousChestsFeature : WorldFeature<ICommonActor>
{
    private TimeZoneInfo _pstTimeZoneInfo;
    private readonly ITexture _helltideChest = Render.GetTexture(SupportedTextureId.UIMinimapIcons_43109186);

    private MysteriousChestsFeature()
    {
        LineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapIconTexture = _helltideChest;
        MapIconSize = 32f;
        MapCircleSize = 10f;
        Try(() => _pstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
    }

    public override IEnumerable<ICommonActor> GetWorldActors()
    {
        return Game.GizmoActors.Where(x => x.ActorSno.SnoId == ActorSnoId.usz_rewardGizmo_Uber);
    }

    public static MysteriousChestsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new MysteriousChestsFeature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = () => Translation.Translate(plugin, "mysterious chests"),
            Resources = new List<AbstractFeatureResource>()
        };
        feature.AddDefaultGroundResources();
        feature.AddDefaultMapResources();

        plugin.Features.Add(feature);
        return feature;
    }

    public override void PaintMap()
    {
        if (!Enabled || !OnMapEnabled || _pstTimeZoneInfo is null)
            return;
        var helltide = Game.HelltideEventMarkers.FirstOrDefault(x => x.EndsInMilliseconds > 0);
        if (helltide is null)
            return;

        var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstTimeZoneInfo);
        var chestAround = Game.GizmoActors.FirstOrDefault(x => x.ActorSno.SnoId == ActorSnoId.usz_rewardGizmo_Uber);

        foreach (var chest in HelltidesStore.GetActiveMysteriousChests(helltide.SubzoneSno.SnoId, now.Hour))
        {
            if (!Map.WorldToMapCoordinate(chest.X, chest.Y, out var mapX, out var mapY))
                continue;

            if (chestAround is not null)
            {
                // hijack the chest actor to get the map coordinates
                if (Math.Abs(mapX - chestAround.Coordinate.MapX) < 20 && Math.Abs(mapY - chestAround.Coordinate.MapY) < 20)
                {
                    mapX = chestAround.Coordinate.MapX;
                    mapY = chestAround.Coordinate.MapY;
                }
            }

            MapLineStyle?.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
            MapIconTexture?.Draw(mapX - (MapIconSize / 2), mapY - (MapIconSize / 2), MapIconSize, MapIconSize);
        }
    }
}