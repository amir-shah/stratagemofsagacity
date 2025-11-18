#!/bin/bash
# Run Godot editor in Docker container

set -e

echo "🎮 Starting Godot Editor in Docker..."
echo ""
echo "Prerequisites:"
echo "  - Docker installed"
echo "  - X11 forwarding enabled (for GUI)"
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker first."
    exit 1
fi

# Set X11 permissions (Linux/Mac)
if [[ "$OSTYPE" == "linux-gnu"* ]]; then
    echo "Setting X11 permissions..."
    xhost +local:docker 2>/dev/null || echo "Warning: Could not set X11 permissions. GUI may not work."
fi

# Build and run dev container
echo "Building Docker image (this may take a few minutes on first run)..."
docker-compose build dev

echo ""
echo "Starting Godot editor..."
echo "Note: The editor window should appear shortly."
echo ""

docker-compose run --rm dev /opt/godot/Godot_v4.3-stable_mono_linux.x86_64 --path /app
