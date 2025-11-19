export default class Camera {
    constructor(width, height) {
        this.x = 0;
        this.y = 0;
        this.width = width;
        this.height = height;
        this.buffer = 100;
    }

    follow(target, mapWidth, mapHeight) {
        // Simple box camera logic from original Game1.cs
        // "if (player.getPoint(camera).X < xCamBuffer) camera.X -= ..."
        // We'll implement a smoother follow or the original logic.
        // Original logic moves camera when player gets near edge of screen.

        // Let's implement standard center-on-player for better feel, 
        // but if we want to be faithful:

        // target is the player entity
        const screenX = target.x - this.x;
        const screenY = target.y - this.y;

        if (screenX < this.buffer) this.x -= target.speed; // Simplified speed usage
        if (screenX > this.width - this.buffer) this.x += target.speed;

        if (screenY < this.buffer) this.y -= target.speed;
        if (screenY > this.height - this.buffer) this.y += target.speed;

        // Clamp
        if (this.x < 0) this.x = 0;
        if (this.y < 0) this.y = 0;
        if (this.x + this.width > mapWidth) this.x = mapWidth - this.width;
        if (this.y + this.height > mapHeight) this.y = mapHeight - this.height;
    }
}
