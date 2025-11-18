# Multi-stage Dockerfile for Stratagem of Sagacity (Godot Edition)
# This allows building and running the game without installing Godot locally

# Stage 1: Build stage with Godot and .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Install dependencies
RUN apt-get update && apt-get install -y \
    wget \
    unzip \
    libglu1-mesa \
    libx11-6 \
    libxcursor1 \
    libxinerama1 \
    libxrandr2 \
    libxi6 \
    libgl1 \
    libasound2 \
    libpulse0 \
    && rm -rf /var/lib/apt/lists/*

# Download and install Godot 4.3 Mono/.NET version
ARG GODOT_VERSION=4.3-stable
ARG GODOT_PLATFORM=linux.x86_64
WORKDIR /opt
RUN wget https://github.com/godotengine/godot/releases/download/${GODOT_VERSION}/Godot_v${GODOT_VERSION}_mono_${GODOT_PLATFORM}.zip && \
    unzip Godot_v${GODOT_VERSION}_mono_${GODOT_PLATFORM}.zip && \
    rm Godot_v${GODOT_VERSION}_mono_${GODOT_PLATFORM}.zip && \
    mv Godot_v${GODOT_VERSION}_mono_${GODOT_PLATFORM} godot

# Add Godot to PATH
ENV PATH="/opt/godot:${PATH}"
ENV GODOT_BIN="/opt/godot/Godot_v${GODOT_VERSION}_mono_${GODOT_PLATFORM}"

# Set working directory
WORKDIR /app

# Copy project files
COPY . .

# Import assets (this triggers Godot to process/import all assets)
RUN ${GODOT_BIN} --headless --import --quit || true

# Build C# project
RUN ${GODOT_BIN} --headless --build-solutions --quit || true

# Export for Linux (headless mode)
RUN mkdir -p /app/build/linux && \
    ${GODOT_BIN} --headless --export-release "Linux/X11" /app/build/linux/StratageOfSagacity.x86_64 || true

# Export for Web
RUN mkdir -p /app/build/web && \
    ${GODOT_BIN} --headless --export-release "Web" /app/build/web/index.html || true

# Stage 2: Runtime stage (smaller image for running)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

# Install runtime dependencies
RUN apt-get update && apt-get install -y \
    libglu1-mesa \
    libx11-6 \
    libxcursor1 \
    libxinerama1 \
    libxrandr2 \
    libxi6 \
    libgl1 \
    libasound2 \
    libpulse0 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy built game from build stage
COPY --from=build /app/build /app/build
COPY --from=build /opt/godot /opt/godot
COPY --from=build /app /app

ENV PATH="/opt/godot:${PATH}"
ENV GODOT_VERSION=4.3-stable
ENV GODOT_PLATFORM=linux.x86_64
ENV GODOT_BIN="/opt/godot/Godot_v${GODOT_VERSION}_mono_${GODOT_PLATFORM}"

# Default command - run the game in editor mode
CMD ["/bin/bash", "-c", "${GODOT_BIN} --path /app"]

# Development stage - includes full Godot editor
FROM build AS dev

WORKDIR /app

# Expose ports for web server (if running web build)
EXPOSE 8000 8080

# Set display for X11 forwarding (allows GUI)
ENV DISPLAY=:0

CMD ["/bin/bash"]
