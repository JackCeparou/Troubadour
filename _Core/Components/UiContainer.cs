namespace T4.Plugins.Troubadour;

public abstract class UiContainer : IUiContainer
{
    public bool Enabled { get; set; } = true;
    public IComponentPlacement Placement { get; init; }
    public UiContainerDirection Direction { get; set; } = UiContainerDirection.Horizontal;
    public UiAlignment Alignment { get; set; } = UiAlignment.Start;
    public List<IUiComponent> Children { get; init; } = new();

    public void Draw()
    {
        if (!Enabled)
            return;

        Placement.Update();
        foreach (var child in Children)
        {
            child.Draw();
        }
    }

    public void DrawOver() => Draw();
    public bool HitTest(float x, float y) => Placement.HitTest(x, y);
}