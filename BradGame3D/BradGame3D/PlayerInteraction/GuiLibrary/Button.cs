using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BradGame3D.PlayerInteraction.GuiLibrary
{
    public class Button : Component
    {
        Action function;
        public Button(Gui g, Component PARENT, string img, Action func) : base(g, PARENT, img)
        {
            function = func;
        }
        public override void doClick()
        {
            Console.WriteLine("Component: " + texName + " was clicked!");
            function();
        }
        
    }
}
