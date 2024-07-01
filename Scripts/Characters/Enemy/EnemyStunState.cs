using System;
using Godot;

public partial class EnemyStunState : EnemyState
{
    protected override void EnterState()
    {
        base.EnterState();

        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_STUN);
        characterNode.AnimPlayerNode.AnimationFinished += HandleAnimationFinished;
    }

    protected override void ExitState()
    {
        characterNode.AnimPlayerNode.AnimationFinished -= HandleAnimationFinished;
    }

    private void HandleAnimationFinished(StringName animName)
    {
        // Check whether player is in attack range, if so then attack.
        if (characterNode.AttackAreaNode.HasOverlappingBodies()) {
            characterNode.StateMachineNode.SwitchState<EnemyAttackState>();
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

}