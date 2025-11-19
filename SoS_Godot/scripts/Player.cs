using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 200.0f;
    [Export] public float RotationSpeed = 5.0f;

    private Sprite2D _sprite;
    private AnimatedSprite2D _animatedSprite;
    private Vector2 _velocity = Vector2.Zero;

    // Projectile scene to instantiate
    [Export] public PackedScene ProjectileScene;

    public override void _Ready()
    {
        // Get the sprite node (we'll set this up in the scene)
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        // Get input direction
        Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        // Set velocity based on input
        Velocity = inputDirection * Speed;

        // Get mouse position and rotate player to face it
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 direction = (mousePos - GlobalPosition).Normalized();
        Rotation = direction.Angle();

        // Move the player
        MoveAndSlide();

        // Update animation based on movement
        UpdateAnimation(inputDirection);
    }

    public override void _Input(InputEvent @event)
    {
        // Handle shooting
        if (@event.IsActionPressed("shoot"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (ProjectileScene == null)
            return;

        // Instantiate projectile
        var projectile = ProjectileScene.Instantiate<Projectile>();

        // Set projectile position and direction
        projectile.GlobalPosition = GlobalPosition;
        projectile.Direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
        projectile.Rotation = Rotation;

        // Add to scene tree (parent's parent is usually the main scene)
        GetTree().Root.AddChild(projectile);

        // Play shooting animation if available
        if (_animatedSprite != null && _animatedSprite.SpriteFrames.HasAnimation("shooting"))
        {
            _animatedSprite.Play("shooting");
        }
    }

    private void UpdateAnimation(Vector2 inputDirection)
    {
        if (_animatedSprite == null)
            return;

        if (inputDirection.Length() > 0)
        {
            // Moving
            if (_animatedSprite.SpriteFrames.HasAnimation("walking"))
                _animatedSprite.Play("walking");
        }
        else
        {
            // Standing still
            if (_animatedSprite.SpriteFrames.HasAnimation("standing"))
                _animatedSprite.Play("standing");
        }
    }
}
