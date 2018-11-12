using Dunkel.Game.Components;
using Dunkel.Game.Utilities;

namespace Dunkel.Game.Components.Attributes
{
    public class BodyComponent : IComponent
    {
        public flint X { get; set; }
        public flint Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Rotation { get; set; }

        public flint PrevX { get; set; }
        public flint PrevY { get; set; }
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

        public void Reset()
        {
            SetDimension(0, 0);
            SetPosition(0, 0);
            SetRotation(0);
        }
    }
}