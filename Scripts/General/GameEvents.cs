using System;

public class GameEvents
{
    // OnStartGame event
    public static event Action OnStartGame;
    public static void RaiseStartGame() => OnStartGame?.Invoke();

    // OnEndGame event
    public static event Action OnEndGame;
    public static void RaiseEndGame() => OnEndGame?.Invoke();
}