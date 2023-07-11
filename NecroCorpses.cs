namespace T4.Plugins.Troubadour;

public sealed class NecroCorpses : BasePlugin, IGameWorldPainter
{
    public Feature Config { get; set; }

    public bool SymbolEnabled { get; set; } = true;
    public IFont Font { get; } = Render.GetFont(255, 255, 0, 0, size: 16f);
    public bool GroundCircleEnabled { get; set; }
    public float GroundCircleSize { get; set; } = 0.5f;
    public ILineStyle GroundCircleStyle { get; } = Render.GetLineStyle(255, 255, 0, 0);

    public NecroCorpses()
    {
        EnabledByDefault = true;
    }

    public override string GetDescription()
        => Translation.Translate(this, "Display necromancer corpses on ground.");

    public override void Load()
    {
        Config = new Feature
        {
            Plugin = this,
            NameOf = nameof(Config),
            DisplayName = () => Translation.Translate(this, "corpses on ground"),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(SymbolEnabled),
                    DisplayText = () => Translation.Translate(this, "show symbol"),
                    Getter = () => SymbolEnabled,
                    Setter = newValue => SymbolEnabled = newValue
                },
                new FontFeatureResource { NameOf = nameof(Font), DisplayText = this.NormalFont, Font = Font },
                new BooleanFeatureResource
                {
                    NameOf = nameof(GroundCircleEnabled),
                    DisplayText = () => Translation.Translate(this, "show ground circle"),
                    Getter = () => GroundCircleEnabled,
                    Setter = newValue => GroundCircleEnabled = newValue
                },
                new FloatFeatureResource
                {
                    NameOf = nameof(GroundCircleSize),
                    DisplayText = this.Radius,
                    Getter = () => GroundCircleSize,
                    Setter = newValue => GroundCircleSize = newValue,
                    MinValue = 0,
                    MaxValue = 2
                },
                new LineStyleFeatureResource { NameOf = nameof(GroundCircleStyle), DisplayText = this.LineStyle, LineStyle = GroundCircleStyle },
            }
        }.Register();
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (layer != GameWorldLayer.Ground)
            return;
        if (!Host.DebugEnabled && Game.MyPlayerActor.PlayerClassSno.SnoId != PlayerClassSnoId.Necromancer)
            return;
        if (!SymbolEnabled && !GroundCircleEnabled)
            return;

        var corpses = Game.GizmoActors.Where(x => x.GizmoType == GizmoType.Necro_Corpse);
        foreach (var corpse in corpses)
        {
            if (SymbolEnabled)
            {
                const string symbol = "🕆"; // "☥";
                Render.WorldToScreenCoordinate(corpse.Coordinate, out var x, out var y);
                var tl = Font.GetTextLayout(symbol);
                tl.DrawText(x - (tl.Width / 2), y - (tl.Height / 2));
                if (Host.DebugEnabled)
                {
                    DrawDebugText(() => $"""
                        Untargetable {corpse.Untargetable}
                        IsStealthed {corpse.IsStealthed}
                        IsNPC {corpse.IsNPC}
                        IsDisabled {corpse.IsDisabled}
                        AttachedToACD {corpse.AttachedToACD}
                        IsLoading {corpse.IsLoading}
                        IsAnimTreeEnabled {corpse.IsAnimTreeEnabled}
                        IsSelected {corpse.IsSelected}
                    """, x, y + 20);
                }
            }

            if (GroundCircleEnabled)
            {
                GroundCircleStyle.DrawWorldEllipse(GroundCircleSize, -1, corpse.Coordinate);
            }
        }
    }
}