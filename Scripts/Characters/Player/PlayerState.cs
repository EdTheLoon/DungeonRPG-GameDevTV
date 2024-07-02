// Make this an abstract class so that it is not accidentally instanced.
using System;
using Godot;

public abstract partial class PlayerState : CharacterState
{
    public override void _Ready()
    {
        // Allow parent to execute its Ready method.
        base._Ready();

        characterNode.GetStatResource(Stat.Health).OnZero += HandleZeroHealth;
    }

    protected void CheckForAttackInput()
    {
        if (Input.IsActionJustPressed(GameConstants.INPUT_ATTACK))
        {
            characterNode.StateMachineNode.SwitchState<PlayerAttackState>();
        }
    }

    protected void FlipFromInput()
    {
        // If player direction equals zero then return
        if (characterNode.direction.X == 0) { return; }
        characterNode.SpriteNode.FlipH = characterNode.direction.X < 0;
    }

    /// <summary>
    /// This method is added to a StatResource.OnZero so that it can be called
    /// when the Enemy's Health falls to zero.
    /// </summary>
    private void HandleZeroHealth()
    {
        characterNode.StateMachineNode.SwitchState<PlayerDeathState>();
    }
}