using Kurcash.Pages;
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
using System.Windows.Shapes;

namespace Kurcash.Windows
{
    /// <summary>
    /// Логика взаимодействия для PanelChooseWindow.xaml
    /// </summary>
    public partial class PanelChooseWindow : Window
    {
        public PanelChooseWindow()
        {
            InitializeComponent();
        }

        private void flowersBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            HomeWindow.WindowContext.home_frame.Navigate(new AdminPage());
        }

        private void sellersBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            HomeWindow.WindowContext.home_frame.Navigate(new SellerAddPage());
        }
    }
}
