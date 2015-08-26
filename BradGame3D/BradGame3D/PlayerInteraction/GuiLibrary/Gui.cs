using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using System.Collections;

namespace BradGame3D.PlayerInteraction.GuiLibrary
{
    public class Gui
    {
        GameScreen gameScreen;
        public Texture2D guiTexture;
        public List<Component> components;
        public GuiTextureAtlas texAtlas;
        public Gui(GameScreen g)
        {
            gameScreen = g;
            
            texAtlas = new GuiTextureAtlas(this);
            loadGuiTex();
            components = new List<Component>();
            
            Component test = new Component(this,"MainBar.png");
            ComponentBounds b = new ComponentBounds();
            b.xAsPercent = 0;
            b.yAsPercent = 0.9f;
            b.heightAsPercent = 0.10f;
            b.widthAsPercent = 1f;
            test.setBounds(b);
            components.Add(test);
        }
        public bool checkClick(int x, int y)
        {
            foreach (Component c in components)
            {
                if (c.wasClicked(x, y)) return true;
            }
            return false;
        }
        public void loadGuiTex()
        {
            guiTexture = gameScreen.game.Content.Load<Texture2D>("GuiElements/sprites");

            texAtlas.loadAtlas("Content/GuiElements/spritesXML.xml");

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
