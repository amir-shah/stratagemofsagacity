export default class MiniMap {
    constructor(game, width, height) {
        this.game = game;
        this.width = width;
        this.height = height;
        this.scale = 0.15; // Map scale factor
        this.padding = 10;
        this.futureTime = 2000; // 2 seconds
    }

    draw(ctx) {
        const mapW = this.game.map.width * this.scale;
        const mapH = this.game.map.height * this.scale;

        // Position: Bottom Right
        const x = this.game.width - mapW - this.padding;
        const y = this.game.height - mapH - this.padding;

        // Draw Background (Dark Blue/Black)
        ctx.fillStyle = 'rgba(0, 10, 20, 0.85)';
        ctx.fillRect(x, y, mapW, mapH);

        // Draw Grid
        ctx.strokeStyle = 'rgba(0, 255, 255, 0.1)';
        ctx.lineWidth = 1;
        ctx.beginPath();
        for (let i = 0; i <= mapW; i += 20) {
            ctx.moveTo(x + i, y);
            ctx.lineTo(x + i, y + mapH);
        }
        for (let i = 0; i <= mapH; i += 20) {
            ctx.moveTo(x, y + i);
            ctx.lineTo(x + mapW, y + i);
        }
        ctx.stroke();

        // Border (Cyan)
        ctx.strokeStyle = '#00ffff';
        ctx.lineWidth = 2;
        ctx.strokeRect(x, y, mapW, mapH);

        // Label with timestamp
        ctx.fillStyle = '#00ff88';
        ctx.font = 'bold 10px monospace';
        ctx.fillText('[ PREDICTIVE MATRIX ]', x + 5, y - 15);
        ctx.font = '9px monospace';
        ctx.fillStyle = 'rgba(0, 255, 136, 0.7)';
        ctx.fillText(`T-minus ${(this.futureTime / 1000).toFixed(2)}s`, x + 5, y - 4);

        // Draw Walls (Hollow Cyan)
        ctx.fillStyle = 'rgba(0, 255, 255, 0.2)';
        this.game.map.walls.forEach(w => {
            ctx.fillRect(x + w.x * this.scale, y + w.y * this.scale, w.w * this.scale, w.h * this.scale);
        });

        // Draw Player (Current) - Cyan Dot
        const p = this.game.player;
        ctx.fillStyle = '#00ffff';
        ctx.beginPath();
        ctx.arc(x + p.x * this.scale, y + p.y * this.scale, 3, 0, Math.PI * 2);
        ctx.fill();

        // Draw Camera View Rect
        const cam = this.game.camera;
        ctx.strokeStyle = 'rgba(255, 255, 255, 0.5)';
        ctx.lineWidth = 1;
        ctx.strokeRect(x + cam.x * this.scale, y + cam.y * this.scale, cam.width * this.scale, cam.height * this.scale);

        // Draw Future Entities
        this.drawFuture(ctx, x, y);
    }

    drawFuture(ctx, offsetX, offsetY) {
        // Enemies (Red)
        ctx.fillStyle = '#ff0000';
        this.game.entities.forEach(e => {
            const future = e.predict(this.futureTime, this.game);
            ctx.fillRect(offsetX + future.x * this.scale, offsetY + future.y * this.scale, 3, 3);

            // Draw line to future pos
            ctx.strokeStyle = 'rgba(255, 0, 0, 0.3)';
            ctx.beginPath();
            ctx.moveTo(offsetX + e.x * this.scale, offsetY + e.y * this.scale);
            ctx.lineTo(offsetX + future.x * this.scale, offsetY + future.y * this.scale);
            ctx.stroke();
        });

        // Projectiles (Yellow)
        ctx.fillStyle = '#ffff00';
        this.game.projectiles.forEach(p => {
            const future = p.predict(this.futureTime);
            ctx.fillRect(offsetX + future.x * this.scale, offsetY + future.y * this.scale, 2, 2);
        });
    }
}
