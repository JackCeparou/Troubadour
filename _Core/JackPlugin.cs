namespace T4.Plugins.Troubadour;

public abstract class JackPlugin : BasePlugin, ITroubadourPlugin
{
    public bool TroubadourExperiment { get; init; }
    public required string Description { get; init; }
    public required PluginCategory Group { get; init; }

    protected IEnumerable<IWorldFeature> WorldFeatures { get; set; } = Array.Empty<IWorldFeature>();

    public sealed override string GetDescription() => TroubadourExperiment
        ? Translation.TranslateExperimentalPlugin(this, Description)
        : Translation.Translate(this, Description);

    // ReSharper disable once ConvertToAutoProperty
    public sealed override PluginCategory Category => Group;

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