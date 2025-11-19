export default class Entity {
    constructor(x, y, assets) {
        this.x = x;
        this.y = y;
        this.assets = assets;
        this.width = 32;
        this.height = 32;
        this.remove = false;
        this.speed = 0;
        this.angle = 0;
    }

    update(dt, game) { }

    draw(ctx) {
        // Debug rect
        // ctx.strokeStyle = 'red';
        // ctx.strokeRect(this.x, this.y, this.width, this.height);
    }

    getRect() {
        return { x: this.x, y: this.y, w: this.width, h: this.height };
    }

    predict(dt) {
        // Base prediction: no movement
        return { x: this.x, y: this.y, width: this.width, height: this.height };
    }
}
