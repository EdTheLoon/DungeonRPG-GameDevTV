using System;

public class GameEvents
{
    // OnStartGame event
    public static event Action OnStartGame;
    public static void RaiseStartGame() => OnStartGame?.Invoke();
}