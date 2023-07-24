//*// all credits to swarm
namespace T4.Plugins.Troubadour;

public class AspectName : BasePlugin, IGameUserInterfacePainter
{
    public IFont FontOnCraftingPanel = Services.Render.GetFont(255, 180, 180, 180, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered).SetShadowColor(255, 0, 0, 0);
    public IFont FontOnCollectionPanel = Services.Render.GetFont(255, 180, 180, 180, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered).SetShadowColor(255, 0, 0, 0);

    public IFont InterestingFontOnCraftingPanel = Services.Render.GetFont(255, 80, 255, 80, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered).SetShadowColor(255, 0, 0, 0);
    public IFont InterestingFontOnCollectionPanel = Services.Render.GetFont(255, 80, 255, 80, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered).SetShadowColor(255, 0, 0, 0);

    public bool CleanupNameOnCraftingPanel { get; private set; } = true;
    public bool CleanupNameOnCollectionPanel { get; private set; } = true;

    public Feature NameOnCraftingPanel { get; private set; }
    public Feature NameOnCollectionPanel { get; private set; }

    public override string GetDescription() => Services.Translation.Translate(this, "display aspect names on crafting panel/collection panel");
    public override PluginCategory Category => PluginCategory.Utility;

    public override void Load()
    {
        base.Load();

        NameOnCraftingPanel = new Feature()
        {
            Plugin = this,
            NameOf = nameof(NameOnCraftingPanel),
            DisplayName = () => Services.Translation.Translate(this, "name on crafting panel"),
            Resources = new()
            {
                new FontFeatureResource(nameof(FontOnCraftingPanel), FontOnCraftingPanel,() => Services.Translation.Translate(this, "font")),
                new FontFeatureResource(nameof(InterestingFontOnCraftingPanel), InterestingFontOnCraftingPanel, () => Services.Translation.Translate(this, "interesting font")),
                new BooleanFeatureResource()
                {
                    NameOf = nameof(CleanupNameOnCraftingPanel),
                    DisplayText = () => Services.Translation.Translate(this, "cleanup aspect name"),
                    Getter = () => CleanupNameOnCraftingPanel,
                    Setter = newValue => CleanupNameOnCraftingPanel = newValue,
                },
            },
        };

        NameOnCollectionPanel = new Feature()
        {
            Plugin = this,
            NameOf = nameof(NameOnCollectionPanel),
            DisplayName = () => Services.Translation.Translate(this, "name on collection panel"),
            Resources = new()
            {
                new FontFeatureResource(nameof(FontOnCollectionPanel), FontOnCollectionPanel, () => Services.Translation.Translate(this, "font")),
                new FontFeatureResource(nameof(InterestingFontOnCollectionPanel), InterestingFontOnCollectionPanel, () => Services.Translation.Translate(this, "interesting font")),
                new BooleanFeatureResource()
                {
                    NameOf = nameof(CleanupNameOnCollectionPanel),
                    DisplayText = () => Services.Translation.Translate(this, "cleanup aspect name"),
                    Getter = () => CleanupNameOnCollectionPanel,
                    Setter = newValue => CleanupNameOnCollectionPanel = newValue,
                },
            },
        };

        Services.Customization.RegisterFeature(NameOnCraftingPanel);
        Services.Customization.RegisterFeature(NameOnCollectionPanel);
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.OverPanels)
            return;

        if (NameOnCollectionPanel.Enabled && Services.UserInterface.CodexOfPowerCollectionControl.Visible)
        {
            foreach (var (aspect, control) in Services.UserInterface.CodexOfPowerCollectionAspectControls)
            {
                if (!control.Visible || control.Height == 0 || control.Height < control.Width * 0.5f)
                    continue;

                var padding = control.Width * 0.1f;

                var text = CleanupNameOnCollectionPanel
                    ? aspect.AffixSno.CombineWithLocalized(null)
                    : aspect.NameLocalized;

                var font = Services.Customization.InterestingAffixes.Contains(aspect.AffixSno)
                    ? InterestingFontOnCollectionPanel
                    : FontOnCollectionPanel;

                var tl = font.GetTextLayout(text, control.Width - (2 * padding));
                tl.DrawText(control.Left + padding, control.Top + ((control.Height - tl.Height) / 2));
            }
        }
        else if (NameOnCraftingPanel.Enabled && Services.UserInterface.OccultistPanelControl.Visible)
        {
            foreach (var (aspect, control) in Services.UserInterface.CodexOfPowerCraftingAspectControls)
            {
                if (!control.Visible || control.Height == 0 || control.Height < control.Width * 0.5f)
                    continue;

                var padding = control.Width * 0.1f;

                var text = CleanupNameOnCraftingPanel
                    ? aspect.AffixSno.CombineWithLocalized(null)
                    : aspect.NameLocalized;

                var font = Services.Customization.InterestingAffixes.Contains(aspect.AffixSno)
                    ? InterestingFontOnCraftingPanel
                    : FontOnCraftingPanel;

                var tl = font.GetTextLayout(text, control.Width - (2 * padding));
                tl.DrawText(control.Left + padding, control.Top + ((control.Height - tl.Height) / 2));
            }
        }
    }
}
//*/