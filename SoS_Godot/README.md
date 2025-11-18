# Stratagem of Sagacity - Godot Edition

This is a modern Godot 4.x migration of the classic 2009 XNA Game Studio project.

## Platform Support

✅ **Windows** - Native desktop build
✅ **macOS** - Native desktop build
✅ **Linux** - Native desktop build
✅ **Web (HTML5)** - Play in browser! [See Web Deployment Guide](WEB_DEPLOYMENT.md)
✅ **iOS** - Mobile export (requires Xcode)
✅ **Android** - Mobile export (requires Android SDK)

The same codebase runs on all platforms!

## Quick Start Options

### Option 1: Docker (No Installation Required!) 🐳

**Fastest way to get started** - Everything runs in containers, no dependencies to install:

```bash
# 1. Validate setup
./scripts/validate-setup.sh

# 2. Test build
./scripts/test-build.sh

# 3. Run Godot editor
./scripts/run-editor.sh
```

**See [DOCKER_SETUP.md](DOCKER_SETUP.md) for full Docker documentation.**

Requires: Docker Desktop only

### Option 2: Native Installation

Install Godot and .NET locally for best performance.

## Requirements

### For Docker Setup (Recommended)
- **Docker Desktop** - [Download here](https://www.docker.com/products/docker-desktop)
- 4GB+ RAM
- 2GB+ disk space

### For Native Development
- **Godot 4.3 or later** with .NET support
- **.NET 8.0 SDK**

### For Web (Players)
- Modern web browser with WebAssembly support (Chrome, Firefox, Safari, Edge)

## Installation

### Using Docker (Zero Dependencies)

```bash
cd SoS_Godot

# Validate everything is ready
./scripts/validate-setup.sh

# Launch Godot editor
./scripts/run-editor.sh

# Or build the game
./scripts/build-game.sh
```

See [DOCKER_SETUP.md](DOCKER_SETUP.md) for detailed instructions.

### Using Native Installation

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

## Building & Exporting

### Desktop Build

The project uses Godot's built-in C# build system:

1. Open the project in Godot
2. Click "Build" in the MSBuild panel (or let it build automatically)
3. Run the project with F5

### Web Export

Export as HTML5 to play in a browser:

1. Go to `Project → Export`
2. Select the "Web" preset
3. Click "Export Project"
4. Choose output folder (e.g., `build/web/`)

**See [WEB_DEPLOYMENT.md](WEB_DEPLOYMENT.md) for detailed deployment instructions** including:
- GitHub Pages (free hosting)
- itch.io (indie game platform)
- Netlify/Vercel (modern web hosting)
- Self-hosting options

### Other Platforms

- **macOS**: Export → macOS preset
- **iOS**: Export → iOS (requires macOS with Xcode)
- **Android**: Export → Android (requires Android SDK)

All exports use the same codebase with platform-specific optimizations.

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
