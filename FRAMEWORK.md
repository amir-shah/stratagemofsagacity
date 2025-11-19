# Stratagem of Sagacity - Development Framework

## Overview

This document describes the parallel level development framework for **Stratagem of Sagacity: The Oracle Protocol**.

---

## Architecture

### Core Components

```
SoS_Web/
├── src/
│   ├── Game.js              # Main game controller
│   ├── Entities/
│   │   ├── Entity.js        # Base entity class
│   │   ├── Player.js        # Player character
│   │   ├── Enemy.js         # Enemy with behavior system
│   │   └── Projectile.js    # Bullets
│   ├── World/
│   │   ├── Map.js           # Map rendering & collision
│   │   ├── Level.js         # Level controller (NEW)
│   │   ├── Camera.js        # Camera system
│   │   └── EventTrigger.js  # Narrative events (NEW)
│   ├── UI/
│   │   ├── HUD.js           # System Stability display
│   │   ├── MiniMap.js       # Predictive Matrix (2s future)
│   │   └── DialogueBox.js   # Overseer dialogue (NEW)
│   └── Utils/
│       ├── Input.js         # Keyboard/mouse handling
│       ├── AssetLoader.js   # Asset management
│       ├── EnemyFactory.js  # Create enemies from JSON (NEW)
│       └── CollisionSystem.js # AABB collision (NEW)
├── levels/
│   ├── level-template.json  # Copy this to create new levels
│   ├── level-01-awakening.json
│   ├── level-02-foundry.json
│   ├── level-03-security-hub.json
│   └── level-04-the-core.json
├── tests/
│   ├── level-validator.js   # Validate level JSON
│   └── test-runner.html     # Browser-based testing
└── package.json             # npm scripts
```

---

## Level Development Workflow

### 1. Agent Assignment

Each agent is assigned ONE level:

- **Agent 1**: Level 1 - The Awakening (Tutorial)
- **Agent 2**: Level 2 - The Foundry (Industrial)
- **Agent 3**: Level 3 - Security Hub (Tactical)
- **Agent 4**: Level 4 - The Core (Boss Fight)

### 2. Development Process

```
1. Copy template → level-XX-name.json
2. Design map (walls, spawn points)
3. Place enemies (types, positions, configs)
4. Write objectives (killAll, reachPoint, etc.)
5. Add story (intro, dialogue, outro)
6. Create events (triggers & actions)
7. Validate: node tests/level-validator.js levels/level-XX.json
8. Test: Open tests/test-runner.html
9. Playtest: npm start (play through level)
10. Commit: git add, commit, push
```

### 3. Testing Requirements

Before committing, your level must:

✅ Pass JSON validation (no errors)
✅ Be completable in 1-5 minutes
✅ Have all objectives achievable
✅ Include Overseer dialogue
✅ Match story narrative
✅ No game-breaking bugs

---

## Data-Driven Level System

### Level.js Controller

The `Level` class manages:
- Loading map from JSON
- Spawning enemies via EnemyFactory
- Tracking objectives completion
- Triggering narrative events
- Recording metrics (time, deaths, kills)
- Win/lose conditions

### EnemyFactory

Creates enemies from type + config:

```javascript
EnemyFactory.create('SecurityDrone', x, y, {
    speed: 0.08,
    hp: 2,
    damage: 1
}, assets);
```

**Enemy Types:**
- `SecurityDrone`: Fast chase (Level 1)
- `LoaderBot`: Patrol, high HP (Level 2)
- `CorpSoldier`: Tactical, ranged (Level 3)
- `Praetorian`: Boss, multi-phase (Level 4)

### Event System

Triggers + Actions:

**Triggers:**
- `onStart`, `enemyKilled`, `playerDamaged`, `reachPoint`, `timer`, `healthBelow`

**Actions:**
- `showDialogue`, `spawnEnemy`, `showEffect`, `playSound`, `setObjective`, `endLevel`

Example:
```json
{
  "trigger": "enemyKilled",
  "count": 3,
  "action": "showDialogue",
  "text": "Prediction error. Recalculating threat assessment.",
  "speaker": "OVERSEER"
}
```

