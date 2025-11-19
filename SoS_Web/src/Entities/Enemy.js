import Entity from './Entity.js';

export default class Enemy extends Entity {
    constructor(x, y, assets) {
        super(x, y, assets);
        this.speed = 0.1;
        this.hp = 3;
    }

    update(dt, game) {
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

    hit() {
        this.hp--;
        if (this.hp <= 0) this.remove = true;
    }

    draw(ctx) {
        const img = this.assets.getImage('enemy_stand');
        if (img) {
            ctx.save();
            ctx.translate(this.x + this.width / 2, this.y + this.height / 2);
            ctx.rotate(this.angle);
            ctx.drawImage(img, -img.width / 2, -img.height / 2);
            ctx.restore();
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
