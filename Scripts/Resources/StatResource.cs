using System;
using Godot;

[GlobalClass]
public partial class StatResource : Resource
{
    // A variable that stores a method to be carried out when the stat
    // reaches zero.
    public event Action OnZero;
    
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