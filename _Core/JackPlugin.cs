namespace T4.Plugins.Troubadour;

public abstract class JackPlugin : BasePlugin, ITroubadourPlugin
{
    protected IEnumerable<IWorldFeature> WorldFeatures { get; init; } = Array.Empty<IWorldFeature>();

    protected JackPlugin(PluginCategory group, string description) : base(group, description)
    {
    }

    protected void PaintWorld(GameWorldLayer layer)
    {
        if (!WorldFeatures.Any(x => x.Enabled))
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground:
                foreach (var feature in WorldFeatures)
                {
                    if (!feature.Enabled || !feature.OnGroundEnabled)
                        continue;

                    feature.PaintGround();
                }

                break;
            case GameWorldLayer.Map:
                foreach (var feature in WorldFeatures)
                {
                    if (!feature.Enabled || !feature.OnMapEnabled)
                        continue;

                    feature.PaintMap();
                }

                break;
        }
    }

    public virtual void OnScreenResize()
    {
    }
}