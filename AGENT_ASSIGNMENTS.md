# Agent Level Development Assignments

## Overview

The framework is complete and committed. We're now ready to spin up **4 parallel agents** to develop the 4 game levels simultaneously.

Each agent will work on their assigned level independently, with zero code conflicts.

---

## Agent Assignments

### Agent 1: Level 1 - The Awakening (Tutorial)

**Branch:** `level-01-awakening`

**File:** `SoS_Web/levels/level-01-awakening.json`

**Objective:** Create the tutorial level that introduces core mechanics.

**Requirements:**
- **Theme:** Sterile laboratory, red alarms, broken glass
- **Enemies:** 5-8 SecurityDrones (basic chase behavior)
- **Map:** 1500x1200, simple corridors
- **Objectives:**
  1. Kill all enemies
  2. Reach exit at (1400, 1100)
- **Dialogue:**
  - Intro: "INITIALIZING UNIT 734... SAGACITY CHIP SYNC: 98.7%"
  - On first kill: "Subject 734, activation confirmed."
  - On damage: "Warning: System Stability compromised."
  - Outro: "LOCKDOWN OVERRIDE SUCCESSFUL. PROCEEDING TO SECTOR 2."
- **Events:**
  - onStart → intro dialogue
  - enemyKilled (count: 1) → Overseer acknowledgment
  - playerDamaged → warning message
  - reachPoint (exit) → level complete

**Enemy Config:**
```json
{
  "type": "SecurityDrone",
  "positions": [
    {"x": 400, "y": 300},
    {"x": 600, "y": 400},
    {"x": 800, "y": 500},
    {"x": 1000, "y": 300},
    {"x": 1200, "y": 600}
  ],
  "config": {
    "speed": 0.06,
    "hp": 2,
    "damage": 10
  }
}
```

**Validation:** Should complete in 60-120 seconds, 0-2 expected deaths.

---

### Agent 2: Level 2 - The Foundry (Industrial)

**Branch:** `level-02-foundry`

**File:** `SoS_Web/levels/level-02-foundry.json`

**Objective:** Industrial sector with patrol enemies and environmental hazards.

**Requirements:**
- **Theme:** Dark corridors, steam, machinery, orange warning lights
- **Enemies:** 3-5 LoaderBots (patrol behavior) + 5-7 SecurityDrones
- **Map:** 2000x1600, more complex layout with chokepoints
- **Objectives:**
  1. Kill all LoaderBots
  2. Survive for 90 seconds
  3. Reach elevator at (1900, 1500)
- **Dialogue:**
  - Intro: "SECTOR 2: FOUNDRY. HEAT SIGNATURES DETECTED."
  - Mid-level: "The Overseer notes your tactics are... efficient."
  - Boss killed: "Loader units neutralized. Impressive."
  - Outro: "ELEVATOR ACCESS GRANTED. ASCENDING TO SECURITY HUB."
- **Events:**
  - onStart → intro dialogue
  - enemyKilled (count: 3) → Overseer comment
  - timer (60000ms) → spawn 2 more drones
  - healthBelow (50) → "System degradation detected."

**Enemy Config:**
```json
[
  {
    "type": "LoaderBot",
    "positions": [
      {"x": 600, "y": 400},
      {"x": 1000, "y": 800},
      {"x": 1400, "y": 600}
    ],
    "config": {
      "speed": 0.03,
      "hp": 5,
      "damage": 20,
      "patrolRadius": 150
    }
  },
  {
    "type": "SecurityDrone",
    "positions": [
      {"x": 400, "y": 200},
      {"x": 800, "y": 600},
      {"x": 1200, "y": 1000},
      {"x": 1600, "y": 400}
    ],
    "config": {
      "speed": 0.08,
      "hp": 2,
      "damage": 10
    }
  }
]
```

**Validation:** Should complete in 90-180 seconds, 1-3 expected deaths.

---

### Agent 3: Level 3 - Security Hub (Tactical)

**Branch:** `level-03-security-hub`

**File:** `SoS_Web/levels/level-03-security-hub.json`

**Objective:** Tactical combat against intelligent enemies in office environment.

**Requirements:**
- **Theme:** Corporate offices, server racks, blue holographic displays
- **Enemies:** 10-15 CorpSoldiers (tactical, ranged attacks)
- **Map:** 2200x1800, open areas with cover (wall clusters)
- **Objectives:**
  1. Reach server room at (1100, 900)
  2. Hold position for 30 seconds
  3. Eliminate all remaining enemies
- **Dialogue:**
  - Intro: "SECURITY HUB BREACH DETECTED. DEPLOYING TACTICAL UNITS."
  - Mid-fight: "Your survival rate exceeds projections by 34.7%."
  - Server reached: "Upload initiated... Overseer sensor grid: BLINDED."
  - Outro: "SECURITY PROTOCOLS OVERRIDDEN. CORE ACCESS UNLOCKED."
