using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Nibiru.Framework
{
    public class UINavigation : UIAtom
    {
        public int SelectedIndex { get; set; } = 0;
        public List<UIAtom> Pages { get; set; } = new List<UIAtom>();
        
        private ContentPresenter? contentPresenter;

        public override FrameworkElement Build()
        {
            var dock = new DockPanel();

            var sidebar = new Border
            {
                Width = 240,
                Background = ThemeManager.SidebarBackground,
                BorderThickness = new Thickness(0)
            };

            var sidebarDock = new DockPanel();

            var bottomStack = new StackPanel
            {
                Margin = new Thickness(0, 6, 0, 6)
            };
            DockPanel.SetDock(bottomStack, System.Windows.Controls.Dock.Bottom);
            sidebarDock.Children.Add(bottomStack);

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            };

            var topStack = new StackPanel
            {
                Margin = new Thickness(0, 6, 0, 6)
            };

            scrollViewer.Content = topStack;
            sidebarDock.Children.Add(scrollViewer);

            ApplyLayout(dock);

            if (Children.Count > 0)
            {
                int globalIndex = 0;
                foreach (var child in Children[0].Children)
                {
                    var button = child as UIButton;
                    if (button != null)
                    {
                        string position = !string.IsNullOrEmpty(button.Position) 
                            ? button.Position 
                            : "top";

                        bool isSelected = globalIndex == SelectedIndex;
                        
                        var navBtn = CreateNavButton(button.Text, button.Icon, isSelected, globalIndex, topStack, bottomStack);
                        
                        if (position.ToLower() == "bottom")
                        {
                            bottomStack.Children.Add(navBtn);
                        }
                        else
                        {
                            topStack.Children.Add(navBtn);
                        }

                        globalIndex++;
                    }
                }
            }

            sidebar.Child = sidebarDock;
            DockPanel.SetDock(sidebar, System.Windows.Controls.Dock.Left);
            dock.Children.Add(sidebar);

            contentPresenter = new ContentPresenter();

            var contentArea = new Border
            {
                Background = ThemeManager.ContentBackground,
                Child = contentPresenter,
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 7, 7, 7),
                BorderBrush = ThemeManager.ContentBorder,
                BorderThickness = new Thickness(1)
            };
            dock.Children.Add(contentArea);

            LoadPage(SelectedIndex);

            return dock;
        }

        private void LoadPage(int pageIndex)
        {
            if (contentPresenter == null) return;

            if (pageIndex >= 0 && pageIndex < Pages.Count)
            {
                contentPresenter.Content = Pages[pageIndex].Build();
            }
            else if (Children.Count > 1)
            {
                contentPresenter.Content = Children[1].Build();
            }
        }

        private FrameworkElement CreateNavButton(string text, string? icon, bool isSelected, int pageIndex, StackPanel topStack, StackPanel bottomStack)
        {
            var mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var selectionBar = new Border
            {
                Width = 3,
                Background = isSelected ? ThemeManager.SelectionIndicator : Brushes.Transparent,
                CornerRadius = new CornerRadius(0, 2, 2, 0)
            };
            Grid.SetColumn(selectionBar, 0);
            mainGrid.Children.Add(selectionBar);

            if (!string.IsNullOrEmpty(icon))
            {
                var iconText = new TextBlock
                {
                    Text = IconHelper.MapIconNameToUnicode(icon),
                    FontSize = 18,
                    Foreground = ThemeManager.NavForeground,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    FontFamily = new FontFamily("Segoe Fluent Icons"),
                    Opacity = isSelected ? ThemeManager.SelectedOpacity : ThemeManager.UnselectedOpacity
                };
                Grid.SetColumn(iconText, 1);
                mainGrid.Children.Add(iconText);
            }

            var label = new TextBlock
            {
                Text = text,
                FontSize = 14,
                Foreground = ThemeManager.NavForeground,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(string.IsNullOrEmpty(icon) ? 12 : 0, 0, 12, 0),
                Opacity = isSelected ? ThemeManager.SelectedOpacity : ThemeManager.UnselectedOpacity
            };
            Grid.SetColumn(label, 2);
            mainGrid.Children.Add(label);

            var btn = new Button
            {
                Content = mainGrid,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch,
                Background = isSelected ? ThemeManager.NavButtonSelected : Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Height = 40,
                Padding = new Thickness(0, 2, 0, 2),
                Cursor = System.Windows.Input.Cursors.Hand,
                Tag = pageIndex
            };

            btn.Click += (s, e) => {
                SelectedIndex = pageIndex;
                LoadPage(pageIndex);
                UpdateAllButtons(topStack, bottomStack, pageIndex);
            };

            var btnBorder = new Border
            {
                Child = btn,
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(6, 2, 6, 2),
                ClipToBounds = true
            };

            btn.MouseEnter += (s, e) => {
                if ((int?)btn.Tag != SelectedIndex)
                {
                    btn.Background = ThemeManager.NavButtonHover;
                    label.Opacity = ThemeManager.SelectedOpacity;
                    if (!string.IsNullOrEmpty(icon) && mainGrid.Children[1] is TextBlock iconTb)
                    {
                        iconTb.Opacity = ThemeManager.SelectedOpacity;
                    }
                }
            };
            
            btn.MouseLeave += (s, e) => {
                bool isCurrentlySelected = (int?)btn.Tag == SelectedIndex;
                btn.Background = isCurrentlySelected ? ThemeManager.NavButtonSelected : Brushes.Transparent;
                label.Opacity = isCurrentlySelected ? ThemeManager.SelectedOpacity : ThemeManager.UnselectedOpacity;
                if (!string.IsNullOrEmpty(icon) && mainGrid.Children[1] is TextBlock iconTb)
                {
                    iconTb.Opacity = isCurrentlySelected ? ThemeManager.SelectedOpacity : ThemeManager.UnselectedOpacity;
                }
            };

            return btnBorder;
        }

        private void UpdateAllButtons(StackPanel topStack, StackPanel bottomStack, int selectedPageIndex)
        {
            foreach (var stack in new[] { topStack, bottomStack })
            {
                foreach (var child in stack.Children)
                {
                    if (child is Border border && border.Child is Button navBtn)
                    {
                        int btnPageIndex = (int?)navBtn.Tag ?? -1;
                        bool isNowSelected = btnPageIndex == selectedPageIndex;

                        navBtn.Background = isNowSelected ? ThemeManager.NavButtonSelected : Brushes.Transparent;

                        if (navBtn.Content is Grid grid && grid.Children.Count > 0)
                        {
                            if (grid.Children[0] is Border selBar)
                            {
                                selBar.Background = isNowSelected ? ThemeManager.SelectionIndicator : Brushes.Transparent;
                            }
                            
                            for (int i = 1; i < grid.Children.Count; i++)
                            {
                                if (grid.Children[i] is TextBlock tb)
                                {
                                    tb.Opacity = isNowSelected ? ThemeManager.SelectedOpacity : ThemeManager.UnselectedOpacity;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}