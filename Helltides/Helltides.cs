namespace T4.Plugins.Troubadour;

public sealed class Helltides : JackPlugin, IGameWorldPainter
{
    public IWorldFeature CinderCaches { get; private set; }
    public IWorldFeature ZoneEvents { get; private set; }
    public IWorldFeature MysteriousChests { get; private set; }
    public IWorldFeature ChestIcons { get; private set; }

    public Helltides()
    {
        Order = -1;
        Group = PluginCategory.WorldEvent;
        Description = "Helltide companion";
    }

    public override void Load()
    {
        WorldFeatures = new[]
        {
            MysteriousChests = MysteriousChestsFeature.Create(this, nameof(MysteriousChests)), //
            // ChestIcons = HelltideChestIconsFeature.Create(this, nameof(ChestIcons)), //
            ZoneEvents = HelltideEventsFeature.Create(this, nameof(ZoneEvents)), //
            CinderCaches = HelltideCindersFeature.Create(this, nameof(CinderCaches))
        };
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (Game.WorldTier < WorldTier.WorldTier3) // there is no helltide below WT3
            return;

        PaintWorld(layer);
    }
}