﻿namespace T4.Plugins.Troubadour;

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
        EnabledByDefault = false;
    }

    public void PaintMenuUserInterface()
    {
        if (!Host.DebugEnabled && !Debug.IsDeveloper)
            return;

        GameObserver.CheckScreenSize();
        HitpointsGlobe.Draw();
        ResourceGlobe.Draw();
    }

    public override PluginCategory Category
        => PluginCategory.ActionBar;

    public override string GetDescription()
        => Translation.TranslateExperimentalPlugin(this, "displays information around the player");

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