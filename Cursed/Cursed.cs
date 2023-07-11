namespace T4.Plugins.Troubadour;

public sealed class Cursed : BasePlugin, IGameWorldPainter
{
    public Feature OnMap { get; private set; }
    public Feature OnGround { get; private set; }

    public float MapCircleSize { get; set; } = 32f;
    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(255, 0, 0, 0);
    public IFillStyle MapFillStyle { get; } = Render.GetFillStyle(128, 255, 0, 0);

    public float GroundIconSize { get; set; } = 100f;
    public ITexture GroundIcon { get; } = Render.GetTexture(SupportedTextureId.UIBuffDebuff_078);

    public Cursed()
    {
        EnabledByDefault = true;
        Order = -1; // draw before chests
    }

    public override string GetDescription()
        => Translation.Translate(this, "Displays cursed chests and shrines on the map and ground.");

    public void PaintGameWorld(GameWorldLayer layer)
    {
        switch (layer)
        {
            case GameWorldLayer.Ground when OnGround.Enabled:
                foreach (var actor in CursedStore.GetCursedActors())
                {
                    var offset = GroundIconSize / 2f;
                    Render.WorldToScreenCoordinate(actor.Coordinate, out var worldX, out var worldY);
                    GroundIcon.Draw(worldX - offset, worldY - offset, GroundIconSize, GroundIconSize);
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
            DisplayName = this.Translate("circle on map"),
            Resources = new List<AbstractFeatureResource>
            {
                new FloatFeatureResource
                {
                    NameOf = nameof(MapCircleSize),
                    DisplayText = this.Radius,
                    MinValue = 0,
                    MaxValue = 100,
                    Getter = () => MapCircleSize,
                    Setter = newValue => MapCircleSize = newValue
                },
                new FillStyleFeatureResource
                {
                    NameOf = nameof(MapFillStyle), DisplayText = this.FillStyle, FillStyle = MapFillStyle
                },
                new LineStyleFeatureResource
                {
                    NameOf = nameof(MapLineStyle), DisplayText = this.LineStyle, LineStyle = MapLineStyle
                },
            }
        }.Register();

        OnGround = new Feature
        {
            Plugin = this,
            NameOf = nameof(OnGround),
            DisplayName = this.Translate("icon on ground"),
            Resources = new List<AbstractFeatureResource>
            {
                new FloatFeatureResource
                {
                    NameOf = nameof(GroundIconSize),
                    DisplayText = this.IconSize,
                    MinValue = 0,
                    MaxValue = 200,
                    Getter = () => GroundIconSize,
                    Setter = newValue => GroundIconSize = newValue
                }
            }
        }.Register();
    }
}