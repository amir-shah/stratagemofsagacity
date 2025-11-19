export default class Map {
    constructor(assets) {
        this.assets = assets;
        this.width = 0;
        this.height = 0;
        this.walls = [];
        this.tileSize = 4; // From original code
    }

    async load(path) {
        const response = await fetch(path);
        const text = await response.text();
        const lines = text.split(/\r?\n/);

        this.width = parseInt(lines[0]);
        this.height = parseInt(lines[1]);

        for (let y = 0; y < this.height; y++) {
            const line = lines[y + 2]; // Skip dimensions
            if (!line) continue;
            for (let x = 0; x < this.width; x++) {
                if (line[x] === 'x') {
                    this.walls.push({ x: x * this.tileSize, y: y * this.tileSize, w: this.tileSize, h: this.tileSize });
                }
            }
        }

        // Convert map dimensions to pixels for camera bounds
        this.width *= this.tileSize;
        this.height *= this.tileSize;
    }

    draw(ctx, camera) {
        // Draw floor
        const floor = this.assets.getImage('floor');
        if (floor) {
            // Tiling floor would be better, but original just drew one big image or tiled it?
            // Original: background = Content.Load<Texture2D>("floor"); 
            // It seems it might be just one big image or repeated.
            // Let's assume repeated for now or just draw it big.
            // For performance, we only draw visible tiles, but let's keep it simple first.
            const ptrn = ctx.createPattern(floor, 'repeat');
            ctx.fillStyle = ptrn;
            ctx.fillRect(camera.x, camera.y, camera.width, camera.height);
        }

        // Draw walls
        const wallImg = this.assets.getImage('wall');
        if (wallImg) {
            this.walls.forEach(w => {
                // Culling
                if (w.x + w.w > camera.x && w.x < camera.x + camera.width &&
                    w.y + w.h > camera.y && w.y < camera.y + camera.height) {
                    ctx.drawImage(wallImg, w.x, w.y, w.w, w.h);
                }
            });
        }
    }

    checkCollision(rect) {
        for (const w of this.walls) {
            if (rect.x < w.x + w.w &&
                rect.x + rect.w > w.x &&
                rect.y < w.y + w.h &&
                rect.y + rect.h > w.y) {
                return true;
            }
        }
        return false;
    }
}
