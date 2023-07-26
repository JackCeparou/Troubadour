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

    public abstract IEnumerable<T> GetWorldActors();
    public virtual void PaintGroundBefore(T actor) { }
    public virtual void PaintGroundAfter(T actor) { }
    public virtual void PaintMapBefore(T actor, float mapX, float mapY) { }
    public virtual void PaintMapAfter(T actor, float mapX, float mapY) { }

    public virtual void PaintGround()
    {
        if (!Enabled || !OnGroundEnabled)
            return;

        foreach (var actor in GetWorldActors())
        {
            PaintGroundBefore(actor);
            LineStyle?.DrawWorldEllipse(WorldCircleSize, -1, actor.Coordinate, strokeWidthCorrection: WorldCircleStroke);
            if (WorldIconTexture is not null)
            {
                var size = WorldIconSize;
                var x = actor.Coordinate.ScreenX - (size / 2);
                var y = actor.Coordinate.ScreenY - (size / 2);
                WorldIconTexture.Draw(x, y, size, size);
            }

            PaintGroundAfter(actor);
        }
    }

    public virtual void PaintMap()
    {
        if (!Enabled || !OnMapEnabled)
            return;

        foreach (var item in GetWorldActors())
        {
            if (!Map.WorldToMapCoordinate(item.Coordinate, out var mapX, out var mapY))
                continue;

            PaintMapBefore(item, mapX, mapY);
            MapLineStyle?.DrawEllipse(mapX, mapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
            if (MapIconTexture is not null)
            {
                var size = MapIconSize;
                var x = item.Coordinate.MapX - (size / 2);
                var y = item.Coordinate.MapY - (size / 2);
                MapIconTexture.Draw(x, y, size, size);
            }

            PaintMapAfter(item, mapX, mapY);
        }
    }

    public void AddDefaultGroundResources()
    {
        AddBooleanResource(nameof(OnGroundEnabled), "on ground", () => OnGroundEnabled, v => OnGroundEnabled = v);
        AddLineStyleResource(nameof(LineStyle), LineStyle, "line style");
        AddFloatResource(nameof(WorldCircleSize), "radius", 0, 2, () => WorldCircleSize, v => WorldCircleSize = v);
        AddFloatResource(nameof(WorldCircleStroke), "stroke", 0, 10, () => WorldCircleStroke, v => WorldCircleStroke = v);
    }

    public void AddDefaultMapResources()
    {
        AddBooleanResource(nameof(OnMapEnabled), "on map", () => OnMapEnabled, v => OnMapEnabled = v);
        AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style");
        AddFloatResource(nameof(MapCircleSize), "map radius", 0, 20, () => MapCircleSize, v => MapCircleSize = v);
        AddFloatResource(nameof(MapCircleStroke), "map stroke", 0, 10, () => MapCircleStroke, v => MapCircleStroke = v);
    }
}