namespace T4.Plugins.Troubadour;

public sealed class Valuables : JackPlugin, IGameWorldPainter
{
    public IWorldFeature Elixirs { get; private set; }
    public IWorldFeature SilentChests { get; private set; }
    public IWorldFeature MountCosmetics { get; private set; }

    public Valuables()
    {
        Order = int.MaxValue;
        Group = PluginCategory.Loot;
        Description = "Highlight most valuable goods.";
    }

    public override void Load()
    {
        WorldFeatures = new[]
        {
            Elixirs = ElixirsFeature.Create(this, nameof(Elixirs)), //
            SilentChests = SilentChestsFeature.Create(this, nameof(SilentChests)), //
            MountCosmetics = MountCosmeticsFeature.Create(this, nameof(MountCosmetics)),
        };
    }

    public void PaintGameWorld(GameWorldLayer layer) => PaintWorld(layer);
}