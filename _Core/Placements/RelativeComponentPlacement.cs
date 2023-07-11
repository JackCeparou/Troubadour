namespace T4.Plugins.Troubadour;

public sealed class RelativeComponentPlacement : IComponentPlacement
{
    public float Left { get; set; }
    public float Top { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }

    public float RelativeLeft { get; set; }
    public float RelativeTop { get; set; }
    public IComponentPlacement RelativeTo { get; set; }
    public UiAnchorPosition Anchor { get; set; }

    public Action<RelativeComponentPlacement> UpdateSize { get; set; }
    public Action<RelativeComponentPlacement> UpdateRelativePosition { get; set; }

    public void Update()
    {
        UpdateSize?.Invoke(this);
        UpdateRelativePosition?.Invoke(this);

        switch (Anchor)
        {
            case UiAnchorPosition.TopLeft:
            case UiAnchorPosition.TopCenter:
            case UiAnchorPosition.TopRight:
                Top = RelativeTo.Top + RelativeTop;
                break;
            case UiAnchorPosition.LeftCenter:
            case UiAnchorPosition.Center:
            case UiAnchorPosition.RightCenter:
                Top = RelativeTo.Top + RelativeTop - (Height / 2);
                break;
            case UiAnchorPosition.BottomLeft:
            case UiAnchorPosition.BottomCenter:
            case UiAnchorPosition.BottomRight:
                Top = RelativeTo.Top + RelativeTop - Height;
                break;
        }

        switch (Anchor)
        {
            case UiAnchorPosition.TopLeft:
            case UiAnchorPosition.LeftCenter:
            case UiAnchorPosition.BottomLeft:
                Left = RelativeTo.Left + RelativeLeft;
                break;
            case UiAnchorPosition.TopCenter:
            case UiAnchorPosition.Center:
            case UiAnchorPosition.BottomCenter:
                Left = RelativeTo.Left + RelativeLeft - (Width / 2);
                break;
            case UiAnchorPosition.TopRight:
            case UiAnchorPosition.RightCenter:
            case UiAnchorPosition.BottomRight:
                Left = RelativeTo.Left + RelativeLeft - Width;
                break;
        }
    }
}