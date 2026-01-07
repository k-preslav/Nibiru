using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Nibiru.Framework
{
    public abstract class UIAtom
    {
        public string? ID { get; set; }
        public List<UIAtom> Children { get; } = new();

        public Dock? Dock { get; set; }

        public Thickness? Margin { get; set; }
        public HorizontalAlignment? HorizontalAlignment { get; set; }
        public VerticalAlignment? VerticalAlignment { get; set; }

        protected void ApplyLayout(FrameworkElement element)
        {
            if (Margin.HasValue)
                element.Margin = Margin.Value;

            if (HorizontalAlignment.HasValue)
                element.HorizontalAlignment = HorizontalAlignment.Value;

            if (VerticalAlignment.HasValue)
                element.VerticalAlignment = VerticalAlignment.Value;
        }

        public abstract FrameworkElement Build();
    }
}