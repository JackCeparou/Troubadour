namespace T4.Plugins.Troubadour;

public sealed class Auras : JackPlugin, IGameUserInterfacePainter, IMenuUserInterfacePainter
{
    public Feature Config { get; private set; }
    // public Feature Hitpoints { get; private set; }
    // public Feature PrimaryResource { get; private set; }

    public bool ShowInTown { get; private set; } = true;

    public HitpointsAuraGlobe HitpointsGlobe { get; set; }
    public ResourceAuraGlobe ResourceGlobe { get; set; }

    public Auras() : base(PluginCategory.ActionBar, "displays information around the player")
    {
        EnabledByDefault = false;
        HitpointsGlobe = new HitpointsAuraGlobe
        {
            Scale = 1f,
            Placement = new RelativeComponentPlacement
            {
                RelativeTo = ScreenAnchors.PlayerFeet,
                Width = Textures.GlobeForeground.Meta.Width,
                Height = Textures.GlobeForeground.Meta.Height,
                RelativeLeft = -75,
                RelativeTop = -25,
                Anchor = UiAnchorPosition.TopRight,
            }
        };
        ResourceGlobe = new ResourceAuraGlobe
        {
            Scale = 1f,
            Placement = new RelativeComponentPlacement
            {
                RelativeTo = ScreenAnchors.PlayerFeet,
                Width = Textures.GlobeForeground.Meta.Width,
                Height = Textures.GlobeForeground.Meta.Height,
                RelativeLeft = 75,
                RelativeTop = -25,
                Anchor = UiAnchorPosition.TopLeft,
            }
        };
        Config = AddFeature(nameof(Config), "config")
            .AddBooleanResource(nameof(ShowInTown), "show in town", () => ShowInTown, v => ShowInTown = v);
    }

    public void PaintMenuUserInterface()
    {
        if (!Host.DebugEnabled && !Debug.IsDeveloper)
            return;

        GameObserver.CheckScreenSize();
        HitpointsGlobe.Draw();
        ResourceGlobe.Draw();
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.BeforeClip)
            return;
        if (!ShowInTown && Game.MyPlayer.LevelAreaSno.IsTown)
            return;

        GameObserver.CheckScreenSize();
        HitpointsGlobe.Draw();
        ResourceGlobe.Draw();
    }
}