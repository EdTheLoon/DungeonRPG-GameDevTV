using Godot;

public partial class EnemyReturnState : EnemyState
{

    public override void _Ready()
    {
        // Carry out the parent class Ready method.
        base._Ready();

        // Set navigation target to the last index we were using.
        destination = GetPointGlobalPosition(characterNode.pointIndex);
    }

    protected override void EnterState()
    {
        // Play Move animation and set NavigationAgent's target position
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);
        destination = GetPointGlobalPosition(characterNode.pointIndex);
        characterNode.AgentNode.TargetPosition = destination;
        
        // Subscribe to ChaseArea BodyEntered signal.
        characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
    }

    protected override void ExitState()
    {
        characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        // If we have reached the start of our patrol Path then switch to EnemyPatrolState
        if (characterNode.AgentNode.IsNavigationFinished()) 
        {
            characterNode.StateMachineNode.SwitchState<EnemyPatrolState>();
            return;
        }

        Move();
    }
}
