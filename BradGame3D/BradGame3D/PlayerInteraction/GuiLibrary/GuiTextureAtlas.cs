using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BradGame3D.PlayerInteraction.GuiLibrary
{
    public struct TextureInfo
    {
        public string name;
        public int x;
        public int y;
        public int width;
        public int height;
    }
    public class GuiTextureAtlas
    {
        Gui gui;
        public IDictionary<string, TextureInfo> textures;
        public GuiTextureAtlas(Gui g)
        {
            gui = g;
            textures = new Dictionary<string, TextureInfo>();
        }
        public void loadAtlas(string a)
        {
            XDocument doc = XDocument.Load(a);
            
            foreach (XElement e in doc.Descendants("SubTexture"))
            {
                TextureInfo tex;
                tex.name = (string) e.Attribute("name");
                tex.x = (int) e.Attribute("x");
                tex.y = (int)e.Attribute("y");
                tex.width = (int)e.Attribute("width");
                tex.height = (int)e.Attribute("height");
                textures.Add(tex.name, tex);
            }
        }
    }
}
