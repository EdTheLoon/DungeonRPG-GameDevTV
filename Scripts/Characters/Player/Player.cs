using System;
using Godot;

public partial class Player : Character
{
    public override void _Ready()
    {
        base._Ready();

        GameEvents.OnReward += HandleReward; // Subscribe to OnReward event
    }

    public override void _Input(InputEvent @event)
    {
        // Character movement
        direction = Input.GetVector(
            GameConstants.INPUT_MOVE_LEFT, GameConstants.INPUT_MOVE_RIGHT, GameConstants.INPUT_MOVE_FORWARD, GameConstants.INPUT_MOVE_BACKWARD
        );
    }
    
    private void HandleReward(RewardResource reward)
    {
        StatResource targetStat = GetStatResource(reward.TargetStat);
        targetStat.StatValue += reward.Amount;
    }
}
