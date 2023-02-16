using Kurcash.Models.StaticModels;
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
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public static HomeWindow WindowContext;
        public HomeWindow()
        {
            InitializeComponent();
            home_frame.Navigate(new CatalogPage());
            WindowContext = this;
          
        }

        private void bucketPageBTN_Click(object sender, RoutedEventArgs e)
        {
            home_frame.Navigate(new BucketPage());
        }

        private void accountBTN_Click(object sender, RoutedEventArgs e)
        {
            home_frame.Navigate(new AddSellerPage());
        }

        private void orderBTN_Click(object sender, RoutedEventArgs e)
        {
            home_frame.Navigate(new OrdersPage());
        }

        private void typesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CatalogBTN_Click(object sender, RoutedEventArgs e)
        {
            home_frame.Navigate(new AdminPage());
        }

        private void catalogPageBtn_Click_1(object sender, RoutedEventArgs e)
        {
            home_frame.Navigate(new CatalogPage());
        }
    }
}
