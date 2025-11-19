/**
 * EventTrigger.js
 * Handles narrative events and gameplay triggers in levels
 */
export default class EventTrigger {
    constructor(config, level) {
        this.config = config;
        this.level = level;
        this.triggered = false;
        this.trigger = config.trigger; // onStart, enemyKilled, playerDamaged, reachPoint, timer
        this.action = config.action;   // showDialogue, spawnEnemy, showEffect, playSound
        this.condition = config.condition || null;
        this.count = config.count || 1;
        this.currentCount = 0;
    }

    /**
     * Check if trigger condition is met
     * @param {string} eventType - Event that occurred
     * @param {Object} data - Additional event data
     * @returns {boolean}
     */
    check(eventType, data = {}) {
        if (this.triggered) return false;
        if (eventType !== this.trigger) return false;

        // Check specific conditions
        switch (this.trigger) {
            case 'onStart':
                this.triggered = true;
                return true;

            case 'enemyKilled':
                this.currentCount++;
                if (this.currentCount >= this.count) {
                    this.triggered = true;
                    return true;
                }
                return false;

            case 'playerDamaged':
                this.triggered = true;
                return true;

            case 'reachPoint':
                const dx = data.playerX - this.config.x;
                const dy = data.playerY - this.config.y;
                const dist = Math.sqrt(dx * dx + dy * dy);
                if (dist < (this.config.radius || 50)) {
                    this.triggered = true;
                    return true;
                }
                return false;

            case 'timer':
                if (data.elapsed >= this.config.time) {
                    this.triggered = true;
                    return true;
                }
                return false;

            case 'healthBelow':
                if (data.health <= this.config.healthThreshold) {
                    this.triggered = true;
                    return true;
                }
                return false;

            default:
                return false;
        }
    }

    /**
     * Execute the trigger's action
     * @param {Game} game - Game instance
     */
    execute(game) {
        switch (this.action) {
            case 'showDialogue':
                this.showDialogue(game);
                break;

            case 'spawnEnemy':
                this.spawnEnemy(game);
                break;

            case 'showEffect':
                this.showEffect(game);
                break;

            case 'playSound':
                this.playSound(game);
                break;

            case 'setObjective':
                this.setObjective(game);
                break;

            case 'endLevel':
                this.endLevel(game);
                break;

            default:
                console.warn(`Unknown trigger action: ${this.action}`);
        }
    }

    showDialogue(game) {
        if (game.dialogueBox) {
            game.dialogueBox.show(this.config.text, this.config.speaker || 'OVERSEER');
        }
    }

    spawnEnemy(game) {
        // Spawn enemy from config
        const { enemyType, x, y, config } = this.config;
        if (game.level) {
            game.level.spawnEnemy(enemyType, x, y, config);
        }
    }

    showEffect(game) {
        // Trigger visual effects (glitch, flash, shake)
        const effect = this.config.effect;
        if (game.effectSystem) {
            game.effectSystem.trigger(effect);
        }
    }

    playSound(game) {
        const sound = this.config.sound;
        const audio = game.assets.getAudio(sound);
        if (audio) {
            audio.play();
        }
    }

    setObjective(game) {
        if (game.level) {
            game.level.setObjective(this.config.objective);
        }
    }

    endLevel(game) {
        if (game.level) {
            game.level.complete();
        }
    }

    reset() {
        this.triggered = false;
        this.currentCount = 0;
    }
}
