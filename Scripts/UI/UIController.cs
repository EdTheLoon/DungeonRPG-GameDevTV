using Godot;
using System.Linq;
using System.Collections.Generic;
using System;

public partial class UIController : Control
{
    private Dictionary<ContainerType, UIContainer> containers;

    public override void _Ready()
    {
        // Get a reference of all child nodes that are of type UIContainer.
        containers = GetChildren().Where(
            (element) => element is UIContainer
        ).Cast<UIContainer>().ToDictionary(
            (element) => element.container
        );

        // Show the Start UI
        containers[ContainerType.Start].Visible = true;

        // Handle the Start button being pressed
        containers[ContainerType.Start].ButtonNode.Pressed += HandleStartPressed;
    }

    private void HandleStartPressed()
    {
        GetTree().Paused = false;
        containers[ContainerType.Start].Visible = false;
    }

}