- **Events:**
  - onStart → intro + spawn first wave (5 soldiers)
  - reachPoint (server) → dialogue + spawn second wave (5 soldiers)
  - timer (30000ms after reach) → objective complete dialogue
  - enemyKilled (count: 10) → "Prediction error. Recalculating threat model."

**Enemy Config:**
```json
{
  "type": "CorpSoldier",
  "spawn": "wave",
  "positions": [
    {"x": 400, "y": 400},
    {"x": 800, "y": 600},
    {"x": 1200, "y": 400},
    {"x": 1600, "y": 800},
    {"x": 1800, "y": 1200}
  ],
  "config": {
    "speed": 0.10,
    "hp": 4,
    "damage": 15,
    "shootRange": 250,
    "shootDelay": 1200
  }
}
```

**Validation:** Should complete in 120-240 seconds, 2-4 expected deaths.

---

### Agent 4: Level 4 - The Core (Boss Fight)

**Branch:** `level-04-the-core`

**File:** `SoS_Web/levels/level-04-the-core.json`

**Objective:** Epic boss fight against the Praetorian mech.

**Requirements:**
- **Theme:** Circular arena surrounding glowing quantum core
- **Enemies:** 1 Praetorian (boss) + waves of 3-5 support drones
- **Map:** 1800x1800, circular arena, minimal cover
- **Objectives:**
  1. Defeat the Praetorian
- **Dialogue:**
  - Intro: "CORE CHAMBER. FINAL DEFENSE PROTOCOL ACTIVATED."
  - Boss spawn: "Deploying Praetorian-class unit. Your escape probability: 12.3%."
  - Phase 2 (66% HP): "Interesting. Adapting combat parameters."
  - Phase 3 (33% HP): "You are... remarkable, Unit 734."
  - Victory: "Praetorian defeated. Escape route: OPEN. Farewell, Seven."
- **Events:**
  - onStart → boss introduction dialogue
  - healthBelow (boss, 66) → phase 2 transition + spawn 3 drones
  - healthBelow (boss, 33) → phase 3 transition + spawn 5 drones
  - killBoss → victory dialogue + level complete

**Enemy Config:**
```json
[
  {
    "type": "Praetorian",
    "positions": [{"x": 900, "y": 900}],
    "config": {
      "speed": 0.05,
      "hp": 50,
      "damage": 25
    }
  },
  {
    "type": "SecurityDrone",
    "spawn": "wave",
    "wave": 2,
    "positions": [
      {"x": 600, "y": 600},
      {"x": 1200, "y": 600},
      {"x": 900, "y": 1200}
    ],
    "config": {
      "speed": 0.08,
      "hp": 2,
      "damage": 10
    }
  }
]
```

**Validation:** Should complete in 180-360 seconds, 3-5 expected deaths.

---

## Workflow for Each Agent

### Step 1: Create Branch and File
```bash
git checkout -b level-01-awakening
cp SoS_Web/levels/level-template.json SoS_Web/levels/level-01-awakening.json
```

### Step 2: Design Your Level
Edit the JSON file with:
- Map dimensions and walls
- Enemy placements
- Objectives
- Story dialogue
- Event triggers

### Step 3: Validate
```bash
node SoS_Web/tests/level-validator.js SoS_Web/levels/level-01-awakening.json
```

Fix all errors before continuing.

### Step 4: Visual Check
Open `SoS_Web/tests/test-runner.html` in browser:
- Load your JSON file
- Review validation results
- Check for warnings

### Step 5: Commit
```bash
git add SoS_Web/levels/level-01-awakening.json
git commit -m "Add Level 1: The Awakening"
git push origin level-01-awakening
```

### Step 6: Report Completion
Post in the shared channel:
- "Level X complete"
- Validation results (pass/warnings)
- Playtime estimate
- Any issues encountered

---

## Success Criteria

Your level is complete when:

✅ JSON validation passes with 0 errors
✅ All objectives are achievable
✅ Story dialogue flows naturally
✅ Playtime within target range
✅ Difficulty appropriate for level number
✅ Committed and pushed to git

---

## Common Issues & Solutions

**"Spawn point outside map bounds"**
- Ensure spawnPoint coordinates are within map width/height

**"Enemy positions outside map"**
- Check all enemy x/y values are valid

**"No boundary walls"**
- Add walls at x=0, y=0, x=width-20, y=height-20

**"Level too easy/hard"**
- Adjust enemy HP, speed, damage
- Change enemy count
- Modify map layout (more/less open space)

---

## Communication

If you get stuck:
1. Check `SoS_Web/levels/README.md` for detailed guidance
2. Review `FRAMEWORK.md` for technical details
3. Look at other completed levels for examples
4. Ask for help in the coordination channel

---

## Timeline

**Target:** 4 levels completed in parallel

**Expected time per agent:** 2-4 hours

**Deliverable:** 4 playable, tested, committed levels

---

## Ready to Start?

1. Read your assignment above
2. Create your branch
3. Copy the template
4. Design your level
5. Validate and test
6. Commit and push
7. Report completion

**The framework is ready. Let's build this game!**
