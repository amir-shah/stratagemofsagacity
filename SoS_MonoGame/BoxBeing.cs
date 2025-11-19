using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SoS
{
    class BoxBeing : Being
    {
        float xInit, yInit;
        int sideLength;

        public BoxBeing(float x, float y, Texture2D _pic,float _scale, int side) : base(x,y,_pic,_scale)
        {
            //defaults
            xInit = x;
            yInit = y;
            sideLength = side;
        }

        public override void UpdateMove(Microsoft.Xna.Framework.GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalMilliseconds;

           pos.X += (float)(xVel * elapsedTime);
           pos.Y += (float)(yVel * elapsedTime);

            if ((pos.X - xInit >= sideLength && xVel > 0)
                || ((pos.X - xInit <= 0) && (xVel < 0)))
            {
                if ((pos.X - xInit >= sideLength && xVel > 0))
                {
                    pos.Y += (pos.X - xInit) - sideLength;
                    pos.X = xInit + sideLength;
                }
                else if ((pos.X - xInit <= 0) && (xVel < 0))
                {
                    pos.Y += pos.X - xInit;
                    pos.X = xInit;
                }
                yVel = xVel;
                xVel = 0;
                
            }
            if ((pos.Y - yInit >= sideLength && yVel > 0)
                || ((pos.Y - yInit <= 0) && (yVel < 0)))
            {
                if (pos.Y - yInit >= sideLength && yVel > 0)
                {
                    pos.X -= (pos.Y - yInit) - sideLength;
                    pos.Y = yInit + sideLength;
                }
                else if ((pos.Y - yInit <= 0) && (yVel < 0))
                {
                    pos.X -= pos.Y - yInit;
                    pos.Y = yInit;
                }
                xVel = -yVel;
                yVel = 0;
                
            }
            picRect.X = (int)pos.X;
            picRect.Y = (int)pos.Y;


        }
        public void setOrigPoint(Vector2 orig)
        {
            xInit = orig.X;
            yInit = orig.Y;
        }
        public override Being Predict(GameTime gameTime)
        {
            BoxBeing futureSelf = new BoxBeing(pos.X, pos.Y,pic,scale,sideLength);
            futureSelf.setVelocity(new Vector2(xVel, yVel));
            futureSelf.setOrigPoint(new Vector2(xInit, yInit));
            futureSelf.UpdateMove(gameTime);
            return (Being)futureSelf;
        }
        public override Matrix getMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(pos, 0.0f));
        }
    }
}
