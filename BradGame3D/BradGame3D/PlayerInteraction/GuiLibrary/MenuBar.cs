using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BradGame3D.PlayerInteraction.GuiLibrary
{
    public class MenuBar : Component
    {
        int buttonCount = 0;
        public enum JUSTIFICATION { LEFT, CENTER, RIGHT };
        public JUSTIFICATION justify = JUSTIFICATION.CENTER;
        public MenuBar(Gui g, Component PARENT) : base(g,PARENT,"MainBar.png")
        {
            setBounds(0, 0.9f, 1f, 0.1f);
        }
        public void addButton(string name,string texName,Action f)
        {
            Button c = new Button(gui, this, texName,f);
            c.name = name;
            buttonCount++;
            float spacing = ((float)1) / (buttonCount+1);

            addComponent(c);

            for (int i = 0; i < children.Count(); i++)
            {
                Component temp = children[i];
                float width = temp.textRect.Width;
                float height = temp.textRect.Height;
                float ratio = width / height;
                float scale = drawRect.Height / height - 0.2f;
                height *= scale;
                width *= scale;

                float scaledWidth = width / drawRect.Width;
                float scaledHeight = height / drawRect.Height;


                if (justify == JUSTIFICATION.CENTER)
                {
                    temp.setBounds((spacing * (i + 1)) - scaledWidth / 2, 0.5f - (scaledHeight) / 2, scaledWidth, scaledHeight);
                }
                else if(justify == JUSTIFICATION.LEFT)
                {

                    //todo lol
                }
            }

        }
    }
}
