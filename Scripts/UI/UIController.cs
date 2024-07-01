using Godot;
using System.Linq;
using System.Collections.Generic;
using System;

public partial class UIController : Control
{
    private Dictionary<ContainerType, UIContainer> containers;
    private bool canPause = false;

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

        // Handle buttons being pressed
        containers[ContainerType.Start].ButtonNode.Pressed += HandleStartPressed;
        containers[ContainerType.Pause].ButtonNode.Pressed += HandlePausePressed;

        GameEvents.OnEndGame += HandleEndGame;
        GameEvents.OnVictory += HandleVictory;
    }

    private void HandlePausePressed()
    {
        GetTree().Paused = false;
        containers[ContainerType.Pause].Visible = false;
        containers[ContainerType.Stats].Visible = true;
    }


    public override void _Input(InputEvent @event)
    {
        if (!canPause) { return; }

        if (!Input.IsActionJustPressed(GameConstants.INPUT_PAUSE))
        {
            return;
        }
        
        containers[ContainerType.Stats].Visible = GetTree().Paused;
        GetTree().Paused = !GetTree().Paused;
        containers[ContainerType.Pause].Visible = GetTree().Paused;
    }

    private void HandleStartPressed()
    {
        // Unpause the entire SceneTree. Hide the Start screen UI. Show the Stats UI.
        GetTree().Paused = false;
        containers[ContainerType.Start].Visible = false;
        containers[ContainerType.Stats].Visible = true;

        // Raise the OnStartGame event
        GameEvents.RaiseStartGame();
        canPause = true;
    }

    private void HandleEndGame()
    {
        containers[ContainerType.Stats].Visible = false;
        containers[ContainerType.Defeat].Visible = true;
        canPause = false;
    }

    private void HandleVictory()
    {
        containers[ContainerType.Stats].Visible = false;
        containers[ContainerType.Victory].Visible = true;
        canPause = false;
        GetTree().Paused = true;
    }
}
