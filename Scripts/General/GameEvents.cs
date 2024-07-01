using System;
using Godot;

public class GameEvents
{
    // Define new events
    public static event Action OnStartGame;
    public static event Action OnEndGame;
    public static event Action<int> OnNewEnemyCount;        // Requires an int to be passed
    public static event Action OnVictory;
    public static event Action<RewardResource> OnReward;    // Requires a RewardResource to be passed

    // Define invoke methods
    // This will invoke the handlers subscribed to the event.
    public static void RaiseStartGame() => OnStartGame?.Invoke();
    public static void RaiseEndGame() => OnEndGame?.Invoke();
    public static void RaiseNewEnemyCount(int count) => OnNewEnemyCount?.Invoke(count);
    public static void RaiseVictory() => OnVictory?.Invoke();
    public static void RaiseReward(RewardResource reward) => OnReward?.Invoke(reward);
}