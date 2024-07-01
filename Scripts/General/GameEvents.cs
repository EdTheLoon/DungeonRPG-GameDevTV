using System;

public class GameEvents
{
    // OnStartGame event
    public static Action OnStartGame;
    public static void RaiseStartGame() => OnStartGame?.Invoke();
}