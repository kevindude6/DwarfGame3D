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
        protected Gui gui;
        protected List<Component> children;
        public Rectangle drawRect;
        public Rectangle textRect;
        public ComponentBounds bounds;
        public string texName;
        public string name;
        public bool AbsolutePositioning = false;
        public bool visible = true;
        public Component(Gui g,string img) : this(g, null, img)
        {

        }
        public Component(Gui g, Component PARENT, string img)
        {
            gui = g;
            parent = PARENT;
            texName = img;
            bounds.textureX = gui.texAtlas.textures[texName].x;
            bounds.textureY = gui.texAtlas.textures[texName].y;
            bounds.textureWidth = gui.texAtlas.textures[texName].width;
            bounds.textureHeight = gui.texAtlas.textures[texName].height;

            textRect = new Rectangle((int)bounds.textureX, (int)bounds.textureY, (int)bounds.textureWidth, (int)bounds.textureHeight);

            children = new List<Component>();
           
        }
        public void setBounds(float x, float y, float w, float h)
        {
            bounds.xAsPercent = x;
            bounds.yAsPercent = y;
            bounds.widthAsPercent = w;
            bounds.heightAsPercent = h;


            

            if (AbsolutePositioning || parent == null)
            {
                drawRect = new Rectangle((int)(bounds.xAsPercent * gui.getScreenWidth()), (int)(bounds.yAsPercent * gui.getScreenHeight()),
                (int)(bounds.widthAsPercent * gui.getScreenWidth()), (int)(bounds.heightAsPercent * gui.getScreenHeight()));
            }
            else
            {
                drawRect = new Rectangle((int)(bounds.xAsPercent * parent.drawRect.Width) + parent.drawRect.X, 
                    (int)(bounds.yAsPercent * parent.drawRect.Height)+ parent.drawRect.Y,
                    (int)(bounds.widthAsPercent * parent.drawRect.Width), 
                    (int)(bounds.heightAsPercent * parent.drawRect.Height));
            }
        }
        public void setBounds(ComponentBounds b)
        {
            setBounds(b.xAsPercent, b.yAsPercent, b.widthAsPercent, b.heightAsPercent);
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
            if (visible)
            {
                drawSelf(spriteBatch);
                drawChildren(spriteBatch);
                foreach (Component c in children)
                {
                    c.drawComponent(spriteBatch);
                }
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
        public virtual void doClick()
        {

        }
        public bool wasClicked(int x, int y)
        {
            if (visible)
            {
                foreach (Component c in children)
                {
                    if (c.wasClicked(x, y)) return true;
                }
                if (drawRect.Contains(x, y))
                {
                    Console.WriteLine("Component: " + texName + " was clicked!");
                    doClick();
                    return true;
                }
                else
                    return false;
            }
            return false;

        }
    }
}
