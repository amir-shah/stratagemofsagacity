export default class HUD {
    constructor(game) {
        this.game = game;
    }

    draw(ctx) {
        const p = this.game.player;

        ctx.save();
        ctx.font = '20px Courier New';
        ctx.textBaseline = 'top';

        // System Stability (Health)
        const healthPercent = (p.hp / 3) * 100; // Assuming max HP is 3 for now
        ctx.fillStyle = healthPercent > 30 ? '#00ff00' : '#ff0000';
        ctx.fillText(`SYSTEM STABILITY: ${healthPercent.toFixed(0)}%`, 20, 20);

        // Health Bar
        ctx.strokeStyle = '#00ff00';
        ctx.strokeRect(20, 45, 200, 15);
        ctx.fillRect(20, 45, 2 * healthPercent, 15);

        // Weapon Status
        ctx.fillStyle = '#00ffff';
        ctx.textAlign = 'right';
        ctx.fillText(`WEAPON: PLASMA CASTER`, this.game.width - 20, 20);
        ctx.fillText(`AMMO: INFINITE`, this.game.width - 20, 45);

        // Flavor Text
        ctx.font = '12px Courier New';
        ctx.fillStyle = 'rgba(0, 255, 0, 0.7)';
        ctx.textAlign = 'left';
        ctx.fillText(`SAGACITY ENGINE: v0.9.2`, 20, this.game.height - 30);
        ctx.fillText(`PREDICTION: ONLINE`, 20, this.game.height - 15);

        ctx.restore();
    }
}
