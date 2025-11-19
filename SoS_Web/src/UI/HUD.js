/**
 * HUD.js - Cyberpunk-themed heads-up display
 * Displays System Stability, objectives, and status
 */
export default class HUD {
    constructor(game) {
        this.game = game;
        this.glitchTimer = 0;
        this.glitchActive = false;
    }

    draw(ctx) {
        const p = this.game.player;
        if (!p) return;

        this.glitchTimer += 16; // Approximate dt
        if (this.glitchTimer > 3000 && Math.random() < 0.005) {
            this.glitchActive = true;
            setTimeout(() => { this.glitchActive = false; }, 80);
        }

        const glitch = this.glitchActive ? (Math.random() - 0.5) * 4 : 0;

        ctx.save();
        ctx.font = 'bold 16px monospace';
        ctx.textBaseline = 'top';

        // === SYSTEM STABILITY (Top Left) ===
        const maxHp = p.maxHp || 100;
        const stabilityPercent = (p.hp / maxHp) * 100;

        // Color based on stability
        let statusColor = '#00ff88'; // Green
        if (stabilityPercent < 30) statusColor = '#ff0044'; // Red
        else if (stabilityPercent < 60) statusColor = '#ffaa00'; // Orange

        ctx.fillStyle = statusColor;
        ctx.fillText(`SYSTEM STABILITY`, 20 + glitch, 20);

        ctx.font = 'bold 24px monospace';
        ctx.fillText(`${stabilityPercent.toFixed(0)}%`, 20 + glitch, 42);

        // Stability bar
        const barWidth = 200;
        const barHeight = 8;
        ctx.strokeStyle = statusColor;
        ctx.lineWidth = 2;
        ctx.strokeRect(20, 72, barWidth, barHeight);

        // Bar fill with glow
        ctx.fillStyle = statusColor;
        ctx.fillRect(20, 72, barWidth * (stabilityPercent / 100), barHeight);

        // Inner glow
        ctx.shadowColor = statusColor;
        ctx.shadowBlur = stabilityPercent < 30 ? 10 : 5;
        ctx.fillRect(20, 72, barWidth * (stabilityPercent / 100), barHeight);
        ctx.shadowBlur = 0;

        // Warning message if critical
        if (stabilityPercent < 20) {
            ctx.font = 'bold 12px monospace';
            ctx.fillStyle = '#ff0044';
            ctx.fillText('! FATAL ERROR IMMINENT !', 20, 88);
        }

        // === OBJECTIVES (Top Center) ===
        if (this.game.level && this.game.level.objectives) {
            ctx.textAlign = 'center';
            ctx.font = 'bold 14px monospace';
            ctx.fillStyle = '#00ffff';

            let yOffset = 20;
            this.game.level.objectives.forEach((obj, idx) => {
                const status = obj.completed ? '[✓]' : '[ ]';
                const color = obj.completed ? '#00ff88' : '#00ffff';
                ctx.fillStyle = color;
                ctx.fillText(`${status} ${obj.description}`, this.game.width / 2, yOffset);
                yOffset += 20;
            });
        }

        // === WEAPON STATUS (Top Right) ===
        ctx.textAlign = 'right';
        ctx.font = 'bold 14px monospace';
        ctx.fillStyle = '#00ffff';
        ctx.fillText('[ WEAPON SYSTEM ]', this.game.width - 20, 20);
        ctx.fillText('TYPE: PLASMA CASTER', this.game.width - 20, 40);
        ctx.fillText('AMMO: ∞', this.game.width - 20, 60);

        // === BOTTOM STATUS BAR ===
        ctx.textAlign = 'left';
        ctx.font = '11px monospace';
        ctx.fillStyle = 'rgba(0, 255, 136, 0.8)';

        const bottomY = this.game.height - 25;

        // Left side - Unit info
        ctx.fillText(`UNIT 734 [ "SEVEN" ]`, 20, bottomY);

        // Center - Sagacity chip status
        ctx.textAlign = 'center';
        const chipStatus = stabilityPercent > 50 ? 'OPTIMAL' : stabilityPercent > 20 ? 'DEGRADED' : 'CRITICAL';
        ctx.fillStyle = stabilityPercent > 50 ? '#00ff88' : stabilityPercent > 20 ? '#ffaa00' : '#ff0044';
        ctx.fillText(`SAGACITY CHIP: ${chipStatus}`, this.game.width / 2, bottomY);

        // Right side - Level/Time
        ctx.textAlign = 'right';
        ctx.fillStyle = 'rgba(0, 255, 136, 0.8)';
        if (this.game.level) {
            const time = (this.game.level.elapsed / 1000).toFixed(1);
            ctx.fillText(`${this.game.level.name.toUpperCase()} | ${time}s`, this.game.width - 20, bottomY);
        }

        // Scanline effect
        if (Math.random() < 0.2) {
            ctx.fillStyle = 'rgba(0, 255, 136, 0.05)';
            const scanY = Math.random() * 100;
            ctx.fillRect(0, scanY, this.game.width, 2);
        }

        ctx.restore();
    }
}
