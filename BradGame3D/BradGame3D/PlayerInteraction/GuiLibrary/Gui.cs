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
        public List<Component> components;
        public Gui(GameScreen g)
        {
            gameScreen = g;
            components = new List<Component>();

            Component test = new Component(this);
            ComponentBounds b;
            b.textureWidth = 1920;
            b.textureHeight = 216;
            b.textureX = 0;
            b.textureY = 0;

            b.heightAsPercent = 0.135f;
            b.widthAsPercent = 1f;
            b.xAsPercent = 0f;
            b.yAsPercent = 0.9f;
            test.setBounds(b);
            components.Add(test);
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
        public void drawGui(SpriteBatch b)
        {
            foreach (Component c in components)
            {
                c.drawComponent(b);
            }
        }
    }
}
