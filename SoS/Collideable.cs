using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoS
{
    public abstract class Collideable : Object
    {
        protected Rectangle picRect;
        protected Texture2D pic;
        protected Vector2 pos, origin;
        protected float rotation;
        protected float scale;
        public virtual bool collides(Collideable other)
        {
            if (getBoundRect().Intersects(other.getBoundRect()))
            {
                
                Matrix transformA = getMatrix();
                Matrix transformB = other.getMatrix();
                int widthA = pic.Width; int heightA = pic.Height;
                int widthB = other.getPic().Width; int heightB = other.getPic().Height;
                Color[] dataA = new Color[widthA * heightA];
                pic.GetData(dataA);
                Color[] dataB = new Color[widthB * heightB];
                other.getPic().GetData(dataB);
                
                // Calculate a matrix which transforms from A's local space into
                // world space and then into B's local space
                Matrix transformAToB = transformA * Matrix.Invert(transformB);

                // When a point moves in A's local space, it moves in B's local space with a
                // fixed direction and distance proportional to the movement in A.
                // This algorithm steps through A one pixel at a time along A's X and Y axes
                // Calculate the analogous steps in B:
                Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
                Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

                // Calculate the top left corner of A in B's local space
                // This variable will be reused to keep track of the start of each row
                Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

                // For each row of pixels in A
                for (int yA = 0; yA < heightA; yA++)
                {
                    // Start at the beginning of the row
                    Vector2 posInB = yPosInB;

                    // For each pixel in this row
                    for (int xA = 0; xA < widthA; xA++)
                    {
                        // Round to the nearest pixel
                        int xB = (int)Math.Round(posInB.X);
                        int yB = (int)Math.Round(posInB.Y);

                        // If the pixel lies within the bounds of B
                        if (0 <= xB && xB < widthB &&
                            0 <= yB && yB < heightB)
                        {
                            // Get the colors of the overlapping pixels
                            Color colorA = dataA[xA + yA * widthA];
                            Color colorB = dataB[xB + yB * widthB];

                            // If both pixels are not completely transparent,
                            if (colorA.A != 0 && colorB.A != 0)
                            {
                                // then an intersection has been found
                                return true;
                            }
                        }

                        // Move to the next pixel in the row
                        posInB += stepX;
                    }

                    // Move to the next row
                    yPosInB += stepY;
                }

                // No intersection found
                return false;

            }
            else
                return false;
        }
        public abstract void collidedWith(Collideable other);

        public virtual Rectangle getRectangle()
        {
            return picRect;
        }
        public Texture2D getPic()
        {
            return pic;
        }
        public virtual Matrix getMatrix()
        {
            //return Matrix.CreateScale(scale) *
            return Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                   Matrix.CreateRotationZ(rotation) *
                   Matrix.CreateTranslation(new Vector3(pos, 0.0f));
        }
        public virtual Rectangle getBoundRect()
        {
            Rectangle bound = picRect;
            bound.X -= bound.Width;
            bound.Y -= bound.Height;
            bound.Width = bound.Width * 2;
            bound.Height = bound.Height * 2;
            return bound;
            /*
            Matrix transform = getMatrix();
            Rectangle pict = picRect;
            pict.X -= (int)(pict.Width / 2.0);
            pict.Y -= (int)(pict.Height / 2.0);
            // Get all four corners in local space
	        Vector2 leftTop = new Vector2(pict.Left, pict.Top);
	        Vector2 rightTop = new Vector2(pict.Right, pict.Top);
	        Vector2 leftBottom = new Vector2(pict.Left, pict.Bottom);
	        Vector2 rightBottom = new Vector2(pict.Right, pict.Bottom);

	        // Transform all four corners into work space
	        Vector2.Transform(ref leftTop, ref transform, out leftTop);
	        Vector2.Transform(ref rightTop, ref transform, out rightTop);
	        Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
	        Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

	        // Find the minimum and maximum extents of the rectangle in world space
	        Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
							  Vector2.Min(leftBottom, rightBottom));
	        Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
							  Vector2.Max(leftBottom, rightBottom));

	        // Return as a rectangle
	        return new Rectangle((int)min.X, (int)min.Y,
						 (int)(max.X - min.X), (int)(max.Y - min.Y));
            */
        }
        public virtual void setRotation(float rot)
        {
            rotation = rot;
        }
        public float getRotation()
        {
            return rotation;
        }
        public virtual void setOrigin(Vector2 orig)
        {
            origin = orig;
        }
        public Vector2 getOrigin()
        {
            return origin;
        }


    }
}
