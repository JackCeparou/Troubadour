namespace T4.Plugins.Troubadour;

public interface IWorldFeature
{
    bool Enabled { get; set; }
    bool OnGroundEnabled { get; set; }
    bool OnMapEnabled { get; set; }

    void PaintGround();
    void PaintMap();
}

public abstract class WorldFeature<T> : Feature, IWorldFeature where T : ICommonActor
{
    public virtual bool OnGroundEnabled { get; set; } = true;
    public virtual ILineStyle LineStyle { get; init; }
    public virtual float WorldCircleSize { get; set; } = 0.5f;
    public virtual float WorldCircleStroke { get; set; } = 2f;
    public virtual ITexture WorldIconTexture { get; init; }
    public virtual float WorldIconSize { get; set; } = 2f;

    public virtual bool OnMapEnabled { get; set; } = true;
    public virtual ILineStyle MapLineStyle { get; init; }
    public virtual float MapCircleSize { get; set; } = 8f;
    public virtual float MapCircleStroke { get; set; } = 4f;
    public virtual ITexture MapIconTexture { get; init; }
    public virtual float MapIconSize { get; set; } = 2f;

    public abstract IEnumerable<T> GetWorldObjects();
    public virtual void PaintGroundBefore(T actor) { }
    public virtual void PaintGroundAfter(T actor) { }
    public virtual void PaintMapBefore(T actor, float mapX, float mapY) { }
    public virtual void PaintMapAfter(T actor, float mapX, float mapY) { }

    public virtual void PaintGround()
    {
        if (!Enabled || !OnGroundEnabled)
            return;

        foreach (var item in GetWorldObjects())
        {
            PaintGroundBefore(item);
            LineStyle?.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
            if (WorldIconTexture is not null)
            {
                var size = WorldIconSize;
                var x = item.Coordinate.ScreenX - (size / 2);
                var y = item.Coordinate.ScreenY - (size / 2);
                WorldIconTexture.Draw(x, y, size, size);
            }

            PaintGroundAfter(item);
        }
    }

    public virtual void PaintMap()
    {
        if (!Enabled || !OnMapEnabled)
            return;

        foreach (var item in GetWorldObjects())
        {
            if (!Map.WorldToMapCoordinate(item.Coordinate, out var mapX, out var mapY))
                continue;

            PaintMapBefore(item, mapX, mapY);
            MapLineStyle?.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
            if (MapIconTexture is not null)
            {
                var size = MapIconSize;
                var x = item.Coordinate.ScreenX - (size / 2);
                var y = item.Coordinate.ScreenY - (size / 2);
                MapIconTexture.Draw(x, y, size, size);
            }

            PaintMapAfter(item, mapX, mapY);
        }
    }

    public void AddDefaultGroundResources()
    {
        Resources.Add(new BooleanFeatureResource
        {
            NameOf = nameof(OnGroundEnabled),
            DisplayText = () => Translation.Translate(Plugin, "on ground"),
            Getter = () => OnGroundEnabled,
            Setter = newValue => OnGroundEnabled = newValue
        });
        Resources.Add(new LineStyleFeatureResource
        {
            NameOf = nameof(LineStyle), DisplayText = () => Translation.Translate(Plugin, "line style"), LineStyle = LineStyle
        });
        Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(WorldCircleSize),
            DisplayText = () => Translation.Translate(Plugin, "radius"),
            Getter = () => WorldCircleSize,
            Setter = newValue => WorldCircleSize = newValue,
            MinValue = 0,
            MaxValue = 2
        });
        Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(WorldCircleStroke),
            DisplayText = () => Translation.Translate(Plugin, "stroke"),
            Getter = () => WorldCircleStroke,
            Setter = newValue => WorldCircleStroke = newValue,
            MinValue = 0,
            MaxValue = 10
        });
    }

    public void AddDefaultMapResources()
    {
        Resources.Add(new BooleanFeatureResource
        {
            NameOf = nameof(OnMapEnabled),
            DisplayText = () => Translation.Translate(Plugin, "on map"),
            Getter = () => OnMapEnabled,
            Setter = newValue => OnMapEnabled = newValue
        });
        Resources.Add(new LineStyleFeatureResource
        {
            NameOf = nameof(MapLineStyle), DisplayText = () => Translation.Translate(Plugin, "map line style"), LineStyle = MapLineStyle
        });
        Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(MapCircleSize),
            DisplayText = () => Translation.Translate(Plugin, "map radius"),
            Getter = () => MapCircleSize,
            Setter = newValue => MapCircleSize = newValue,
            MinValue = 0,
            MaxValue = 20
        });
        Resources.Add(new FloatFeatureResource
        {
            NameOf = nameof(MapCircleStroke),
            DisplayText = () => Translation.Translate(Plugin, "map stroke"),
            Getter = () => MapCircleStroke,
            Setter = newValue => MapCircleStroke = newValue,
            MinValue = 0,
            MaxValue = 10
        });
    }
}