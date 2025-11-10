using Godot;
using System;

public partial class BoxEnemy : Enemy
{
    [Export] public float BoxSize = 200.0f;

    private Vector2 _startPosition;
    private Vector2 _currentTarget;
    private int _cornerIndex = 0;
    private Vector2[] _corners;

    public override void _Ready()
    {
        base._Ready();

        _startPosition = Position;

        // Define the four corners of the box patrol path
        _corners = new Vector2[]
        {
            _startPosition, // Top-left
            _startPosition + new Vector2(BoxSize, 0), // Top-right
            _startPosition + new Vector2(BoxSize, BoxSize), // Bottom-right
            _startPosition + new Vector2(0, BoxSize) // Bottom-left
        };

        _currentTarget = _corners[1];
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move towards current corner
        Vector2 direction = (_currentTarget - Position).Normalized();
        Velocity = direction * Speed;

        MoveAndSlide();

        // Check if reached current corner
        if (Position.DistanceTo(_currentTarget) < 5.0f)
        {
            // Move to next corner
            _cornerIndex = (_cornerIndex + 1) % _corners.Length;
            _currentTarget = _corners[_cornerIndex];
        }

        // Update rotation to face movement direction
        if (Velocity.Length() > 0)
        {
            Rotation = Velocity.Angle();
        }
    }
}
