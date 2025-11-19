/**
 * CollisionSystem.js
 * Handles AABB collision detection for entities, projectiles, and walls
 */
export default class CollisionSystem {
    /**
     * Check if two rectangles overlap (AABB collision)
     * @param {Object} a - {x, y, w, h}
     * @param {Object} b - {x, y, w, h}
     * @returns {boolean}
     */
    static checkAABB(a, b) {
        return a.x < b.x + b.w &&
               a.x + a.w > b.x &&
               a.y < b.y + b.h &&
               a.y + a.h > b.y;
    }

    /**
     * Check if entity collides with any wall in the map
     * @param {Entity} entity
     * @param {Map} map
     * @returns {boolean}
     */
    static checkWallCollision(entity, map) {
        const rect = entity.getRect();
        return map.checkCollision(rect);
    }

    /**
     * Check projectile vs enemies collision
     * @param {Projectile} projectile
     * @param {Array<Enemy>} enemies
     * @returns {Enemy|null} - Hit enemy or null
     */
    static checkProjectileEnemies(projectile, enemies) {
        const pRect = projectile.getRect();

        for (const enemy of enemies) {
            if (enemy.remove) continue;

            const eRect = enemy.getRect();
            if (this.checkAABB(pRect, eRect)) {
                return enemy;
            }
        }

        return null;
    }

    /**
     * Check player vs enemies collision
     * @param {Player} player
     * @param {Array<Enemy>} enemies
     * @returns {Array<Enemy>} - All colliding enemies
     */
    static checkPlayerEnemies(player, enemies) {
        const pRect = player.getRect();
        const collisions = [];

        for (const enemy of enemies) {
            if (enemy.remove) continue;

            const eRect = enemy.getRect();
            if (this.checkAABB(pRect, eRect)) {
                collisions.push(enemy);
            }
        }

        return collisions;
    }

    /**
     * Check if point is within rectangle
     * @param {number} x
     * @param {number} y
     * @param {Object} rect - {x, y, w, h}
     * @returns {boolean}
     */
    static pointInRect(x, y, rect) {
        return x >= rect.x && x <= rect.x + rect.w &&
               y >= rect.y && y <= rect.y + rect.h;
    }

    /**
     * Get distance between two points
     * @param {number} x1
     * @param {number} y1
     * @param {number} x2
     * @param {number} y2
     * @returns {number}
     */
    static distance(x1, y1, x2, y2) {
        const dx = x2 - x1;
        const dy = y2 - y1;
        return Math.sqrt(dx * dx + dy * dy);
    }

    /**
     * Check if entity is within bounds
     * @param {Entity} entity
     * @param {number} width - Map width
     * @param {number} height - Map height
     * @returns {boolean}
     */
    static inBounds(entity, width, height) {
        const rect = entity.getRect();
        return rect.x >= 0 && rect.x + rect.w <= width &&
               rect.y >= 0 && rect.y + rect.h <= height;
    }
}
