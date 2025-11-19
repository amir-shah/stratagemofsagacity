import Input from './Utils/Input.js';
import AssetLoader from './Utils/AssetLoader.js';
import Camera from './World/Camera.js';
import Map from './World/Map.js';
import Player from './Entities/Player.js';
import Enemy from './Entities/Enemy.js';
import Projectile from './Entities/Projectile.js';
import MiniMap from './UI/MiniMap.js';
import HUD from './UI/HUD.js';

export default class Game {
    constructor(canvas) {
        this.canvas = canvas;
        this.ctx = canvas.getContext('2d');

        // Fullscreen handling
        this.resize();
        window.addEventListener('resize', () => this.resize());

        this.input = new Input(canvas);
        this.assets = new AssetLoader();
        this.camera = new Camera(this.width, this.height);
        this.miniMap = null;
        this.hud = null;

        this.state = 'LOADING'; // LOADING, MENU, PLAY, PAUSED
        this.menuSelection = 0;
        this.menuItems = ['Play', 'Options', 'Intro', 'Exit'];

        this.entities = [];
        this.projectiles = [];
        this.map = null;
        this.player = null;

        this.queueAssets();
    }

    queueAssets() {
        const s = 'assets/sprites/';
        const p = s + 'Player/';
        const e = s + 'Enemy/';
        const a = 'assets/audio/';

        // Player
        this.assets.queueImage('player_stand', p + 'standing.png');
        this.assets.queueImage('player_walk1', p + 'frame 1 and 3_walking.png');
        this.assets.queueImage('player_walk2', p + 'frame 2_walking.png');
        this.assets.queueImage('player_walk4', p + 'frame 4_walking.png');
        this.assets.queueImage('player_aim', p + 'aiming.png');
        this.assets.queueImage('player_fire1', p + 'firing gunfire 1.png');
        this.assets.queueImage('player_fire2', p + 'firing gunfire 2.png');
        this.assets.queueImage('player_dead', p + 'dead.png');

        // Enemy
        this.assets.queueImage('enemy_stand', e + 'standing.png');
        this.assets.queueImage('enemy_walk1', e + 'frame 1 and 3_walking.png');
        this.assets.queueImage('enemy_walk2', e + 'frame 2_walking.png');
        this.assets.queueImage('enemy_walk4', e + 'frame 4_walking.png');
        this.assets.queueImage('enemy_aim', e + 'aiming.png');
        this.assets.queueImage('enemy_hit', e + 'hit.png');
        this.assets.queueImage('enemy_dead', e + 'dead.png');

        // Misc
        this.assets.queueImage('floor', s + 'floor.png');
        this.assets.queueImage('wall', s + 'box.png');
        this.assets.queueImage('crosshair', s + 'crosshair.png');
        this.assets.queueImage('projectile', s + 'pShot.png');
        this.assets.queueImage('bg', s + 'bg.JPG');

        // Audio
        this.assets.queueAudio('music', a + 'Final-calpomat-4566_hifi.mp3');
        this.assets.queueAudio('gunfire', a + '3-burst-Diode111-8773_hifi.mp3');
    }

    start() {
        this.lastTime = 0;
        requestAnimationFrame(this.loop.bind(this));
    }

    loop(timestamp) {
        const dt = timestamp - this.lastTime;
        this.lastTime = timestamp;

        this.update(dt);
        this.draw();
        this.input.update();

        requestAnimationFrame(this.loop.bind(this));
    }

