namespace T4.Plugins.Troubadour;

public sealed class HitpointsAuraGlobe : AuraGlobe
{
    private readonly IFillStyle _style = Render.GetFillStyle(200, 255, 0, 0);

    public override IFillStyle GetFillStyle()
        => _style;

    public override Func<float> GetCurrentValue { get; } = () => Game.MyPlayerActor?.HitpointsCur ?? 666f;
    public override Func<float> GetMaxValue { get; } = () => Game.MyPlayerActor?.HitpointsMax ?? 1000f;
}