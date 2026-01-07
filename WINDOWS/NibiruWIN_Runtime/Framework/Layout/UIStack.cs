using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nibiru.Framework
{
    class UIStack : UIAtom
    {
        public Orientation Orientation { get; set; } = Orientation.Vertical;
        public double Spacing { get; set; } = 0;

        public override FrameworkElement Build()
        {
            var panel = new StackPanel
            {
                Orientation = this.Orientation,
                
            };

            ApplyLayout(panel);

            for (int i = 0; i < Children.Count; i++)
            {
                var element = Children[i].Build();

                // Apply spacing between children (not after the last item)
                if (Spacing > 0 && element is FrameworkElement fe && i < Children.Count - 1)
                {
                    fe.Margin = Orientation == Orientation.Vertical
                        ? new Thickness(fe.Margin.Left, fe.Margin.Top, fe.Margin.Right, fe.Margin.Bottom + Spacing)
                        : new Thickness(fe.Margin.Left, fe.Margin.Top, fe.Margin.Right + Spacing, fe.Margin.Bottom);
                }

                panel.Children.Add(element);
            }

            return panel;
        }
    }
}
