using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dunkel.Game.Input
{
    public class InputManager
    {
        private KeyboardState _keyboardStateCurrent;
        private KeyboardState _keyboardStatePrevious;
        private MouseState _mouseStateCurrent;
        private MouseState _mouseStatePrevious;

        public InputManager() 
        {
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState)
        {
            _keyboardStatePrevious = _keyboardStateCurrent;
            _keyboardStateCurrent = keyboardState;
            _mouseStatePrevious = _mouseStateCurrent;
            _mouseStateCurrent = mouseState;
        }

        public Keys[] GetPressedKeys() =>
            _keyboardStateCurrent.GetPressedKeys();

        public Point GetMousePosition() =>
            _mouseStateCurrent.Position;

        public bool IsKeyPressed(Keys key) =>
               _keyboardStatePrevious.IsKeyUp(key)
            && _keyboardStateCurrent.IsKeyDown(key);

        public bool IsKeyDown(Keys key) =>
            _keyboardStateCurrent.IsKeyDown(key);

        public bool IsKeyAnyPressed() =>
            _keyboardStateCurrent.GetPressedKeys().Length > _keyboardStatePrevious.GetPressedKeys().Length;

        public bool IsKeyAnyDown() =>
            _keyboardStateCurrent.GetPressedKeys().Length > 0;

        public bool IsMouseLeftPressed() =>
               _mouseStatePrevious.LeftButton == ButtonState.Pressed
            && _mouseStateCurrent.LeftButton == ButtonState.Released;

        public bool IsMouseLeftDown() =>
            _mouseStateCurrent.LeftButton == ButtonState.Pressed;

        public bool IsMouseRightPressed() =>
               _mouseStatePrevious.RightButton == ButtonState.Pressed
            && _mouseStateCurrent.RightButton == ButtonState.Released;

        public bool IsMouseRightDown() =>
            _mouseStateCurrent.RightButton == ButtonState.Pressed;

        public bool IsMouseAnyPressed() =>
            IsMouseRightPressed() || IsMouseLeftPressed();

        public bool IsMouseAnyDown() =>
            IsMouseRightDown() || IsMouseLeftDown();

        public bool HasMouseMoved() =>
            _mouseStatePrevious.Position != _mouseStateCurrent.Position;
    }
}