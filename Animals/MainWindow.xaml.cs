using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Animals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(Home);
        }
        public void ChangePage(UserControl control)
        {
            Home.Visibility = Visibility.Collapsed;
            Mammals.Visibility = Visibility.Collapsed;
            control.Visibility = Visibility.Visible;
        }
        private void MammalsBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(Mammals);
        }
    }
}
