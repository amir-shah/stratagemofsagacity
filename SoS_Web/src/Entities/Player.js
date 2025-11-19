import Entity from './Entity.js';
import Projectile from './Projectile.js';

export default class Player extends Entity {
    constructor(x, y, assets) {
        super(x, y, assets);
        this.speed = 0.2; // px per ms
        this.state = 'stand'; // stand, walk, fire, dead
        this.animTimer = 0;
        this.fireTimer = 0;
        this.fireRate = 200; // ms
    }

    update(dt, game) {
        const input = game.input;
        let moving = false;
        let dx = 0;
        let dy = 0;

        if (input.isDown('KeyW')) dy -= 1;
        if (input.isDown('KeyS')) dy += 1;
        if (input.isDown('KeyA')) dx -= 1;
        if (input.isDown('KeyD')) dx += 1;

        if (dx !== 0 || dy !== 0) {
            moving = true;
            const len = Math.sqrt(dx * dx + dy * dy);
            dx /= len;
            dy /= len;

            const nextX = this.x + dx * this.speed * dt;
            const nextY = this.y + dy * this.speed * dt;

            // Collision check (simple)
            if (!game.map.checkCollision({ x: nextX, y: this.y, w: this.width, h: this.height })) {
                this.x = nextX;
            }
            if (!game.map.checkCollision({ x: this.x, y: nextY, w: this.width, h: this.height })) {
                this.y = nextY;
            }
        }

        // Aiming
        const mx = input.mouse.x + game.camera.x;
        const my = input.mouse.y + game.camera.y;
        this.angle = Math.atan2(my - (this.y + this.height / 2), mx - (this.x + this.width / 2));

        // Shooting
        if (input.isMouseDown()) {
            if (this.fireTimer <= 0) {
                this.fireTimer = this.fireRate;
                this.shoot(game);
                this.state = 'fire';
            }
        }
        if (this.fireTimer > 0) this.fireTimer -= dt;

        // Animation State
        if (this.fireTimer > 0) this.state = 'fire';
        else if (moving) this.state = 'walk';
        else this.state = 'stand';

        this.animTimer += dt;
    }

    shoot(game) {
        const cx = this.x + this.width / 2;
        const cy = this.y + this.height / 2;
        game.projectiles.push(new Projectile(cx, cy, this.angle, this.assets));

        const snd = this.assets.getAudio('gunfire');
        if (snd) {
            snd.currentTime = 0;
            snd.play().catch(() => { });
        }
    }

    draw(ctx) {
        let img = this.assets.getImage('player_stand');

        // Simple animation logic
        if (this.state === 'walk') {
            const frame = Math.floor(this.animTimer / 100) % 4;
            if (frame === 0) img = this.assets.getImage('player_walk1');
            else if (frame === 1) img = this.assets.getImage('player_walk2');
            else if (frame === 2) img = this.assets.getImage('player_walk1');
            else img = this.assets.getImage('player_walk4');
        } else if (this.state === 'fire') {
            const frame = Math.floor(this.animTimer / 50) % 2;
            if (frame === 0) img = this.assets.getImage('player_fire1');
            else img = this.assets.getImage('player_fire2');
        }

        if (img) {
            ctx.save();
            ctx.translate(this.x + this.width / 2, this.y + this.height / 2);
            ctx.rotate(this.angle);
            ctx.drawImage(img, -img.width / 2, -img.height / 2);
            ctx.restore();
        }
    }
}
