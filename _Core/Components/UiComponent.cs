namespace T4.Plugins.Troubadour;

public abstract class UiComponent : IUiComponent
{
    public bool Enabled { get; set; } = true;

    public required IComponentPlacement Placement { get; init; }

    protected UiComponent()
    {
    }

    protected abstract void DrawImpl();

    public virtual void Draw()
    {
        if (!Enabled)
            return;

        Placement.Update();
        DrawImpl();
    }

    public void DrawOver() => Draw();
    public bool HitTest(float x, float y) => Placement.HitTest(x, y);
}