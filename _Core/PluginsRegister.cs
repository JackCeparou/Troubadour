namespace T4.Plugins.Troubadour;

public static class PluginsRegister
{
    private static List<ITroubadourPlugin> TroubadourPlugins { get; } = new();

    public static T Register<T>(T plugin) where T : ITroubadourPlugin
    {
        TroubadourPlugins.Add(plugin);
        return plugin;
    }

    public static void OnScreenResize()
    {
        ScreenAnchors.Update();

        foreach (var plugin in TroubadourPlugins)
        {
            plugin.OnScreenResize();
        }
    }
}