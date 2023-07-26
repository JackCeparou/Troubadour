//*// all credits to swarm

namespace T4.Plugins.Troubadour;

public class AspectName : JackPlugin, IGameUserInterfacePainter
{
    public IFont FontOnCraftingPanel =
        Render.GetFont(255, 180, 180, 180, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered)
            .SetShadowColor(255, 0, 0, 0);

    public IFont FontOnCollectionPanel =
        Render.GetFont(255, 180, 180, 180, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered)
            .SetShadowColor(255, 0, 0, 0);

    public IFont InterestingFontOnCraftingPanel =
        Render.GetFont(255, 80, 255, 80, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered)
            .SetShadowColor(255, 0, 0, 0);

    public IFont InterestingFontOnCollectionPanel =
        Render.GetFont(255, 80, 255, 80, bold: true, wordWrap: true, shadowMode: FontShadowMode.Heavy, alignment: TextAlignment.Centered)
            .SetShadowColor(255, 0, 0, 0);

    public bool CleanupNameOnCraftingPanel { get; private set; } = true;
    public bool CleanupNameOnCollectionPanel { get; private set; } = true;

    public Feature NameOnCraftingPanel { get; }
    public Feature NameOnCollectionPanel { get; }

    public AspectName() : base(PluginCategory.Utility, "display aspect names on crafting panel/collection panel")
    {
        NameOnCraftingPanel = AddFeature(nameof(NameOnCraftingPanel), "name on crafting panel")
            .AddFontResource(nameof(FontOnCraftingPanel), FontOnCraftingPanel, "font")
            .AddFontResource(nameof(InterestingFontOnCraftingPanel), InterestingFontOnCraftingPanel, "interesting font")
            .AddBooleanResource(nameof(CleanupNameOnCraftingPanel), "cleanup aspect name",
                () => CleanupNameOnCraftingPanel,
                v => CleanupNameOnCraftingPanel = v);

        NameOnCollectionPanel = AddFeature(nameof(NameOnCollectionPanel), "name on collection panel")
            .AddFontResource(nameof(FontOnCollectionPanel), FontOnCollectionPanel, "font")
            .AddFontResource(nameof(InterestingFontOnCollectionPanel), InterestingFontOnCollectionPanel, "interesting font")
            .AddBooleanResource(nameof(CleanupNameOnCollectionPanel), "cleanup aspect name",
                () => CleanupNameOnCollectionPanel,
                v => CleanupNameOnCollectionPanel = v);
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.OverPanels)
            return;

        if (NameOnCollectionPanel.Enabled && UserInterface.CodexOfPowerCollectionControl.Visible)
        {
            foreach (var (aspect, control) in UserInterface.CodexOfPowerCollectionAspectControls)
            {
                if (!control.Visible || control.Height == 0 || control.Height < control.Width * 0.5f)
                    continue;

                var padding = control.Width * 0.1f;

                var text = CleanupNameOnCollectionPanel
                    ? aspect.AffixSno.CombineWithLocalized(null)
                    : aspect.NameLocalized;

                var font = Customization.InterestingAffixes.Contains(aspect.AffixSno)
                    ? InterestingFontOnCollectionPanel
                    : FontOnCollectionPanel;

                var tl = font.GetTextLayout(text, control.Width - (2 * padding));
                tl.DrawText(control.Left + padding, control.Top + ((control.Height - tl.Height) / 2));
            }
        }
        else if (NameOnCraftingPanel.Enabled && UserInterface.OccultistPanelControl.Visible)
        {
            foreach (var (aspect, control) in UserInterface.CodexOfPowerCraftingAspectControls)
            {
                if (!control.Visible || control.Height == 0 || control.Height < control.Width * 0.5f)
                    continue;

                var padding = control.Width * 0.1f;

                var text = CleanupNameOnCraftingPanel
                    ? aspect.AffixSno.CombineWithLocalized(null)
                    : aspect.NameLocalized;

                var font = Customization.InterestingAffixes.Contains(aspect.AffixSno)
                    ? InterestingFontOnCraftingPanel
                    : FontOnCraftingPanel;

                var tl = font.GetTextLayout(text, control.Width - (2 * padding));
                tl.DrawText(control.Left + padding, control.Top + ((control.Height - tl.Height) / 2));
            }
        }
    }
}