namespace T4.Plugins.Troubadour;

public sealed class Cursed : JackPlugin, IGameWorldPainter
{
    public Feature OnMap { get; private set; }
    public Feature OnGround { get; private set; }

    public float MapCircleSize { get; set; } = 32f;
    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(255, 0, 0, 0);
    public IFillStyle MapFillStyle { get; } = Render.GetFillStyle(128, 255, 0, 0);

    public float GroundIconSize { get; set; } = 100f;
    public ITexture GroundIcon { get; } = Render.GetTexture(SupportedTextureId.UIBuffDebuff_3845477221);

    public Cursed()
    {
        Order = -1; // draw before chests
        Group = PluginCategory.Fight;
        Description = "Displays cursed chests and shrines on the map and ground.";
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                foreach (var actor in CursedStore.GetCursedActors())
                {
                    var offset = GroundIconSize / 2f;
                    GroundIcon.Draw(actor.Coordinate.ScreenX - offset, actor.Coordinate.ScreenY - offset, GroundIconSize, GroundIconSize);
                }

                break;
            case GameWorldLayer.Map when OnMap.Enabled:
                foreach (var actor in CursedStore.GetCursedActors())
                {
                    if (!Map.WorldToMapCoordinate(actor.Coordinate, out var mapX, out var mapY))
                        continue;

                    var size = MapCircleSize * 1.25f;
                    var offset = size / 2f;
                    if (actor.IsShrine())
                    {
                        Textures.ShrineIcon.Draw(mapX - offset, mapY - offset, size, size);
                    }

                    MapFillStyle.FillEllipse(mapX, mapY, offset, offset, false);
                    MapLineStyle.DrawEllipse(mapX, mapY, offset, offset, false);
                }

                break;
        }
    }

    public override void Load()
    {
        CursedStore.Init();
        OnMap = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnMap),
            DisplayName = () => Translation.Translate(this, "circle on map"),
            Resources = new List<AbstractFeatureResource>
            {
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleSize),
                    DisplayText = () => Translation.Translate(this, "radius"),
                    MinValue = 0,
                    MaxValue = 100,
                    Getter = () => MapCircleSize,
                    Setter = newValue => MapCircleSize = newValue
                },
                new FillStyleFeatureResource
                {
                    NameOf = nameof(MapFillStyle), DisplayText = () => Translation.Translate(this, "fill style"), FillStyle = MapFillStyle
                },
                new LineStyleFeatureResource
                {
                    NameOf = nameof(MapLineStyle), DisplayText = () => Translation.Translate(this, "line style"), LineStyle = MapLineStyle
                },
            }
        }.Register();

        OnGround = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnGround),
            DisplayName = () => Translation.Translate(this, "icon on ground"),
            Resources = new List<AbstractFeatureResource>
            {
                new FloatFeatureResource
                {
                    NameOf = nameof(GroundIconSize),
                    DisplayText = () => Translation.Translate(this, "icon size"),
                    MinValue = 0,
                    MaxValue = 200,
                    Getter = () => GroundIconSize,
                    Setter = newValue => GroundIconSize = newValue
                }
            }
        }.Register();
    }
}