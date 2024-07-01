using Godot;
using System;

public partial class Camera : Camera3D
{
    [Export] private Node target;
    [Export] private Vector3 positionFromTarget;
    public override void _Ready()
    {
        // Subscribe to the OnStartGame event
        GameEvents.OnStartGame += HandleOnStartGame;
    }

    private void HandleOnStartGame()
    {
        Reparent(target);
        Position = positionFromTarget;
    }

}
