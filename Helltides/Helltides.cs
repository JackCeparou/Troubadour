namespace T4.Plugins.Troubadour;

public sealed class Helltides : BasePlugin, IGameWorldPainter
{
    public IWorldFeature CinderCaches { get; private set; }
    public IWorldFeature ZoneEvents { get; private set; }
    public IWorldFeature MysteriousChests { get; private set; }

    public Helltides()
    {
        Order = -1;
        EnabledByDefault = true;
    }

    public override PluginCategory Category
        => PluginCategory.WorldEvent;

    public override string GetDescription()
        => Translation.Translate(this, "Helltide companion");

    public override void Load()
    {
        CinderCaches = HelltideCindersFeature.Create(this, nameof(CinderCaches));
        ZoneEvents = HelltideEventsFeature.Create(this, nameof(ZoneEvents));
        MysteriousChests = MysteriousChestsFeature.Create(this, nameof(MysteriousChests));
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (Game.WorldTier < WorldTier.WorldTier3) // there is no helltide in T1 and T2
            return;
        if (!CinderCaches.Enabled && !MysteriousChests.Enabled && !ZoneEvents.Enabled)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground:
                CinderCaches.PaintGround();
                MysteriousChests.PaintGround();

                break;
            case GameWorldLayer.Map:
                CinderCaches.PaintMap();
                MysteriousChests.PaintMap();
                ZoneEvents.PaintMap();

                break;
        }
    }
}