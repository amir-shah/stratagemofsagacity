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
        List<List<Texture2D>> playerMovement;
        Point playerMovementState = new Point(0, 0);

        public Player(float x,float y, Texture2D _pic,float _scale,Game1 _game) : base(x,y,_pic,_scale)
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
        public Player(Texture2D _pic, Rectangle _picRect, float _rotation, float _health, Color _color, Game1 _game)
                    : base(_pic,_picRect,_rotation,_health,_color)
        {
            xVel = 0f; yVel = 0f;
            game = _game;
            isMouseDown = false;
        }
        public void loadMovementList(List<List<Texture2D>> pm)
        {
            playerMovement = pm;
        }
        public override void UpdateMove(GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;
            //MouseState mouse = Mouse.GetState();
            //rotation = -(float)Math.Atan2((((picRect.X + (picRect.Width / 2))) - mouse.X) * (Math.PI / 180), (((picRect.Y + (picRect.Height / 2))) - mouse.Y) * (Math.PI / 180));
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            float mX, mY, pX, pY;
            mX = mouse.X; mY = mouse.Y;
            pX = pos.X - game.getCamera().X; pY = pos.Y - game.getCamera().Y;
            rotation = -(float)Math.Atan2((double)(pX - mX),(double)(pY -mY));
            
            xVel = 0f; yVel = 0f;
            if (keyState.IsKeyDown(Keys.W) && canMoveUp)
            {
                yVel = -speed;
                if (playerMovement != null)
                {
                    playerMovementState.X = 1;
                    playerMovementState.Y++;
                    if (playerMovementState.Y >= playerMovement[playerMovementState.X].Count)
                    {
                        playerMovementState.Y = 0;
                    }
                    setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                    width = (int)(pic.Width * scale); height = (int)(pic.Height * scale);
                    picRect = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
                }
            }
            if (keyState.IsKeyDown(Keys.S) && canMoveDown)
            {
                yVel = speed;
                if (playerMovement != null)
                {
                    playerMovementState.X = 1;
                    playerMovementState.Y++;
                    if (playerMovementState.Y >= playerMovement[playerMovementState.X].Count)
                    {
                        playerMovementState.Y = 0;
                    }
                    setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                    width = (int)(pic.Width * scale); height = (int)(pic.Height * scale);
                    picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
                }
            }
            if (keyState.IsKeyDown(Keys.A) && canMoveLeft)
            {
                xVel = -speed;
                if (playerMovement != null)
                {
                    playerMovementState.X = 1;
                    playerMovementState.Y++;
                    if (playerMovementState.Y >= playerMovement[playerMovementState.X].Count)
                    {
                        playerMovementState.Y = 0;
                    }
                    setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                    width = (int)(pic.Width * scale); height = (int)(pic.Height * scale);
                    picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
                }
            }
            if (keyState.IsKeyDown(Keys.D) && canMoveRight)
            {
                xVel = speed;
                //if (playerMovement != null)
                //{
                    playerMovementState.X = 1;
                    playerMovementState.Y++;
                    if (playerMovementState.Y >= playerMovement[playerMovementState.X].Count)
                    {
                        playerMovementState.Y = 0;
                    }
                    setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                    width = (int)(pic.Width * scale); height = (int)(pic.Height * scale);
                    picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
                //}
            }
            if (keyState.IsKeyUp(Keys.W) && keyState.IsKeyUp(Keys.A) && keyState.IsKeyUp(Keys.S) && keyState.IsKeyUp(Keys.D))
            {
                playerMovementState.X = 0;
                playerMovementState.Y = 0;
                //setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                //picRect = new Rectangle(picRect.X, picRect.Y, pic.Width / 2, pic.Height / 2);
            }

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (!isMouseDown)
                {
                    float shotX = pos.X + (float)((width + 20) * Math.Sin(rotation));
                    float shotY = pos.Y + (float)((height + 20)* -Math.Cos(rotation));
                    Projectile shot = new Projectile(shotX,shotY,"pShot", .3f,21,21,game);
                    shot.setRotation(rotation);
                    game.addProjectile(shot);
                    //Graphics
                    if (playerMovement != null)
                    {
                        playerMovementState.X = 2;
                        playerMovementState.Y++;
                        if (playerMovementState.Y >= playerMovement[playerMovementState.X].Count)
                        {
                            playerMovementState.Y = 0;
                        }
                        setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                        width = (int)(pic.Width * scale); height = (int)(pic.Height * scale);
                        picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
                    }
                    game.playGunfire();
                    isMouseDown = true;
                }
            }
            if (mouse.LeftButton == ButtonState.Released)
            {
                isMouseDown = false;
                playerMovementState.X = 0;
                playerMovementState.Y = 0;
                setSprite(playerMovement[playerMovementState.X][playerMovementState.Y]);
                width = (int)(pic.Width * scale); height = (int)(pic.Height * scale);
                picRect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            }

            pos.X += (float)(xVel * elapsedTime);
            pos.Y += (float)(yVel * elapsedTime);
            if (pos.X <= 0)
            {
                pos.X = 0;
            }
            if (pos.Y <= 0)
            {
                pos.Y = 0;
            }
            if (pos.X + width >= Game1.map.getWidth())
            {
                pos.X = Game1.map.getWidth() - width;
            }
            if (pos.Y + height >= Game1.map.getHeight())
            {
                pos.Y = Game1.map.getHeight() - height;
            }
            picRect.X = (int)pos.X; picRect.Y = (int)pos.Y;
            canMoveUp = true; canMoveDown = true; canMoveLeft = true; canMoveRight = true;
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
        public override void collidedWith(Collideable other) { }
    }
}
