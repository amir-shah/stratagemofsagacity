# Docker Development Environment

Run Stratagem of Sagacity without installing Godot, .NET, or any dependencies locally! Everything runs in Docker containers.

## Quick Start (3 commands)

```bash
# 1. Clone the repository
git clone <repo-url>
cd stratagemofsagacity/SoS_Godot

# 2. Test the build
./scripts/test-build.sh

# 3. Run the game editor
./scripts/run-editor.sh
```

That's it! The Godot editor will open in a Docker container.

---

## Prerequisites

**Required:**
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (Windows/Mac/Linux)
- 4GB+ available RAM
- 2GB+ disk space for Docker images

**Optional (for GUI on Linux):**
- X11 server (usually pre-installed)

---

## Available Commands

### 🧪 Test Build
```bash
./scripts/test-build.sh
```
Validates that the project builds correctly. Run this first!

**What it does:**
1. Builds Docker image with Godot + .NET SDK
2. Tests asset imports
3. Tests C# compilation
4. Reports any errors

**Expected output:**
```
✅ Asset import successful
✅ C# build successful
✅ All tests passed!
```

---

### 🎮 Run Godot Editor
```bash
./scripts/run-editor.sh
```
Opens the full Godot editor in a container.

**What it does:**
- Launches Godot 4.3 with .NET support
- Mounts your project folder (changes persist)
- Enables GUI through X11 forwarding

**Usage:**
- Edit scenes, scripts, and assets normally
- All changes save to your local filesystem
- Press F5 to run the game
- Close editor when done (container auto-removes)

**Troubleshooting:**
- **Windows/Mac:** Requires X11 server (XQuartz on Mac, VcXsrv on Windows)
- **Linux:** Should work out of the box

---

### 🏗️ Build Game (All Platforms)
```bash
./scripts/build-game.sh
```
Exports the game for Linux and Web.

**What it does:**
1. Builds optimized game binaries
2. Creates `./build/linux/` - Linux executable
3. Creates `./build/web/` - HTML5 browser version

**Output:**
```
SoS_Godot/
└── build/
    ├── linux/
    │   └── StratageOfSagacity.x86_64
    └── web/
        ├── index.html
        ├── index.js
        ├── index.wasm
        └── index.pck
```

**Note:** First run requires downloading export templates (~500MB).

---

### 🌐 Run Web Version
```bash
./scripts/run-web.sh
```
Starts a local web server to test the HTML5 build.

**What it does:**
- Serves the game at `http://localhost:8000`
- Automatically builds if needed
- Press Ctrl+C to stop

**Usage:**
1. Run the script
2. Open browser to http://localhost:8000
3. Play the game!

---

### 🔧 Manual Docker Commands

If you prefer manual control:

```bash
# Build Docker image
docker-compose build dev

# Run Godot editor
docker-compose run --rm dev /opt/godot/Godot_v4.3-stable_mono_linux.x86_64 --path /app

# Build game exports
docker-compose run --rm build

# Start web server
docker-compose up web

# Interactive shell in container
docker-compose run --rm dev /bin/bash
```

---

## VS Code Dev Containers

For the best development experience, use VS Code with Dev Containers extension.

### Setup

1. **Install VS Code Extensions:**
   - "Dev Containers" (ms-vscode-remote.remote-containers)
   - "C#" (ms-dotnettools.csharp)

2. **Open in Container:**
   - Open SoS_Godot folder in VS Code
   - Press `F1` → "Dev Containers: Reopen in Container"
   - Wait for container to build (first time only)

3. **Develop:**
   - Full IntelliSense for C#
   - Integrated terminal with Godot commands
   - Extensions auto-installed
   - Run `./scripts/run-editor.sh` in terminal

### Features

✅ Pre-configured C# development environment
✅ Godot tools integration
✅ Git support
✅ Port forwarding (8000, 8080, 6005)
✅ Persistent volumes for cache

---

## Platform-Specific Notes

### Windows

