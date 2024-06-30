using Godot;

public partial class PlayerIdleState : PlayerState
{
    // Override the engine's Physics process and replace it with our own.
    public override void _PhysicsProcess(double delta)
    {
        // If Player is moving
        if (characterNode.direction != Vector2.Zero)
        {
            // Use the StateMachine to switch to PlayerMoveState.
            characterNode.StateMachineNode.SwitchState<PlayerMoveState>();
        }
    }

        // Override the engine's Input process so we can carry out our own input handling.
    public override void _Input(InputEvent @event)
    {
        CheckForAttackInput();

        // Determine if the input matches an action from our engine's Input Map, in this case, Dash.
        if (Input.IsActionJustPressed(GameConstants.INPUT_DASH))
        {
            // Use the StateMachine to switch to PlayerDashState.
            characterNode.StateMachineNode.SwitchState<PlayerDashState>();
        }
    }

    // Override PlayerState's EnterState method
    protected override void EnterState()
    {
        // Let the original EnterState still carry out it's work.
        base.EnterState();

        // Play the Idle animation.
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_IDLE);
    }
}
