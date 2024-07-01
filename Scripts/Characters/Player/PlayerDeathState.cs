using Godot;

public partial class PlayerDeathState : PlayerState
{
    protected override void EnterState()
    {
        characterNode.AnimPlayerNode.Play(GameConstants.ANIM_DEATH);
        characterNode.AnimPlayerNode.AnimationFinished += HandleAnimationFinished;
    }

    private void HandleAnimationFinished(StringName animName)
    {
        // Raise the OnEndGame event
        GameEvents.RaiseEndGame();
        // Deletes a node from the SceneTree hierarchy. This deletes the enemy.
        characterNode.QueueFree();
    }

}
