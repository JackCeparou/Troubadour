//*

namespace T4.Plugins.Troubadour;

public class Auras : BasePlugin, IGameUserInterfacePainter, IMenuUserInterfacePainter
{
    public Feature Config { get; private set; }
    // public Feature Hitpoints { get; private set; }
    // public Feature PrimaryResource { get; private set; }

    public bool ShowInTown { get; private set; } = true;

    public HitpointsAuraGlobe HitpointsGlobe { get; set; }
    public ResourceAuraGlobe ResourceGlobe { get; set; }

    public Auras()
    {
        EnabledByDefault = IsDevSession;
    }

    public void PaintMenuUserInterface()
    {
        if (!IsDevSession)
            return;

        GameObserver.CheckScreenSize();
        HitpointsGlobe.Draw();
        ResourceGlobe.Draw();
    }

    //TODO: translations when ready to enable by default
    public override string GetDescription() => "Displays information around the player:\nEXPERIMENTAL / IN DEVELOPMENT";

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.BeforeClip)
            return;
        if (!ShowInTown && IsInTown)
            return;

        GameObserver.CheckScreenSize();
        HitpointsGlobe.Draw();
        ResourceGlobe.Draw();
    }

    public override void Load()
    {
        HitpointsGlobe = new HitpointsAuraGlobe
        {
            Scale = 0.8f,
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
            Scale = 0.8f,
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
                    DisplayText = this.Translate("show in town"),
                    Getter = () => ShowInTown,
                    Setter = newValue => ShowInTown = newValue,
                },
            },
        }.Register();
    }
}
//*/