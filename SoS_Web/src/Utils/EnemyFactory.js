/**
 * EnemyFactory.js
 * Creates enemy instances from type definitions and configurations
 */
import Enemy from '../Entities/Enemy.js';

export default class EnemyFactory {
    /**
     * Create an enemy from type and configuration
     * @param {string} type - Enemy type (SecurityDrone, LoaderBot, CorpSoldier, Praetorian)
     * @param {number} x - X position
     * @param {number} y - Y position
     * @param {Object} config - Enemy-specific configuration
     * @param {AssetLoader} assets - Asset loader instance
     * @returns {Enemy}
     */
    static create(type, x, y, config, assets) {
        switch (type) {
            case 'SecurityDrone':
                return this.createSecurityDrone(x, y, config, assets);

            case 'LoaderBot':
                return this.createLoaderBot(x, y, config, assets);

            case 'CorpSoldier':
                return this.createCorpSoldier(x, y, config, assets);

            case 'Praetorian':
                return this.createPraetorian(x, y, config, assets);

            default:
                console.warn(`Unknown enemy type: ${type}, creating default Enemy`);
                return new Enemy(x, y, assets);
        }
    }

    /**
     * Security Drone - Basic enemy, fast and predictable
     */
    static createSecurityDrone(x, y, config, assets) {
        const enemy = new Enemy(x, y, assets);
        enemy.type = 'SecurityDrone';
        enemy.speed = config.speed || 0.08;
        enemy.hp = config.hp || 2;
        enemy.damage = config.damage || 1;
        enemy.width = 24;
        enemy.height = 24;
        enemy.behavior = 'chase'; // Simple chase behavior
        return enemy;
    }

    /**
     * Loader Bot - Slow, heavy, patrols an area
     */
    static createLoaderBot(x, y, config, assets) {
        const enemy = new Enemy(x, y, assets);
        enemy.type = 'LoaderBot';
        enemy.speed = config.speed || 0.03;
        enemy.hp = config.hp || 5;
        enemy.damage = config.damage || 2;
        enemy.width = 40;
        enemy.height = 40;
        enemy.behavior = 'patrol';
        enemy.patrolRadius = config.patrolRadius || 150;
        enemy.patrolCenter = { x, y };
        enemy.patrolAngle = 0;
        return enemy;
    }

    /**
     * Corp Soldier - Fast, uses cover, more intelligent
     */
    static createCorpSoldier(x, y, config, assets) {
        const enemy = new Enemy(x, y, assets);
        enemy.type = 'CorpSoldier';
        enemy.speed = config.speed || 0.12;
        enemy.hp = config.hp || 4;
        enemy.damage = config.damage || 2;
        enemy.width = 28;
        enemy.height = 28;
        enemy.behavior = 'tactical'; // Uses cover, flanks
        enemy.shootRange = config.shootRange || 200;
        enemy.shootCooldown = 0;
        enemy.shootDelay = config.shootDelay || 1500; // ms between shots
        return enemy;
    }

    /**
     * Praetorian - Boss enemy, multi-phase, heavy armor
     */
    static createPraetorian(x, y, config, assets) {
        const enemy = new Enemy(x, y, assets);
        enemy.type = 'Praetorian';
        enemy.speed = config.speed || 0.05;
        enemy.hp = config.hp || 50;
        enemy.maxHp = enemy.hp;
        enemy.damage = config.damage || 3;
        enemy.width = 64;
        enemy.height = 64;
        enemy.behavior = 'boss';
        enemy.phase = 1;
        enemy.attackPattern = 'sweep'; // Changes per phase
        enemy.isBoss = true;
        return enemy;
    }

    /**
     * Create multiple enemies from spawn configuration
     * @param {Array} enemyConfigs - Array of enemy spawn configs from level JSON
     * @param {AssetLoader} assets
     * @returns {Array<Enemy>}
     */
    static createFromLevelConfig(enemyConfigs, assets) {
        const enemies = [];

        for (const enemyGroup of enemyConfigs) {
            const { type, positions, config, spawn } = enemyGroup;

            // Skip wave-spawned enemies (they spawn during gameplay)
            if (spawn === 'wave') continue;

            for (const pos of positions) {
                const enemy = this.create(type, pos.x, pos.y, config, assets);
                enemies.push(enemy);
            }
        }

        return enemies;
    }

    /**
     * Get default configuration for enemy type
     * @param {string} type
     * @returns {Object}
     */
    static getDefaultConfig(type) {
        const configs = {
            SecurityDrone: { speed: 0.08, hp: 2, damage: 1 },
            LoaderBot: { speed: 0.03, hp: 5, damage: 2, patrolRadius: 150 },
            CorpSoldier: { speed: 0.12, hp: 4, damage: 2, shootRange: 200, shootDelay: 1500 },
            Praetorian: { speed: 0.05, hp: 50, damage: 3 }
        };

        return configs[type] || { speed: 0.1, hp: 3, damage: 1 };
    }
}
