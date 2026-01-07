using System.Windows;
using System.Windows.Controls;

namespace Nibiru.Framework
{
    public class UIDock : UIAtom
    {
        public bool LastChildFill { get; set; } = true;

        public override FrameworkElement Build()
        {
            var panel = new DockPanel
            {
                LastChildFill = LastChildFill
            };

            ApplyLayout(panel);

            foreach (var child in Children)
            {
                var element = child.Build();
                if (child.Dock.HasValue)
                    DockPanel.SetDock(element, child.Dock.Value);
                panel.Children.Add(element);
            }

            return panel;
        }
    }
}