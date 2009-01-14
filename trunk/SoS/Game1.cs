using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using SOS;
using GameStateManagement;
using Microsoft.Xna.Framework.Media;

namespace SoS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Map map;
        MouseState mouseState;
        Texture2D wallSprite, background, mouseSprite, enemySprite, playerSprite;
        Rectangle camera = new Rectangle(0,0, WIDTH, HEIGHT);
        int xCamBuffer = 100, yCamBuffer = 100; //cameraSpeed = 1;
        const int future = 2000;
        public static Texture2D pixel;
        public static int WIDTH = 800, HEIGHT = 537;
        int miniNum = 3;
        Player player;


        List<Being> beings = new List<Being>();
        List<Projectile> projectiles = new List<Projectile>();
        
        //Menu stuff
        int time = 1800; // 30 seconds
        const int menuState = 0, pausedState = 1, playState = 2, optionsState = 3, introState = 4, controlsState = 5;
        StateMachine stateMachine = new StateMachine(0);
        bool pauseHeld = false;
        SpriteFont font;
        KeyboardState oldKeyState, newKeyState;
        Texture2D BG; //Background
        int xOffset = 30;
        int yOffset = 40;
        String[] menuItems, optionsMenuItems;
        int selected = 0;
        InputState input = new InputState();
        Song hellRaider;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            hellRaider = Content.Load<Song>("hellRaider");
            MediaPlayer.Play(hellRaider);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.Title = "Stratagem of Sagacity - v 0.1 Pre-Alpha";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            wallSprite = Content.Load<Texture2D>("box");
            pixel = Content.Load<Texture2D>("blankPixel");
            background = Content.Load<Texture2D>("floor");
            mouseSprite = Content.Load<Texture2D>("crosshair");
            enemySprite = Content.Load<Texture2D>("tank");
            playerSprite = Content.Load<Texture2D>("turtle");
            map = new Map(10000, 10000, background);
            List<Obstacle> walls = new List<Obstacle>();
            walls.Add(new Wall(wallSprite, 10, 10, 2, 5));
            walls.Add(new Wall(wallSprite, 100, 300, 10, 1));
            walls.Add(new Wall(wallSprite, 1000, 100, 3, 3));
            walls.Add(new Wall(wallSprite, 100, 700, 1, 1));
            map.loadObstacles(walls);
            player = new Player(playerSprite, new Rectangle(100,100,50,50),Color.White, this);
            beings.Add(new Being(100, 10, enemySprite));
            beings.Add(new BoxBeing(950, 60, enemySprite, 200));
            beings.Add(player);

            //Menu Stuff
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BG = Content.Load<Texture2D>("bg");
            font = Content.Load<SpriteFont>("TimesNewRoman");
            menuItems = new String[4] { "play", "options", "intro", "exit" };
            optionsMenuItems = new String[2] { "controls", "back" };
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (stateMachine.getState() != pausedState && !pauseHeld && stateMachine.getState() != menuState)
                    stateMachine.changeState(pausedState);
                else if (stateMachine.getState() == pausedState && !pauseHeld)
                    stateMachine.changeState(stateMachine.getPrevState());
                pauseHeld = true;
                /*if (stateMachine.getState() != pausedState && stateMachine.getPrevState() != pausedState)
                    stateMachine.changeState(pausedState);
                if 
                    stateMachine.changeState(playState);*/
            }
            else
                pauseHeld = false;
            if (stateMachine.getState() == playState)
            {
                int elapsedTime = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                foreach (Being b in beings)
                {
                    b.UpdateMove(gameTime);
                }
                foreach (Projectile p in projectiles)
                {
                    p.UpdateMove(gameTime);
                }
                mouseState = Mouse.GetState();
                if (player.getPoint(camera).X < xCamBuffer)
                    camera.X -= (int)(player.getSpeed() * elapsedTime); // xCamBuffer - mouseState.X;
                if (player.getPoint(camera).X > WIDTH - xCamBuffer)
                    camera.X += (int)(player.getSpeed() * elapsedTime); // mouseState.X - (WIDTH - xCamBuffer);
                if (player.getPoint(camera).Y < yCamBuffer)
                    camera.Y -= (int)(player.getSpeed() * elapsedTime); //yCamBuffer - mouseState.Y;
                if (player.getPoint(camera).Y > HEIGHT - yCamBuffer)
                    camera.Y += (int)(player.getSpeed() * elapsedTime); // mouseState.Y - (HEIGHT - yCamBuffer);
                if (camera.X < 0) camera.X = 0;
                if (camera.X + WIDTH > map.getWidth()) camera.X = map.getWidth() - WIDTH;
                if (camera.Y < 0) camera.Y = 0;
                if (camera.Y + HEIGHT > map.getHeight()) camera.Y = map.getHeight() - HEIGHT;
                // TODO: Add your update logic here
                checkCollisions();
            }
            else if (stateMachine.getState() == menuState)
            {
                updateMenu(gameTime);
            }
            else if (stateMachine.getState() == optionsState)
            {
                updateOptionsMenu(gameTime);
            }
            else if (stateMachine.getState() == controlsState)
            {
                updateControlsMenu(gameTime);
            }
            base.Update(gameTime);
        }
       public void checkCollisions()
        {
            //put everything on screen in one list
            List<Collideable> all = addAll();
             
            for (int i = 0; i < all.Count; i++)
            {
                for (int j = i + 1; j < all.Count; j++)
                {
                    Collideable me = all[i];
                    Collideable him = all[j];

                    if (me.collides(him))
                    {
                        him.collidedWith(me);
                        me.collidedWith(him);

                    }
                }
            }
        }
       private List<Collideable> addAll()
       {
           List<Collideable> all = new List<Collideable>();
           foreach (Being b in beings)
           {
               all.Add(b);
           }
           foreach (Projectile p in projectiles)
           {
               all.Add(p);
           }
           foreach (Obstacle o in map.getObs())
           {
               all.Add(o);
           }
           return all;
       }
        public void updateControlsMenu(GameTime gameTime)
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();

            if (oldKeyState.IsKeyUp(Keys.Down) && newKeyState.IsKeyDown(Keys.Down))
            {
                if (selected < optionsMenuItems.Length)
                    selected++;
                if (selected == optionsMenuItems.Length)
                    selected = 0;
                Console.WriteLine(selected);
            }

            else if (oldKeyState.IsKeyUp(Keys.Up) && newKeyState.IsKeyDown(Keys.Up))
            {
                if (selected == 0)
                    selected = optionsMenuItems.Length;
                if (selected > 0)
                    selected--;
                Console.WriteLine(selected);
            }

            if (oldKeyState.IsKeyUp(Keys.Enter) && newKeyState.IsKeyDown(Keys.Enter))
                changePlayState();
        }

        public void updateOptionsMenu(GameTime gameTime)
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();

            if (oldKeyState.IsKeyUp(Keys.Down) && newKeyState.IsKeyDown(Keys.Down))
            {
                if (selected < optionsMenuItems.Length)
                    selected++;
                if (selected == optionsMenuItems.Length)
                    selected = 0;
                Console.WriteLine(selected);
            }

            else if (oldKeyState.IsKeyUp(Keys.Up) && newKeyState.IsKeyDown(Keys.Up))
            {
                if (selected == 0)
                    selected = optionsMenuItems.Length;
                if (selected > 0)
                    selected--;
                Console.WriteLine(selected);
            }

            if (oldKeyState.IsKeyUp(Keys.Enter) && newKeyState.IsKeyDown(Keys.Enter))
                changeOptionsState();
        }

        public void updateMenu(GameTime gameTime)
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();
    
            if (oldKeyState.IsKeyUp(Keys.Down) && newKeyState.IsKeyDown(Keys.Down))
            {
                if (selected < menuItems.Length)
                    selected++;
                if (selected == menuItems.Length)
                    selected = 0;
                Console.WriteLine(selected);
            }

            else if (oldKeyState.IsKeyUp(Keys.Up) && newKeyState.IsKeyDown(Keys.Up))
            {
                if (selected == 0)
                    selected = menuItems.Length;
                if (selected > 0)
                    selected--;
                Console.WriteLine(selected);
            }

            if (oldKeyState.IsKeyUp(Keys.Enter) && newKeyState.IsKeyDown(Keys.Enter))
                changePlayState();
        }

        public void changePlayState()
        {
            switch (menuItems[selected])
            {
                case "play":
                    stateMachine.changeState(2);
                    break;
                case "options":
                    stateMachine.changeState(3);
                    break;
                case "intro":
                    stateMachine.changeState(4);
                    break;
                case "exit":
                    this.Exit();
                    break;
            }

            
        }
        public void changeOptionsState()
        {
            switch (optionsMenuItems[selected])
            {
                case "controls":
                    stateMachine.changeState(5);
                    break;
                case "back":
                    stateMachine.changeState(0);
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            if (stateMachine.getState() == menuState)
            {
                graphics.GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                switch (stateMachine.getState())
                {
                    case menuState:
                        drawMenu(gameTime);
                        break;
                    case playState:
                        spriteBatch.Draw(BG, Vector2.Zero, Color.White);
                        break;
                    case pausedState:
                        spriteBatch.Draw(BG, Vector2.Zero, Color.White);
                        break;
                    case introState:
                        break;

                }
                spriteBatch.End();
            }
            else if (stateMachine.getState() == optionsState)
            {
                graphics.GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                switch (stateMachine.getState())
                {
                    case controlsState:
                        drawOptionsMenu(gameTime);
                        break;

                }
                spriteBatch.End();
            }
            else if (stateMachine.getState() == playState)
            {
                Rectangle miniMap = new Rectangle((int)(WIDTH - (WIDTH / miniNum)), (int)(HEIGHT - (HEIGHT / miniNum)), (int)(WIDTH / miniNum), (int)(HEIGHT / miniNum));
                spriteBatch.Begin();
                map.draw(spriteBatch, camera);


                foreach (Being b in beings)
                {
                    b.Draw(camera, spriteBatch);
                }
                foreach (Projectile p in projectiles)
                {
                    p.Draw(camera, spriteBatch);
                }
                spriteBatch.Draw(mouseSprite, new Rectangle(mouseState.X - 5, mouseState.Y - 5, 10, 10), Color.White);
                //After everything else is drawn
                //Draw mini map
                map.drawMini(spriteBatch, camera, miniMap);
                for (int i = miniMap.X; i < miniMap.X + miniMap.Width; i++)
                {
                    spriteBatch.Draw(pixel, new Rectangle(i, miniMap.Y, 1, 1), Color.Black);
                }
                for (int i = miniMap.Y; i < miniMap.Y + miniMap.Height; i++)
                {
                    spriteBatch.Draw(pixel, new Rectangle(miniMap.X, i, 1, 1), Color.Black);
                }
                GameTime futureTime = new GameTime(new TimeSpan(0, 0, 0, 0, future), new TimeSpan(0, 0, 0, 0, future), new TimeSpan(0, 0, 0, 0, future), new TimeSpan(0, 0, 0, 0, future));
                foreach (Being b in beings)
                {
                    Being fB = b.Predict(futureTime);
                    if (fB.getRectangle().Intersects(camera))
                    {
                        fB.drawMini(spriteBatch, camera, miniMap);
                    }
                }
                foreach (Projectile p in projectiles)
                {
                    Projectile fP = p.Predict(futureTime);
                    if (fP.intersects(camera))
                    {
                        fP.drawMini(spriteBatch, camera, miniMap);
                    }
                }
                spriteBatch.End();
            }
                if (stateMachine.getState() == pausedState)
                {
                    String message = "PAUSED";
                    Vector2 mSize = font.MeasureString(message);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font,message,new Vector2((WIDTH/2f)-mSize.X,(HEIGHT/2f)-mSize.Y),Color.White);
                    spriteBatch.End();
                }
                
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        public void drawOptionsMenu(GameTime gameTime)
        {
            spriteBatch.Draw(BG, Vector2.Zero, Color.White);
            String name = "Stratagem of Sagacity";
            spriteBatch.DrawString(font, name, new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString(name).X / 2, 0), Color.GreenYellow);
            for (int c = 0; c < optionsMenuItems.Length; c++)
            {
                if (selected == c)
                    spriteBatch.DrawString(font, optionsMenuItems[c], new Vector2(xOffset, 100 + c * yOffset), Color.Yellow);
                else
                    spriteBatch.DrawString(font, optionsMenuItems[c], new Vector2(xOffset, 100 + c * yOffset), Color.White);
            }
        }
        public void drawMenu(GameTime gameTime)
        {
            spriteBatch.Draw(BG, Vector2.Zero, Color.White);
            String name = "Stratagem of Sagacity";
            spriteBatch.DrawString(font, name, new Vector2(graphics.PreferredBackBufferWidth / 2 - font.MeasureString(name).X / 2, 0), Color.GreenYellow);
            for (int c = 0; c < menuItems.Length; c++)
            {
                if (selected == c)
                    spriteBatch.DrawString(font, menuItems[c], new Vector2(xOffset, 100 + c * yOffset), Color.Yellow);
                else
                    spriteBatch.DrawString(font, menuItems[c], new Vector2(xOffset, 100 + c * yOffset), Color.White);
            }
        }
        public bool remove(Collideable other)
        {
            if (other is Being)
            {
                return beings.Remove((Being)other);
            }
            else if(other is Projectile)
            {
                return projectiles.Remove((Projectile)other);
            }
            else if (other is Obstacle)
            {
                return map.remove((Obstacle)other);
            }
            return false;
        }
        public void addProjectile(Projectile p)
        {
            p.setPic(Content.Load<Texture2D>(p.getPicName()));
            projectiles.Add(p);

        }
        public Rectangle getCamera()
        {
            return camera;
        }
        
    }
}
