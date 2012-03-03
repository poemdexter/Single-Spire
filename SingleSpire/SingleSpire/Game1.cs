using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpireVenture.Utilities;
using SpireVenture.Managers;
using SpireVenture.Screen.Screens;

namespace SingleSpire
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ClientOptions.Initialize();

            SetWindowProperties();

            // create and start first screen
            screenManager = new ScreenManager(this, graphics);
            Components.Add(screenManager);
            screenManager.AddScreen(new MainMenuScreen());
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        private void SetWindowProperties()
        {
            graphics.PreferredBackBufferWidth = ClientOptions.ResolutionWidth;
            graphics.PreferredBackBufferHeight = ClientOptions.ResolutionHeight;
            graphics.IsFullScreen = ClientOptions.Fullscreen;
        }
    }
}
