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
        // If player is not in attack range then we should check if they're in
        // chasing range. If they're not in chase range then we will return to patrolling.
        if (characterNode.AttackAreaNode.GetOverlappingBodies().Count == 0)
        {
            // Check whether the player is in chase range.
            if (characterNode.ChaseAreaNode.GetOverlappingBodies().Count == 0)
            {
                // Chase the player
                characterNode.StateMachineNode.SwitchState<EnemyReturnState>();
                return;
            }
            // Chase
            characterNode.StateMachineNode.SwitchState<EnemyChaseState>();
            return;
        }

        // Player is within attack range so keep attacking.
        Attack();
    }

    private void PerformHit() 
    {        
        characterNode.DisableHitbox(false);
    }
}
