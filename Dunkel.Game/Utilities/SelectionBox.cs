using System;
using Dunkel.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dunkel.Game.Utilities
{
    public class SelectionBox
    {
        public Rectangle Box => _box;
        public bool HasSelected { get; private set; }
        public bool IsSelecting { get; private set; }

        private Point _startPoint;
        private Rectangle _box;

        public void Input(InputManager im)
        {
            HasSelected = false;

            if (im.IsMouseLeftDown())
            {
                var mouse = im.GetMousePosition();

                if (!IsSelecting)
                {
                    IsSelecting = true;
                    
                    _startPoint.X = mouse.X;
                    _startPoint.Y = mouse.Y;

                    _box.X = _startPoint.X;
                    _box.Y = _startPoint.Y;
                    _box.Width = 0;
                    _box.Height = 0;
                }
                else
                {
                    _box.X = Math.Min(mouse.X, _startPoint.X);
                    _box.Y = Math.Min(mouse.Y, _startPoint.Y);
                    _box.Width = Math.Abs(mouse.X - _startPoint.X);
                    _box.Height = Math.Abs(mouse.Y - _startPoint.Y);
                }
            }
            else if (im.IsMouseLeftPressed())
            {
                IsSelecting = false;
                HasSelected = true;

                var mouse = im.GetMousePosition();

                _box.X = Math.Min(mouse.X, _startPoint.X);
                _box.Y = Math.Min(mouse.Y, _startPoint.Y);
                _box.Width = Math.Abs(mouse.X - _startPoint.X);
                _box.Height = Math.Abs(mouse.Y - _startPoint.Y);
            }
        }

        public void Draw(SpriteBatch sb, float delta)
        {
            if (!IsSelecting) { return; }

            sb.DrawRectangle(
                x: _box.X,
                y: _box.Y,
                width: _box.Width,
                height: _box.Height,
                color: Color.White
            );
            
            sb.DrawFilledRectangle(
                x: _box.X,
                y: _box.Y,
                width: _box.Width,
                height: _box.Height,
                color: Color.White * 0.1f
            );
        }
    }
}