namespace T4.Plugins.Troubadour;

public static class FeatureExtensions
{
    public static T Register<T>(this T feature) where T : Feature
    {
        feature.Resources.TrimExcess();
        Customization.RegisterFeature(feature);
        return feature;
    }
}