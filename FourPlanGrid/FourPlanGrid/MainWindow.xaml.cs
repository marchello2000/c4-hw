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

namespace FourPlanGrid
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

        private void Player1NameGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Resources["rowHeight"] = new GridLength(50);
        }
    }
}
