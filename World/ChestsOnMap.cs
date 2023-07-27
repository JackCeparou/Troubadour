//*// all credits to swarm

namespace T4.Plugins.Troubadour;

public class ChestsOnMap : BasePlugin, IGameWorldPainter
{
    public ITexture RegularChestIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_503639160, 128);
    public ITexture EventRewardChestIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_288383018, 128);
    public ITexture QuestChestIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_1985863719, 128);
    public float RegularChestSize { get; set; } = 3.0f;
    public float EventRewardChestSize { get; set; } = 4.0f;
    public float QuestChestSize { get; set; } = 5.0f;
    public float SilentChestSize { get; set; } = 3.0f;
    public float SilentChestWorldCircleSize { get; set; } = 1f;
    public bool SilentChestOnGround { get; set; } = true;
    public ILineStyle SilentChestLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);

    private readonly Feature _regularChestOnMap;
    private readonly Feature _eventRewardChestOnMap;
    private readonly Feature _questChestOnMap;
    private readonly Feature _silentChestOnMap;

    public ChestsOnMap() : base(PluginCategory.Map, "display chest icons on the map")
    {
        _regularChestOnMap = AddFeature(nameof(_regularChestOnMap), "regular chest on map")
            .AddFloatResource(nameof(RegularChestSize), "icon size",
                1.0f, 11.0f, () => RegularChestSize, v => RegularChestSize = v);

        _eventRewardChestOnMap = AddFeature(nameof(_eventRewardChestOnMap), "event chest on map")
            .AddFloatResource(nameof(EventRewardChestSize), "icon size",
                1.0f, 11.0f, () => EventRewardChestSize, v => EventRewardChestSize = v);

        _questChestOnMap = AddFeature(nameof(_questChestOnMap), "quest chest on map")
            .AddFloatResource(nameof(QuestChestSize), "icon size",
                1.0f, 11.0f, () => QuestChestSize, v => QuestChestSize = v);

        _silentChestOnMap = AddFeature(nameof(_silentChestOnMap), "silent chest on map")
            .AddFloatResource(nameof(SilentChestSize), "icon size",
                1.0f, 11.0f, () => SilentChestSize, v => SilentChestSize = v)
            .AddBooleanResource(nameof(SilentChestOnGround), "circle on ground",
                () => SilentChestOnGround, v => SilentChestOnGround = v);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (_regularChestOnMap.Enabled && layer == GameWorldLayer.Map)
        {
            var chests = Game.GizmoActors
                .Where(x => x.World.WorldSno == Map.MapWorldSno && x.GizmoType == GizmoType.Chest && x.ActorSno.NameEnglish is "Chest" or "Resplendent Chest");

            var size = Game.WindowHeight / 100f * RegularChestSize;
            foreach (var chest in chests)
            {
                if (!chest.Coordinate.IsOnMap)
                    continue;

                RegularChestIcon.Draw(chest.Coordinate.MapX - (size / 2), chest.Coordinate.MapY - (size / 2), size, size, sharpen: false);
            }
        }

        if (_eventRewardChestOnMap.Enabled && layer == GameWorldLayer.Map)
        {
            var chests = Game.GizmoActors.Where(x => x.World.WorldSno == Map.MapWorldSno && x.GizmoType == GizmoType.Event_Reward_Chest);
            var size = Game.WindowHeight / 100f * EventRewardChestSize;
            foreach (var chest in chests)
            {
                if (!chest.Coordinate.IsOnMap)
                    continue;

                EventRewardChestIcon.Draw(chest.Coordinate.MapX - (size / 2), chest.Coordinate.MapY - (size / 2), size, size, sharpen: false);
            }
        }

        if (_questChestOnMap.Enabled && layer == GameWorldLayer.Map)
        {
            var chests = Game.GizmoActors.Where(x => x.World.WorldSno == Map.MapWorldSno && x.GizmoType == GizmoType.Quest_Chest);
            var size = Game.WindowHeight / 100f * QuestChestSize;
            foreach (var chest in chests)
            {
                if (!chest.Coordinate.IsOnMap)
                    continue;

                QuestChestIcon.Draw(chest.Coordinate.MapX - (size / 2), chest.Coordinate.MapY - (size / 2), size, size, sharpen: false);
            }
        }

        if (_silentChestOnMap.Enabled)
        {
            var chests = Game.GizmoActors
                .Where(x => x.World.WorldSno == Map.MapWorldSno && x.GizmoType == GizmoType.Chest && x.ActorSno.NameEnglish is "Silent Chest");
            var size = Game.WindowHeight / 100f * SilentChestSize;
            foreach (var chest in chests)
            {
                switch (layer)
                {
                    case GameWorldLayer.Ground when SilentChestOnGround:
                        if (!chest.Coordinate.IsOnScreen)
                            continue;

                        SilentChestLineStyle.DrawWorldEllipse(SilentChestWorldCircleSize, -1, chest.Coordinate, strokeWidthCorrection: 2f);
                        RegularChestIcon.Draw(chest.Coordinate.MapX - (size / 2), chest.Coordinate.MapY - (size / 2), size, size, sharpen: false);
                        break;
                    case GameWorldLayer.Map:
                        if (!chest.Coordinate.IsOnMap)
                            continue;

                        SilentChestLineStyle.DrawEllipse(chest.Coordinate.MapX, chest.Coordinate.MapY, -1, (size / 2), strokeWidthCorrection: 4f);
                        RegularChestIcon.Draw(chest.Coordinate.MapX - (size / 2), chest.Coordinate.MapY - (size / 2), size, size, sharpen: false);
                        break;
                }
            }
        }
    }
}