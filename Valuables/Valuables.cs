namespace T4.Plugins.Troubadour;

public sealed class Valuables : BasePlugin, IGameUserInterfacePainter, IGameWorldPainter
{
    public Feature Elixirs { get; private set; }
    public Feature MountCosmetics { get; private set; }

    private static bool OnGroundEnabled => ElixirsStore.OnGroundEnabled || MountCosmeticsStore.OnGroundEnabled;
    private static bool OnMapEnabled => ElixirsStore.OnMapEnabled || MountCosmeticsStore.OnMapEnabled;

    public Valuables()
    {
        Order = int.MaxValue;
        EnabledByDefault = true;
    }

    public override string GetDescription()
        => Translation.Translate(this, "Highlight most valuable goods.");

    public override void Load()
    {
        Elixirs = this.CreateElixirFeature(nameof(Elixirs));
        MountCosmetics = this.CreateMountCosmeticFeature(nameof(MountCosmetics));
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (!Elixirs.Enabled && !MountCosmetics.Enabled)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground when OnGroundEnabled:
                if (Elixirs.Enabled)
                    ElixirsStore.PaintGround();
                if (MountCosmetics.Enabled)
                    MountCosmeticsStore.PaintGround();
                break;
            case GameWorldLayer.Map when OnMapEnabled:
                if (Elixirs.Enabled)
                    ElixirsStore.PaintMap();
                if (MountCosmetics.Enabled)
                    MountCosmeticsStore.PaintMap();
                break;
        }
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        // needed ???
    }
}