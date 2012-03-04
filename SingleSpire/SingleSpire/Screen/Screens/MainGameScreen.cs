using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpireVenture.Screen.Framework;
using SpireVenture.Utilities;

namespace SpireVenture.Screen.Screens
{
    class MainGameScreen : GameScreen
    {
        Texture2D texture;
        int x = 0;

        public MainGameScreen()
        {

        }

        public override void LoadContent()
        {
            texture = ScreenManager.Game.Content.Load<Texture2D>("Screen/ipinput_bg");
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Up))
            {
                x += 1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            //// get our transformation matrix so we can center camera on player
            //ClientGameManager.Instance.CheckForPlayerEntity(ClientGameManager.Instance.Username);
            //Vector2 lerpPos = ClientGameManager.Instance.LerpPlayer(ClientGameManager.Instance.PlayerEntities[ClientGameManager.Instance.Username]);
            //Matrix transformMatrix = Matrix.CreateTranslation(-lerpPos.X + graphics.Viewport.Width / 2,
            //                                                  -lerpPos.Y + graphics.Viewport.Height / 2,
            //                                                  1);

            //// *** transformation spritebatch.  everything drawn here will be in relation to the player as the center of screen
            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix);
            //spriteBatch.End();

            // *** static position spritebatch.  everything drawn here will not move when player moves.  use for UI, HUD, etc.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            spriteBatch.Draw(texture, new Vector2(x, 10), Color.White);

            spriteBatch.End();
        }
    }
}
