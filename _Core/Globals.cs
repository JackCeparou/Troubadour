global using static T4.Services;
global using static T4.Plugins.Troubadour.DebugService;
global using static T4.Plugins.Troubadour.Globals;
global using static T4.Plugins.Troubadour.RenderExtensions;
global using static T4.Plugins.Troubadour.PluginsRegister;

namespace T4.Plugins.Troubadour;

public static partial class Globals
{
    public static bool IsInTown => Game?.MyPlayer?.LevelAreaSno?.IsTown ?? false;

    public static GameObserverService GameObserver { get; } = new();
}