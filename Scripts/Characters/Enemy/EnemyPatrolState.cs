using System;
using Godot;

public partial class EnemyPatrolState : EnemyState
{
    // Export this field so that we can refer to the Timer assigned by the editor.
    [Export] private Timer idleTimerNode;

    // Make our wait times configurable in the editor
    [Export(PropertyHint.Range, "0,10,0.5")]
    private float minIdleTime = 0;
    [Export(PropertyHint.Range, "1,20,0.5")]
    private float maxIdleTime = 4;

    protected override void EnterState()
    {
        // Play Move animation
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);

        // Move to the second point in our path. We use the second point here
        // because this code block is only expected to execute when the enemy
        // reaches the first point of the Path in EnemyReturnState.
        characterNode.pointIndex++;
        destination = GetPointGlobalPosition(characterNode.pointIndex);
        characterNode.AgentNode.TargetPosition = destination;

        // Subscribe to signals.
        characterNode.AgentNode.NavigationFinished += HandleNavigationFinished;
        characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
        idleTimerNode.Timeout += HandleTimeout;
    }

    protected override void ExitState()
    {
        // Unsubscribe from signals. This is required for proper signal cleanup.
        characterNode.AgentNode.NavigationFinished -= HandleNavigationFinished;
        characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
        idleTimerNode.Timeout -= HandleTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (idleTimerNode.IsStopped()) // Only move when the idle Timer is not running.
        {
            Move();
        }
    }

    private void HandleNavigationFinished()
    {
        // Play Idle animation and wait for a random period of time between the min/max IdleTime.
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_IDLE);        
        RandomNumberGenerator rng = new();
        idleTimerNode.WaitTime = rng.RandfRange(minIdleTime, maxIdleTime);

        // Subscribe to Timeout signal and start the timer.
        idleTimerNode.Start();
    }

    private void HandleTimeout()
    {
        // Update next target navigation point in the index
        characterNode.pointIndex++;        
        characterNode.pointIndex = Mathf.Wrap(characterNode.pointIndex, 0, characterNode.PathNode.Curve.PointCount);

        // Play animation and move towards the point.
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_MOVE);
        destination = GetPointGlobalPosition(characterNode.pointIndex);
        characterNode.AgentNode.TargetPosition = destination;
    }

}
