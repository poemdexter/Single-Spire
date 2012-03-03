using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SpireVenture.Screen.Framework;
using SpireVenture.Utilities;

namespace SpireVenture.Screens.Screens
{
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    class OptionsScreen : GameScreen
    {
        private GameScreen ParentScreen;
        private const string TitleText = "Options";

        private List<MenuEntry> MenuEntries = new List<MenuEntry>();
        private List<MenuEntry> ResolutionEntries = new List<MenuEntry>();
        private List<Resolution> Resolutions = new List<Resolution>();
        private int SelectedEntry = 0;
        private int ResolutionSelectedEntry = 0;

        private bool ResolutionActive = false;
        private bool ResolutionChanged = false;
        private bool FullscreenChanged = false;

        public OptionsScreen(GameScreen parentScreen)
        {
            ParentScreen = parentScreen;
            ParentScreen.CurrentScreenState = ScreenState.Hidden;

            MenuEntries.Add(new MenuEntry("Resolution"));
            MenuEntries.Add(new MenuEntry("Fullscreen?"));
            MenuEntries.Add(new MenuEntry("Back"));
            MenuEntries[0].Active = true;

            var displays = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Select(x => new { x.Width, x.Height }).Distinct();
            foreach (var display in displays)
            {
                ResolutionEntries.Add(new MenuEntry(display.Width + " x " + display.Height));
                Resolutions.Add(new Resolution(display.Width, display.Height));
            }
            ResolutionEntries[0].Active = true;
        }

        public override void LoadContent() { }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Up))
            {
                if (!ResolutionActive)
                {
                    SelectedEntry--;

                    if (SelectedEntry < 0)
                        SelectedEntry = MenuEntries.Count - 1;
                }
                else if (ResolutionActive)
                {
                    ResolutionSelectedEntry--;

                    if (ResolutionSelectedEntry < 0)
                        ResolutionSelectedEntry = ResolutionEntries.Count - 1;
                }
            }
            if (input.IsNewKeyPress(Keys.Down))
            {
                if (!ResolutionActive)
                {
                    SelectedEntry++;

                    if (SelectedEntry >= MenuEntries.Count)
                        SelectedEntry = 0;
                }
                else if (ResolutionActive)
                {
                    ResolutionSelectedEntry++;

                    if (ResolutionSelectedEntry >= ResolutionEntries.Count)
                        ResolutionSelectedEntry = 0;
                }
            }
            if (input.IsNewKeyPress(Keys.Enter))
            {
                switch (SelectedEntry)
                {
                    case (int)OptionsEntry.Resolution:
                        if (ResolutionActive)
                        {
                            Resolution res = Resolutions[ResolutionSelectedEntry];
                            this.ScreenManager.Graphics.PreferredBackBufferWidth = res.Width;
                            this.ScreenManager.Graphics.PreferredBackBufferHeight = res.Height;
                            ClientOptions.SetResolution(res.Height, res.Width);

                            ResolutionChanged = true;
                            ResolutionActive = false;
                        }
                        else if (!ResolutionActive)
                        {
                            ResolutionActive = true;
                        }
                        break;
                    case (int)OptionsEntry.Fullscreen:
                        if (ClientOptions.Fullscreen)
                        {
                            this.ScreenManager.Graphics.ToggleFullScreen();
                            ClientOptions.SetFullscreen(false);
                        }
                        else 
                        {
                            this.ScreenManager.Graphics.ToggleFullScreen();
                            ClientOptions.SetFullscreen(true);
                        }
                        FullscreenChanged = true;
                        break;
                    case (int)OptionsEntry.Exit:
                        ClientOptions.Save(); // save client options
                        ScreenManager.RemoveScreen(this);
                        ParentScreen.CurrentScreenState = ScreenState.Active;
                        break;
                }
            }
            if (input.IsNewKeyPress(Keys.Escape))
            {
                if (ResolutionActive)
                {
                    ResolutionActive = false;
                }
                else
                {
                    ClientOptions.Save();
                    ScreenManager.RemoveScreen(this);
                    ParentScreen.CurrentScreenState = ScreenState.Active;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntries[i].Active = (i == SelectedEntry) ? true : false;
            }

            if (ResolutionActive)
            {
                for (int i = 0; i < ResolutionEntries.Count; i++)
                {
                    ResolutionEntries[i].Active = (i == ResolutionSelectedEntry) ? true : false;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (ResolutionChanged)
            {
                this.ScreenManager.Graphics.ApplyChanges();
                ResolutionChanged = false;
            }

            if (FullscreenChanged)
            {
                this.ScreenManager.Graphics.ApplyChanges();
                FullscreenChanged = false;
            }

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            // draw background
            float Scale = 4f;

            // draw title
            spriteBatch.DrawString(font, TitleText, new Vector2(graphics.Viewport.Width / 2, 40), Color.White, 0, font.MeasureString(TitleText) / 2, Scale, SpriteEffects.None, 0);

            // draw options
            int x = 0;
            foreach (MenuEntry entry in MenuEntries)
            {
                spriteBatch.DrawString(font, entry.Text, new Vector2(50, 140 + x), entry.getColor(), 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                x += 30;
            }

            if (ResolutionActive)
            {
                int x1 = 0;
                foreach (MenuEntry entry in ResolutionEntries)
                {
                    spriteBatch.DrawString(font, entry.Text, new Vector2(450, 140 + x1), entry.getColor(), 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                    x1 += 15;
                }
            }

            spriteBatch.End();
        }
    }
}