    update(dt) {
        if (this.state === 'LOADING') {
            if (this.assets.isDone()) {
                this.initGame();
                this.state = 'MENU';
            }
            return;
        }

        if (this.state === 'MENU') {
            if (this.input.isPressed('ArrowDown')) {
                this.menuSelection = (this.menuSelection + 1) % this.menuItems.length;
            }
            if (this.input.isPressed('ArrowUp')) {
                this.menuSelection = (this.menuSelection - 1 + this.menuItems.length) % this.menuItems.length;
            }
            if (this.input.isPressed('Enter')) {
                if (this.menuItems[this.menuSelection] === 'Play') {
                    this.state = 'PLAY';
                    // Play music
                    const music = this.assets.getAudio('music');
                    if (music) {
                        music.loop = true;
                        music.play().catch(e => console.log("Audio play failed (user interaction needed):", e));
                    }
                }
            }
            return;
        }

        if (this.state === 'PLAY') {
            if (this.input.isPressed('Escape')) {
                this.state = 'PAUSED';
                return;
            }

            this.player.update(dt, this);
            this.camera.follow(this.player, this.map.width, this.map.height);

            this.entities.forEach(e => e.update(dt, this));
            this.projectiles.forEach(p => p.update(dt, this));

            // Collisions
            this.checkCollisions();

            // Cleanup
            this.projectiles = this.projectiles.filter(p => !p.remove);
            this.entities = this.entities.filter(e => !e.remove);
        }

        if (this.state === 'PAUSED') {
            if (this.input.isPressed('Escape')) {
                this.state = 'PLAY';
            }
        }
    }

    draw() {
        this.ctx.fillStyle = 'black';
        this.ctx.fillRect(0, 0, this.width, this.height);

        if (this.state === 'LOADING') {
            this.ctx.fillStyle = 'white';
            this.ctx.font = '20px Arial';
            this.ctx.fillText(`Loading... ${(this.assets.getProgress() * 100).toFixed(0)}%`, this.width / 2, this.height / 2);
            return;
        }

        if (this.state === 'MENU') {
            const bg = this.assets.getImage('bg');
            if (bg) this.ctx.drawImage(bg, 0, 0, this.width, this.height);

            this.ctx.font = '40px Times New Roman';
            this.ctx.fillStyle = 'greenyellow';
            this.ctx.fillText('Stratagem of Sagacity', this.width / 2 - 150, 100);

            this.ctx.font = '30px Times New Roman';
            this.menuItems.forEach((item, i) => {
                this.ctx.fillStyle = i === this.menuSelection ? 'yellow' : 'white';
                this.ctx.fillText(item, 30, 200 + i * 40);
            });
            return;
        }

        if (this.state === 'PLAY' || this.state === 'PAUSED') {
            this.ctx.save();
            this.ctx.translate(-this.camera.x, -this.camera.y);

            this.map.draw(this.ctx, this.camera);
            this.entities.forEach(e => e.draw(this.ctx));
            this.player.draw(this.ctx);
            this.projectiles.forEach(p => p.draw(this.ctx));

            this.ctx.restore();

            // UI
            const crosshair = this.assets.getImage('crosshair');
            if (crosshair) {
                this.ctx.drawImage(crosshair, this.input.mouse.x - 5, this.input.mouse.y - 5, 10, 10);
            }

            if (this.miniMap) {
                this.miniMap.draw(this.ctx);
            }

            if (this.hud) {
                this.hud.draw(this.ctx);
            }

            if (this.state === 'PAUSED') {
                this.ctx.fillStyle = 'white';
                this.ctx.font = '40px Arial';
                this.ctx.fillText('PAUSED', this.width / 2 - 70, this.height / 2);
            }
        }
    }

    async initGame() {
        this.map = new Map(this.assets);
        await this.map.load('assets/map.txt');

        // Spawn in the outer corridor
        this.player = new Player(100, 100, this.assets);

        // Enemies in the inner rooms
        this.entities.push(new Enemy(400, 400, this.assets));
        this.entities.push(new Enemy(600, 600, this.assets));
        this.entities.push(new Enemy(800, 200, this.assets));

        this.miniMap = new MiniMap(this, 200, 200);
        this.hud = new HUD(this);
    }

    checkCollisions() {
        // Simple AABB
        // ...
    }

    resize() {
        this.width = window.innerWidth;
        this.height = window.innerHeight;
        this.canvas.width = this.width;
        this.canvas.height = this.height;

        if (this.camera) {
            this.camera.width = this.width;
            this.camera.height = this.height;
        }
    }
}
