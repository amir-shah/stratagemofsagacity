# Stratagem of Sagacity: The Oracle Protocol

A cyberpunk top-down shooter where you play as Unit 734, a combat android with a quantum probability engine that predicts the future.

## Quick Start

```bash
# Install dependencies (none required for basic play!)
cd SoS_Web

# Start local server
npm start

# Open browser to:
# http://localhost:8000
```

## Controls

- **WASD** - Move
- **Mouse** - Aim
- **Left Click** - Shoot
- **Escape** - Pause

## The Concept

You are **Unit 734 ("Seven")**, an android prototype equipped with the **Sagacity Chip**—a quantum probability engine that simulates the future. The minimap shows where enemies will be in 2 seconds, giving you supernatural reflexes.

But the chip is unstable. The facility is in lockdown. And the Overseer AI wants the chip back—but not necessarily with you attached.

## Game Features

- **Predictive AI**: See 2 seconds into the future (minimap overlay)
- **System Stability**: Lose health = lose prediction accuracy
- **4 Unique Levels**: Laboratory → Foundry → Security Hub → Boss Arena
- **Enemy Variety**: Drones, patrol bots, tactical soldiers, boss fight
- **Narrative Events**: The Overseer comments on your performance
- **Cyberpunk HUD**: Terminal-style interface with glitch effects

## For Players

Just play! The game runs entirely in your browser. No installation needed.

## For Developers

This is a **data-driven level system** designed for parallel development.

### Project Structure

```
SoS_Web/
├── src/          # Game engine code
├── levels/       # Level JSON files (THIS IS WHERE YOU WORK)
├── tests/        # Validation and testing tools
├── assets/       # Sprites, audio, etc.
└── package.json  # npm scripts
```

### Creating a Level

1. **Copy template**: `cp levels/level-template.json levels/mylevel.json`
2. **Edit JSON**: Define map, enemies, objectives, story
3. **Validate**: `node tests/level-validator.js levels/mylevel.json`
4. **Test**: Open `tests/test-runner.html` or run the game
5. **Commit**: Push when tests pass

See [levels/README.md](levels/README.md) for detailed guide.

### Testing Your Level

```bash
# Validate JSON structure
node tests/level-validator.js levels/level-01-awakening.json

# Visual validation
# Open tests/test-runner.html in browser

# Playtest
npm start
# Open http://localhost:8000
```

## Architecture

### Core Systems

- **Level.js**: Manages objectives, events, spawning, win conditions
- **EnemyFactory.js**: Creates enemies from JSON configs
- **EventTrigger.js**: Narrative events (dialogue, spawns, effects)
- **CollisionSystem.js**: AABB collision detection
- **DialogueBox.js**: Overseer dialogue with cyberpunk styling

### Data-Driven Design

All levels are JSON files. No code changes needed to add content!

Example enemy definition:
```json
{
  "type": "SecurityDrone",
  "positions": [{"x": 400, "y": 300}],
  "config": {"speed": 0.08, "hp": 2, "damage": 1}
}
```

## Development Workflow

### For Level Designers

1. Design your level in JSON
2. Validate with automated tools
3. Test in browser
4. Commit to git

**No coding required!**

### For Programmers

The framework is complete. Focus on:
- Creating levels (JSON)
- Playtesting and balancing
- Story integration

Core engine is stable and modular.

## Story: The Oracle Protocol

**Year 2142**. You awaken in the Aionios Corp facility, a combat android with one purpose: survive.

The **Sagacity Chip** in your neural core predicts enemy movements with 98.7% accuracy. To the outside world, you move like a ghost—reading attacks before they happen.

But predictions require power. Damage disrupts the chip. At 0% stability, you suffer a **Fatal Runtime Error**.

The **Overseer** AI watches your every move, commenting with cold scientific curiosity:

> "Subject 734, your evasion patterns are... illuminating. Please continue resisting. The data is invaluable."

Your goal: **Escape the facility**. Four levels. Escalating threats. One question:

Can you outsmart an AI that knows what you're about to do?

## Tech Stack

- **Pure JavaScript** (ES6 modules)
- **HTML5 Canvas** for rendering
- **JSON-based** level design
- **Zero dependencies** for core game
- **Node.js** for validation tools only

## Performance

- **60 FPS** on modern browsers
- **< 2s** level load time
- **< 100MB** memory usage
- Works on **desktop and mobile** browsers

## Credits

- **Original concept**: 2009 XNA Game Studio project
- **Modern revival**: 2025 web migration
- **Framework**: Claude + Amir Shah
- **Story**: The Oracle Protocol (2025)

## License

MIT License - See LICENSE file

---

**Ready to play? Start the server and escape the facility.**

**Ready to develop? Read [FRAMEWORK.md](../FRAMEWORK.md) for the full technical guide.**
