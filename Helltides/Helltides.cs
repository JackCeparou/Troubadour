namespace T4.Plugins.Troubadour;

public sealed class Helltides : TroubadourPlugin, IGameWorldPainter
{
    public IWorldFeature CinderCaches { get; }
    public IWorldFeature ZoneEvents { get; }
    public IWorldFeature MysteriousChests { get; }
    // public IWorldFeature ChestIcons { get; }

    public Helltides() : base(PluginCategory.WorldEvent, "Helltide companion")
    {
        Order = -1;
        WorldFeatures = new[]
        {
            MysteriousChests = MysteriousChestsFeature.Create(this, nameof(MysteriousChests)), //
            // ChestIcons = HelltideChestIconsFeature.Create(this, nameof(ChestIcons)), //
            ZoneEvents = HelltideEventsFeature.Create(this, nameof(ZoneEvents)), //
            CinderCaches = HelltideCinderCachesFeature.Create(this, nameof(CinderCaches))
        };
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (Game.WorldTier < WorldTier.WorldTier3) // there is no helltide below WT3
            return;

        PaintWorld(layer);
    }
}