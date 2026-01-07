using System.Windows;
using System.Windows.Media;

namespace Nibiru.Framework
{
    public static class ThemeManager
    {
        public static ThemeMode CurrentTheme => Application.Current.ThemeMode;

        // Navigation colors
        public static Brush SidebarBackground => CurrentTheme == ThemeMode.Dark
            ? Brushes.Transparent
            : new SolidColorBrush(Color.FromRgb(243, 243, 243));

        public static Brush ContentBackground => new SolidColorBrush(CurrentTheme == ThemeMode.Dark
            ? Color.FromArgb(20, 255, 255, 255)
            : Color.FromRgb(255, 255, 255));

        public static Brush ContentBorder => new SolidColorBrush(CurrentTheme == ThemeMode.Dark
            ? Color.FromArgb(0, 0, 0, 0)
            : Color.FromRgb(235, 235, 235));

        public static Brush SelectionIndicator => new SolidColorBrush(Color.FromRgb(0, 150, 220));

        public static Brush NavButtonSelected => new SolidColorBrush(CurrentTheme == ThemeMode.Dark
            ? Color.FromArgb(20, 255, 255, 255)
            : Color.FromRgb(231, 234, 238));

        public static Brush NavButtonHover => new SolidColorBrush(CurrentTheme == ThemeMode.Dark
            ? Color.FromArgb(15, 255, 255, 255)
            : Color.FromRgb(250, 250, 250));

        public static Brush NavForeground => CurrentTheme == ThemeMode.Dark
            ? Brushes.White
            : new SolidColorBrush(Color.FromRgb(32, 32, 32));

        // Text colors
        public static Brush TextForeground => CurrentTheme == ThemeMode.Dark
            ? Brushes.White
            : Brushes.Black;

        public static double SelectedOpacity => 1.0;
        public static double UnselectedOpacity => 0.8;
    }
}
