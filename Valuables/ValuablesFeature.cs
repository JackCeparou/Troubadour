namespace T4.Plugins.Troubadour;

public abstract class ValuablesFeature : Feature
{
    public bool OnGroundEnabled { get; set; } = true;
    public ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float WorldCircleSize { get; set; } = 0.5f;
    public float WorldCircleStroke { get; set; } = 2f;

    public bool OnMapEnabled { get; set; } = true;
    public ILineStyle MapLineStyle { get; init; } = Render.GetLineStyle(200, 255, 255, 0);
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 4f;

    public virtual void AddResourceBefore() { }
    public virtual void AddResourceAfter() { }

    public static Func<IItem, bool> WorldItemPredicate { get; } = _ => true;

    public void PaintGround()
    {
        if (!OnGroundEnabled)
            return;

        var items = Game.Items.Where(WorldItemPredicate);
        foreach (var item in items)
        {
            LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
        }
    }

    public void PaintMap()
    {
        if (!OnMapEnabled)
            return;

        var items = Game.Items.Where(WorldItemPredicate);
        foreach (var item in items)
        {
            if (!Map.WorldToMapCoordinate(item.Coordinate, out var mapX, out var mapY))
                continue;

            MapLineStyle.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
        }
    }

    public static T Create<T>(IPlugin plugin, string nameOf, string displayName) where T : ValuablesFeature, new()
    {
        var feature = new T { Plugin = plugin, NameOf = nameOf, DisplayName = plugin.Translate(displayName), Resources = new List<AbstractFeatureResource>() };
        feature.AddResourceBefore();
        feature.Resources.Add(new BooleanFeatureResource
        {
            NameOf = nameof(OnGroundEnabled),
            DisplayText = plugin.Translate("on ground"),
            Getter = () => feature.OnGroundEnabled,
            Setter = newValue => feature.OnGroundEnabled = newValue
        });
        feature.Resources.Add(new LineStyleFeatureResource { NameOf = nameof(LineStyle), DisplayText = plugin.LineStyle, LineStyle = feature.LineStyle });
        feature.Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(WorldCircleSize),
            DisplayText = plugin.Radius,
            Getter = () => feature.WorldCircleSize,
            Setter = newValue => feature.WorldCircleSize = newValue,
            MinValue = 0,
            MaxValue = 2
        });
        feature.Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(WorldCircleStroke),
            DisplayText = plugin.Stroke,
            Getter = () => feature.WorldCircleStroke,
            Setter = newValue => feature.WorldCircleStroke = newValue,
            MinValue = 0,
            MaxValue = 10
        });
        feature.Resources.Add(new BooleanFeatureResource
        {
            NameOf = nameof(OnMapEnabled),
            DisplayText = plugin.Translate("on map"),
            Getter = () => feature.OnMapEnabled,
            Setter = newValue => feature.OnMapEnabled = newValue
        });
        feature.Resources.Add(new LineStyleFeatureResource { NameOf = nameof(MapLineStyle), DisplayText = plugin.MapLineStyle, LineStyle = feature.MapLineStyle });
        feature.Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(MapCircleSize),
            DisplayText = plugin.MapRadius,
            Getter = () => feature.MapCircleSize,
            Setter = newValue => feature.MapCircleSize = newValue,
            MinValue = 0,
            MaxValue = 20
        });
        feature.Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(MapCircleStroke),
            DisplayText = plugin.MapStroke,
            Getter = () => feature.MapCircleStroke,
            Setter = newValue => feature.MapCircleStroke = newValue,
            MinValue = 0,
            MaxValue = 10
        });
        feature.AddResourceAfter();

        return feature.Register();
    }
}