namespace T4.Plugins.Troubadour;

public class Auras : JackPlugin, IGameUserInterfacePainter, IMenuUserInterfacePainter
{
    public Feature Config { get; private set; }
    // public Feature Hitpoints { get; private set; }
    // public Feature PrimaryResource { get; private set; }

    public bool ShowInTown { get; private set; } = true;

    public HitpointsAuraGlobe HitpointsGlobe { get; set; }
    public ResourceAuraGlobe ResourceGlobe { get; set; }

    public Auras()
    {
        EnabledByDefault = false;
        TroubadourExperiment = true;
        Group = PluginCategory.ActionBar;
        Description = "Displays information around the player";
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

    public override void Load()
    {
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
        Config = new Feature
        {
            Plugin = this,
            NameOf = nameof(Config),
            DisplayName = () => Translation.Translate(this, nameof(Config)),
            Resources = new()
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowInTown),
                    DisplayText = () => Translation.Translate(this, "show in town"),
                    Getter = () => ShowInTown,
                    Setter = newValue => ShowInTown = newValue,
                },
            },
        }.Register();
    }
}