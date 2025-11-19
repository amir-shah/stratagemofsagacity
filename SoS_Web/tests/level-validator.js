/**
 * level-validator.js
 * Validates level JSON files for correctness
 * Run with: node tests/level-validator.js levels/level-01-awakening.json
 */

import fs from 'fs';
import path from 'path';

const VALID_ENEMY_TYPES = ['SecurityDrone', 'LoaderBot', 'CorpSoldier', 'Praetorian'];
const VALID_SPAWN_TYPES = ['immediate', 'timed', 'wave'];
const VALID_OBJECTIVE_TYPES = ['killAll', 'reachPoint', 'survive', 'killBoss'];
const VALID_TRIGGER_TYPES = ['onStart', 'enemyKilled', 'playerDamaged', 'reachPoint', 'timer', 'healthBelow'];
const VALID_ACTION_TYPES = ['showDialogue', 'spawnEnemy', 'showEffect', 'playSound', 'setObjective', 'endLevel'];

class LevelValidator {
    constructor(levelPath) {
        this.levelPath = levelPath;
        this.errors = [];
        this.warnings = [];
        this.level = null;
    }

    async validate() {
        console.log(`\n🔍 Validating: ${this.levelPath}`);
        console.log('='.repeat(60));

        // Load JSON
        if (!this.loadJSON()) {
            return false;
        }

        // Run validation checks
        this.validateStructure();
        this.validateMap();
        this.validateEnemies();
        this.validateObjectives();
        this.validateEvents();
        this.validateStory();

        // Report results
        this.report();

        return this.errors.length === 0;
    }

    loadJSON() {
        try {
            const data = fs.readFileSync(this.levelPath, 'utf8');
            this.level = JSON.parse(data);
            return true;
        } catch (err) {
            this.errors.push(`Failed to load JSON: ${err.message}`);
            return false;
        }
    }

    validateStructure() {
        const required = ['id', 'name', 'description', 'map', 'enemies', 'objectives'];
        required.forEach(field => {
            if (!this.level[field]) {
                this.errors.push(`Missing required field: ${field}`);
            }
        });
    }

    validateMap() {
        const map = this.level.map;
        if (!map) return;

        if (!map.width || !map.height) {
            this.errors.push('Map missing width or height');
        }

        if (map.width < 500 || map.height < 500) {
            this.warnings.push('Map very small (< 500x500), may be too cramped');
        }

        if (map.width > 5000 || map.height > 5000) {
            this.warnings.push('Map very large (> 5000x5000), may have performance issues');
        }

        if (!map.spawnPoint) {
            this.errors.push('Map missing spawnPoint');
        } else {
            if (map.spawnPoint.x < 0 || map.spawnPoint.x > map.width ||
                map.spawnPoint.y < 0 || map.spawnPoint.y > map.height) {
                this.errors.push('Spawn point outside map bounds');
            }
        }

        if (!map.walls || map.walls.length === 0) {
            this.warnings.push('Map has no walls (no boundaries?)');
        }
    }

    validateEnemies() {
        const enemies = this.level.enemies;
        if (!enemies || enemies.length === 0) {
            this.warnings.push('No enemies defined');
            return;
        }

        enemies.forEach((enemyGroup, idx) => {
            if (!enemyGroup.type) {
                this.errors.push(`Enemy group ${idx}: Missing type`);
            } else if (!VALID_ENEMY_TYPES.includes(enemyGroup.type)) {
                this.errors.push(`Enemy group ${idx}: Invalid type "${enemyGroup.type}"`);
            }

            if (!enemyGroup.positions || enemyGroup.positions.length === 0) {
                this.errors.push(`Enemy group ${idx}: No spawn positions`);
            } else {
                enemyGroup.positions.forEach((pos, posIdx) => {
                    if (pos.x < 0 || pos.x > this.level.map.width ||
                        pos.y < 0 || pos.y > this.level.map.height) {
                        this.errors.push(`Enemy group ${idx}, position ${posIdx}: Outside map bounds`);
                    }
                });
            }

            if (enemyGroup.spawn && !VALID_SPAWN_TYPES.includes(enemyGroup.spawn)) {
                this.warnings.push(`Enemy group ${idx}: Unknown spawn type "${enemyGroup.spawn}"`);
            }
        });
    }

