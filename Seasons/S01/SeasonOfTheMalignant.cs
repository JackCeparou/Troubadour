namespace T4.Plugins.Troubadour;

public sealed class SeasonOfTheMalignant : JackPlugin, IGameWorldPainter
{
    public IWorldFeature MalignantHearts { get; private set; }
    public IWorldFeature CagedHearts { get; private set; }

    public SeasonOfTheMalignant()
    {
        Order = int.MaxValue;
        Group = PluginCategory.Loot;
        Description = "Season 1 companion";
    }

    public override void Load()
    {
        WorldFeatures = new[]
        {
            MalignantHearts = MalignantHeartsFeature.Create(this, nameof(MalignantHearts)), //
            CagedHearts = CagedMalignantHeartsFeature.Create(this, nameof(CagedHearts)), //
            // MountCosmetics = MountCosmeticsFeature.Create(this, nameof(MountCosmetics)),
        };
    }

    public void PaintGameWorld(GameWorldLayer layer) => PaintWorld(layer);
}