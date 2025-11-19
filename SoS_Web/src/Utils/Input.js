export default class Input {
    constructor(canvas) {
        this.keys = {};
        this.prevKeys = {};
        this.mouse = { x: 0, y: 0, leftButton: false, prevLeftButton: false };
        this.canvas = canvas;

        window.addEventListener('keydown', (e) => {
            this.keys[e.code] = true;
        });

        window.addEventListener('keyup', (e) => {
            this.keys[e.code] = false;
        });

        window.addEventListener('mousemove', (e) => {
            const rect = this.canvas.getBoundingClientRect();
            this.mouse.x = e.clientX - rect.left;
            this.mouse.y = e.clientY - rect.top;
        });

        window.addEventListener('mousedown', (e) => {
            if (e.button === 0) this.mouse.leftButton = true;
        });

        window.addEventListener('mouseup', (e) => {
            if (e.button === 0) this.mouse.leftButton = false;
        });
    }

    update() {
        this.prevKeys = { ...this.keys };
        this.mouse.prevLeftButton = this.mouse.leftButton;
    }

    isDown(code) {
        return !!this.keys[code];
    }

    isPressed(code) {
        return !!this.keys[code] && !this.prevKeys[code];
    }

    isMouseDown() {
        return this.mouse.leftButton;
    }

    isMousePressed() {
        return this.mouse.leftButton && !this.mouse.prevLeftButton;
    }
}
