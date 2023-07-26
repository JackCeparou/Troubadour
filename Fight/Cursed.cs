namespace T4.Plugins.Troubadour;

public sealed class Cursed : JackPlugin, IGameWorldPainter
{
    public Feature OnMap { get; }
    public Feature OnGround { get; }

    public float MapCircleSize { get; set; } = 32f;
    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(255, 0, 0, 0);
    public IFillStyle MapFillStyle { get; } = Render.GetFillStyle(128, 255, 0, 0);

    public float GroundIconSize { get; set; } = 100f;
    public ITexture GroundIcon { get; } = Render.GetTexture(SupportedTextureId.UIBuffDebuff_3845477221);

    public Cursed() : base(PluginCategory.Fight, "Displays cursed chests and shrines on the map and ground.")
    {
        Order = -1; // draw before default chests plugin
        OnMap = AddFeature(nameof(OnMap), "on map")
            .AddFloatResource(nameof(MapCircleSize), "radius", 0, 100, () => MapCircleSize, v => MapCircleSize = v)
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFillStyleResource(nameof(MapFillStyle), MapFillStyle, "fill style");
        OnGround = AddFeature(nameof(OnGround), "icon on ground")
            .AddFloatResource(nameof(GroundIconSize), "icon size", 0, 200, () => GroundIconSize, v => GroundIconSize = v);
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
                    if (!actor.Coordinate.IsOnMap)
                        continue;

                    var size = MapCircleSize * 1.25f;
                    var offset = size / 2f;
                    var mapX = actor.Coordinate.MapX;
                    var mapY = actor.Coordinate.MapY;
                    if (actor.ActorSno.SnoId == ActorSnoId.DE_CursedShrine_Debuff_Trigger)
                    {
                        Textures.ShrineIcon.Draw(mapX - offset, mapY - offset, size, size);
                    }

                    MapFillStyle.FillEllipse(mapX, mapY, offset, offset, false);
                    MapLineStyle.DrawEllipse(mapX, mapY, offset, offset, false);
                }

                break;
        }
    }
}