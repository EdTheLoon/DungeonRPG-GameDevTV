using Godot;
using System;
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
    [Export] public Area3D HitboxNode { get; private set; }
    [Export] public CollisionShape3D HitboxShapeNode { get; private set; }

    [ExportGroup("AI Nodes")]
    [Export] public Path3D PathNode { get; private set; }
    [Export] public NavigationAgent3D AgentNode { get; private set; }
    [Export] public Area3D ChaseAreaNode { get; private set; }
    [Export] public Area3D AttackAreaNode { get; private set; }    

    [ExportGroup("Shaders")]
    [Export] public Timer ShaderTimer { get; protected set; }
    private ShaderMaterial shader;



    // MEMBER FIELDS --------------------------------------------------------
    // An index for keeping track of which point in a path we are going to (AI).
    // Uses a setter to ensure the index never goes higher than the number of points
    // in the assigned path.
    private int _pointIndex;
    public int PointIndex 
    {
        get => _pointIndex;
        set 
        {
            _pointIndex = Mathf.Wrap(value, 0, PathNode.Curve.PointCount);
        }
    }

    public Vector2 direction = new();


    // METHODS --------------------------------------------------------------

    public override void _Ready()
    {
        shader = (ShaderMaterial)SpriteNode.MaterialOverlay; // Cast to ShaderMaterial.

        SpriteNode.TextureChanged += HandleTextureChanged;
        HurtboxNode.AreaEntered += HandleHurtboxEntered;
        ShaderTimer.Timeout += () => shader.SetShaderParameter("active", false);
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

    private void HandleTextureChanged()
    {
        shader.SetShaderParameter("tex", SpriteNode.Texture);
    }

    /// <summary>
    /// Handles the character hurtbox being triggered.
    /// </summary>
    /// <param name="area">The area that triggered the signal.</param>
    private void HandleHurtboxEntered(Area3D area)
    {
        // If the area is not an instance of AttackHitbox then return.
        // Otherwise, store a reference to the AttackHitbox in variable hitbox.
        if (area is not IHitbox hitbox) { return; }

        // We can then call GetDamage() from the AttackHitbox class.
        float damage = hitbox.GetDamage();
        
        // Apply the damage.
        StatResource health = GetStatResource(Stat.Health);
        health.StatValue -= damage;

        shader.SetShaderParameter("active", true);
        ShaderTimer.Start();
    }

    public StatResource GetStatResource(Stat stat)
    {
        // Use Microsoft LINQ and lambda function to return the requested Stat.
        return stats.Where((element) => element.StatType == stat).FirstOrDefault();
    }

    public void DisableHitbox(bool flag) 
    {
        HitboxShapeNode.Disabled = flag;
    }
}
