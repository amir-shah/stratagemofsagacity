# Web Deployment Guide

Deploy Stratagem of Sagacity as a browser-based game! This guide covers multiple deployment options.

## Quick Start

### Export from Godot

1. **Open Project in Godot 4.3+**
2. **Install Web Export Templates** (if not already installed):
   - Go to `Editor → Manage Export Templates`
   - Click "Download and Install"
3. **Export the Game**:
   - Go to `Project → Export`
   - Select "Web" preset (already configured in `export_presets.cfg`)
   - Click "Export Project"
   - Choose output folder (e.g., `build/web/`)
4. **Done!** You'll get HTML5/WebAssembly files ready to host

### Test Locally

You **cannot** simply open `index.html` in a browser due to CORS restrictions. Use a local server:

```bash
# Option 1: Python 3
cd build/web
python3 -m http.server 8000

# Option 2: Node.js
npx http-server build/web -p 8000

# Option 3: PHP
cd build/web
php -S localhost:8000
```

Then open http://localhost:8000 in your browser.

---

## Deployment Options

### 1. GitHub Pages (FREE)

Perfect for open-source projects and quick sharing.

**Setup:**

```bash
# 1. Export game to docs/ folder (for GitHub Pages)
# In Godot: Export → Web → Set path to "docs/index.html"

# 2. Create .nojekyll file (prevents Jekyll processing)
touch docs/.nojekyll

# 3. Commit and push
git add docs/
git commit -m "Add web build for GitHub Pages"
git push

# 4. Enable GitHub Pages
# Go to: Repository Settings → Pages → Source: "main branch /docs folder"
```

Your game will be live at: `https://username.github.io/repository-name/`

**Pros:** Free, easy, version controlled
**Cons:** Public repositories only (for free tier), 100MB file limit

---

### 2. itch.io (FREE)

Best for indie game distribution with built-in community features.

**Setup:**

1. **Export game** from Godot to a folder (e.g., `build/web/`)
2. **Zip the folder**: `zip -r StratageOfSagacity-Web.zip build/web/`
3. **Upload to itch.io**:
   - Go to https://itch.io/game/new
   - Set "Kind of project" to "HTML"
   - Upload the ZIP file
   - Check "This file will be played in the browser"
   - Set viewport dimensions: 800x537 (or your preferred size)
   - Click "Embed in page"
4. **Publish!**

**Pros:** Game community, analytics, optional pay-what-you-want
**Cons:** Requires itch.io account

**Example:** https://yourusername.itch.io/stratagem-of-sagacity

---

### 3. Netlify (FREE)

Modern hosting with automatic deployments from Git.

**Setup:**

```bash
# 1. Export to build/web/ folder

# 2. Create netlify.toml
cat > netlify.toml << EOF
[build]
  publish = "build/web"
  command = "echo 'No build needed - using pre-built Godot export'"

[[headers]]
  for = "/*"
  [headers.values]
    Cross-Origin-Embedder-Policy = "require-corp"
    Cross-Origin-Opener-Policy = "same-origin"
EOF

# 3. Deploy via Netlify CLI
npm install -g netlify-cli
netlify deploy --prod --dir=build/web
```

Or connect your GitHub repo at https://app.netlify.com for auto-deployments.

**Pros:** Free SSL, custom domains, auto-deploy from Git
**Cons:** 100GB/month bandwidth limit (free tier)

---

### 4. Vercel (FREE)

Similar to Netlify, great for web apps.

**Setup:**

```bash
# 1. Export to build/web/

# 2. Install Vercel CLI
npm install -g vercel

# 3. Deploy
cd build/web
vercel --prod
```

Or connect GitHub repo at https://vercel.com for auto-deployments.

**Pros:** Fast CDN, free SSL, simple setup
**Cons:** 100GB/month bandwidth (hobby tier)

---

### 5. Self-Hosting (Your Own Server)

Host on any web server (Apache, Nginx, etc.).

