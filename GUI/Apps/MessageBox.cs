using CSOS.GUI.Core;
using Mosa.External.x86.Drawing;
using Mosa.External.x86.Drawing.Fonts;

namespace CSOS.GUI.Apps
{
    public class MessageBox : App
    {
        private readonly string _text;
        public MessageBox(string text = "")
        {
            //(BitFont.Calculate(Core.System.DefaultFontName, text) + 45) + 
            Width = (16 + BitFont.Calculate(Core.System.DefaultFontName, Title) + (15 * 2)) + 2;
            _text = text;
            //Dock = false;
            Title = " ";
            if (windowGraphics == null) windowGraphics = new VirtualGraphics(Width, Height);

        }
        public override void UIUpdate()
        {
            windowGraphics.Clear(0xFFFFFF);
            windowGraphics.DrawBitFontString(Core.System.DefaultFontName, 0x0, _text, 5, 5);
            windowGraphics.Update();
            Program.graphics.DrawImageASM(windowGraphics.bitmap, X, Y);
        }
        public override void InputUpdate()
        {

        }

    }
}