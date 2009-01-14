using System;
using System.Collections.Generic;
using System.Text;
using SOS;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SoS
{
    class BoxBeing : Being
    {
        int xInit, yInit;
        int sideLength;

        public BoxBeing(int x, int y, Texture2D _pic, int side) : base(x,y,_pic)
        {
            //defaults
            xInit = x;
            yInit = y;
            sideLength = side;
        }

        public override void UpdateMove(Microsoft.Xna.Framework.GameTime gameTime)
        {
            int elapsedTime = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            picRect.X += (int)(xVel * elapsedTime);
            picRect.Y += (int)(yVel * elapsedTime);

            if ((picRect.X - xInit >= sideLength && xVel > 0)
                || ((picRect.X - xInit <= 0) && (xVel < 0)))
            {
                if ((picRect.X - xInit >= sideLength && xVel > 0))
                {
                    picRect.Y += (picRect.X - xInit) - sideLength;
                    picRect.X = xInit + sideLength;
                }
                else if ((picRect.X - xInit <= 0) && (xVel < 0))
                {
                    picRect.Y += picRect.X - xInit;
                    picRect.X = xInit;
                }
                yVel = xVel;
                xVel = 0;
                
            }
            if ((picRect.Y - yInit >= sideLength && yVel > 0)
                || ((picRect.Y - yInit <= 0) && (yVel < 0)))
            {
                if (picRect.Y - yInit >= sideLength && yVel > 0)
                {
                    picRect.X -= (picRect.Y - yInit) - sideLength;
                    picRect.Y = yInit + sideLength;
                }
                else if ((picRect.Y - yInit <= 0) && (yVel < 0))
                {
                    picRect.X -= picRect.Y - yInit;
                    picRect.Y = yInit;
                }
                xVel = -yVel;
                yVel = 0;
                
            }


        }
        public void setOrigin(Point orig)
        {
            xInit = orig.X;
            yInit = orig.Y;
        }
        public override Being Predict(GameTime gameTime)
        {
            BoxBeing futureSelf = new BoxBeing(picRect.X, picRect.Y, pic,sideLength);
            futureSelf.setVelocity(new Vector2(xVel, yVel));
            futureSelf.setOrigin(new Point(xInit, yInit));
            futureSelf.UpdateMove(gameTime);
            return (Being)futureSelf;
        }
    }
}
