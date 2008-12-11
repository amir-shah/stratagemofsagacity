using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SoS
{
    public class Map
    {
        int width, height;
        Color backgroundColor = Color.Beige;
        Texture2D background;
        Rectangle spriteRect;
        List<Obstacle> obs = new List<Obstacle>();
        public Map(int _width, int _height, Color _background)
        {
            width = _width;
            height = _height;
            backgroundColor = _background;
        }
        public Map(int _width, int _height, Texture2D _background)
        {
            width = _width;
            height = _height;
            background = _background;
            spriteRect = new Rectangle(0, 0, _background.Width, _background.Height);
        }

        public bool loadObstacles(List<Obstacle> obstacles)
        {
            obs.AddRange(obstacles);
            return true;
        }

        public void draw(SpriteBatch batch, Rectangle scope)
        {
            batch.GraphicsDevice.Clear(backgroundColor);
            if (background != null && scope.Intersects(new Rectangle(0,0,background.Width,background.Height)))
            {
                int picWidth = scope.Width, picHeight = scope.Height;
                if (picWidth > background.Width - scope.X)
                    picWidth = background.Width - scope.X;
                if (picHeight > background.Height - scope.Y)
                    picHeight = background.Height - scope.Y;
                batch.Draw(background, new Rectangle(0, 0, picWidth, picHeight), new Rectangle(scope.X,scope.Y, picWidth, picHeight), Color.White);
            }
            foreach (Obstacle o in obs)
            {
                if (o.intersects(scope))
                    o.draw(batch, scope);
            }
        }
        public void drawMini(SpriteBatch batch, Rectangle scope, Rectangle mini)
        {
            if (background != null && scope.Intersects(new Rectangle(0, 0, background.Width, background.Height)))
            {
                int picWidth = scope.Width, picHeight = scope.Height;
                double factor = scope.Width / mini.Width;
                if (picWidth > background.Width - scope.X)
                    picWidth = background.Width - scope.X;
                if (picHeight > background.Height - scope.Y)
                    picHeight = background.Height - scope.Y;
                batch.Draw(background, new Rectangle(mini.X, mini.Y, (int)(picWidth/factor), (int)(picHeight/factor)), new Rectangle(scope.X, scope.Y, picWidth, picHeight), Color.White);
            }
            foreach (Obstacle o in obs)
            {
                if (o.intersects(scope))
                    o.drawMini(batch,scope,mini);
            }
        }
        public int getWidth()
        {
            return width;
        }
        public int getHeight()
        {
            return height;
        }
    }
}
