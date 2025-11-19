import Entity from './Entity.js';

export default class Enemy extends Entity {
    constructor(x, y, assets) {
        super(x, y, assets);
        this.type = 'Enemy';
        this.speed = 0.1;
        this.hp = 3;
        this.damage = 1;
        this.behavior = 'chase'; // chase, patrol, tactical, boss

        // Patrol behavior
        this.patrolCenter = { x, y };
        this.patrolRadius = 100;
        this.patrolAngle = 0;

        // Tactical behavior
        this.shootRange = 200;
        this.shootCooldown = 0;
        this.shootDelay = 1500;

        // Boss behavior
        this.isBoss = false;
        this.phase = 1;
        this.maxHp = this.hp;
    }

    update(dt, game) {
        switch (this.behavior) {
            case 'chase':
                this.updateChase(dt, game);
                break;
            case 'patrol':
                this.updatePatrol(dt, game);
                break;
            case 'tactical':
                this.updateTactical(dt, game);
                break;
            case 'boss':
                this.updateBoss(dt, game);
                break;
            default:
                this.updateChase(dt, game);
        }
    }

    updateChase(dt, game) {
        const player = game.player;
        const dx = player.x - this.x;
        const dy = player.y - this.y;
        const dist = Math.sqrt(dx * dx + dy * dy);

        if (dist > 5) {
            this.x += (dx / dist) * this.speed * dt;
            this.y += (dy / dist) * this.speed * dt;
        }

        this.angle = Math.atan2(dy, dx);
    }

    updatePatrol(dt, game) {
        // Circle around patrol center
        this.patrolAngle += 0.001 * dt;
        const targetX = this.patrolCenter.x + Math.cos(this.patrolAngle) * this.patrolRadius;
        const targetY = this.patrolCenter.y + Math.sin(this.patrolAngle) * this.patrolRadius;

        const dx = targetX - this.x;
        const dy = targetY - this.y;
        const dist = Math.sqrt(dx * dx + dy * dy);

        if (dist > 5) {
            this.x += (dx / dist) * this.speed * dt;
            this.y += (dy / dist) * this.speed * dt;
        }

        // Face movement direction
        this.angle = Math.atan2(dy, dx);

        // Chase player if too close
        const player = game.player;
        const playerDist = Math.sqrt((player.x - this.x) ** 2 + (player.y - this.y) ** 2);
        if (playerDist < 150) {
            this.updateChase(dt, game);
        }
    }

    updateTactical(dt, game) {
        const player = game.player;
        const dx = player.x - this.x;
        const dy = player.y - this.y;
        const dist = Math.sqrt(dx * dx + dy * dy);

        this.angle = Math.atan2(dy, dx);

        // Keep distance, shoot from range
        if (dist < this.shootRange - 50) {
            // Too close, back away
            this.x -= (dx / dist) * this.speed * dt;
            this.y -= (dy / dist) * this.speed * dt;
        } else if (dist > this.shootRange + 50) {
            // Too far, move closer
            this.x += (dx / dist) * this.speed * dt;
            this.y += (dy / dist) * this.speed * dt;
        }

        // Shooting logic (to be implemented by game)
        this.shootCooldown -= dt;
    }

    updateBoss(dt, game) {
        // Boss behaviors change per phase
        const healthPercent = this.hp / this.maxHp;

        if (healthPercent < 0.33 && this.phase < 3) {
            this.phase = 3;
            this.speed *= 1.5;
        } else if (healthPercent < 0.66 && this.phase < 2) {
            this.phase = 2;
            this.attackPattern = 'spiral';
        }

        // Basic chase for now
        this.updateChase(dt, game);
    }

    hit(damage = 1) {
        this.hp -= damage;
        if (this.hp <= 0) {
            this.remove = true;
            return true; // Killed
        }
        return false; // Still alive
    }

    draw(ctx) {
        const img = this.assets.getImage('enemy_stand');
        if (img) {
            ctx.save();
            ctx.translate(this.x + this.width / 2, this.y + this.height / 2);
            ctx.rotate(this.angle);
            ctx.drawImage(img, -img.width / 2, -img.height / 2);
            ctx.restore();

            // Draw HP bar for bosses
            if (this.isBoss) {
                const barWidth = 60;
                const barHeight = 6;
                const barX = this.x + this.width / 2 - barWidth / 2;
                const barY = this.y - 15;

                ctx.fillStyle = '#333';
                ctx.fillRect(barX, barY, barWidth, barHeight);

                ctx.fillStyle = '#ff0000';
                const hpPercent = this.hp / this.maxHp;
                ctx.fillRect(barX, barY, barWidth * hpPercent, barHeight);

                ctx.strokeStyle = '#fff';
                ctx.lineWidth = 1;
                ctx.strokeRect(barX, barY, barWidth, barHeight);
            }
        }
    }

    predict(dt, game) {
        const dist = this.speed * dt;
        const dx = Math.cos(this.angle) * dist;
        const dy = Math.sin(this.angle) * dist;

        return {
            x: this.x + dx,
            y: this.y + dy,
            width: this.width,
            height: this.height
        };
    }
}
