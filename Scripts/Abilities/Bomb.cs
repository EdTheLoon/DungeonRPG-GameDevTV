using Godot;
using System;

public partial class Bomb : Node3D
{
    [Export(PropertyHint.Range,"1,30,1")]             
    public float BombDamage { get; private set; } = 10; // The damage to be applied by the bomb.
    [Export] private AnimationPlayer animPlayerNode;    // A reference to the scene's AnimationPlayer

    public override void _Ready()
    {
        animPlayerNode.AnimationFinished += HandleExpandAnimationFinished;
    }

    private void HandleExpandAnimationFinished(StringName animName)
    {
        if (animName == GameConstants.ANIM_BOMB_EXPAND) 
        {
            animPlayerNode.Play(GameConstants.ANIM_BOMB_EXPLOSION);
        }
        else 
        {
            QueueFree();
        }
    }
}
