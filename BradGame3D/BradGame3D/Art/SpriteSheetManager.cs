using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace BradGame3D.Art
{
    public class SpriteSheetManager
    {
        public Dictionary<string, SpriteSheetEnhanced> dict = new Dictionary<string, SpriteSheetEnhanced>();
        //List<SpriteSheet> sheets;
        public GameScreen gameScreen;
        public SpriteSheetManager(GameScreen g)
        {
            gameScreen = g;

           // System.Type t = System.Type.GetType("BradGame3D.Art.SpriteSheet");
           // xserial = new XmlSerializer(t);
            //loadSheet("Sprite_creature_squirrel");
        }
        public void loadSheet(String fileName, World2 w)
        {
            SpriteSheet s = gameScreen.game.Content.Load<SpriteSheet>("SpriteData/" + fileName + "DATA");
            s.setImage(gameScreen.game.Content.Load<Texture2D>("SpriteData/" + fileName));
            SpriteSheetEnhanced sheet = new SpriteSheetEnhanced(w, gameScreen, s);
            dict.Add(sheet.s.name, sheet);
        }
    }
}
