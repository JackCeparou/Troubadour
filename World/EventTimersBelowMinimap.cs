//*// all credits to swarm

namespace T4.Plugins.Troubadour;

public class EventTimersBelowMinimap : BasePlugin, IGameUserInterfacePainter
{
    public ITexture LegionEventIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_4293250490, 256);
    public ITexture LegionEventIconActive { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_3039116543, 256);
    public ITexture WorldBossEventIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_2528081359, 256);
    public ITexture WorldBossEventIconActive { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_658781990, 256);
    public ITexture HelltideEventIcon { get; } = Render.GetTexture(SupportedTextureId.UIMinimapIcons_3613452396, 256);
    public IFont Font { get; } = Render.GetFont(255, 222, 206, 189, bold: true, shadowMode: FontShadowMode.Heavy);
    public float IconSize { get; set; } = 2.5f;
    public bool ShowHelltide { get; private set; } = true;
    public bool HideHelltideBelowWorldTier3 { get; private set; } = true;

    private readonly Feature _iconsBelowMinimap;

    public EventTimersBelowMinimap()
        : base(PluginCategory.WorldEvent, "display event timers below the minimap")
    {
        _iconsBelowMinimap = AddFeature(nameof(_iconsBelowMinimap), "icon and name on map")
            .AddFontResource(nameof(Font), Font, "font")
            .AddFloatResource(nameof(IconSize), "icon size",
                1.0f, 11.0f, () => IconSize, v => IconSize = v)
            .AddBooleanResource(nameof(ShowHelltide), "show helltide",
                () => ShowHelltide, v => ShowHelltide = v)
            .AddBooleanResource(nameof(HideHelltideBelowWorldTier3), "hide helltide below world tier 3",
                () => HideHelltideBelowWorldTier3, v => HideHelltideBelowWorldTier3 = v);
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.BeforeClip)
            return;
        if (Map.CurrentMode != MapMode.Minimap)
            return;
        if (!_iconsBelowMinimap.Enabled)
            return;

        var iconSize = Game.WindowHeight / 100f * IconSize;

        var x = UserInterface.MinimapControl.Left;
        var y = UserInterface.MinimapControl.Top + UserInterface.MinimapControl.Height - (iconSize * 0.175f);

        if (ShowHelltide && (!HideHelltideBelowWorldTier3 || Game.WorldTier > WorldTier.WorldTier2))
        {
            var helltideEvent = Game.HelltideEventMarkers.FirstOrDefault(m => m.EndsInMilliseconds > 0);
            if (helltideEvent != null)
            {
                HelltideEventIcon.Draw(x, y, iconSize, iconSize);

                var tl = Font.GetTextLayout(ValueFormatter.EstimatedTimeInFutureToString(helltideEvent.EndsInMilliseconds));
                x += iconSize * 0.9f;
                tl.DrawText(x, y + ((iconSize - tl.Height) / 2) - 1);
                x += tl.Width + (iconSize * 0.1f);
            }
            else
            {
                HelltideEventIcon.Draw(x, y, iconSize, iconSize);

                var nextHelltideInMs = GetMillisecondsToNextHelltide();

                var tl = Font.GetTextLayout("+" + ValueFormatter.EstimatedTimeInFutureToString(nextHelltideInMs));
                x += iconSize * 0.9f;
                tl.DrawText(x, y + ((iconSize - tl.Height) / 2) - 1);
                x += tl.Width + (iconSize * 0.1f);
            }
        }

        foreach (var marker in Game.TimedEventMarkers)
        {
            var icon = marker.SubzoneSno.SnoId is SubzoneSnoId.Frac_WorldBoss or SubzoneSnoId.Hawe_WorldBoss or SubzoneSnoId.Kehj_WorldBoss
                or SubzoneSnoId.Scos_WorldBoss or SubzoneSnoId.Step_WorldBoss
                ? marker.StartsInMilliseconds > 0
                    ? WorldBossEventIcon
                    : WorldBossEventIconActive
                : marker.StartsInMilliseconds > 0
                    ? LegionEventIcon
                    : LegionEventIconActive;

            icon.Draw(x, y, iconSize, iconSize);

            if (marker.StartsInMilliseconds > 0)
            {
                var tl = Font.GetTextLayout(ValueFormatter.EstimatedTimeInFutureToString(marker.StartsInMilliseconds));
                x += iconSize * 0.9f;
                tl.DrawText(x, y + ((iconSize - tl.Height) / 2) - 1);
                x += tl.Width + (iconSize * 0.1f);
            }
        }
    }

    private static readonly DateTime HelltideReferenceDateTime = new(2023, 6, 22, 0, 15, 0, DateTimeKind.Utc);

    private static int GetMillisecondsToNextHelltide()
    {
        const int millisecondsBetweenHelltides = 135 * 60 * 1000; // 2h15m from the start of one helltide to the start of the next
        var milliseconds = millisecondsBetweenHelltides - ((DateTime.UtcNow - HelltideReferenceDateTime).TotalMilliseconds % millisecondsBetweenHelltides);
        if (milliseconds > int.MaxValue)
            return int.MaxValue;

        return (int)milliseconds;
    }
}