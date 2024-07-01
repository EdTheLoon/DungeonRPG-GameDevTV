using Godot;
using System;

public partial class Bomb : Node3D
{
    [Export] private AnimationPlayer animPlayerNode;

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
