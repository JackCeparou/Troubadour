namespace T4.Plugins.Troubadour;

public interface IUiContainer : IUiComponent
{
    UiContainerDirection Direction { get; set; }
    UiAlignment Alignment { get; set; }
    List<IUiComponent> Children { get; }
}