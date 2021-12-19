namespace CSOS.GUI.Core
{
    public static class ThemeManager
    {
        public struct Theme
        {
            public uint FocusedColor;
            public uint HoverColor;
            public uint UnFocusedColor;
            public uint BackgroundColor;
            public uint CursorOutlineColor;
            public uint CursorFillColor;
            public uint TextColor;
            public uint WindowMinimizeButtonColor;
            public uint WindowCloseButtonColor;
            public uint NoDockIcon;
            public uint NoAppIcon;
            public uint AppBackground;
            public uint DockBackgroundColor;
        }
        public static Theme CurrentTheme { get; private set; }
        public static void SetTheme(Theme theme)
        {
            CurrentTheme = theme;
        }
    }
}