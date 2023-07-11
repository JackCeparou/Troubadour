namespace T4.Plugins.Troubadour;

public sealed class ResourceAuraGlobe : AuraGlobe
{
    public Dictionary<PlayerClassSnoId, IFillStyle> ResourceForegrounds { get; } = new()
    {
        { PlayerClassSnoId.Barbarian, Render.GetFillStyle(200, 15, 196, 205) },
        { PlayerClassSnoId.Druid, Render.GetFillStyle(200, 15, 196, 205) },
        { PlayerClassSnoId.Necromancer, Render.GetFillStyle(200, 15, 196, 205) },
        { PlayerClassSnoId.Rogue, Render.GetFillStyle(200, 15, 196, 205) },
        { PlayerClassSnoId.Sorcerer, Render.GetFillStyle(255, 15, 196, 205) },
    };

    public override IFillStyle GetFillStyle()
    {
        if (Game.MyPlayerActor is null)
            return ResourceForegrounds[PlayerClassSnoId.Sorcerer];

        return !ResourceForegrounds.TryGetValue(Game.MyPlayerActor.PlayerClassSno.SnoId, out var style)
            ? ResourceForegrounds[PlayerClassSnoId.Sorcerer]
            : style;
    }

    public override Func<float> GetCurrentValue { get; } = () => Game.MyPlayerActor?.PrimaryResourceCur ?? 666f;
    public override Func<float> GetMaxValue { get; } = () => Game.MyPlayerActor?.PrimaryResourceMax ?? 1000f;
}