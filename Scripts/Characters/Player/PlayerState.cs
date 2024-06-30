// Make this an abstract class so that it is not accidentally instanced.
using Godot;

public abstract partial class PlayerState : CharacterState
{    
    protected void CheckForAttackInput()
    {
        if (Input.IsActionJustPressed(GameConstants.INPUT_ATTACK))
        {
            characterNode.StateMachineNode.SwitchState<PlayerAttackState>();
        }
    }
}