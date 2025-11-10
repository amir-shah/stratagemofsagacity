# Stratagem of Sagacity - MonoGame Edition

This is a **migrated version** of the original XNA 3.0 project, now running on **MonoGame** - a modern, cross-platform game framework that is the spiritual successor to Microsoft XNA.

## What Changed

- **Framework**: XNA 3.0 → MonoGame 3.8.2
- **Target**: .NET 3.5 → .NET 8.0
- **Platform**: XNA Windows → MonoGame DesktopGL (Windows, Linux, macOS)
- **Removed**: Obsolete XNA namespaces (GamerServices, Net, Storage)
- **Content Pipeline**: XNA Content Pipeline → MonoGame Content Builder (.mgcb)

## Prerequisites

Before you can build and run this project, you need to install:

### 1. .NET SDK 8.0 or later

**Windows:**
- Download from: https://dotnet.microsoft.com/download/dotnet/8.0
- Run the installer and follow the prompts

**macOS:**
```bash
brew install dotnet
```

**Linux (Ubuntu/Debian):**
```bash
wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

### 2. MonoGame Dependencies

**Windows:**
- No additional dependencies needed

**macOS:**
- Install Mono: `brew install mono`

**Linux:**
```bash
sudo apt-get install -y libsdl2-dev libsdl2-mixer-dev libfreetype6-dev
```

## Building the Project

### Option 1: Using .NET CLI (Recommended)

1. **Navigate to the project directory:**
   ```bash
   cd SoS_MonoGame
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Build the project:**
   ```bash
   dotnet build
   ```

4. **Run the game:**
   ```bash
   dotnet run
   ```

### Option 2: Using Visual Studio 2022

1. Open `SoS_MonoGame.csproj` in Visual Studio 2022
2. Right-click the project → **Restore NuGet Packages**
3. Press **F5** to build and run

### Option 3: Using Visual Studio Code

1. Install the **C# Dev Kit** extension
2. Open the `SoS_MonoGame` folder
3. Press **F5** to build and run

## Publishing

To create a standalone executable:

**Windows:**
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

**macOS:**
```bash
dotnet publish -c Release -r osx-x64 --self-contained
```

**Linux:**
```bash
dotnet publish -c Release -r linux-x64 --self-contained
```

The output will be in: `bin/Release/net8.0/[runtime]/publish/`

## Project Structure

```
SoS_MonoGame/
├── SoS_MonoGame.csproj      # Project file with MonoGame references
├── Content/                  # Game assets
│   ├── Content.mgcb         # MonoGame Content Builder project
│   ├── Player/              # Player sprites
│   ├── Enemy/               # Enemy sprites
│   ├── Gun Fire/            # Gun fire effects
│   ├── *.png, *.jpg         # Textures
│   ├── *.mp3, *.wav         # Audio files
│   ├── *.txt                # Map files
│   └── TimesNewRoman.spritefont  # Font
├── *.cs                     # C# source files
├── app.manifest             # Windows app manifest
├── Icon.ico                 # Game icon
└── README.md                # This file
```

## Game Controls

- **Movement**: WASD keys
- **Aim**: Mouse
- **Shoot**: Left mouse button
- **Menu Navigation**: Arrow keys + Enter

## Known Issues & Notes

1. **Content Pipeline**: The MonoGame Content Pipeline processes assets at build time. If you add new assets, you need to add them to `Content/Content.mgcb`.

2. **Icon**: The Icon.ico file is currently a placeholder. Replace it with a proper .ico file for a custom game icon.

3. **Audio**: MP3 files work on Windows and macOS. On Linux, you might need to use OGG format instead.

4. **Case Sensitivity**: MonoGame is case-sensitive on Linux/macOS. Ensure file paths in code match the actual file names exactly.

5. **Original Code**: The game logic is from 2009-2010 and has not been modernized. Some patterns may be outdated, but the code works as-is.

## Editing Content

To add or modify game assets:

### Using MonoGame Content Builder Editor (MGCB Editor)

1. Install the MGCB Editor tool:
   ```bash
   dotnet tool install -g dotnet-mgcb-editor
   mgcb-editor --register
   ```

2. Open the content project:
   ```bash
   mgcb-editor Content/Content.mgcb
   ```

3. Add/remove/edit assets using the GUI

### Manual Editing

Alternatively, edit `Content/Content.mgcb` directly to add new assets. Follow the existing pattern:

```
#begin YourFile.png
/importer:TextureImporter
/processor:TextureProcessor
/processorParam:ColorKeyColor=255,0,255,255
/processorParam:ColorKeyEnabled=True
/processorParam:GenerateMipmaps=False
/processorParam:PremultiplyAlpha=True
/processorParam:ResizeToPowerOfTwo=False
/processorParam:MakeSquare=False
/processorParam:TextureFormat=Color
/build:YourFile.png
```

## Troubleshooting

### Build Errors

- **"SDK not found"**: Make sure .NET 8.0 SDK is installed
- **"Package not found"**: Run `dotnet restore`
- **Content build failed**: Check that all files in Content.mgcb actually exist

### Runtime Errors

- **"Content file not found"**: Check file paths are case-sensitive correct
- **"SDL2 not found"** (Linux): Install SDL2 libraries (see prerequisites)
- **Audio not playing**: Check audio file formats and ensure audio device is available

## Resources

- **MonoGame Documentation**: https://docs.monogame.net/
- **MonoGame Community**: https://community.monogame.net/
- **MonoGame GitHub**: https://github.com/MonoGame/MonoGame

## Original Project

This project was originally created using XNA Game Studio 3.0 circa 2009-2010. The migration preserves all original game logic and assets while updating the framework to MonoGame for modern compatibility.

**Original Framework**: Microsoft XNA Game Studio 3.0
**Original Target**: .NET Framework 3.5
**Original Platform**: Windows only

**Migrated Framework**: MonoGame 3.8.2
**Current Target**: .NET 8.0
**Current Platforms**: Windows, Linux, macOS

---

## License

Please refer to the original project license if applicable.
