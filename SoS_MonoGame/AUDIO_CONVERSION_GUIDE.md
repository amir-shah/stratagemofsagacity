# Audio Conversion Guide

The game currently has background music from the WAV file, but to get **all the original audio** (multiple tracks + gunfire sounds), you need to convert the MP3 files to OGG format.

## Why OGG instead of MP3?

MonoGame on Windows has limited MP3 codec support. OGG Vorbis is:
- ✅ Fully supported by MonoGame on all platforms
- ✅ Open-source and royalty-free
- ✅ Better compression than WAV
- ✅ Similar quality to MP3

## Quick Conversion Methods

### Option 1: Using Audacity (Free, Easy, GUI)

1. **Download Audacity**: https://www.audacityteam.org/download/
2. **For each MP3 file**:
   - Open the MP3 in Audacity (File → Open)
   - Go to File → Export → Export as OGG
   - Choose quality (5-7 is good for game audio)
   - Save with the same name but .ogg extension
3. **Place OGG files** in `Content/` folder

### Option 2: Using ffmpeg (Command Line, Batch Convert)

1. **Download ffmpeg**: https://ffmpeg.org/download.html
2. **Navigate to Content folder**:
   ```powershell
   cd SoS_MonoGame\Content
   ```
3. **Convert all MP3s**:
   ```powershell
   # Convert each file
   ffmpeg -i "3-burst-Diode111-8773_hifi.mp3" -c:a libvorbis -q:a 5 "3-burst-Diode111-8773_hifi.ogg"
   ffmpeg -i "Final-calpomat-4566_hifi.mp3" -c:a libvorbis -q:a 5 "Final-calpomat-4566_hifi.ogg"
   ffmpeg -i "hellRaider.mp3" -c:a libvorbis -q:a 5 "hellRaider.ogg"
   ffmpeg -i "Moonli-SLrec-6885_hifi.mp3" -c:a libvorbis -q:a 5 "Moonli-SLrec-6885_hifi.ogg"
   ffmpeg -i "_Shif-Edgen-6832_hifi.mp3" -c:a libvorbis -q:a 5 "_Shif-Edgen-6832_hifi.ogg"
   ```

### Option 3: Using Online Converter (No Install)

1. Go to: https://cloudconvert.com/mp3-to-ogg
2. Upload each MP3 file
3. Download the converted OGG files
4. Place them in `Content/` folder

## Files to Convert

You have these MP3 files that need conversion:
- `3-burst-Diode111-8773_hifi.mp3` - Gunfire sound
- `Final-calpomat-4566_hifi.mp3` - Background music
- `hellRaider.mp3` - Background music
- `Moonli-SLrec-6885_hifi.mp3` - Background music
- `_Shif-Edgen-6832_hifi.mp3` - Background music

## After Conversion

### Step 1: Update Content.mgcb

Open `Content/Content.mgcb` and add these entries:

```
#begin 3-burst-Diode111-8773_hifi.ogg
/importer:OggImporter
/processor:SoundEffectProcessor
/processorParam:Quality=Best
/build:3-burst-Diode111-8773_hifi.ogg

#begin Final-calpomat-4566_hifi.ogg
/importer:OggImporter
/processor:SongProcessor
/processorParam:Quality=Best
/build:Final-calpomat-4566_hifi.ogg

#begin hellRaider.ogg
/importer:OggImporter
/processor:SongProcessor
/processorParam:Quality=Best
/build:hellRaider.ogg

#begin Moonli-SLrec-6885_hifi.ogg
/importer:OggImporter
/processor:SongProcessor
/processorParam:Quality=Best
/build:Moonli-SLrec-6885_hifi.ogg

#begin _Shif-Edgen-6832_hifi.ogg
/importer:OggImporter
/processor:SongProcessor
/processorParam:Quality=Best
/build:_Shif-Edgen-6832_hifi.ogg
```

### Step 2: Update Game1.cs

Replace the audio loading code in `Game1.cs`:

```csharp
// In the constructor or LoadContent:
backgroundMusic = Content.Load<Song>("Final-calpomat-4566_hifi");
MediaPlayer.Play(backgroundMusic);
MediaPlayer.IsRepeating = true;

gunfireSound = Content.Load<SoundEffect>("3-burst-Diode111-8773_hifi");

// In playGunfire() method:
public void playGunfire()
{
    gunfireSound.Play(0.3f, 0.0f, 0.0f); // volume, pitch, pan
}
```

### Step 3: Rebuild and Test

```powershell
dotnet clean
dotnet build
dotnet run
```

## Troubleshooting

**"Could not find ContentImporter for .ogg"**
- Make sure you used `OggImporter` not `Mp3Importer`

**No sound playing**
- Check Windows volume mixer
- Verify OGG files aren't corrupted (try playing them in VLC)

**Sound too loud/quiet**
- Adjust the first parameter in `.Play()` calls (0.0 = silent, 1.0 = full volume)

**Want looping background music?**
```csharp
MediaPlayer.IsRepeating = true;
```

---

**Current Status**: Background music playing from WAV file ✅
**To Enable Full Audio**: Follow this guide to convert and enable MP3→OGG files
