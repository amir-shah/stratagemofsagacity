#!/bin/bash
# Test that the project builds and imports correctly

set -e

echo "🧪 Testing Godot project build..."
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker first."
    exit 1
fi

# Build Docker image
echo "Step 1/3: Building Docker image..."
docker-compose build dev

# Test asset import
echo ""
echo "Step 2/3: Testing asset import..."
docker-compose run --rm dev /bin/bash -c "
    /opt/godot/Godot_v4.3-stable_mono_linux.x86_64 --headless --import --quit
    if [ \$? -eq 0 ]; then
        echo '✅ Asset import successful'
    else
        echo '❌ Asset import failed'
        exit 1
    fi
"

# Test C# build
echo ""
echo "Step 3/3: Testing C# project build..."
docker-compose run --rm dev /bin/bash -c "
    /opt/godot/Godot_v4.3-stable_mono_linux.x86_64 --headless --build-solutions --quit
    if [ \$? -eq 0 ]; then
        echo '✅ C# build successful'
    else
        echo '❌ C# build failed'
        exit 1
    fi
"

echo ""
echo "✅ All tests passed! Project is ready to run."
echo ""
echo "Next steps:"
echo "  - Run editor:  ./scripts/run-editor.sh"
echo "  - Build game:  ./scripts/build-game.sh"
echo "  - Run web:     ./scripts/run-web.sh"
