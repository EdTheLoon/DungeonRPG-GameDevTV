using Godot;
using System;

public partial class TreasureChest : StaticBody3D
{
    [Export] private Area3D areaNode;       // Area3D node for the player detection area.
    [Export] private Sprite3D spritenode;   // Sprite3D node for the interaction icon.
    [Export]private RewardResource reward;  // Use our custom RewardResource.

    public override void _Ready()
    {
        // Subscribe to the event and make the icon visible/invisible.
        areaNode.BodyEntered += HandleBodyEntered;
        areaNode.BodyExited += HandleBodyExited;

        // Disable engine processes.
        SetProcessInput(false);
        SetPhysicsProcess(false);
    }

    public override void _Input(InputEvent @event)
    {
        // If the input is not INPUT_INTERACT then return.
        if (!Input.IsActionJustPressed(GameConstants.INPUT_INTERACT)) { return; }
        interact();
    }

    private void HandleBodyEntered(Node3D body)
    {
        // Make sprite visible and process input
        spritenode.Visible = true;
        SetProcessInput(true);
    }

    private void HandleBodyExited(Node3D body)
    {
        // Make sprite invisible and disable input
        spritenode.Visible = false;
        SetProcessInput(false);
    }

    /// <summary>
    /// Implements the actions to take when the chest is interacted with.
    /// </summary>
    private void interact() 
    {        
        areaNode.Monitoring = false;
        spritenode.Visible = false;
        GameEvents.RaiseReward(reward);
        SetProcessInput(false);
    }
}
