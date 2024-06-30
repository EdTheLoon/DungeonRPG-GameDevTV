using System;
using Godot;

public partial class PlayerAttackState : PlayerState
{

    // Reference to the combo timer in editor
    [Export] private Timer comboTimerNode;

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
        // Play animation. Should be Attack1 or Attack2
        // Uses -1 for blend.
        // Plays animation at 1.5x speed
        characterNode.AnimPlayerNode.Play(
            GameConstants.ANIM_ATTACK + comboCounter,
            -1,
            1.5f);

        // Subscribe to AnimationFinished signal.
        characterNode.AnimPlayerNode.AnimationFinished += HandleAnimationFinished;
    }

    protected override void ExitState()
    {
        // Unsubscribe from signals for cleanup.
        characterNode.AnimPlayerNode.AnimationFinished -= HandleAnimationFinished;
        comboTimerNode.Start();
    }

    private void HandleAnimationFinished(StringName animName)
    {
        // Increment combo counter and then wrap to keep within the limits.
        comboCounter++;
        comboCounter = Mathf.Wrap(comboCounter, minComboCount, maxComboCount+1);
        
        // Switch to PlayerIdleState
        characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
    }

}
