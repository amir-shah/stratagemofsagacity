/**
 * DialogueBox.js
 * Displays Overseer dialogue and narrative text with cyberpunk styling
 */
export default class DialogueBox {
    constructor() {
        this.visible = false;
        this.text = '';
        this.speaker = 'OVERSEER';
        this.displayTime = 0;
        this.duration = 4000; // 4 seconds default
        this.alpha = 0;
        this.fadeSpeed = 0.003;

        // Visual settings
        this.width = 600;
        this.height = 100;
        this.padding = 15;
        this.glitchEffect = false;
        this.glitchTimer = 0;
    }

    /**
     * Show dialogue message
     * @param {string} text - Message to display
     * @param {string} speaker - Who is speaking (default: OVERSEER)
     * @param {number} duration - How long to display in ms (default: 4000)
     */
    show(text, speaker = 'OVERSEER', duration = 4000) {
        this.text = text;
        this.speaker = speaker;
        this.duration = duration;
        this.displayTime = 0;
        this.visible = true;
        this.alpha = 0;
    }

    /**
     * Hide dialogue immediately
     */
    hide() {
        this.visible = false;
        this.alpha = 0;
    }

    /**
     * Update animation
     * @param {number} dt - Delta time
     */
    update(dt) {
        if (!this.visible) return;

        this.displayTime += dt;
        this.glitchTimer += dt;

        // Fade in
        if (this.displayTime < 500) {
            this.alpha = Math.min(1, this.alpha + this.fadeSpeed * dt);
        }
        // Fade out at end
        else if (this.displayTime > this.duration - 500) {
            this.alpha = Math.max(0, this.alpha - this.fadeSpeed * dt);
        }
        // Full opacity
        else {
            this.alpha = 1;
        }

        // Random glitch effect
        if (this.glitchTimer > 2000 && Math.random() < 0.01) {
            this.glitchEffect = true;
            setTimeout(() => { this.glitchEffect = false; }, 100);
        }

        // Auto-hide when done
        if (this.displayTime >= this.duration) {
            this.visible = false;
        }
    }

    /**
     * Draw dialogue box
     * @param {CanvasRenderingContext2D} ctx
     * @param {number} screenWidth
     * @param {number} screenHeight
     */
    draw(ctx, screenWidth, screenHeight) {
        if (!this.visible || this.alpha === 0) return;

        ctx.save();
        ctx.globalAlpha = this.alpha;

        // Position at bottom center
        const x = (screenWidth - this.width) / 2;
        const y = screenHeight - this.height - 50;

        // Glitch offset
        let glitchX = 0;
        let glitchY = 0;
        if (this.glitchEffect) {
            glitchX = (Math.random() - 0.5) * 6;
            glitchY = (Math.random() - 0.5) * 4;
        }

        // Background - dark with neon border
        ctx.fillStyle = 'rgba(0, 0, 0, 0.85)';
        ctx.fillRect(x + glitchX, y + glitchY, this.width, this.height);

        // Neon border
        ctx.strokeStyle = this.speaker === 'OVERSEER' ? '#00ff88' : '#ff0088';
        ctx.lineWidth = 2;
        ctx.strokeRect(x + glitchX, y + glitchY, this.width, this.height);

        // Inner glow
        ctx.strokeStyle = this.speaker === 'OVERSEER' ? 'rgba(0, 255, 136, 0.3)' : 'rgba(255, 0, 136, 0.3)';
        ctx.lineWidth = 4;
        ctx.strokeRect(x + glitchX + 2, y + glitchY + 2, this.width - 4, this.height - 4);

        // Speaker label
        ctx.fillStyle = this.speaker === 'OVERSEER' ? '#00ff88' : '#ff0088';
        ctx.font = 'bold 12px monospace';
        ctx.fillText(`[ ${this.speaker} ]`, x + this.padding + glitchX, y + this.padding + 15 + glitchY);

        // Divider line
        ctx.strokeStyle = ctx.fillStyle;
        ctx.lineWidth = 1;
        ctx.beginPath();
        ctx.moveTo(x + this.padding, y + this.padding + 22);
        ctx.lineTo(x + this.width - this.padding, y + this.padding + 22);
        ctx.stroke();

        // Message text
        ctx.fillStyle = '#ffffff';
        ctx.font = '14px monospace';
        this.wrapText(ctx, this.text, x + this.padding + glitchX, y + this.padding + 42 + glitchY, this.width - this.padding * 2, 18);

        // Scanline effect
        if (Math.random() < 0.3) {
            const scanline = Math.random() * this.height;
            ctx.fillStyle = 'rgba(0, 255, 136, 0.1)';
            ctx.fillRect(x, y + scanline, this.width, 2);
        }

        ctx.restore();
    }

    /**
     * Wrap text to fit within width
     * @param {CanvasRenderingContext2D} ctx
     * @param {string} text
     * @param {number} x
     * @param {number} y
     * @param {number} maxWidth
     * @param {number} lineHeight
     */
    wrapText(ctx, text, x, y, maxWidth, lineHeight) {
        const words = text.split(' ');
        let line = '';
        let yPos = y;

        for (let i = 0; i < words.length; i++) {
            const testLine = line + words[i] + ' ';
            const metrics = ctx.measureText(testLine);

            if (metrics.width > maxWidth && i > 0) {
                ctx.fillText(line, x, yPos);
                line = words[i] + ' ';
                yPos += lineHeight;
            } else {
                line = testLine;
            }
        }
        ctx.fillText(line, x, yPos);
    }

    /**
     * Queue multiple messages
     * @param {Array<string>} messages
     * @param {number} delayBetween - Delay between messages in ms
     */
    queueMessages(messages, delayBetween = 500) {
        let totalDelay = 0;
        messages.forEach((msg, index) => {
            setTimeout(() => {
                this.show(msg);
            }, totalDelay);
            totalDelay += this.duration + delayBetween;
        });
    }
}
