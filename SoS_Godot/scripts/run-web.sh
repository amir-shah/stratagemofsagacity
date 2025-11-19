#!/bin/bash
# Run the web version locally using Docker

set -e

echo "🌐 Starting web server for HTML5 build..."
echo ""

# Check if web build exists
if [ ! -f "./build/web/index.html" ]; then
    echo "❌ Web build not found. Building now..."
    ./scripts/build-game.sh
fi

echo "Starting server at http://localhost:8000"
echo "Press Ctrl+C to stop"
echo ""

docker-compose up web
