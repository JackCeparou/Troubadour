namespace T4.Plugins.Troubadour;

public sealed class GameObserverService
{
    public float LastKnownWidth { get; private set; }
    public float LastKnownHeight { get; private set; }

    private uint _frame;

    public void CheckScreenSize()
    {
        if (_frame == Game.RenderFrame)
            return;

        _frame = Game.RenderFrame;
        if (Math.Abs(LastKnownWidth - Game.WindowWidth) <= 0.0001f && Math.Abs(LastKnownHeight - Game.WindowHeight) <= 0.0001f)
            return;

        LastKnownWidth = Game.WindowWidth;
        LastKnownHeight = Game.WindowHeight;

        // OnScreenResize();
        ScreenAnchors.Update();
    }
}