using System.IO;
using System.Windows;
using Nibiru.Framework;

namespace NibiruWIN_Runtime
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Application.Current.ThemeMode = ThemeMode.Dark;

            var json = File.ReadAllText("ui_layout.json");

            UIAtom? root = JsonAtomParser.Parse(json);
            if (root == null)
            {
                MessageBox.Show("Failed to initialize.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
                return;
            }

            RootContent.Content = root.Build();
        }
    }
}