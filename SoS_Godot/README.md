# Stratagem of Sagacity - Godot Edition

This is a modern Godot 4.x migration of the classic 2009 XNA Game Studio project.

## Requirements

- **Godot 4.3 or later** with .NET support
- **.NET 8.0 SDK**

## Installation

1. **Install Godot 4.3+ (.NET version)**
   - Download from: https://godotengine.org/download
   - Make sure to get the .NET/.Mono version for C# support

2. **Install .NET 8.0 SDK**
   - Download from: https://dotnet.microsoft.com/download

3. **Open the project**
   - Launch Godot
   - Click "Import"
   - Navigate to the `SoS_Godot` folder
   - Select `project.godot`

## How to Play

### Controls
- **WASD** - Move player
- **Mouse** - Aim
- **Left Click** - Shoot
- **Escape** - Pause (when implemented)

### Gameplay
You control a character in a top-down shooter environment. Move around the map, aim with your mouse, and shoot enemies. Avoid obstacles and survive!

## Project Structure

```
SoS_Godot/
├── assets/
│   ├── sprites/     # All game sprites (player, enemies, environment)
│   ├── audio/       # Sound effects and music
│   └── Content/     # Map data files
├── scenes/          # Godot scene files
│   ├── Main.tscn    # Main game scene
│   ├── Player.tscn  # Player character
│   ├── Enemy.tscn   # Basic enemy
│   ├── BoxEnemy.tscn # Patrol enemy
│   └── Projectile.tscn # Player projectile
├── scripts/         # C# game scripts
│   ├── GameManager.cs
│   ├── Player.cs
│   ├── Enemy.cs
│   ├── BoxEnemy.cs
│   └── Projectile.cs
└── project.godot    # Godot project configuration
```

## Features

### Implemented
- Player movement with WASD
- Mouse-based aiming and rotation
- Shooting mechanics
- Enemy AI (basic and patrol patterns)
- Map loading from text files
- Camera system that follows the player
- Collision detection
- Pixel-perfect rendering

### From Original Game
- 2D top-down shooter mechanics
- Multiple enemy types
- Map system with obstacles
- Sprite-based graphics
- Projectile system

### Not Yet Implemented
- Menu system
- Pause functionality
- Sound effects and music
- Animated sprites
- Advanced enemy behaviors
- Power-ups and items

## Development

This migration preserves the core gameplay mechanics from the original 2009 XNA version while modernizing the codebase for Godot 4.x.

### Key Changes from MonoGame Version
- XNA/MonoGame `SpriteBatch` → Godot `Sprite2D` nodes
- Manual update loops → Godot's `_Process()` and `_PhysicsProcess()`
- Rectangle-based collision → Godot's built-in physics engine
- Texture2D loading → Godot's resource system
- Manual camera → Godot's Camera2D with smoothing

## Building

The project uses Godot's built-in C# build system. To build:

1. Open the project in Godot
2. Click "Build" in the MSBuild panel (or let it build automatically)
3. Run the project with F5

## Troubleshooting

### "Script error" messages
- Ensure .NET 8.0 SDK is installed
- Click "Build" in the MSBuild panel in Godot
- Check that all C# scripts are properly attached to nodes

### Sprites not appearing
- Check that texture filter is set to "Nearest" for pixel art
- Verify sprite paths in the .tscn files
- Ensure sprites are in the `assets/sprites/` directory

### Map not loading
- Verify `Content/map.txt` exists and has correct format
- Check console output for error messages

## Credits

Original game created in 2009-2010 using XNA Game Studio 3.0.
Migrated to Godot 4.x in 2025.
