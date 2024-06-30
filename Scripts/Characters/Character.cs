using Godot;
using System.Linq;

public abstract partial class Character : CharacterBody3D
{
    // EDITOR ACCESSIBLE FIELDS ----------------------------------------------

    // Stats
    [ExportGroup("Stats")] 
    [Export] private StatResource[] stats;

    // ExportGroup allows the following exported variables to be grouped
    // within the editor.
    [ExportGroup("Required Editor Nodes")]
    // Exported variables allow these to be edited in the Inspector in Godot.
    [Export] public Sprite3D SpriteNode { get; private set; }
    [Export] public AnimationPlayer AnimPlayerNode { get; private set; }
    [Export] public StateMachine StateMachineNode { get; private set; }
    [Export] public Area3D HurtboxNode { get; private set; }

    [ExportGroup("AI Nodes")]
    [Export] public Path3D PathNode { get; private set; }
    [Export] public NavigationAgent3D AgentNode { get; private set; }
    [Export] public Area3D ChaseAreaNode { get; private set; }
    [Export] public Area3D AttackAreaNode { get; private set; }    


    // MEMBER FIELDS --------------------------------------------------------

    // An index for keeping track of which point in a path we are going to (AI)
    public int pointIndex = 0;
    public Vector2 direction = new();


    // METHODS --------------------------------------------------------------

    public override void _Ready()
    {
        HurtboxNode.AreaEntered += HandleHurtboxEntered;
    }

    /// <summary>
    /// Flip's the character sprite horizontally to match movement direction
    /// </summary>
    public void Flip()
    {
        bool isNotMovingHorizontally = Velocity.X == 0;

        if (isNotMovingHorizontally) { return; }

        // Use conditional assignment to set this true or false.
        bool isMovingLeft = Velocity.X < 0;
        SpriteNode.FlipH = isMovingLeft;
    }

    /// <summary>
    /// Handles the character hurtbox being triggered.
    /// </summary>
    /// <param name="area">The area that triggered the signal.</param>
    private void HandleHurtboxEntered(Area3D area)
    {
        // Get the health Stat and a reference to the character
        StatResource health = GetStatResource(Stat.Health);
        Character attacker = area.GetOwner<Character>();        
        StatResource strength = attacker.GetStatResource(Stat.Strength);
        // Apply damage by using the attacker's strength to decrease health.
        health.StatValue -= strength.StatValue;
        GD.Print($"Attacker Strength: {strength.StatValue}");
        GD.Print($"Health: {health.StatValue}");
    }

    public StatResource GetStatResource(Stat stat)
    {
        // Use Microsoft LINQ and lambda function to return the requested Stat.
        return stats.Where((element) => element.StatType == stat).FirstOrDefault();
    }

}
