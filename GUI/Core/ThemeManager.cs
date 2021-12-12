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
        private static Theme currentTheme;
        public static Theme GetTheme() => currentTheme;
        public static void SetTheme(Theme theme) => currentTheme = theme;
    }
}