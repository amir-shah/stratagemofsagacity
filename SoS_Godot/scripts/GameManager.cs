using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class GameManager : Node2D
{
	[Export] public PackedScene EnemyScene;
	[Export] public PackedScene BoxEnemyScene;
	[Export] public PackedScene ProjectileScene;
	[Export] public Vector2 MapSize = new Vector2(1200, 1200);

	private Player _player;
	private List<Enemy> _enemies = new List<Enemy>();
	private List<Node2D> _projectiles = new List<Node2D>();
	private Camera2D _camera;

	// Map data
	private int _mapWidth;
	private int _mapHeight;
	private List<StaticBody2D> _walls = new List<StaticBody2D>();

	public override void _Ready()
	{
		// Find the player node
		_player = GetNode<Player>("Player");
		_camera = GetNode<Camera2D>("Camera2D");

		// Load map from file
		LoadMapFromFile("res://Content/map.txt");

		// Spawn initial enemies
		SpawnEnemy(new Vector2(100, 10));
		SpawnBoxEnemy(new Vector2(950, 60), 200);
	}

	private void LoadMapFromFile(string mapPath)
	{
		if (!FileAccess.FileExists(mapPath))
		{
			GD.PrintErr($"Map file not found: {mapPath}");
			return;
		}

		using var file = FileAccess.Open(mapPath, FileAccess.ModeFlags.Read);

		_mapWidth = file.GetLine().ToInt();
		_mapHeight = file.GetLine().ToInt();

		GD.Print($"Loading map: {_mapWidth}x{_mapHeight}");

		string[,] map = new string[_mapHeight, _mapWidth];

		// Read map data
		for (int i = 0; i < _mapHeight - 1; i++)
		{
			string line = file.GetLine();
			if (line == null) break;

			char[] chars = line.ToCharArray();
			for (int j = 0; j < _mapWidth && j < chars.Length; j++)
			{
				map[i, j] = chars[j].ToString();
			}
		}

		// Create walls based on map data
		int tileSize = 4; // Original tile size from MonoGame
		for (int y = 0; y < _mapHeight; y++)
		{
			for (int x = 0; x < _mapWidth; x++)
			{
				if (map[y, x] == "x")
				{
					CreateWall(new Vector2(x * tileSize, y * tileSize), tileSize);
				}
			}
		}

		GD.Print("Map loaded successfully");
	}

	private void CreateWall(Vector2 position, int size)
	{
		var wall = new StaticBody2D();
		wall.Position = position;

		var sprite = new Sprite2D();
		sprite.Texture = GD.Load<Texture2D>("res://assets/sprites/box.png");
		sprite.Scale = new Vector2(size / 32.0f, size / 32.0f); // Assuming 32x32 box sprite
		sprite.TextureFilter = Sprite2D.TextureFilterEnum.Nearest;

		var collision = new CollisionShape2D();
		var shape = new RectangleShape2D();
		shape.Size = new Vector2(size, size);
		collision.Shape = shape;

		wall.AddChild(sprite);
		wall.AddChild(collision);
		AddChild(wall);
		_walls.Add(wall);
	}

	private void SpawnEnemy(Vector2 position)
	{
		if (EnemyScene == null)
		{
			GD.PrintErr("Enemy scene not set!");
			return;
		}

		var enemy = EnemyScene.Instantiate<Enemy>();
		enemy.Position = position;
		AddChild(enemy);
		_enemies.Add(enemy);
	}

	private void SpawnBoxEnemy(Vector2 position, float boxSize)
	{
		if (BoxEnemyScene == null)
		{
			GD.PrintErr("BoxEnemy scene not set!");
			return;
		}

		var enemy = BoxEnemyScene.Instantiate<BoxEnemy>();
		enemy.Position = position;
		enemy.BoxSize = boxSize;
		AddChild(enemy);
		_enemies.Add(enemy);
	}

	public void SpawnProjectile(Vector2 position, Vector2 direction)
	{
		if (ProjectileScene == null)
		{
			GD.PrintErr("Projectile scene not set!");
			return;
		}

		var projectile = ProjectileScene.Instantiate<Area2D>();
		projectile.Position = position;

		// Set the direction for the projectile script
		if (projectile.HasMethod("SetDirection"))
		{
			projectile.Call("SetDirection", direction);
		}

		AddChild(projectile);
	}

	public override void _Process(double delta)
	{
		// Update camera to follow player
		if (_player != null && _camera != null)
		{
			_camera.Position = _player.Position;

			// Clamp camera to map bounds
			float halfWidth = GetViewportRect().Size.X / 2;
			float halfHeight = GetViewportRect().Size.Y / 2;

			_camera.Position = new Vector2(
				Mathf.Clamp(_camera.Position.X, halfWidth, MapSize.X - halfWidth),
				Mathf.Clamp(_camera.Position.Y, halfHeight, MapSize.Y - halfHeight)
			);
		}

		// Clean up dead enemies
		for (int i = _enemies.Count - 1; i >= 0; i--)
		{
			if (_enemies[i] == null || !IsInstanceValid(_enemies[i]))
			{
				_enemies.RemoveAt(i);
			}
		}
	}

	public Player GetPlayer()
	{
		return _player;
	}
}
