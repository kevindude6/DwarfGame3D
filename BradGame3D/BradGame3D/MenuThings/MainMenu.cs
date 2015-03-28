using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace BradGame3D.MenuThings
{
    class MainMenu : MenuScreen
    {
        public MainMenu(Game1 tg) : base(tg)
        {

        }
        public override void init()
        {
            Action a = delegate() { g.Exit(); };
            Button b = new Button(this, null, g.Window.ClientBounds.Width / 2 - 200, g.Window.ClientBounds.Height / 2 + 150, 400, 100, g.Content.Load<Texture2D>("grass"), a);
            Action c = delegate() { g.startGame(); };
            Button d = new Button(this, null, g.Window.ClientBounds.Width / 2 - 200, g.Window.ClientBounds.Height / 2 + 0, 400, 100, g.Content.Load<Texture2D>("grass"), c);
            b.text = "Exit Game";
            d.text = "Start Game";
            components.Add(b);
            components.Add(d);

            Button terrainEnter = new Button(this, null, g.Window.ClientBounds.Width / 2 - 250, g.Window.ClientBounds.Height / 2 + 300, 500, 100, g.Content.Load<Texture2D>("grass"), (Action)delegate() { g.enterTerrainMenu(); });
            terrainEnter.text = "Enter Terrain Menu";
            components.Add(terrainEnter);
        }
    }
}
