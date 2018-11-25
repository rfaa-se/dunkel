using Dunkel.Game.Components;
using Dunkel.Game.Utilities;
using Microsoft.Xna.Framework;

namespace Dunkel.Game.Components.Attributes
{
    public class BodyComponent : IComponent
    {
        public Flint X { get; set; }
        public Flint Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Rotation { get; set; }
        public Point Center { get => new Point(X + Width / 2, Y + Height / 2); }

        public Flint PrevX { get; set; }
        public Flint PrevY { get; set; }
        public int PrevWidth { get; set; }
        public int PrevHeight { get; set; }
        public int PrevRotation { get; set; }

        public void SetDimension(int width, int height)
        {
            Width = PrevWidth = width;
            Height = PrevHeight = height;
        }

        public void SetPosition(int x, int y)
        {
            X = PrevX = x;
            Y = PrevY = y;
        }

        public void SetRotation(int rotation)
        {
            Rotation = PrevRotation = rotation;
        }

        public bool Intersects(Rectangle box)
        {
            return box.Intersects(new Rectangle(X, Y, Width, Height));
        }

        public void Reset()
        {
            SetDimension(0, 0);
            SetPosition(0, 0);
            SetRotation(0);
        }
    }
}