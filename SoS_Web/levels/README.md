# Level Development Guide

This directory contains all game levels for **Stratagem of Sagacity: The Oracle Protocol**.

## Quick Start

1. **Copy the template**: `cp level-template.json level-XX-yourname.json`
2. **Edit your level**: Fill in story, map, enemies, objectives
3. **Validate**: `node tests/level-validator.js levels/level-XX-yourname.json`
4. **Test**: Open `tests/test-runner.html` in browser or run the game
5. **Commit**: Push to your branch when tests pass

---

## Level Structure

Each level is a JSON file with these sections:

### 1. Metadata
```json
{
  "id": "level-01-awakening",
  "name": "The Awakening",
  "description": "Escape the containment chamber"
}
```

### 2. Story
```json
{
  "story": {
    "intro": "Text shown at start",
    "overseerDialogue": ["Line 1", "Line 2"],
    "outro": "Text when complete"
  }
}
```

### 3. Map
```json
{
  "map": {
    "width": 2000,
    "height": 1600,
    "tileSize": 4,
    "theme": "laboratory",
    "spawnPoint": {"x": 100, "y": 100},
    "walls": [
      {"x": 0, "y": 0, "width": 2000, "height": 20}
    ]
  }
}
```

**Tips:**
- Keep width/height between 1000-3000 for good pacing
- Always include boundary walls (top, left, bottom, right)
- Spawn point should be in a safe area

### 4. Enemies
```json
{
  "enemies": [
    {
      "type": "SecurityDrone",
      "positions": [{"x": 400, "y": 300}],
      "config": {"speed": 0.08, "hp": 2, "damage": 1}
    }
  ]
}
```

**Enemy Types:**
- **SecurityDrone**: Fast, basic chase behavior (Level 1)
- **LoaderBot**: Slow, patrols area, high HP (Level 2)
- **CorpSoldier**: Tactical, uses cover, shoots from range (Level 3)
- **Praetorian**: Boss, multi-phase, heavy armor (Level 4)

### 5. Objectives
```json
{
  "objectives": [
    {
      "type": "killAll",
      "description": "Eliminate all security drones"
    },
    {
      "type": "reachPoint",
      "x": 1900,
      "y": 1500,
      "description": "Reach the exit"
    }
  ]
}
```

**Objective Types:**
- `killAll`: Defeat all enemies
- `reachPoint`: Get to location (x, y)
- `survive`: Last N milliseconds
- `killBoss`: Defeat the boss enemy

### 6. Events
```json
{
  "events": [
    {
      "trigger": "enemyKilled",
      "count": 3,
      "action": "showDialogue",
      "text": "Prediction error. Recalculating..."
    }
  ]
}
```

**Triggers:**
- `onStart`: Level begins
- `enemyKilled`: Enemy dies (use `count` for specific number)
- `playerDamaged`: Player takes damage
- `reachPoint`: Player near location (x, y, radius)
- `timer`: After N milliseconds
- `healthBelow`: Health drops below threshold

**Actions:**
- `showDialogue`: Display text
- `spawnEnemy`: Add enemy mid-level
- `showEffect`: Visual effect (glitch, flash)
- `playSound`: Audio cue
- `setObjective`: Update objectives
- `endLevel`: Force level complete

---

## Testing Your Level

### Step 1: Validate JSON
```bash
node tests/level-validator.js levels/level-XX-yourname.json
```

Fix all errors before proceeding.

### Step 2: Visual Test
Open `tests/test-runner.html` in browser:
1. Click "Choose File" → select your level JSON
2. Click "Load & Validate"
3. Review validation results

### Step 3: Playtest
Run the game and play your level:
```bash
npm start
# Open http://localhost:8000
```

Check:
- Can you complete all objectives?
- Is difficulty appropriate?
- Do events trigger correctly?
- Is dialogue clear and engaging?

### Step 4: Metrics
Expected metrics (from `validation` section):
- **minPlayTime**: 30-60s minimum
- **maxPlayTime**: 120-300s maximum
- **expectedDeaths**: 1-3 for casual difficulty
- **difficultyRating**: 1 (easy) to 5 (boss level)

---

## Level Design Guidelines

### Pacing
- **Level 1** (Tutorial): 5-10 enemies, simple layout, lots of dialogue
- **Level 2** (Build-up): 10-15 enemies, more complex layout, introduce new mechanics
- **Level 3** (Climax): 15-20 enemies, tactical challenge, environmental hazards
- **Level 4** (Boss): Boss fight + minions, arena style, multi-phase

### Enemy Placement
- Start with 2-3 enemies visible from spawn
- Add challenge gradually (waves, ambushes)
- Don't spawn enemies on top of player
- Give player space to retreat

### Overseer Dialogue
The Overseer is:
- Cold and clinical ("Subject 734...")
- Scientifically curious ("Fascinating...")
- Not hostile, just observing ("Please continue resisting. The data is invaluable.")

Examples:
- "Prediction error detected. Recalculating threat assessment."
- "Your survival rate has exceeded projections by 34.7%."
- "Subject 734, your evasion patterns are... illuminating."

### Map Design
Use walls to create:
- Corridors (funneling)
- Rooms (arena fights)
- Cover (tactical positioning)
- Chokepoints (defensive stands)

---

## Common Issues

### "Spawn point outside map bounds"
- Ensure `spawnPoint.x < map.width` and `spawnPoint.y < map.height`

### "No boundary walls"
- Add walls at edges: x=0, y=0, x=width-20, y=height-20

### "Enemy positions outside map"
- Check all `positions` are within map dimensions

### "Level too easy/hard"
- Adjust enemy `hp`, `speed`, `damage`
- Change enemy count
- Modify map layout (more/less cover)

---

## Level Checklist

Before submitting, verify:

- [ ] JSON validates without errors
- [ ] All objectives are achievable
- [ ] Story dialogue flows logically
- [ ] Enemy placements feel fair
- [ ] Playable in 1-5 minutes
- [ ] Tested at least 3 times
- [ ] No game-breaking bugs
- [ ] Fits The Oracle Protocol narrative

---

## Getting Help

- **Validation errors**: Check template structure
- **Gameplay issues**: Adjust enemy configs
- **Story questions**: Refer to main story doc
- **Technical problems**: Check Game.js or ask for help

Good luck, and remember: **The Overseer is watching your progress with great interest.**
