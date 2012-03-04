using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SpireVenture.Screen.Framework;

namespace SpireVenture.Screen.Screens
{
    class PopUpWarningScreen : GameScreen
    {
        private GameScreen ParentScreen;
        private Texture2D BackgroundTexture;
        private string Message = "";

        public PopUpWarningScreen(GameScreen parentScreen, string message)
        {
            ParentScreen = parentScreen;
            Message = message;
        }

        public override void LoadContent()
        {
            BackgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("Screen/ipinput_bg");
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Enter) || input.IsNewKeyPress(Keys.Escape) || input.IsNewKeyPress(Keys.Space))
            {
                ScreenManager.RemoveScreen(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            // draw background
            float bgscale = 4f;
            int midscreen = graphics.Viewport.Height / 2;

            spriteBatch.Draw(BackgroundTexture, new Vector2(graphics.Viewport.Width / 2, midscreen), BackgroundTexture.Bounds, Color.White, 0f, new Vector2(BackgroundTexture.Width / 2, BackgroundTexture.Height / 2), bgscale, SpriteEffects.None, 0f);

            // check message too long, need to split it in half.
            if (font.MeasureString(Message).X > 200)
            {
                string[] words = Message.Split(' ');
                int half = (int)Math.Floor((double)words.Length / 2);
                string firsthalf = StringSplitter(words, 0, half - 1);
                string secondhalf = StringSplitter(words, half, words.Length - half - 1);
                spriteBatch.DrawString(font, firsthalf, new Vector2(graphics.Viewport.Width / 2, midscreen - 10), Color.White, 0, font.MeasureString(firsthalf) / 2, 2f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, secondhalf, new Vector2(graphics.Viewport.Width / 2, midscreen + 10), Color.White, 0, font.MeasureString(secondhalf) / 2, 2f, SpriteEffects.None, 0);
            }
            else 
            {
                spriteBatch.DrawString(font, Message, new Vector2(graphics.Viewport.Width / 2, midscreen + 10), Color.White, 0, font.MeasureString(Message) / 2, 2f, SpriteEffects.None, 0);
            }
            
            spriteBatch.End();
        }

        private string StringSplitter(string[] source, int startPos, int length)
        {
            StringBuilder builder = new StringBuilder();
            for (int x = 0; x <= length; x++)
            {
                builder.Append(source[startPos + x]).Append(' ');
            }
            return builder.ToString();
        }
    }
}
