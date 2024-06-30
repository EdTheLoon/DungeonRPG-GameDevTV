using Godot;

// Make this an abstract class so that it is not accidentally instanced.
public abstract partial class CharacterState : Node
{
    // Stores a reference to the Character's root node.
    protected Character characterNode;
    
    // Override the engine's Ready process so we can do our own initialisation.
    public override void _Ready()
    {
        // Get the root node of the Character.
        characterNode = GetOwner<Character>();

        // We don't want the Physics and Input processes to run when the State is loaded.
        SetPhysicsProcess(false);
        SetProcessInput(false);
    }
    
    // Override the engine's Notifications process so we can add to it.
    public override void _Notification(int what)
    {
        // Let Godot still carry out it's usual notification handling.
        base._Notification(what);

        // Notification state handling
        if (what == GameConstants.NOTIFICATION_ENTER_STATE)
        {
            EnterState();
            SetPhysicsProcess(true);
            SetProcessInput(true);
        }
        else if (what == GameConstants.NOTIFICATION_EXIT_STATE)
        {
            SetPhysicsProcess(false);
            SetProcessInput(false);
            ExitState();
        }
    }

    protected virtual void EnterState() {}

    protected virtual void ExitState() {}
}