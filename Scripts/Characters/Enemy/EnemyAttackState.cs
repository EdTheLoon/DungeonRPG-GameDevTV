using System.Linq;
using Godot;

public partial class EnemyAttackState : EnemyState
{
    // Export this field so that we can refer to the Timer assigned by the editor.
    [Export] private Timer attackTimerNode;

    // Make our wait times configurable in the editor
    [Export(PropertyHint.Range, "0.5,5,0.5")]
    private float minWaitTime = 1;
    [Export(PropertyHint.Range, "1,10,0.5")]
    private float maxWaitTime = 2;

    protected override void EnterState()
    {
        // Subscribe to signals
        characterNode.AnimPlayerNode.AnimationFinished += HandleAnimationFinished;
        attackTimerNode.Timeout += HandleAttackTimerTimeout;
        Attack();
    }


    protected override void ExitState()
    {
        // Unsubscribe from signals.
        attackTimerNode.Timeout -= HandleAttackTimerTimeout;
        characterNode.AnimPlayerNode.AnimationFinished -= HandleAnimationFinished;
    }

    private void Attack()
    {
        // Determine attack direction for hitbox        
        Node3D target = characterNode.AttackAreaNode.GetOverlappingBodies().FirstOrDefault();
        Vector3 targetPosition = characterNode.GlobalPosition.DirectionTo(target.GlobalPosition);
        // Move the hitbox and use a multiplier to move it further out
        float multiplier = 1.5f;
        characterNode.HitboxNode.Position = targetPosition * multiplier;

        // Flip sprite if Player is in different direction
        characterNode.SpriteNode.FlipH = targetPosition.X < 0;

        // Random wait time after attacking
        RandomNumberGenerator rng = new();
        attackTimerNode.WaitTime = rng.RandfRange(minWaitTime, maxWaitTime);
        attackTimerNode.Start();

        // Plays the Attack animation
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_ATTACK);
    }

    private void HandleAnimationFinished(StringName animName)
    {
        // Runs once an animation is finished. 
        // Note: Because Idle animation is looping it never emits this signal.
        // This signal will only be emitted at the end of the attack animation
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_IDLE);
        characterNode.DisableHitbox(true);
    }

    private void HandleAttackTimerTimeout()
    {
        // Check whether player is in attack range, if so then attack.
        if (characterNode.AttackAreaNode.HasOverlappingBodies()) {
            Attack();
            return;
        }

        // Check whether the player is in chase range. If so, then chase.
        if (characterNode.ChaseAreaNode.HasOverlappingBodies())
        {
            characterNode.StateMachineNode.SwitchState<EnemyChaseState>();
            return;
        }

        // Return to patrol
        characterNode.StateMachineNode.SwitchState<EnemyPatrolState>();
    }

    private void PerformHit() 
    {        
        characterNode.DisableHitbox(false);
    }
}
