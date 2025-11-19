import Entity from './Entity.js';

export default class Projectile extends Entity {
    constructor(x, y, angle, assets) {
        super(x, y, assets);
        this.angle = angle;
        this.speed = 0.8;
        this.life = 2000; // ms
        this.width = 10;
        this.height = 10;
    }

    update(dt, game) {
        this.life -= dt;
        if (this.life <= 0) {
            this.remove = true;
            return;
        }

        this.x += Math.cos(this.angle) * this.speed * dt;
        this.y += Math.sin(this.angle) * this.speed * dt;

        if (game.map.checkCollision(this.getRect())) {
            this.remove = true;
        }

        // Enemy collision
        for (const e of game.entities) {
            const r = e.getRect();
            if (this.x < r.x + r.w && this.x + this.width > r.x &&
                this.y < r.y + r.h && this.y + this.height > r.y) {
                e.hit();
                this.remove = true;
                break;
            }
        }
    }

    draw(ctx) {
        const img = this.assets.getImage('projectile');
        if (img) {
            ctx.save();
            ctx.translate(this.x, this.y);
            ctx.rotate(this.angle);
            ctx.drawImage(img, -img.width / 2, -img.height / 2);
            ctx.restore();
        } else {
            ctx.fillStyle = 'yellow';
            ctx.fillRect(this.x, this.y, 5, 5);
        }
    }

    predict(dt) {
        const dist = this.speed * dt;
        return {
            x: this.x + Math.cos(this.angle) * dist,
            y: this.y + Math.sin(this.angle) * dist,
            width: this.width,
            height: this.height
        };
    }
}