**GUI Support:**
1. Install [VcXsrv](https://sourceforge.net/projects/vcxsrv/)
2. Launch XLaunch with default settings
3. Set environment variable:
   ```powershell
   $env:DISPLAY="host.docker.internal:0"
   ```
4. Run `./scripts/run-editor.sh`

**Or use WSL2:**
```bash
wsl --install Ubuntu
# Then follow Linux instructions
```

### macOS

**GUI Support:**
1. Install [XQuartz](https://www.xquartz.org/)
2. Open XQuartz → Preferences → Security
3. Enable "Allow connections from network clients"
4. Restart XQuartz
5. Allow connections:
   ```bash
   xhost +localhost
   ```
6. Run `./scripts/run-editor.sh`

**Alternative (No GUI):**
Use headless builds:
```bash
docker-compose run --rm dev /opt/godot/Godot_v4.3-stable_mono_linux.x86_64 --headless --build-solutions
```

### Linux

**Should work out of the box!**

If GUI doesn't appear:
```bash
xhost +local:docker
./scripts/run-editor.sh
```

---

## Architecture

The Docker setup uses multi-stage builds for efficiency:

```
┌─────────────────────────────────────┐
│  Base: .NET SDK 8.0                 │
│  + Godot 4.3 Mono                   │
│  + Dependencies                     │
└─────────────────────────────────────┘
                ↓
        ┌───────────────┐
        │  Build Stage  │
        │  - Import     │
        │  - Compile    │
        │  - Export     │
        └───────────────┘
                ↓
    ┌──────────────────────┐
    │  Runtime Stage       │
    │  (Smaller image)     │
    │  - Run game          │
    │  - No build tools    │
    └──────────────────────┘
```

### Image Sizes
- **Dev image:** ~2.5GB (includes Godot editor)
- **Build image:** ~2.5GB (includes build tools)
- **Runtime image:** ~1.2GB (runtime only)

### Volumes
- Project files: Mounted from host (changes persist)
- Godot cache: Docker volume (speeds up rebuilds)
- Godot config: Docker volume (saves settings)

---

## CI/CD Integration

Use Docker for automated builds:

### GitHub Actions Example

```yaml
name: Build Game
on: [push]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Build game
        run: |
          cd SoS_Godot
          docker-compose run --rm build
      - name: Upload artifacts
        uses: actions/upload-artifact@v3
        with:
          name: game-builds
          path: SoS_Godot/build/
```

---

## Troubleshooting

### Docker build fails

**Issue:** "Cannot connect to Docker daemon"
**Fix:**
```bash
# Linux
sudo systemctl start docker

# Mac/Windows
# Start Docker Desktop application
```

---

### GUI doesn't appear

**Issue:** Editor window doesn't open
**Fix (Linux):**
```bash
xhost +local:docker
export DISPLAY=:0
./scripts/run-editor.sh
```

**Fix (Mac):**
```bash
# In XQuartz terminal
xhost +localhost
export DISPLAY=host.docker.internal:0
./scripts/run-editor.sh
```

**Fix (Windows):**
```powershell
# Start VcXsrv, then:
$env:DISPLAY="host.docker.internal:0"
./scripts/run-editor.sh
```

---

### Slow performance

**Issue:** Container is slow
**Fixes:**
1. Increase Docker memory limit (Docker Desktop → Settings → Resources)
2. Use volumes instead of bind mounts (already configured)
3. Close other applications
4. On Mac: Ensure Docker uses Apple Silicon image if on M1/M2

---

### Port already in use

**Issue:** "Port 8000 already in use"
**Fix:**
```bash
# Find and kill process using port
lsof -ti:8000 | xargs kill -9

# Or use different port
docker-compose run --rm -p 8001:8000 web
```

---

### "Export failed" messages

**Issue:** Game export fails
**Cause:** Export templates not installed
**Fix:**
```bash
# Run editor first
./scripts/run-editor.sh

# In Godot editor:
# Editor → Manage Export Templates → Download and Install

# Then rebuild
./scripts/build-game.sh
```

---

## Advantages of Docker Setup

✅ **No local dependencies** - Everything in containers
✅ **Consistent environment** - Same setup everywhere
✅ **Easy cleanup** - `docker-compose down -v`
✅ **CI/CD ready** - Use same containers in pipelines
✅ **Cross-platform** - Works on Windows/Mac/Linux
✅ **Version locked** - Godot 4.3 + .NET 8.0
✅ **Isolated** - Doesn't affect system

---

## Performance Comparison

| Method | Setup Time | Disk Usage | Performance |
|--------|------------|------------|-------------|
| **Docker** | 5-10 min (first time) | ~2.5GB | 95% native |
| **Native** | 10-30 min | ~1.5GB | 100% |

**Recommendation:**
- **Developers:** Native install for best performance
- **CI/CD:** Docker for consistency
- **Contributors:** Docker for quick setup
- **Testing:** Docker to avoid dependency issues

---

## Next Steps

1. ✅ Run `./scripts/test-build.sh` to validate setup
2. ✅ Run `./scripts/run-editor.sh` to open Godot
3. ✅ Make changes to the game
4. ✅ Run `./scripts/build-game.sh` to export
5. ✅ Run `./scripts/run-web.sh` to test in browser

**Happy game development! 🎮**
