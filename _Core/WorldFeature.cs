namespace T4.Plugins.Troubadour;

public abstract class WorldFeature<T> : Feature where T : ICommonActor
{
    public bool OnGroundEnabled { get; set; } = true;
    public ILineStyle LineStyle { get; init; }
    public float WorldCircleSize { get; set; } = 0.5f;
    public float WorldCircleStroke { get; set; } = 2f;
    public bool OnMapEnabled { get; set; } = true;
    public ILineStyle MapLineStyle { get; init; }
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 4f;

    public abstract IEnumerable<T> GetObjects();
    public virtual void PaintGroundExtra(T item) { }
    public virtual void PaintMapExtra(T item) { }

    public void PaintGround()
    {
        if (!OnGroundEnabled)
            return;

        foreach (var item in GetObjects())
        {
            LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
            PaintGroundExtra(item);
        }
    }

    public void PaintMap()
    {
        if (!OnMapEnabled)
            return;

        foreach (var item in GetObjects())
        {
            if (!Map.WorldToMapCoordinate(item.Coordinate, out var mapX, out var mapY))
                continue;

            MapLineStyle.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
            PaintMapExtra(item);
        }
    }

    public void AddDefaultGroundResources(IPlugin plugin)
    {
        Resources.Add(new BooleanFeatureResource
        {
            NameOf = nameof(OnGroundEnabled),
            DisplayText = plugin.Translate("on ground"),
            Getter = () => OnGroundEnabled,
            Setter = newValue => OnGroundEnabled = newValue
        });
        Resources.Add(new LineStyleFeatureResource { NameOf = nameof(LineStyle), DisplayText = plugin.LineStyle, LineStyle = LineStyle });
        Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(WorldCircleSize),
            DisplayText = plugin.Radius,
            Getter = () => WorldCircleSize,
            Setter = newValue => WorldCircleSize = newValue,
            MinValue = 0,
            MaxValue = 2
        });
        Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(WorldCircleStroke),
            DisplayText = plugin.Stroke,
            Getter = () => WorldCircleStroke,
            Setter = newValue => WorldCircleStroke = newValue,
            MinValue = 0,
            MaxValue = 10
        });
    }

    public void AddDefaultMapResources(IPlugin plugin)
    {
        Resources.Add(new BooleanFeatureResource
        {
            NameOf = nameof(OnMapEnabled),
            DisplayText = plugin.Translate("on map"),
            Getter = () => OnMapEnabled,
            Setter = newValue => OnMapEnabled = newValue
        });
        Resources.Add(new LineStyleFeatureResource
        {
            NameOf = nameof(MapLineStyle), DisplayText = plugin.MapLineStyle, LineStyle = MapLineStyle
        });
        Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(MapCircleSize),
            DisplayText = plugin.MapRadius,
            Getter = () => MapCircleSize,
            Setter = newValue => MapCircleSize = newValue,
            MinValue = 0,
            MaxValue = 20
        });
        Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(MapCircleStroke),
            DisplayText = plugin.MapStroke,
            Getter = () => MapCircleStroke,
            Setter = newValue => MapCircleStroke = newValue,
            MinValue = 0,
            MaxValue = 10
        });
    }
}