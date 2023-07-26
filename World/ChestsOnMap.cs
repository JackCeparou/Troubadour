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
    public float SilentChestSize { get; set; } = 5.0f;

    private readonly Feature _regularChestOnMap;
    private readonly Feature _eventRewardChestOnMap;
    private readonly Feature _questChestOnMap;

    public ChestsOnMap()
        : base(PluginCategory.Map, "display chest icons on the map")
    {
        _regularChestOnMap = AddFeature(nameof(_regularChestOnMap), "regular chest on map")
            .AddFloatResource(nameof(RegularChestSize), "icon size", 1.0f, 11.0f,
                getter: () => RegularChestSize,
                setter: newValue => RegularChestSize = newValue);

        _eventRewardChestOnMap = AddFeature(nameof(_eventRewardChestOnMap), "event chest on map")
            .AddFloatResource(nameof(EventRewardChestSize), "icon size", 1.0f, 11.0f,
                getter: () => EventRewardChestSize,
                setter: newValue => EventRewardChestSize = newValue);

        _questChestOnMap = AddFeature(nameof(_questChestOnMap), "quest chest on map")
            .AddFloatResource(nameof(QuestChestSize), "icon size", 1.0f, 11.0f,
                getter: () => QuestChestSize,
                setter: newValue => QuestChestSize = newValue);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (layer != GameWorldLayer.Map)
            return;

        if (_regularChestOnMap.Enabled)
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

        if (_eventRewardChestOnMap.Enabled)
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

        if (_questChestOnMap.Enabled)
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
    }
}