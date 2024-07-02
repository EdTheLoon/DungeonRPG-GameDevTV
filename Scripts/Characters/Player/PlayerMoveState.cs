using Godot;

public partial class PlayerMoveState : PlayerState
{
    // Export the speed member so it can be edited in the editor
    [Export(PropertyHint.Range,"1.0,10")]
    protected float MovementSpeed { get; private set; } = 5;
    
    // Override the engine's Physics process and replace it with our own.
    public override void _PhysicsProcess(double delta)
    {
        // If player movement stops then switch to PlayerIdleState.
        if (characterNode.direction == Vector2.Zero)
        {
            characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
        }

        // Move the Player.
        characterNode.Velocity = new(characterNode.direction.X, 0, characterNode.direction.Y);
        characterNode.Velocity *= MovementSpeed;
        characterNode.MoveAndSlide();

        // Flip will flip the sprite horizontally to match movement direction.
        FlipFromInput();
    }

    // Override the engine's Input process so we can carry out our own input handling.
    public override void _Input(InputEvent @event)
    {
        CheckForAttackInput();

        // Determine if the input matches an action from our engine's Input Map, in this case, Dash.
        if (Input.IsActionJustPressed(GameConstants.ANIM_DASH))
        {
            // Use the StateMachine to switch to PlayerDashState.
            characterNode.StateMachineNode.SwitchState<PlayerDashState>();
        }
    }

    // Override PlayerState's EnterState method
    protected override void EnterState()
    {
        // Play the Move animation.
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);
    }
}
