namespace T4.Plugins.Troubadour;

public sealed class Helltides : BasePlugin, IGameWorldPainter
{
    public IWorldFeature CinderCaches { get; private set; }

    public Helltides()
    {
        Order = -1;
        EnabledByDefault = false;
    }

    public override PluginCategory Category
        => PluginCategory.WorldEvent;

    public override string GetDescription()
        => Translation.Translate(this, "Helltide companion");

    public override void Load()
    {
        CinderCaches = HelltideCindersFeature.Create(this, nameof(CinderCaches));
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (!CinderCaches.Enabled)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground:
                CinderCaches.PaintGround();

                break;
            case GameWorldLayer.Map:
                CinderCaches.PaintMap();

                break;
        }
    }
}