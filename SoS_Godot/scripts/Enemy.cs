using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export] public float Speed = 100.0f;
    [Export] public float Health = 1.0f;

    private Vector2 _velocity = Vector2.Right;
    private Sprite2D _sprite;
    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        // Get sprite node
        _animatedSprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
        if (_animatedSprite == null)
            _sprite = GetNodeOrNull<Sprite2D>("Sprite2D");

        // Set initial random velocity
        _velocity = new Vector2(
            (float)GD.RandRange(-1, 1),
            (float)GD.RandRange(-1, 1)
        ).Normalized() * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Simple bouncing movement
        Velocity = _velocity;

        // Move and check for collisions
        var collision = MoveAndSlide();

        // Bounce off walls
        if (GetSlideCollisionCount() > 0)
        {
            var collisionInfo = GetSlideCollision(0);
            _velocity = _velocity.Bounce(collisionInfo.GetNormal());
        }

        // Keep within map bounds (simplified - would need actual map bounds)
        var screenSize = GetViewportRect().Size;
        if (Position.X < 0 || Position.X > screenSize.X)
            _velocity.X = -_velocity.X;
        if (Position.Y < 0 || Position.Y > screenSize.Y)
            _velocity.Y = -_velocity.Y;

        // Update rotation to face movement direction
        if (_velocity.Length() > 0)
        {
            Rotation = _velocity.Angle();
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play death animation if available
        if (_animatedSprite != null && _animatedSprite.SpriteFrames.HasAnimation("dead"))
        {
            _animatedSprite.Play("dead");
            // Wait for animation to finish before removing
            _animatedSprite.AnimationFinished += () => QueueFree();
        }
        else
        {
            QueueFree();
        }
    }
}
