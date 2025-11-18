#!/bin/bash
# Build game for all platforms using Docker

set -e

echo "🏗️  Building Stratagem of Sagacity..."
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker first."
    exit 1
fi

# Build the Docker image
echo "Step 1/2: Building Docker image..."
docker-compose build build

# Run the build
echo ""
echo "Step 2/2: Exporting game for all platforms..."
echo "This will create builds in ./build/ directory:"
echo "  - ./build/linux/   (Linux executable)"
echo "  - ./build/web/     (HTML5 for browsers)"
echo ""

docker-compose run --rm build /bin/bash -c "
    echo '📦 Exporting for Linux...'
    mkdir -p /app/build/linux
    /opt/godot/Godot_v4.3-stable_mono_linux.x86_64 --headless --export-release 'Linux/X11' /app/build/linux/StratageOfSagacity.x86_64 || echo 'Linux export failed (expected if templates not installed)'

    echo ''
    echo '🌐 Exporting for Web...'
    mkdir -p /app/build/web
    /opt/godot/Godot_v4.3-stable_mono_linux.x86_64 --headless --export-release 'Web' /app/build/web/index.html || echo 'Web export failed (expected if templates not installed)'

    echo ''
    echo '✅ Build process complete!'
    echo ''
    echo 'Note: If exports failed, install export templates in Godot editor first.'
"

echo ""
echo "✅ Done! Check ./build/ directory for output files."
echo ""
echo "To test web build locally:"
echo "  docker-compose up web"
echo "  Then open: http://localhost:8000"
