// namespace T4.Plugins.Troubadour;
//
// public class Completionist : BasePlugin, IGameWorldPainter
// {
//     public ITexture Icon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_182, 96);
//     public IFont Font { get; } = Render.GetFont(255, 255, 255, 255);
//     public float IconSize { get; set; } = 2.2f;
//     public float MaxMapZoomLevel { get; set; } = 7;
//
//     public Feature IconOnMap { get; private set; }
//
//     public override string GetDescription() => Translation.Translate(this, "display all Altar Of Lilith locations on the map");
//
//     public override void Load()
//     {
//         base.Load();
//
//         IconOnMap = new Feature()
//         {
//             Plugin = this,
//             NameOf = nameof(IconOnMap),
//             DisplayName = () => Translation.Translate(this, "icon on map"),
//             Resources = new()
//             {
//                 new FloatFeatureResource()
//                 {
//                     NameOf = nameof(IconSize),
//                     DisplayText = () => Translation.Translate(this, "icon size"),
//                     MinValue = 1.0f,
//                     MaxValue = 11.0f,
//                     Getter = () => IconSize,
//                     Setter = newValue => IconSize = newValue,
//                 },
//                 new FloatFeatureResource()
//                 {
//                     NameOf = nameof(MaxMapZoomLevel),
//                     DisplayText = () => Translation.Translate(this, "maximum zoom level"),
//                     MinValue = 1.0f,
//                     MaxValue = 10.0f,
//                     Getter = () => MaxMapZoomLevel,
//                     Setter = newValue => MaxMapZoomLevel = newValue,
//                 },
//             },
//         };
//
//         Customization.RegisterFeature(IconOnMap);
//     }
//
//     public void PaintGameWorld(GameWorldLayer layer)
//     {
//         if (layer != GameWorldLayer.Map)
//             return;
//         if (Map.MapZoomLevel > MaxMapZoomLevel)
//             return;
//         if (!IconOnMap.Enabled)
//             return;
//         if (Map.MapWorldSno.SnoId != WorldSnoId.Sanctuary_Eastern_Continent)
//             return;
//
//         var size = Game.WindowHeight / 100f * IconSize;
//
//         var markers = Game.GlobalMarkers
//             .Where(x => x.WorldSno == Map.MapWorldSno);
//             // .Where(x => x.WorldSno == Map.MapWorldSno && x.ActorSno?.NameEnglish == "Altar of Lilith");
//
//         foreach (var marker in markers)
//         {
//             var isOnMap = Map.WorldToMapCoordinate(marker.WorldCoordinate, out var mapX, out var mapY);
//             if (!isOnMap)
//                 continue;
//             var text = $"""
// {marker.ActorSno.SnoId}
// {string.Join(", ", marker.SubZoneSnoList.Select(x => x.SnoId))}
// """;
//             var tl = Font.GetTextLayout(text);
//             var x = mapX - (tl.Width / 2);
//             var y = mapY - (tl.Height / 2);
//             tl.DrawText(x, y);
//             // Icon.Draw(mapX - (size / 2), mapY - (size / 2), size, size, sharpen: false);
//         }
//     }
// }