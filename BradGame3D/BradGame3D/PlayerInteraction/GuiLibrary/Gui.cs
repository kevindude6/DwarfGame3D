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
        public MenuBar mainMenuBar;
        public MenuBar buildMenuBar;
        
        public Gui(GameScreen g)
        {
            gameScreen = g;
            
            texAtlas = new GuiTextureAtlas(this);
            loadGuiTex();
            components = new List<Component>();

            MenuBar mainMenuBar = new MenuBar(this, null);
            components.Add(mainMenuBar);
            

            MenuBar buildMenuBar = new MenuBar(this, null);
            components.Add(buildMenuBar);

            mainMenuBar.addButton("DigIcon", "DigIcon.png",delegate() {gameScreen.game.Exit();});
            mainMenuBar.addButton("BuildIcon", "BuildIcon.png", delegate() { buildMenuBar.visible = true; mainMenuBar.visible = false; gameScreen.mouseControl.mode = MouseController.MODE.BUILD;});

            buildMenuBar.visible = false;
            buildMenuBar.addButton("DirtButton", "DirtIcon.png", delegate() { gameScreen.mouseControl.currentBuild = 3; });
            buildMenuBar.addButton("StoneButton", "StoneIcon.png", delegate() {gameScreen.mouseControl.currentBuild = 2;});
            buildMenuBar.addButton("CobbleButton", "StoneIcon.png", delegate() { gameScreen.mouseControl.currentBuild = 4; });
            buildMenuBar.addButton("PlankButton", "StoneIcon.png", delegate() { gameScreen.mouseControl.currentBuild = 7; });
            buildMenuBar.addButton("SandstoneButton", "StoneIcon.png", delegate() { gameScreen.mouseControl.currentBuild = 9; });
           
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
