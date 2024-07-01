using System;
using Godot;

[GlobalClass]
public partial class StatResource : Resource
{
    // Events
    public event Action OnZero;
    public event Action OnUpdate;
    
    // What type of Stat is this? Uses an enum.
    [Export] public Stat StatType { get; private set; }

    // The stat value with its own getter and setter
    private float _statValue;
    [Export] public float StatValue
    {
        get => _statValue;
        set
        {
            // Update the stat and clamp so it doesn't fall below zero.
            _statValue = Mathf.Clamp(value, 0, Mathf.Inf);

            OnUpdate?.Invoke();

            // If stat falls below zero then do something.
            if (_statValue == 0) 
            {
                // ? dictates that if the action does not exist then
                // don't invoke the action
                OnZero?.Invoke();
            }
        }
    }
}