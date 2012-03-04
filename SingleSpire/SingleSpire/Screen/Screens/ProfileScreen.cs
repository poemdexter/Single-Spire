using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using SpireVenture.Screen.Framework;
using SpireVenture.Utilities;

namespace SpireVenture.Screen.Screens
{
    class ProfileScreen : GameScreen
    {
        private GameScreen ParentScreen;
        List<MenuEntry> MenuEntries = new List<MenuEntry>();
        List<MenuEntry> ProfileEntries = new List<MenuEntry>();
        int SelectedMenuEntry = -1;
        int SelectedProfileEntry = -1;
        private const string TitleText = "Profiles";
        bool OnProfiles = false, OnMenu = false;
        private bool NewProfileActive = false;
        private StringBuilder KeyboardInput;
        private KeyboardStringBuilder ProfileStringBuilder;
        private string[] ProfileFiles;

        public ProfileScreen(GameScreen parentScreen)
        {
            KeyboardInput = new StringBuilder();
            ProfileStringBuilder = new KeyboardStringBuilder();

            ParentScreen = parentScreen;
            ParentScreen.CurrentScreenState = ScreenState.Hidden;

            ProfileFiles = FileGrabber.findLocalProfiles();
            CreateProfilesMenuEntries(ProfileFiles);
            MenuEntries.Add(new MenuEntry("New"));
            MenuEntries.Add(new MenuEntry("Cancel"));

            if (ProfileEntries.Count > 0)
            {
                OnProfiles = true;
                SelectedProfileEntry = 0;
                ProfileEntries[0].Active = true;
            }
            else
            {
                OnMenu = true;
                SelectedMenuEntry = 0;
                MenuEntries[0].Active = true;
            }
        }

        public override void LoadContent() { }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Up))
            {
                if (OnProfiles)
                {
                    SelectedProfileEntry--;

                    if (SelectedProfileEntry < 0)
                    {
                        SelectedProfileEntry = -1;
                        SelectedMenuEntry = MenuEntries.Count - 1;
                        OnProfiles = false;
                        OnMenu = true;
                    }
                }
                else if (OnMenu)
                {
                    SelectedMenuEntry--;

                    if (SelectedMenuEntry < 0)
                    {
                        if (ProfileEntries.Count > 0)
                        {
                            SelectedMenuEntry = -1;
                            SelectedProfileEntry = ProfileEntries.Count - 1;
                            OnProfiles = true;
                            OnMenu = false;
                        }
                        else
                        {
                            SelectedMenuEntry = MenuEntries.Count - 1;
                        }
                    }
                }
            }
            if (input.IsNewKeyPress(Keys.Down))
            {
                if (OnProfiles)
                {
                    SelectedProfileEntry++;

                    if (SelectedProfileEntry >= ProfileEntries.Count)
                    {
                        SelectedProfileEntry = -1;
                        SelectedMenuEntry = 0;
                        OnProfiles = false;
                        OnMenu = true;
                    }
                }
                else if (OnMenu)
                {
                    SelectedMenuEntry++;

                    if (SelectedMenuEntry >= MenuEntries.Count)
                    {
                        if (ProfileEntries.Count > 0)
                        {
                            SelectedMenuEntry = -1;
                            SelectedProfileEntry = 0;
                            OnProfiles = true;
                            OnMenu = false;
                        }
                        else
                        {
                            SelectedMenuEntry = 0;
                        }
                    }
                }
            }
            if (input.IsNewKeyPress(Keys.Enter))
            {
                if (OnMenu)
                {
                    switch (SelectedMenuEntry)
                    {
                        case (int)ProfileEntry.New:
                            if (NewProfileActive)
                            {
                                // done typing (save it)
                                string newProfileName = KeyboardInput.ToString();
                                if (ProfileFiles != null && ProfileFiles.Length > 0)
                                {
                                    if (DoesProfileExist(newProfileName))
                                    {
                                       ScreenManager.AddScreen(new PopUpWarningScreen(this, "Profile with this name already exists."));
                                    }
                                    else
                                    {
                                        CreateNewProfile(newProfileName);
                                    }
                                }
                                else
                                {
                                    CreateNewProfile(newProfileName);
                                }
                                KeyboardInput.Clear();
                                NewProfileActive = false;
                            }
                            else if (!NewProfileActive)
                            {
                                // start taking keystrokes
                                NewProfileActive = true;
                            }
                            break;
                        case (int)ProfileEntry.Cancel:
                            ScreenManager.RemoveScreen(this);
                            ParentScreen.CurrentScreenState = ScreenState.Active;
                            break;
                    }
                }
                else if (OnProfiles)
                {
                    ScreenManager.AddScreen(new MainGameScreen());
                    ScreenManager.RemoveScreen(this);
                }
            }
            if (input.IsNewKeyPress(Keys.Escape))
            {
                if (NewProfileActive)
                {
                    KeyboardInput.Clear();
                    NewProfileActive = false;
                }
                ScreenManager.RemoveScreen(this);
                ParentScreen.CurrentScreenState = ScreenState.Active;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntries[i].Active = (i == SelectedMenuEntry) ? true : false;
            }
            if (ProfileEntries.Count > 0)
            {
                for (int j = 0; j < ProfileEntries.Count; j++)
                {
                    ProfileEntries[j].Active = (j == SelectedProfileEntry) ? true : false;
                }
            }

            if (NewProfileActive)
            {
                ProfileStringBuilder.Process(Keyboard.GetState(), gameTime, KeyboardInput);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            // draw background
            float scale = 4f;

            // draw title
            spriteBatch.DrawString(font, TitleText, new Vector2(graphics.Viewport.Width / 2, 40), Color.White, 0, font.MeasureString(TitleText) / 2, scale, SpriteEffects.None, 0);

            // draw profiles
            if (ProfileEntries.Count > 0)
            {
                int x1 = 0;
                foreach (MenuEntry entry in ProfileEntries)
                {
                    spriteBatch.DrawString(font, entry.Text, new Vector2(50, 150 + x1), entry.getColor(), 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                    x1 += 30;
                }
            }

            // draw options
            int x = 0;
            foreach (MenuEntry entry in MenuEntries)
            {
                spriteBatch.DrawString(font, entry.Text, new Vector2(50, graphics.Viewport.Height - 20 - (30 * MenuEntries.Count) + x), entry.getColor(), 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
                x += 30;
            }

            if (NewProfileActive)
            {
                String newtxt = KeyboardInput.ToString() + "_";
                spriteBatch.DrawString(font, newtxt, new Vector2(150, graphics.Viewport.Height - 20 - (30 * (MenuEntries.Count + (int)ProfileEntry.New))), Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            }
            spriteBatch.End();
        }

        private void CreateNewProfile(string newProfileName)
        {
            ProfileFiles = new string[] { newProfileName };
            FileGrabber.createNewProfile(newProfileName);
            ProfileEntries.Add(new MenuEntry(newProfileName));
        }

        private bool DoesProfileExist(string profileName)
        {
            if (ProfileFiles.Length > 0)
            {
                foreach (string filename in ProfileFiles)
                {
                    if (profileName.Equals(Path.GetFileNameWithoutExtension(filename)))
                        return true;
                }
            }
            return false;
        }

        private void CreateProfilesMenuEntries(string[] profileFiles)
        {
            if (profileFiles.Length > 0)
            {
                foreach (string filename in profileFiles)
                {
                    ProfileEntries.Add(new MenuEntry(Path.GetFileNameWithoutExtension(filename)));
                }
            }
        }
    }
}
