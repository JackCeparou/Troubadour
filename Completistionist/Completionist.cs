// namespace T4.Plugins.Troubadour;
//
// public class Completionist : BasePlugin, IGameWorldPainter, IMenuUserInterfacePainter
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
//     public void PaintMenuUserInterface()
//     {
//         // var z = GameData.AllQuestSno.Where(x => x.BountyType == BountyType.Event);
//         // // DrawDevText(() => z.Count().ToString());
//         // DrawDevText(() => string.Join(Environment.NewLine, z.Select(x => $"{x.SnoId}")), y: 0);
//     }
//     
//     public HashSet<QuestSnoId> Blacklist = new()
//     {
//         QuestSnoId.Bounty_LE_Tier1_Step_PvP_CompleteAny1, //963800u, // 0xEB4D8
//         QuestSnoId.Bounty_LE_Tier2_Frac_TundraN_CompleteAny3, //1292295u, // 0x13B807
//         QuestSnoId.Bounty_LE_Tier2_Frac_TundraS_CompleteAny3, //608337u, // 0x94851
//         QuestSnoId.Bounty_LE_Tier2_Hawe_Crossway_CompleteAny3, //1241683u, // 0x12F253
//         QuestSnoId.Bounty_LE_Tier2_Hawe_Delta_CompleteAny3, //1241805u, // 0x12F2CD
//         QuestSnoId.Bounty_LE_Tier2_Hawe_Fens_CompleteAny3, //1241962u, // 0x12F36A
//         QuestSnoId.Bounty_LE_Tier2_Hawe_Marsh_CompleteAny3, //1241974u, // 0x12F376
//         QuestSnoId.Bounty_LE_Tier2_Hawe_Verge_CompleteAny3, //1241982u, // 0x12F37E
//         QuestSnoId.Bounty_LE_Tier2_Hawe_Wetland_CompleteAny3, //1241991u, // 0x12F387
//         QuestSnoId.Bounty_LE_Tier2_Kehj_LowDesert_CompleteAny3, //1076318u, // 0x106C5E
//         QuestSnoId.Bounty_LE_Tier2_Kehj_Ridge_CompleteAny3, //1290329u, // 0x13B059
//         QuestSnoId.Bounty_LE_Tier2_Scos_Coast_CompleteAny3, //884844u, // 0xD806C
//         QuestSnoId.Bounty_LE_Tier2_Scos_Highlands_CompleteAny3, //1219060u, // 0x1299F4
//         QuestSnoId.Bounty_LE_Tier2_Scos_Lowlands_CompleteAny3, //884842u, // 0xD806A
//         QuestSnoId.Bounty_LE_Tier2_Scos_Moors_CompleteAny3, //1219035u, // 0x1299DB
//         QuestSnoId.Bounty_LE_Tier2_Step_Central_CompleteAny3, //920784u, // 0xE0CD0
//         QuestSnoId.Bounty_LE_Tier2_Step_South_CompleteAny3, //920704u, // 0xE0C80
//     };
//
//     public void PaintGameWorld(GameWorldLayer layer)
//     {
//         if (layer != GameWorldLayer.Map)
//             return;
//         // if (Map.MapZoomLevel > MaxMapZoomLevel)
//         //     return;
//         if (!IconOnMap.Enabled)
//             return;
//         if (Map.MapWorldSno.SnoId != WorldSnoId.Sanctuary_Eastern_Continent)
//             return;
//
//         var events = GameData.AllQuestSno
//             .Where(x => x.BountyType is BountyType.Event /*or BountyType.OWC*/)
//             .Where(x => !Blacklist.Contains(x.SnoId))
//             .Where(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Step_South or SubzoneSnoId.Step_Central
//                 or SubzoneSnoId.Scos_Coast or SubzoneSnoId.Scos_Deep_Forest
//                 or SubzoneSnoId.Frac_Tundra_N or SubzoneSnoId.Frac_Tundra_S
//                 or SubzoneSnoId.Hawe_Verge or SubzoneSnoId.Hawe_Wetland
//                 or SubzoneSnoId.Kehj_Oasis or SubzoneSnoId.Kehj_LowDesert or SubzoneSnoId.Kehj_HighDesert
//             )
//             // .Where(x => x.SubzoneSno?.SnoId is SubzoneSnoId.Kehj_Oasis or SubzoneSnoId.Kehj_LowDesert or SubzoneSnoId.Kehj_HighDesert)
//             ;
//         foreach (var @event in events)
//         {
//             var isOnMap = Map.WorldToMapCoordinate(@event.WorldCoordinate, out var mapX, out var mapY);
//             if (!isOnMap)
//                 continue;
//
//             var size = 24f;
//             Textures.BountyEvent.Draw(mapX - (size / 2f), mapY - (size / 2f), size, size, sharpen: false);
//         }
//
//         var markers = Game.GlobalMarkers
//                 .Where(x => x.WorldSno == Map.MapWorldSno)
//                 .Where(x => false)
//             ;
//         // .Where(x => x.WorldSno == Map.MapWorldSno); // && x.ActorSno.SnoId == ActorSnoId.usz_rewardGizmo_Uber);
//         // .Where(x => x.WorldSno == Map.MapWorldSno && x.ActorSno?.NameEnglish == "Altar of Lilith");
//
//         foreach (var marker in markers)
//         {
//             var isOnMap = Map.WorldToMapCoordinate(marker.WorldCoordinate, out var mapX, out var mapY);
//             if (!isOnMap)
//                 continue;
//             var text = $"""
// {marker.ActorSno?.SnoId} {marker.GizmoType}
// """;
// //            {string.Join(", ", marker.SubZoneSnoList?.Select(x => x?.SnoId.ToString() ?? string.Empty) ?? Array.Empty<string>())}
//             var tl = Font.GetTextLayout(text);
//             var x = mapX - (tl.Width / 2);
//             var y = mapY - (tl.Height / 2);
//             tl.DrawText(x, y);
//             // Icon.Draw(mapX - (size / 2), mapY - (size / 2), size, size, sharpen: false);
//         }
//     }
// }