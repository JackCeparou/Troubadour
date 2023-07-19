global using static T4.Services;
global using static T4.Plugins.Troubadour.DebugService;
global using static T4.Plugins.Troubadour.Globals;
global using static T4.Plugins.Troubadour.RenderExtensions;

namespace T4.Plugins.Troubadour;

public static partial class Globals
{
    public static GameObserverService GameObserver { get; } = new();
}