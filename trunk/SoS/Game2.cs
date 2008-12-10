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
using Microsoft.Xna.Framework.Media;
using GameStateManagement;

namespace SoS
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game2 : Microsoft.Xna.Framework.Game
    {
        int time = 1800; // 30 seconds
        const int menuState = 0, pausedState = 1, playState = 2, optionsState = 3, introState =4;
        StateMachine stateMachine = new StateMachine(0);
        SpriteFont font;
        KeyboardState oldKeyState, newKeyState;
        GraphicsDeviceManager graphics; //Default graphics device
        SpriteBatch spriteBatch; //Sprite Batch for drawing graphics
        Texture2D BG; //Background
        int xOffset = 30;
        int yOffset = 40;
        String[] menuItems;
        int selected = 0;
        InputState input = new InputState();
        Song hellRaider;

        public Game2()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 537;
            Content.RootDirectory = "Content";
            hellRaider = Content.Load<Song>("hellRaider");
            MediaPlayer.Play(hellRaider);
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.Title = "Stratagem of Sagacity - v 0.1 Pre-Alpha";

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BG = Content.Load<Texture2D>("bg");
            font = Content.Load<SpriteFont>("TimesNewRoman");
            menuItems = new String[4]{"play", "options", "intro", "exit"};
            
            
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            
            updateMenu(gameTime);

            base.Update(gameTime);
        }

        public void updateMenu(GameTime gameTime)
        {
            handleInput(input);

        }

        public void handleInput(InputState input)
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
            if (oldKeyState.IsKeyUp(Keys.Escape) && newKeyState.IsKeyDown(Keys.Escape))
                stateMachine.changeState(1);
        }

        public void changePlayState(){
            switch(menuItems[selected]){
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

        protected override void Draw(GameTime gameTime)
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
            base.Draw(gameTime);
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
        
    }
}
