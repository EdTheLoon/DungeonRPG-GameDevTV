using Godot;

public abstract partial class Ability : Node3D 
{
    [Export] public float Damage { get; private set; } = 10; // The damage to be applied by the ability.
    [Export] protected AnimationPlayer animPlayerNode;    // A reference to the scene's AnimationPlayer
}