using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpireVenture.Screen.Framework
{
    class MenuEntry
    {
        public bool Active { get; set; }
        public string Text { get; set; }

        public MenuEntry(string text)
        {
            Text = text;
            Active = false;
        }

        public Color getColor()
        {
            return (Active) ? Color.Yellow : Color.White;
        }
    }

    public enum MainMenuEntry
    {
        Start,
        Options,
        Exit
    }

    public enum EndGameEntry
    {
        Yes,
        No
    }

    public enum OptionsEntry
    {
        Resolution,
        Fullscreen,
        Exit
    }

    public enum ProfileEntry
    {
        New,
        Cancel
    }

    public enum MainGameOptionsEntry
    {
        ExitMain,
        ExitDesktop,
        Cancel
    }
}
