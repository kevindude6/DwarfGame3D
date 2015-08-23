using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace BradGame3D.PlayerInteraction.GuiLibrary
{
    public struct ComponentBounds
    {
        public float xAsPercent;
        public float yAsPercent;
        public float widthAsPercent;
        public float heightAsPercent;
        public float textureX;
        public float textureY;
        public float textureWidth;
        public float textureHeight;
    }
    public class Component
    {
        private Component parent = null;
        private Gui gui;
        private List<Component> children;
        public Rectangle drawRect;
        public Rectangle textRect;
        public ComponentBounds bounds;
        public Component(Gui g) : this(g, null)
        {

        }
        public Component(Gui g, Component PARENT)
        {
            gui = g;
            parent = PARENT;
            children = new List<Component>();
           
        }
        public void setBounds(ComponentBounds b)
        {
            bounds.xAsPercent = b.xAsPercent;
            bounds.yAsPercent = b.yAsPercent;
            bounds.widthAsPercent = b.widthAsPercent;
            bounds.heightAsPercent = b.heightAsPercent;

            bounds.textureX = b.textureX;
            bounds.textureY = b.textureY;
            bounds.textureWidth = b.textureWidth;
            bounds.textureHeight = b.textureHeight;


            textRect = new Rectangle((int)bounds.textureX, (int)bounds.textureY, (int)bounds.textureWidth, (int)bounds.textureHeight);
            drawRect = new Rectangle((int)(bounds.xAsPercent * gui.getScreenWidth()), (int)(bounds.yAsPercent * gui.getScreenHeight()),
                (int)(bounds.widthAsPercent * gui.getScreenWidth()), (int)(bounds.heightAsPercent * gui.getScreenHeight()));
        }
        public bool addComponent(Component t)
        {
            if (t != null)
            {
                children.Add(t);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool removeComponent(Component t)
        {
            if (t != null)
            {
                return children.Remove(t);
            }
            else
            {
                return false;
            }
        }
        public void drawComponent(SpriteBatch spriteBatch)
        {
            drawSelf(spriteBatch);
            drawChildren(spriteBatch);
            foreach(Component c in children)
            {
                c.drawComponent(spriteBatch);
            }
        }
        public virtual void drawSelf(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gui.guiTexture, drawRect, textRect, Color.White);      
        }
        public void drawChildren(SpriteBatch spriteBatch)
        {
            foreach (Component c in children)
            {
                c.drawComponent(spriteBatch);
            }
        }
    }
}