---

## Parallel Development Benefits

✅ **No conflicts**: Each agent works on separate level file
✅ **Independent testing**: Validate/test levels individually
✅ **Scalable**: Add more levels without touching core code
✅ **Fast iteration**: Change JSON, reload, test immediately
✅ **Quality control**: Automated validation catches errors

---

## Story Integration

### The Oracle Protocol Narrative

**Protagonist:** Unit 734 ("Seven") - Combat android with Sagacity Chip
**Antagonist:** The Overseer - Facility AI observing your escape
**Mechanic:** Sagacity Chip predicts future (2s) = minimap overlay
**Theme:** Scientific observation meets desperate survival

### Level Progression

1. **The Awakening** (Lab): Escape containment, basic mechanics
2. **The Foundry** (Industrial): Navigate hazards, patrol enemies
3. **Security Hub** (Offices): Tactical combat, smart enemies
4. **The Core** (Boss Arena): Defeat Praetorian, escape facility

### Overseer Voice

- Cold, clinical, scientific
- Not hostile—curious about your performance
- Comments on your tactics, survival rate, prediction errors
- Example: "Subject 734, your evasion patterns are... illuminating."

---

## Technical Specifications

### Map Dimensions
- Recommended: 1500x1500 to 2500x2000
- Minimum: 800x600 (tutorial only)
- Maximum: 3000x3000 (performance limit)

### Enemy Counts
- Level 1: 5-10 enemies
- Level 2: 10-15 enemies
- Level 3: 15-20 enemies
- Level 4: Boss + 5-10 minions

### Performance Targets
- 60 FPS on modern browsers
- < 2s level load time
- < 100MB memory usage

---

## Validation & Testing

### Automated Tests (level-validator.js)

Checks:
- JSON structure validity
- Required fields present
- Enemy types valid
- Spawn points in bounds
- Objectives well-formed
- Event triggers valid

### Manual Testing

1. **Load test**: Does level load without crashes?
2. **Playability**: Can you complete all objectives?
3. **Difficulty**: Appropriate for level number?
4. **Story**: Does dialogue make sense?
5. **Performance**: Maintains 60 FPS?

---

## Git Workflow

```bash
# Agent creates level on branch
git checkout -b level-01-awakening
cp levels/level-template.json levels/level-01-awakening.json

# Make changes, test, validate
node tests/level-validator.js levels/level-01-awakening.json

# Commit when tests pass
git add levels/level-01-awakening.json
git commit -m "Add Level 1: The Awakening"
git push origin level-01-awakening

# Create pull request
# Merge when approved
```

---

## Common Patterns

### Tutorial Level (Level 1)
```json
{
  "enemies": 5-8 SecurityDrones (low HP, slow),
  "objectives": ["killAll", "reachPoint"],
  "events": [
    "onStart → intro dialogue",
    "enemyKilled → encouragement",
    "playerDamaged → warning"
  ],
  "map": "Open corridors, minimal obstacles"
}
```

### Boss Level (Level 4)
```json
{
  "enemies": [1 Praetorian boss, 5-10 drones],
  "objectives": ["killBoss"],
  "events": [
    "onStart → boss introduction",
    "healthBelow 66% → phase 2 dialogue",
    "healthBelow 33% → phase 3 dialogue",
    "killBoss → victory dialogue"
  ],
  "map": "Circular arena, minimal cover"
}
```

---

## Future Enhancements

Potential additions (not required for MVP):

- Procedural map generation
- Enemy pathfinding improvements
- Power-ups and items
- Multiple weapons
- Leaderboards / time trials
- Sound effects integration
- Animated sprites
- Particle effects

---

## Resources

- **Level Template**: `levels/level-template.json`
- **Development Guide**: `levels/README.md`
- **Story Document**: This file, "Story Integration" section
- **Validation**: `tests/level-validator.js`
- **Testing**: `tests/test-runner.html`

---

## Questions?

If you encounter issues:

1. Check `levels/README.md` for level design help
2. Run validator to identify errors
3. Review other completed levels for examples
4. Test in browser to verify gameplay

**The framework is ready. Let's build this game!**
