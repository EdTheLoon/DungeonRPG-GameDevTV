using Godot;
using System;

public partial class WorldEnvironment : Godot.WorldEnvironment
{
    public override void _Ready()
    {
        base._Ready();

        // Enable volumetric fog. This allows us to have the volumetric fog
        // turned off in the editor but enabled when the game is run.
        Environment.VolumetricFogEnabled = true;        
    }
}
