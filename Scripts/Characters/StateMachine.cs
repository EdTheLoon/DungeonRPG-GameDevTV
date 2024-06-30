using Godot;

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
        // Instantiate a new empty Node. Iterate through the list of
        // possible states and if there is a match then apply it to our 
        // newState.
        Node newState = null;
        foreach (Node state in states)
        {
            if (state is T)
            {
                newState = state;
            }
        }

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
