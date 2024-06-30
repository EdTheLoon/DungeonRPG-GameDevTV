using Godot;

public partial class EnemyIdleState : EnemyState
{
    protected override void EnterState()
    {
        // Play Idle animation.
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_IDLE);

        // Subscribe to ChaseArea BodyEntered signal.
        characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
    }

    protected override void ExitState()
    {
        characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Switch to the EnemyReturnState.
        characterNode.StateMachineNode.SwitchState<EnemyReturnState>();
    }
}
