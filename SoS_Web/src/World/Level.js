/**
 * Level.js
 * Core level controller - manages objectives, events, spawning, and win conditions
 */
import Map from './Map.js';
import EventTrigger from './EventTrigger.js';
import EnemyFactory from '../Utils/EnemyFactory.js';

export default class Level {
    constructor(config, game) {
        this.config = config;
        this.game = game;
        this.id = config.id;
        this.name = config.name;

        // Level state
        this.map = null;
        this.enemies = [];
        this.objectives = [];
        this.events = [];
        this.completed = false;
        this.failed = false;
        this.startTime = 0;
        this.elapsed = 0;

        // Metrics for testing
        this.metrics = {
            enemiesKilled: 0,
            playerDeaths: 0,
            damageTaken: 0,
            shotsFired: 0,
            accuracy: 0,
            completionTime: 0
        };
    }

    /**
     * Load and initialize the level
     */
    async load() {
        console.log(`Loading level: ${this.name}`);

        // Create map
        this.map = new Map(this.game.assets);
        await this.loadMap();

        // Load enemies
        this.enemies = EnemyFactory.createFromLevelConfig(
            this.config.enemies,
            this.game.assets
        );

        // Setup objectives
        this.loadObjectives();

        // Setup event triggers
        this.loadEvents();

        // Show intro dialogue
        if (this.config.story && this.config.story.intro) {
            this.game.dialogueBox.show(this.config.story.intro, 'SYSTEM', 3000);
        }

        this.startTime = Date.now();
        console.log(`Level loaded: ${this.enemies.length} enemies`);
    }

    /**
     * Load map from config
     */
    async loadMap() {
        const mapConfig = this.config.map;

        // Set map dimensions
        this.map.width = mapConfig.width;
        this.map.height = mapConfig.height;
        this.map.tileSize = mapConfig.tileSize || 4;

        // Load walls
        if (mapConfig.walls) {
            this.map.walls = mapConfig.walls.map(w => ({
                x: w.x,
                y: w.y,
                w: w.width,
                h: w.height
            }));
        }

        // TODO: Support different layout types (procedural, grid, custom)
        // For now, just use provided walls
    }

    /**
     * Load objectives from config
     */
    loadObjectives() {
        this.objectives = this.config.objectives.map(obj => ({
            ...obj,
            completed: false
        }));
    }

    /**
     * Load event triggers from config
     */
    loadEvents() {
        if (this.config.events) {
            this.events = this.config.events.map(e => new EventTrigger(e, this));
        }

        // Trigger onStart events
        this.triggerEvent('onStart');
    }

    /**
     * Update level logic
     * @param {number} dt - Delta time
     */
    update(dt) {
        if (this.completed || this.failed) return;

        this.elapsed += dt;

        // Update enemies
        this.enemies = this.enemies.filter(e => !e.remove);
        this.enemies.forEach(e => e.update(dt, this.game));

        // Check objectives
        this.checkObjectives();

        // Check timer events
        this.triggerEvent('timer', { elapsed: this.elapsed });

        // Check win/lose conditions
        if (this.checkWinCondition()) {
            this.complete();
        }

        if (this.checkLoseCondition()) {
            this.fail();
        }
    }

    /**
     * Check if objectives are completed
     */
    checkObjectives() {
        this.objectives.forEach(obj => {
            if (obj.completed) return;

            switch (obj.type) {
                case 'killAll':
                    if (this.enemies.length === 0) {
                        obj.completed = true;
                        console.log('Objective complete: Kill all enemies');
                    }
                    break;

                case 'reachPoint':
                    const dx = this.game.player.x - obj.x;
                    const dy = this.game.player.y - obj.y;
                    const dist = Math.sqrt(dx * dx + dy * dy);
                    if (dist < 30) {
                        obj.completed = true;
                        console.log('Objective complete: Reached exit');
                    }
                    break;

                case 'survive':
                    if (this.elapsed >= obj.duration) {
                        obj.completed = true;
                        console.log('Objective complete: Survived');
                    }
                    break;

                case 'killBoss':
                    const bossAlive = this.enemies.some(e => e.isBoss);
                    if (!bossAlive) {
                        obj.completed = true;
                        console.log('Objective complete: Boss defeated');
                    }
                    break;
            }
        });
    }

    /**
     * Check if level is won
     * @returns {boolean}
     */
    checkWinCondition() {
        return this.objectives.every(obj => obj.completed);
    }

    /**
     * Check if level is lost
     * @returns {boolean}
     */
    checkLoseCondition() {
        // Player is dead (checked in Game.js)
        return false;
    }

    /**
     * Complete the level
     */
    complete() {
        if (this.completed) return;

        this.completed = true;
        this.metrics.completionTime = this.elapsed;

        console.log(`Level complete! Time: ${(this.elapsed / 1000).toFixed(2)}s`);

        // Show outro dialogue
        if (this.config.story && this.config.story.outro) {
            this.game.dialogueBox.show(this.config.story.outro, 'SYSTEM', 5000);
        }

        // Trigger end event
        this.triggerEvent('endLevel');
    }

    /**
     * Fail the level
     */
    fail() {
        if (this.failed) return;
        this.failed = true;
        console.log('Level failed');
    }

    /**
     * Reset level to initial state
     */
    async reset() {
        this.completed = false;
        this.failed = false;
        this.elapsed = 0;
        this.startTime = Date.now();
        this.metrics = {
            enemiesKilled: 0,
            playerDeaths: 0,
            damageTaken: 0,
            shotsFired: 0,
            accuracy: 0,
            completionTime: 0
        };

        // Reset events
        this.events.forEach(e => e.reset());

        // Reload level
        await this.load();
    }

    /**
     * Trigger an event
     * @param {string} eventType
     * @param {Object} data
     */
    triggerEvent(eventType, data = {}) {
        this.events.forEach(event => {
            if (event.check(eventType, data)) {
                event.execute(this.game);
            }
        });
    }

    /**
     * Spawn enemy during gameplay (from event trigger)
     * @param {string} type
     * @param {number} x
     * @param {number} y
     * @param {Object} config
     */
    spawnEnemy(type, x, y, config) {
        const enemy = EnemyFactory.create(type, x, y, config, this.game.assets);
        this.enemies.push(enemy);
        this.game.entities.push(enemy);
        console.log(`Spawned ${type} at (${x}, ${y})`);
    }

    /**
     * Record enemy killed (for metrics and events)
     */
    onEnemyKilled() {
        this.metrics.enemiesKilled++;
        this.triggerEvent('enemyKilled');
    }

    /**
     * Record player damage (for metrics and events)
     * @param {number} damage
     */
    onPlayerDamaged(damage) {
        this.metrics.damageTaken += damage;
        this.triggerEvent('playerDamaged', { damage });
    }

    /**
     * Record player death
     */
    onPlayerDeath() {
        this.metrics.playerDeaths++;
    }

    /**
     * Get spawn point for player
     * @returns {{x: number, y: number}}
     */
    getSpawnPoint() {
        return this.config.map.spawnPoint || { x: 100, y: 100 };
    }

    /**
     * Get metrics for testing/validation
     * @returns {Object}
     */
    getMetrics() {
        return {
            ...this.metrics,
            completionTime: this.elapsed,
            objectivesComplete: this.objectives.filter(o => o.completed).length,
            totalObjectives: this.objectives.length,
            success: this.completed,
            failed: this.failed
        };
    }
}
