using System;
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
        attackTimerNode.Timeout += HandleAttackTimerTimeout;
        Attack();
    }


    protected override void ExitState()
    {
        attackTimerNode.Timeout -= HandleAttackTimerTimeout;
    }

    private void Attack()
    {
        RandomNumberGenerator rng = new();        
        attackTimerNode.WaitTime = rng.RandfRange(minWaitTime, maxWaitTime);
        attackTimerNode.Start();
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_ATTACK);
    }

    private void HandleAttackTimerTimeout()
    {
        // If player is not in attack range then we should check if they're in
        // chasing range. If they're not in chase range then we will return to patrolling.
        if (characterNode.AttackAreaNode.GetOverlappingBodies().Count == 0) {
            // Check whether the player is in chase range.
            if (characterNode.ChaseAreaNode.GetOverlappingBodies().Count == 0) {
                // Chase the player
                characterNode.StateMachineNode.SwitchState<EnemyReturnState>();
                return;
            }
            // Attack
            characterNode.StateMachineNode.SwitchState<EnemyChaseState>();
            return;
        }
        
        // Player is within attack range so keep attacking.
        Attack();
    }
}
