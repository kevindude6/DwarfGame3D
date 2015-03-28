using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BradGame3D.MenuThings
{
    class Button : MenuComponent
    {
        public Texture2D tex;
        public string text = "Hello World!";
        public SpriteFont font;
        Action clickFunc;
        public Button(MenuScreen m, MenuComponent p, int tx, int ty, int tw, int th, Texture2D ttex, Action tClickFunc) : base(m,p,tx,ty,tw,th)
        {
            tex = ttex;
            font = m.g.Content.Load<SpriteFont>("sego");
            clickFunc = tClickFunc;
        }
        public override void doClick()
        {
            clickFunc();
        }
        public override void draw(SpriteBatch b)
        {
            b.Draw(tex, bounds , color);
            Vector2 stringOrigin = new Vector2(font.MeasureString(text).X/2, font.MeasureString(text).Y/2);
            Vector2 stringPos = new Vector2(bounds.Center.X, bounds.Center.Y);
            
            //b.DrawString(font, text, stringPos, color);
            b.DrawString(font, text, stringPos, color, 0f, stringOrigin, scale, SpriteEffects.None, 0);
        }
        public override void doUpdate()
        {
            if (isMouseOver)
            {
                scale = 1.2f;
                updateBounds();
            }
            else
            {
                scale = 1;
                updateBounds();
            }
        }

        

    }
}
