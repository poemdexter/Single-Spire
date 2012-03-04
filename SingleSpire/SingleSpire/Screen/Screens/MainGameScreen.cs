using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpireVenture.Screen.Framework;
using SpireVenture.Utilities;
using EntityFramework;
using EntityFramework.Components;
using SingleSpire.EntityFramework.Args;
using SingleSpire.EntityFramework.Actions;

namespace SpireVenture.Screen.Screens
{
    class MainGameScreen : GameScreen
    {
        private GraphicsDevice Graphics;
        private SpriteBatch Batch;
        private SpriteFont Font;

        Texture2D texture;
        Entity player;

        public MainGameScreen()
        {
            player = new Entity();
            player.AddComponent(new Movement(20,0,new Vector2(10,10)));
            player.AddAction(new Move());
        }

        public override void LoadContent()
        {
            texture = ScreenManager.Game.Content.Load<Texture2D>("Entities/Characters/char_bandit");
            Graphics = ScreenManager.GraphicsDevice;
            Batch = ScreenManager.SpriteBatch;
            Font = ScreenManager.Font;
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsNewKeyPress(Keys.Up))
            {
                player.DoAction("Move", new MoveArgs(MoveArgs.Direction.Up));
            }
            else if (input.IsNewKeyPress(Keys.Down))
            {
                player.DoAction("Move", new MoveArgs(MoveArgs.Direction.Down));
            }
            else if (input.IsNewKeyPress(Keys.Left))
            {
                player.DoAction("Move", new MoveArgs(MoveArgs.Direction.Left));
            }
            else if (input.IsNewKeyPress(Keys.Right))
            {
                player.DoAction("Move", new MoveArgs(MoveArgs.Direction.Right));
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            Movement movement = player.GetComponent("Movement") as Movement;
            Vector2 PlayerLerpposition = movement.GetLerpedPlayerPosition();

            // *** transformation spritebatch.  everything drawn here will be in relation to the player as the center of screen
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, GetPlayerTranslation(PlayerLerpposition));

            Batch.End();

            // *** static position spritebatch.  everything drawn here will not move when player moves.  use for UI, HUD, etc.
            Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);

            Batch.Draw(texture, PlayerLerpposition, texture.Bounds, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0);

            Batch.End();
        }

        private Matrix GetPlayerTranslation(Vector2 playerPosition)
        {
            return Matrix.CreateTranslation(-playerPosition.X + Graphics.Viewport.Width / 2,
                                            -playerPosition.Y + Graphics.Viewport.Height / 2,
                                            1);
        }
    }
}
