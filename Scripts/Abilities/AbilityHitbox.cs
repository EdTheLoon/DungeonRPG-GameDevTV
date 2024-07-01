using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class AbilityHitbox : Area3D, IHitbox
{
    public bool CanStun() => true;
    public float GetDamage() => GetOwner<Ability>().Damage;
}
