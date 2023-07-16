namespace T4.Plugins.Troubadour;

public static partial class Translations
{
    public static string TranslateExperimentalPlugin(this ITranslationService service, IPlugin plugin, string description)
    {
        return Translation.Translate(plugin, description + "\nEXPERIMENTAL / IN DEVELOPMENT");
    }
}