global using static T4.Services;
global using static T4.Plugins.Troubadour.DebugService;
global using static T4.Plugins.Troubadour.RenderExtensions;

namespace T4.Plugins.Troubadour;

public abstract class TroubadourPlugin : BasePlugin
{
    protected IEnumerable<IWorldFeature> WorldFeatures { get; init; } = Array.Empty<IWorldFeature>();

    protected TroubadourPlugin(PluginCategory group, string description) : base(group, description)
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
}