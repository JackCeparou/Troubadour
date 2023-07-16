namespace T4.Plugins.Troubadour;

public sealed class Valuables : BasePlugin, IGameWorldPainter
{
    public IWorldFeature Elixirs { get; private set; }
    public IWorldFeature SilentChests { get; private set; }
    public IWorldFeature MountCosmetics { get; private set; }

    private List<IWorldFeature> WorldFeatures { get; } = new();

    public Valuables()
    {
        Order = int.MaxValue;
        EnabledByDefault = true;
    }

    public override PluginCategory Category
        => PluginCategory.Loot;

    public override string GetDescription()
        => Translation.Translate(this, "Highlight most valuable goods.");

    public override void Load()
    {
        Elixirs = ElixirsFeature.Create(this, nameof(Elixirs));
        SilentChests = SilentChestsFeature.Create(this, nameof(SilentChests));
        MountCosmetics = MountCosmeticsFeature.Create(this, nameof(MountCosmetics));

        WorldFeatures.Add(Elixirs);
        WorldFeatures.Add(SilentChests);
        WorldFeatures.Add(MountCosmetics);
        WorldFeatures.TrimExcess(); // yeah, i know
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (!WorldFeatures.Any(x => x.Enabled))
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground:
                WorldFeatures.PaintGround();

                break;
            case GameWorldLayer.Map:
                WorldFeatures.PaintMap();

                break;
        }
    }
}