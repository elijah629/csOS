using Mosa.External.x86;
using System.Collections.Generic;

namespace CSOS.Helper
{
    public class Menu
    {
        private readonly string[] opts_;
        private readonly int left_;
        private readonly int top_;
        private readonly string title_;
        private bool running = true;
        public Menu(string title, string[] options, int left, int top)
        {
            title_ = title;
            top_ = top;
            left_ = left;
            opts_ = options;
        }
        public int Start()
        {
            int selected = 0;
            List<Tuple<string, bool>> items = new List<Tuple<string, bool>>();
            for (int i = 0; i < opts_.Length; i++)
            {
                items.Add(new Tuple<string, bool>(opts_[i], false));
            }
            items[selected] = new Tuple<string, bool>(items[selected].Item1, true);
            while (running == true)
            {
                VBEConsole.Clear();
                VBEConsole.WriteLine("W = UP", 0x0000FF);
                VBEConsole.WriteLine("S = DOWN", 0x0000FF);
                VBEConsole.SetPosition(left_ * 8, (top_ - 1) * 16);
                VBEConsole.Write(title_, 0x0000FF);
                foreach (Tuple<string, bool> option in items)
                {
                    if (option.Item2 == true)
                    {
                        selected = items.IndexOf(option);
                        VBEConsole.SetPosition(left_ * 8, (items.IndexOf(option) + top_) * 16);
                        VBEConsole.Write($"> {option.Item1}", 0x00FF00);
                    }
                    else
                    {
                        VBEConsole.SetPosition(left_ * 8, (items.IndexOf(option) + top_) * 16);
                        VBEConsole.Write($"  {option.Item1}", 0xFFFFFF);
                    }
                }
                switch (VBEConsole.ReadKey())
                {
                    case Mosa.External.x86.Driver.KeyCode.W:
                        if (selected > 0)
                        {
                            items[selected] = new Tuple<string, bool>(items[selected].Item1, false);
                            selected -= 1;
                            items[selected] = new Tuple<string, bool>(items[selected].Item1, true);
                        }
                        break;
                    case Mosa.External.x86.Driver.KeyCode.S:
                        if (selected < items.Count - 1)
                        {
                            items[selected] = new Tuple<string, bool>(items[selected].Item1, false);
                            selected += 1;
                            items[selected] = new Tuple<string, bool>(items[selected].Item1, true);
                        }
                        break;
                    case Mosa.External.x86.Driver.KeyCode.Enter:
                        running = false;
                        return selected;
                    default:
                        break;
                }
            }
            Stop();
            return -1;
        }
        public void Stop()
        {
            running = false;
        }
    }
}