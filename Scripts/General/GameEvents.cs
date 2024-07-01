using System;
using Godot;

public class GameEvents
{
    // Define new events
    public static event Action OnStartGame;
    public static event Action OnEndGame;
    public static event Action<int> OnNewEnemyCount; // by adding <int> we can pass a parameter

    // Define invoke methods
    // This will invoke the handlers subscribed to the event.
    public static void RaiseStartGame() => OnStartGame?.Invoke();
    public static void RaiseEndGame() => OnEndGame?.Invoke();
    public static void RaiseNewEnemyCount(int count) => OnNewEnemyCount?.Invoke(count);
}