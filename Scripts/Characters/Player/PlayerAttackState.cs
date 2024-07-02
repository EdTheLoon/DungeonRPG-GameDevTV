using System;
using Godot;

public partial class PlayerAttackState : PlayerState
{
    [Export] private PackedScene lightningScene;    // Reference to the lightning scene.
    [Export] private Timer comboTimerNode;          // Reference to the combo timer in editor.

    // Combat combo counters
    private int comboCounter = 1;
    private int minComboCount = 1;
    private int maxComboCount = 2;

    public override void _Ready()
    {
        base._Ready();

        // Subscribe to signals
        // Use a lambda function so that we don't have to type unnecessary code
        comboTimerNode.Timeout += () => comboCounter = 1;
    }


    protected override void EnterState()
    {
        // Flip our sprite to match the attack direction being held. 
        // This accounts for a bug where we can't turn around if an enemy is blocking
        // our back because of a collision.
        FlipSprite();

        // Play animation. Should be Attack1 or Attack2
        // Uses -1 for blend.
        // Plays animation at 1.5x speed
        characterNode.AnimPlayerNode.Play(
            GameConstants.ANIM_ATTACK + comboCounter,
            -1,
            1.5f);

        // Subscribe to AnimationFinished signal.
        characterNode.AnimPlayerNode.AnimationFinished += HandleAnimationFinished;
        characterNode.HitboxNode.BodyEntered += HandleBodyEntered;
    }

    protected override void ExitState()
    {
        // Unsubscribe from signals for cleanup.
        characterNode.AnimPlayerNode.AnimationFinished -= HandleAnimationFinished;
        characterNode.HitboxNode.BodyEntered -= HandleBodyEntered;
        comboTimerNode.Start();
    }

    private void HandleAnimationFinished(StringName animName)
    {
        // Increment combo counter and then wrap to keep within the limits.
        comboCounter++;
        comboCounter = Mathf.Wrap(comboCounter, minComboCount, maxComboCount + 1);
        
        // Switch to PlayerIdleState and disable hitbox
        characterNode.DisableHitbox(true);
        characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
    }

    private void HandleBodyEntered(Node3D body)
    {
        if (comboCounter != maxComboCount) { return ; }
        Node3D lightning = lightningScene.Instantiate<Node3D>();
        GetTree().CurrentScene.AddChild(lightning);
        lightning.GlobalPosition = body.GlobalPosition;
    }

    private void PerformHit() 
    {
        // Use ternary operator to determine which position the hitbox should be in.
        Vector3 newHitboxPosition = characterNode.SpriteNode.FlipH ? Vector3.Left : Vector3.Right;
        float distanceMultiplier = 1.25f;
        characterNode.HitboxNode.Position = newHitboxPosition * distanceMultiplier;

        // Enable the hitbox
        characterNode.DisableHitbox(false);
    }
}
