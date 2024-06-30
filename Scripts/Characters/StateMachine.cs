using Godot;
using System.Linq;

public partial class StateMachine : Node
{
    // Export these members so they can be edited in the properties editor
    [Export] private Node currentState;
    [Export] private Node[] states;

    // Override the engine's Ready method so we can utilise our own.
    public override void _Ready()
    {
        currentState.Notification(GameConstants.NOTIFICATION_ENTER_STATE);
    }

    /// <summary>
    /// Switch to a new state. Pass in the state's Type to T.
    /// </summary>
    /// <typeparam name="T">The TYPE of the State to switch to.</typeparam>
    public void SwitchState<T>()
    {
        // Use Microsoft LINQ to check if the State is a valid possible state.
        Node newState = states.Where((state) => state is T).FirstOrDefault();

        // Empty state checking
        if (newState == null) { return; }
        
        // Send notification to current state to exit.
        currentState.Notification(GameConstants.NOTIFICATION_EXIT_STATE);
        
        // Then update currentState to our newState and send a notification 
        // to that state to enter it.
        currentState = newState;
        currentState.Notification(GameConstants.NOTIFICATION_ENTER_STATE);
    }
}
