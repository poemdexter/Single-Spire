using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpireVenture.Utilities
{
    [Serializable()]
    public class PlayerSave
    {
        public string Username { get; set; }
        public Vector2 Position { get; set; }

        public PlayerSave(string username)
        {
            Username = username;
            //Position = GameConstants.DefaultStartPosition; // default spot if new player
        }
    }
}
