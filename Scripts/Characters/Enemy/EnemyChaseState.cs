using Godot;
using System;
using System.Linq;

public partial class EnemyChaseState : EnemyState
{
    // Export this field so that we can refer to the Timer assigned by the editor.
    [Export] private Timer updateTimerNode;

    // Reference to the Player
    private CharacterBody3D target;

    protected override void EnterState()
    {
        // Play Move animation and set the chase target to the Player.
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);
        // We have to Cast as CharacterBody3D here to work with the First method properly.
        target = characterNode.ChaseAreaNode.GetOverlappingBodies().First() as CharacterBody3D;

        // Subscribe to required signals
        updateTimerNode.Timeout += HandleUpdateTargetDestinationTimeout;
        characterNode.AttackAreaNode.BodyEntered += HandleAttackAreaBodyEntered;
        characterNode.ChaseAreaNode.BodyExited += HandleChaseAreaBodyExited;
    }

    protected override void ExitState()
    {
        // Unsubscribe from signals for cleanup.
        updateTimerNode.Timeout -= HandleUpdateTargetDestinationTimeout;
        characterNode.AttackAreaNode.BodyEntered -= HandleAttackAreaBodyEntered;
        characterNode.ChaseAreaNode.BodyExited -= HandleChaseAreaBodyExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        Move();
    }

    private void HandleUpdateTargetDestinationTimeout()
    {
        // Update destination to the Player's global position
        destination = target.GlobalPosition;
        characterNode.AgentNode.TargetPosition = destination;
    }

    private void HandleAttackAreaBodyEntered(Node3D body)
    {
        // Switch to EnemyAttackState
        characterNode.StateMachineNode.SwitchState<EnemyAttackState>();
    }

    private void HandleChaseAreaBodyExited(Node3D body)
    {
        // Switch to EnemyReturnState
        characterNode.StateMachineNode.SwitchState<EnemyReturnState>();
    }
}
