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
        int x, y, width, height;//all in units
        Texture2D sprite;
        private bool visible = true;
        Color color = Color.White;

        public Wall(Texture2D _sprite,int _x, int _y)
        {
            x = _x;
            y = _y;
            width = 1;
            height = 1;
            sprite = _sprite;
            //there is sprite, so just sprite bounds
            picRect = new Rectangle(x, y, sprite.Width, sprite.Height);
        }
        public Wall(Texture2D _sprite, int _x, int _y, int _width, int _height)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            sprite = _sprite;
            //more than 1 sprite in sequence, total bounds
            picRect = new Rectangle(x, y, sprite.Width*width, sprite.Height*height);
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
            int curX = x - scope.X, curY = y - scope.Y;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle current = new Rectangle(curX, curY, sprite.Width, sprite.Height);
                    batch.Draw(sprite, current, color);
                    curX += sprite.Width;
                }
                curX = x - scope.X;
                curY += sprite.Height;
            }
        }
        public override void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            Console.WriteLine("drawMini: " + width + " " + height);
            if (!visible)
                return;
            double factor = scope.Width / mini.Width;
            int curX = x - scope.X, curY = y - scope.Y;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle current = new Rectangle((int)(mini.X + (curX / factor)), (int)(mini.Y + (curY / factor)), (int)(sprite.Width / factor), (int)(sprite.Height / factor));
                    batch.Draw(sprite, current, color);
                    curX += sprite.Width;
                }
                curX = x - scope.X;
                curY += sprite.Height;
            }
        }
    }
}
