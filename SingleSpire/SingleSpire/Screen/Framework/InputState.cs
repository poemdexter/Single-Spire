using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpireVenture.Screen.Framework
{
    public class InputState
    {
        public KeyboardState CurrentKeyboardState;
        public KeyboardState LastKeyboardState;
        private int KeyboardElapsedTime;

        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();
            LastKeyboardState = new KeyboardState();
            KeyboardElapsedTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            KeyboardElapsedTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public bool IsNewKeyPress(Keys key)
        {
            if (KeyboardElapsedTime <= 0)
            {
                if (CurrentKeyboardState.IsKeyDown(key))
                {
                    KeyboardElapsedTime = 200;
                    return true;
                }
            }
            return false;
        }
    }
}
