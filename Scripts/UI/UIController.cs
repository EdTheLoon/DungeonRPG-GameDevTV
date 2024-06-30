using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class UIController : Control
{
    private Dictionary<ContainerType, UIContainer> containers;

    public override void _Ready()
    {
        containers = GetChildren().Where(
            (element) => element is UIContainer
        ).Cast<UIContainer>().ToDictionary(
            (element) => element.container
        );

        containers[ContainerType.Start].Visible = true;
    }
}
