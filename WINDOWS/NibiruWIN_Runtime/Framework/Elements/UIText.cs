using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nibiru.Framework
{
    class UIText : UIAtom
    {
        public string Text { get; set; } = "Text";
        public double FontSize { get; set; } = 14;
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;

        public override FrameworkElement Build()
        {
            var text = new TextBlock
            {
                Text = Text,
                FontSize = FontSize,
                FontWeight = FontWeight,
                Foreground = ThemeManager.TextForeground,
                TextAlignment = TextAlignment
            };

            ApplyLayout(text);
            return text;
        }
    }
}
