using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nibiru.Framework
{
    internal class UIButton : UIAtom
    {
        public string Text { get; set; } = "Button";
        public string? Icon { get; set; }
        public string? Position { get; set; } // "top" or "bottom" ONLY FOR UI NAVIGATION

        public bool IsAccent { get; set; } = false;

        public override FrameworkElement Build()
        {
            object content;
            var mappedIcon = IconHelper.MapIconNameToUnicode(Icon);
            
            if (!string.IsNullOrEmpty(mappedIcon))
            {
                var panel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                };

                var iconBlock = new TextBlock
                {
                    Text = mappedIcon,
                    FontFamily = new System.Windows.Media.FontFamily("Segoe Fluent Icons"),
                    FontSize = 16,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Margin = new Thickness(0, 1, 6, 0)
                };
                panel.Children.Add(iconBlock);

                var textBlock = new TextBlock
                {
                    Text = Text,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };
                panel.Children.Add(textBlock);

                content = panel;
            }
            else
            {
                content = this.Text;
            }

            var butn = new Button
            {
                Content = content,
                Style = IsAccent
                    ? (Style)Application.Current.FindResource("AccentButtonStyle")
                    : (Style)Application.Current.FindResource("DefaultButtonStyle"),
                VerticalContentAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center
            };

            ApplyLayout(butn);
            return butn;
        }
    }
}
