using System;
using System.Collections.Generic;
using System.Text;
using SOS;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoS
{
    class Player : Being
    {
        float speed = .2f;
        Game1 game;
        bool isMouseDown;
        public Player(int x, int y, Texture2D _pic, Game1 _game) : base(x,y,_pic)
        {
           xVel = 0f; yVel = 0f;
           game = _game;
           isMouseDown = false;
        }
        public Player(Texture2D _pic, Rectangle rect, Color c, Game1 _game)
            : base(_pic, rect, c)
        {
            xVel = 0f; yVel = 0f;
            game = _game;
            isMouseDown = false;
        }
        public Player(Texture2D _pic, Rectangle _picRect, float _rotation, int _velocity, float _health, Color _color, Game1 _game)
                    : base(_pic,_picRect,_rotation,_velocity,_health,_color)
        {
            xVel = 0f; yVel = 0f;
            game = _game;
            isMouseDown = false;
        } 
        public override void UpdateMove(GameTime gameTime)
        {
            int elapsedTime = (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            //MouseState mouse = Mouse.GetState();
            //rotation = -(float)Math.Atan2((((picRect.X + (picRect.Width / 2))) - mouse.X) * (Math.PI / 180), (((picRect.Y + (picRect.Height / 2))) - mouse.Y) * (Math.PI / 180));
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            xVel = 0f; yVel = 0f;
            if (keyState.IsKeyDown(Keys.W))
            {
                yVel = -speed;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                yVel = speed;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                xVel = -speed;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                xVel = speed;
            }
            picRect.X += (int)(xVel * elapsedTime);
            picRect.Y += (int)(yVel * elapsedTime);
            if (picRect.X <= 0)
            {
                picRect.X = 0;
            }
            if (picRect.Y <= 0)
            {
                picRect.Y = 0;
            }
            if (picRect.X + pic.Width >= Game1.map.getWidth())
            {
                picRect.X = Game1.map.getWidth() - pic.Width;
            }
            if (picRect.Y + pic.Height >= Game1.map.getHeight())
            {
                picRect.Y = Game1.map.getHeight() - pic.Height;
            }
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (!isMouseDown)
                {
                    Projectile shot = new Projectile("pShot", new Rectangle(picRect.X, picRect.Y, 20, 20), .35f, color, rotation);
                    shot.setRotation(rotation);
                    game.addProjectile(shot);
                    isMouseDown = true;
                }
            }
            if (mouse.LeftButton == ButtonState.Released)
            {
                isMouseDown = false;
            }
        }
        public override void  Draw(Rectangle scope, SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();
            int mX, mY;
            mX = mouse.X + scope.X; mY = mouse.Y + scope.Y;
            rotation = -(float)Math.Atan2((((picRect.X + (picRect.Width / 2))) - mX), (((picRect.Y + (picRect.Height / 2))) - mY));
 	        base.Draw(scope, spriteBatch);
        }
        public override Being Predict(GameTime gameTime)
        {
            Player futureSelf = new Player(pic, picRect, color,game);
            futureSelf.setVelocity(new Vector2(xVel, yVel)); futureSelf.setRotation(rotation);
            return (Being)futureSelf;
        }
        public float getSpeed()
        {
            return speed;
        }
    }
}
