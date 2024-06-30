using System;
using Godot;

public abstract partial class EnemyState : CharacterState
{  
    // Export the speed member so it can be edited in the editor
    [Export(PropertyHint.Range,"1.0,10")]
    protected float MovementSpeed { get; private set; } = 2.0f;

    protected Vector3 destination;

    public override void _Ready()
    {
        // Allow parent class to execute its Ready method
        base._Ready();

        characterNode.GetStatResource(Stat.Health).OnZero += HandleZeroHealth;
    }

    /// <summary>
    /// Returns the GlobalPosition of a Point in a Path.
    /// </summary>
    /// <param name="index">The index of the Point in the Path.</param>
    /// <returns>Returns a Vector3 containing the GlobalPosition.</returns>
    protected Vector3 GetPointGlobalPosition(int index)
    {
        Vector3 localPos = characterNode.PathNode.Curve.GetPointPosition(index);
        Vector3 globalPos = characterNode.PathNode.GlobalPosition;
        return globalPos + localPos;        
    }

    /// <summary>
    /// Moves the Enemy towards its destination.
    /// </summary>
    protected void Move() {
        // Update the NavigationAgent and get a walkable destination.
        destination = characterNode.AgentNode.GetNextPathPosition();

        // Move towards the destination and flip the sprite if needed.
        characterNode.Velocity = characterNode.GlobalPosition.DirectionTo(destination);
        characterNode.Velocity *= MovementSpeed;
        characterNode.MoveAndSlide();
        characterNode.Flip();
    }

    /// <summary>
    /// The signal handler for the ChaseArea being entered.
    /// </summary>
    /// <param name="body">The Node3D body that entered the ChaseArea.</param>
    protected void HandleChaseAreaBodyEntered(Node3D body) 
    {
        characterNode.StateMachineNode.SwitchState<EnemyChaseState>();
    }

    /// <summary>
    /// This method is added to a StatResource.OnZero so that it can be called
    /// when the Enemy's Health falls to zero.
    /// </summary>
    private void HandleZeroHealth()
    {
        // Switch to EnemyDeathState.
        characterNode.StateMachineNode.SwitchState<EnemyDeathState>();
    }
}