**Nginx Configuration:**

```nginx
server {
    listen 80;
    server_name yourdomain.com;
    root /var/www/stratagem;
    index index.html;

    # Enable CORS for WebAssembly
    add_header Cross-Origin-Embedder-Policy "require-corp";
    add_header Cross-Origin-Opener-Policy "same-origin";

    # Cache static assets
    location ~* \.(wasm|pck|js|png|jpg|ico)$ {
        expires 30d;
        add_header Cache-Control "public, immutable";
    }
}
```

**Apache Configuration (.htaccess):**

```apache
# Enable CORS headers
Header set Cross-Origin-Embedder-Policy "require-corp"
Header set Cross-Origin-Opener-Policy "same-origin"

# Cache static files
<FilesMatch "\.(wasm|pck|js|png|jpg|ico)$">
    Header set Cache-Control "max-age=2592000, public"
</FilesMatch>
```

---

## Performance Optimization

### Reduce File Size

1. **In Godot Export Settings:**
   - Enable "Optimize for Size"
   - Disable unused modules
   - Use lossy compression for textures

2. **Compress Assets:**
   ```bash
   # Use tools like pngquant for images
   pngquant assets/sprites/*.png --ext .png --force

   # Convert large audio to OGG Vorbis
   ffmpeg -i music.wav -c:a libvorbis -q:a 6 music.ogg
   ```

3. **Enable Gzip Compression:**
   - Most hosting platforms do this automatically
   - For self-hosting, enable in server config

### Loading Screen

The custom HTML template in `web/index.html` includes a styled loading screen. Modify it to match your game's branding.

---

## Mobile Browser Support

The game works on mobile browsers with touch events:

**To optimize for mobile:**

1. Add touch controls (virtual joystick)
2. Test on various screen sizes
3. Consider portrait/landscape orientation
4. Optimize performance for mobile GPUs

---

## Troubleshooting

### Game doesn't load

- **Check browser console** for errors
- Ensure CORS headers are set correctly
- Verify you're using a web server (not `file://`)
- Test in Chrome/Firefox (best WebAssembly support)

### Black screen

- Check Godot export logs for errors
- Verify all assets are included in export
- Test with a fresh export

### Poor performance

- Reduce texture quality in export settings
- Disable post-processing effects
- Lower resolution or add quality settings

### Controls not working

- Ensure canvas has focus (click on game)
- Check browser console for input errors
- Test in different browsers

---

## Recommended: GitHub Pages Quick Setup

For the fastest deployment:

```bash
# In your repository root
mkdir -p docs
# Export from Godot to docs/index.html
touch docs/.nojekyll
git add docs/
git commit -m "Add web build"
git push
# Enable GitHub Pages in repository settings → Pages → Source: main/docs
```

Your game will be live in minutes!

---

## Progressive Web App (PWA)

To make your game installable as a PWA:

1. Enable in `export_presets.cfg`:
   ```
   progressive_web_app/enabled=true
   ```

2. Add icons (144x144, 180x180, 512x512)

3. Configure manifest and service worker

Players can then "install" your game like a native app!

---

## Analytics (Optional)

Track player engagement by adding Google Analytics or similar:

```html
<!-- Add to custom HTML head in web/index.html -->
<script async src="https://www.googletagmanager.com/gtag/js?id=GA_MEASUREMENT_ID"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());
  gtag('config', 'GA_MEASUREMENT_ID');
</script>
```

---

## Summary

| Platform | Cost | Difficulty | Best For |
|----------|------|------------|----------|
| **GitHub Pages** | Free | Easy | Quick sharing, open source |
| **itch.io** | Free | Easiest | Indie game distribution |
| **Netlify/Vercel** | Free | Easy | Professional hosting |
| **Self-Hosting** | Varies | Medium | Full control |

**Recommendation:** Start with **itch.io** for game distribution or **GitHub Pages** for quick sharing!
