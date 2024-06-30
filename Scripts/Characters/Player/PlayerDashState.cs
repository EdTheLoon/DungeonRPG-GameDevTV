using Godot;

public partial class PlayerDashState : PlayerState
{
    // Allows us to refer to the Timer node assigned in the engine.
    [Export] private Timer dashTimerNode;
    // Allow us to set the dash speed in engine. 
    // PropertyHint.Range allows us to hint at a suitable range
    [Export(PropertyHint.Range,"1,20")] private float dashSpeed = 10;

    // Override the engine's Ready process so we can do our own initialisation.
    public override void _Ready()
    {
        base._Ready();

        // Subscribe to the Timeout signal sent by the DashTimerNode
        dashTimerNode.Timeout += HandleDashTimeout;

        // We don't want the Physics and Input processes to run when the State is loaded.
        SetPhysicsProcess(false);
        SetProcessInput(false);
    }

    // Override the engine's Physics process and replace it with our own.
    public override void _PhysicsProcess(double delta)
    {
        // Move the Player character.
        characterNode.MoveAndSlide();

        // Flip will flip the sprite horizontally to match movement direction.
        characterNode.Flip();
    }

    // Override the engine's Notifications process so we can add to it.
    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_DASH);

        // Use Player's current movement to determine dash velocity
        characterNode.Velocity = new(characterNode.direction.X, 0, characterNode.direction.Y);

        // Check if Player was in PlayerIdleState when dash was pressed
        if (characterNode.Velocity == Vector3.Zero)
        {
            // If SpriteNode is flipped then Dash direction is Left, otherwise it is right.
            characterNode.Velocity = characterNode.SpriteNode.FlipH ? Godot.Vector3.Left : Vector3.Right;
        }

        // Apply the Dash speed multiplier
        characterNode.Velocity *= dashSpeed;
        dashTimerNode.Start();
    }

    // This method is called when the Timer runs out. Switches the Player's
    // state back to PlayerIdleState.
    private void HandleDashTimeout()
    {
        characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
    }
}