    validateObjectives() {
        const objectives = this.level.objectives;
        if (!objectives || objectives.length === 0) {
            this.errors.push('No objectives defined');
            return;
        }

        objectives.forEach((obj, idx) => {
            if (!obj.type) {
                this.errors.push(`Objective ${idx}: Missing type`);
            } else if (!VALID_OBJECTIVE_TYPES.includes(obj.type)) {
                this.errors.push(`Objective ${idx}: Invalid type "${obj.type}"`);
            }

            if (!obj.description) {
                this.warnings.push(`Objective ${idx}: Missing description`);
            }

            if (obj.type === 'reachPoint' && (!obj.x || !obj.y)) {
                this.errors.push(`Objective ${idx}: reachPoint missing x or y`);
            }

            if (obj.type === 'survive' && !obj.duration) {
                this.errors.push(`Objective ${idx}: survive missing duration`);
            }
        });
    }

    validateEvents() {
        const events = this.level.events;
        if (!events) return;

        events.forEach((event, idx) => {
            if (!event.trigger) {
                this.errors.push(`Event ${idx}: Missing trigger`);
            } else if (!VALID_TRIGGER_TYPES.includes(event.trigger)) {
                this.warnings.push(`Event ${idx}: Unknown trigger type "${event.trigger}"`);
            }

            if (!event.action) {
                this.errors.push(`Event ${idx}: Missing action`);
            } else if (!VALID_ACTION_TYPES.includes(event.action)) {
                this.warnings.push(`Event ${idx}: Unknown action type "${event.action}"`);
            }

            if (event.action === 'showDialogue' && !event.text) {
                this.errors.push(`Event ${idx}: showDialogue missing text`);
            }

            if (event.trigger === 'reachPoint' && (!event.x || !event.y)) {
                this.errors.push(`Event ${idx}: reachPoint trigger missing x or y`);
            }
        });
    }

    validateStory() {
        const story = this.level.story;
        if (!story) {
            this.warnings.push('No story section defined');
            return;
        }

        if (!story.intro) {
            this.warnings.push('No intro dialogue');
        }

        if (!story.outro) {
            this.warnings.push('No outro dialogue');
        }
    }

    report() {
        console.log('\n📊 VALIDATION RESULTS:');
        console.log('-'.repeat(60));

        if (this.errors.length === 0 && this.warnings.length === 0) {
            console.log('✅ Perfect! No errors or warnings.');
        }

        if (this.errors.length > 0) {
            console.log(`\n❌ ERRORS (${this.errors.length}):`);
            this.errors.forEach((err, idx) => {
                console.log(`  ${idx + 1}. ${err}`);
            });
        }

        if (this.warnings.length > 0) {
            console.log(`\n⚠️  WARNINGS (${this.warnings.length}):`);
            this.warnings.forEach((warn, idx) => {
                console.log(`  ${idx + 1}. ${warn}`);
            });
        }

        console.log('\n' + '='.repeat(60));
        if (this.errors.length === 0) {
            console.log('✅ LEVEL VALID - Ready to test!\n');
        } else {
            console.log('❌ LEVEL INVALID - Fix errors before testing\n');
        }
    }
}

// CLI Interface
if (import.meta.url === `file://${process.argv[1]}`) {
    const levelPath = process.argv[2];

    if (!levelPath) {
        console.error('Usage: node level-validator.js <path-to-level.json>');
        process.exit(1);
    }

    const validator = new LevelValidator(levelPath);
    validator.validate().then(success => {
        process.exit(success ? 0 : 1);
    });
}

export default LevelValidator;
