using Godot;

public partial class PlayerAttackState : PlayerState
{
    // Combat combo counters
    private int comboCounter = 1;
    private int minComboCount = 1;
    private int maxComboCount = 2;

    protected override void EnterState()
    {
        // Play animation. Should be Attack1 or Attack2
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_ATTACK + comboCounter);

        // Subscribe to AnimationFinished signal.
        characterNode.AnimPlayerNode.AnimationFinished += HandleAnimationFinished;
    }

    protected override void ExitState()
    {
        // Unsubscribe from signals for cleanup.
        characterNode.AnimPlayerNode.AnimationFinished -= HandleAnimationFinished;
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
