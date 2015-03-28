using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BradGame3D.MenuThings
{
    abstract class MenuComponent
    {
        public int x;
        public int y;
        public int prefWidth;
        public int prefHeight;
        public float scale = 1;
        public bool isMouseOver;
        public Color color = Color.White;
        public Rectangle bounds;
        
        public MenuComponent parent;
        public MenuScreen menuScreen;

        public MenuComponent(MenuScreen m, MenuComponent p, int tx, int ty, int tprefWidth, int tprefHeight)
        {
            parent = p;
            menuScreen = m;
            x = tx;
            y = ty;
            prefHeight = tprefHeight;
            prefWidth = tprefWidth;
            updateBounds();
        }

        public void updateBounds()
        {
            bounds = new Rectangle(getScaledX(), getScaledY(), (int)(prefWidth * scale), (int)(prefHeight * scale));
        }

        public MenuComponent(MenuScreen m, MenuComponent p)
        {
            parent = p;
            menuScreen = m;
        }
        abstract public void doClick();
        abstract public void draw(SpriteBatch b);
        abstract public void doUpdate();
        public int getScaledX() { return (x) - (int)(prefWidth / 2 * scale - prefWidth / 2); }
        public int getScaledY() { return (y) - (int)(prefHeight / 2 * scale - prefHeight / 2); }
        
        public bool isInBounds(int tx, int ty)
        {
            return bounds.Contains(tx, ty);
        }
    }
}
