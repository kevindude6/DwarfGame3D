using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace BradGame3D.PlayerInteraction.GuiLibrary
{
    public class Gui
    {
        GameScreen gameScreen;
        public Texture2D guiTexture;
        public Gui(GameScreen g)
        {
            gameScreen = g;
        }
        public void loadGuiTex()
        {
            guiTexture = gameScreen.game.Content.Load<Texture2D>("guiTexture");
        }
        public int getScreenWidth()
        {
            return gameScreen.game.Window.ClientBounds.Width;
        }
        public int getScreenHeight()
        {
            return gameScreen.game.Window.ClientBounds.Height;
        }
    }
}
