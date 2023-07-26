namespace T4.Plugins.Troubadour;

public sealed class Valuables : JackPlugin, IGameWorldPainter
{
    public IWorldFeature Elixirs { get; private set; }
    public IWorldFeature SilentChests { get; private set; }
    public IWorldFeature MountCosmetics { get; private set; }

    public Valuables() : base(PluginCategory.Loot, "Highlight most valuable goods.")
    {
        Order = int.MaxValue;
        WorldFeatures = new[]
        {
            Elixirs = ElixirsFeature.Create(this, nameof(Elixirs)), //
            SilentChests = SilentChestsFeature.Create(this, nameof(SilentChests)), //
            MountCosmetics = MountCosmeticsFeature.Create(this, nameof(MountCosmetics)),
        };
    }

    public void PaintGameWorld(GameWorldLayer layer) => PaintWorld(layer);
}