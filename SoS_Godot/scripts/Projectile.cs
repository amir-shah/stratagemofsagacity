using Godot;
using System;

public partial class Projectile : Area2D
{
    [Export] public float Speed = 400.0f;
    [Export] public float LifeTime = 3.0f; // Seconds before auto-destroying

    public Vector2 Direction = Vector2.Right;

    private float _timeAlive = 0.0f;

    public override void _Ready()
    {
        // Connect the body_entered signal to handle collisions
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move projectile
        Position += Direction * Speed * (float)delta;

        // Track lifetime
        _timeAlive += (float)delta;
        if (_timeAlive >= LifeTime)
        {
            QueueFree(); // Destroy projectile
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        // Check if hit an enemy or obstacle
        if (body is Enemy || body.IsInGroup("obstacles"))
        {
            // Damage enemy if applicable
            if (body is Enemy enemy)
            {
                enemy.TakeDamage(1.0f);
            }

            // Destroy projectile on impact
            QueueFree();
        }
    }
}
