using System.Windows;

namespace TowerDefense.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            new GameWindow { Owner = this }.ShowDialog();
        }
    }
}
