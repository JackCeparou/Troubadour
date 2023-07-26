namespace T4.Plugins.Troubadour;

public sealed class NecroCorpses : JackPlugin, IGameWorldPainter
{
    public Feature Config { get; }

    public bool SymbolEnabled { get; set; } = true;
    public IFont Font { get; } = Render.GetFont(255, 255, 0, 0, size: 16f);
    public bool GroundCircleEnabled { get; set; }
    public float GroundCircleSize { get; set; } = 0.5f;
    public ILineStyle GroundCircleStyle { get; } = Render.GetLineStyle(255, 255, 0, 0);

    public NecroCorpses() : base(PluginCategory.Fight, "Display necromancer corpses on ground.")
    {
        Config = AddFeature(nameof(Config), "config")
            .AddBooleanResource(nameof(SymbolEnabled), "show symbol", () => SymbolEnabled, v => SymbolEnabled = v)
            .AddFontResource(nameof(Font), Font, "normal font")
            .AddBooleanResource(nameof(GroundCircleEnabled), "show ground circle", () => GroundCircleEnabled, v => GroundCircleEnabled = v)
            .AddFloatResource(nameof(GroundCircleSize), "radius",
                0, 2, () => GroundCircleSize, v => GroundCircleSize = v)
            .AddLineStyleResource(nameof(GroundCircleStyle), GroundCircleStyle, "line style");
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
            }

            if (GroundCircleEnabled)
            {
                GroundCircleStyle.DrawWorldEllipse(GroundCircleSize, -1, corpse.Coordinate);
            }
        }
    }
}