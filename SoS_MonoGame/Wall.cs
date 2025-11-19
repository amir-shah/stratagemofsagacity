using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
/*Wall Class by Wesley Ripley
 *  
 * Class that represents a wall in the tank buster game
 */
namespace SoS
{
    class Wall : Obstacle
    {
        int width, height;//all in units
        //Texture2D sprite;
        private bool visible = true;
        Color color = Color.White;
        bool showRect = false;
        public Wall(Texture2D _sprite,float _x, float _y)
        {
            pos.X = _x;
            pos.Y = _y;
            width = 1;
            height = 1;
            pic = _sprite;
            //there is sprite, so just sprite bounds
            picRect = new Rectangle((int)pos.X, (int)pos.Y, pic.Width, pic.Height);
        }
        public Wall(Texture2D _sprite, float _x, float _y, int _width, int _height)
        {
            pos.X = _x;
            pos.Y = _y;
            width = _width;
            height = _height;
            pic = _sprite;
            //more than 1 sprite in sequence, total bounds
            picRect = new Rectangle((int)pos.X, (int)pos.Y, pic.Width * width, pic.Height * height);
        }
        public bool isVisible()
        {
            return visible;
        }
        public void setVisible(bool visibility)
        {
            visible = visibility;
        }
        public void setColor(Color newColor)
        {
            color = newColor;
        }
        public override void draw(SpriteBatch batch,Rectangle scope)
        {
            if (!visible)
                return;
            int curX = (int)pos.X - scope.X, curY = (int)pos.Y - scope.Y;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle current = new Rectangle(curX, curY, pic.Width, pic.Height);
                    batch.Draw(pic, current, color);
                    curX += pic.Width;
                }
                curX = (int)pos.X - scope.X;
                curY += pic.Height;
            }
            if (showRect)
            {
                Rectangle rect = getBoundRect();
                Rectangle rect2 = picRect;
                batch.Draw(pic, new Rectangle(rect.Left, rect.Top, rect.Width, 5), Color.Black);
                batch.Draw(pic, new Rectangle(rect.Left, rect.Bottom, rect.Width, 5), Color.Black);
                batch.Draw(pic, new Rectangle(rect.Left, rect.Top, 5, rect.Height), Color.Black);
                batch.Draw(pic, new Rectangle(rect.Right, rect.Top, 5, rect.Height), Color.Black);
                
                batch.Draw(pic, new Rectangle(rect2.Left, rect2.Top, rect2.Width, 5), Color.Red);
                batch.Draw(pic, new Rectangle(rect2.Left, rect2.Bottom, rect2.Width, 5), Color.Red);
                batch.Draw(pic, new Rectangle(rect2.Left, rect2.Top, 5, rect2.Height), Color.Red);
                batch.Draw(pic, new Rectangle(rect2.Right, rect2.Top, 5, rect2.Height), Color.Red);
            }
            /*top.X -= scope.X; top.Y -= scope.Y;
            bottom.X -= scope.X; bottom.Y -= scope.Y;
            left.X -= scope.X; left.Y -= scope.Y;
            right.X -= scope.X; right.Y -= scope.Y;
            batch.Draw(sprite, top, Color.Red);
            batch.Draw(sprite, bottom, Color.Red);
            batch.Draw(sprite, left, Color.Red);
            batch.Draw(sprite, right, Color.Red);*/
        }
        public override void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            //Console.WriteLine("drawMini: " + width + " " + height);
            if (!visible)
                return;
            double factor = scope.Width / mini.Width;
            int curX = (int)pos.X - scope.X, curY = (int)pos.Y - scope.Y;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle current = new Rectangle((int)(mini.X + (curX / factor)), (int)(mini.Y + (curY / factor)), (int)(pic.Width / factor), (int)(pic.Height / factor));
                    batch.Draw(pic, current, color);
                    curX += pic.Width;
                }
                curX = (int)pos.X - scope.X;
                curY += pic.Height;
            }
        }
        public override Rectangle getBoundRect()
        {
            return picRect;
        }
        public override bool collides(Collideable other)
        {
            int curX = (int)pos.X;
            int curY = (int)pos.Y;
            Rectangle tempRec = picRect;
            Vector2 tempPos = pos;
            bool collide = false;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    picRect = new Rectangle(curX, curY, pic.Width, pic.Height);
                    pos = new Vector2((float)curX,(float)curY);
                    collide = base.collides(other);
                    if (collide)
                    {
                        picRect = tempRec;
                        pos = tempPos;
                        return true;
                    }
                    curX += pic.Width;
                }
                curX = (int)pos.X;
                curY += pic.Height;
            }
            picRect = tempRec;
            pos = tempPos;
            return collide;
        }
    }
}
