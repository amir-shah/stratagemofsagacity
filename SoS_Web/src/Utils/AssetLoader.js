export default class AssetLoader {
    constructor() {
        this.images = {};
        this.audio = {};
        this.toLoad = 0;
        this.loaded = 0;
    }

    queueImage(key, src) {
        this.toLoad++;
        const img = new Image();
        img.src = src;
        img.onload = () => this.loaded++;
        img.onerror = () => {
            console.error(`Failed to load image: ${src}`);
            this.loaded++;
        };
        this.images[key] = img;
    }

    queueAudio(key, src) {
        this.toLoad++;
        const aud = new Audio();
        aud.src = src;
        aud.oncanplaythrough = () => {
            // Audio loading can be tricky, we'll count it as loaded when it can play
            if (!aud.counted) {
                this.loaded++;
                aud.counted = true;
            }
        };
        aud.onerror = () => {
            console.error(`Failed to load audio: ${src}`);
            this.loaded++;
        };
        this.audio[key] = aud;
    }

    isDone() {
        return this.loaded === this.toLoad;
    }

    getProgress() {
        return this.toLoad === 0 ? 1 : this.loaded / this.toLoad;
    }

    getImage(key) {
        return this.images[key];
    }

    getAudio(key) {
        return this.audio[key];
    }
}
