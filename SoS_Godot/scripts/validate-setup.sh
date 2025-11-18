#!/bin/bash
# Validate that Docker setup is complete and ready to use

set -e

echo "🔍 Validating Docker setup for Stratagem of Sagacity..."
echo ""

ERRORS=0
WARNINGS=0

# Check required files exist
echo "Checking required files..."
FILES=(
    "Dockerfile"
    "docker-compose.yml"
    ".devcontainer/devcontainer.json"
    "scripts/test-build.sh"
    "scripts/run-editor.sh"
    "scripts/build-game.sh"
    "scripts/run-web.sh"
    "project.godot"
    "SoS_Godot.csproj"
)

for file in "${FILES[@]}"; do
    if [ -f "$file" ]; then
        echo "  ✅ $file"
    else
        echo "  ❌ $file NOT FOUND"
        ERRORS=$((ERRORS + 1))
    fi
done

echo ""

# Check scripts are executable
echo "Checking script permissions..."
for script in scripts/*.sh; do
    if [ -x "$script" ]; then
        echo "  ✅ $script (executable)"
    else
        echo "  ⚠️  $script (not executable - fixing...)"
        chmod +x "$script"
        WARNINGS=$((WARNINGS + 1))
    fi
done

echo ""

# Check Docker is installed
echo "Checking Docker installation..."
if command -v docker &> /dev/null; then
    DOCKER_VERSION=$(docker --version)
    echo "  ✅ Docker installed: $DOCKER_VERSION"

    # Check if Docker is running
    if docker info > /dev/null 2>&1; then
        echo "  ✅ Docker daemon is running"
    else
        echo "  ⚠️  Docker daemon is not running (start Docker Desktop)"
        WARNINGS=$((WARNINGS + 1))
    fi

    # Check docker-compose
    if command -v docker-compose &> /dev/null; then
        COMPOSE_VERSION=$(docker-compose --version)
        echo "  ✅ docker-compose installed: $COMPOSE_VERSION"
    else
        echo "  ⚠️  docker-compose not found (may use 'docker compose' instead)"
        WARNINGS=$((WARNINGS + 1))
    fi
else
    echo "  ❌ Docker not installed"
    echo "     Install from: https://www.docker.com/products/docker-desktop"
    ERRORS=$((ERRORS + 1))
fi

echo ""

# Check project structure
echo "Checking project structure..."
DIRS=(
    "assets/sprites"
    "assets/audio"
    "scenes"
    "scripts"
    "web"
)

for dir in "${DIRS[@]}"; do
    if [ -d "$dir" ]; then
        FILE_COUNT=$(find "$dir" -type f | wc -l)
        echo "  ✅ $dir ($FILE_COUNT files)"
    else
        echo "  ⚠️  $dir directory missing"
        WARNINGS=$((WARNINGS + 1))
    fi
done

echo ""

# Check C# scripts
echo "Checking C# scripts..."
CS_FILES=(
    "scripts/Player.cs"
    "scripts/Enemy.cs"
    "scripts/BoxEnemy.cs"
    "scripts/Projectile.cs"
    "scripts/GameManager.cs"
)

for cs_file in "${CS_FILES[@]}"; do
    if [ -f "$cs_file" ]; then
        echo "  ✅ $cs_file"
    else
        echo "  ❌ $cs_file NOT FOUND"
        ERRORS=$((ERRORS + 1))
    fi
done

echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"

if [ $ERRORS -eq 0 ] && [ $WARNINGS -eq 0 ]; then
    echo "✅ All checks passed! Setup is complete."
    echo ""
    echo "Next steps:"
    echo "  1. ./scripts/test-build.sh    - Test Docker build"
    echo "  2. ./scripts/run-editor.sh    - Launch Godot editor"
    echo "  3. ./scripts/build-game.sh    - Build game exports"
    echo ""
    exit 0
elif [ $ERRORS -eq 0 ]; then
    echo "⚠️  Setup complete with $WARNINGS warning(s)"
    echo ""
    echo "You can proceed, but review warnings above."
    echo ""
    exit 0
else
    echo "❌ Setup incomplete: $ERRORS error(s), $WARNINGS warning(s)"
    echo ""
    echo "Fix the errors above before proceeding."
    echo ""
    exit 1
fi
