using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BradGame3D.MenuThings
{
    class MenuScreen
    {
        public Game1 g;
        protected List<MenuComponent> components = new List<MenuComponent>();
        protected MouseState oldMouseState = Mouse.GetState();
        public MenuScreen(Game1 tg)
        {
            g = tg;
           
            init();
        }
        public virtual void init()
        { }
        public void checkMouse()
        {
            MouseState n = Mouse.GetState();
            if (n.X != oldMouseState.X || n.Y != oldMouseState.Y)
            {
                foreach (MenuComponent i in components)
                {
                    if (i.isInBounds(n.X, n.Y))
                        i.isMouseOver = true;
                    else
                        i.isMouseOver = false;
                }
            }
            if (n.LeftButton.Equals(ButtonState.Pressed) && oldMouseState.LeftButton.Equals(ButtonState.Released))
            {
                foreach (MenuComponent i in components)
                {
                    if (i.isInBounds(n.X, n.Y))
                        i.doClick();
                }
            }
            oldMouseState = n;
        }
        public void updateMenu(GameTime gt)
        {
            checkMouse();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {

            }

            foreach (MenuComponent i in components)
            {
                i.doUpdate();
            }
        }
        public virtual void drawMenu(GameTime gt)
        {
            g.graphics.GraphicsDevice.Clear(Color.SlateGray);
            g.spriteBatch.Begin();
            foreach (MenuComponent i in components)
            {
                i.draw(g.spriteBatch);
            }
            g.spriteBatch.End();
        }

    }
}
