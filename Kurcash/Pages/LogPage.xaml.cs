using Kurcash.Models.DatabaseContextModels;
using Kurcash.Models.StaticModels;
using Kurcash.Windows;
using Microsoft.EntityFrameworkCore;
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

namespace Kurcash.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {
        
        public LogPage()
        {
            InitializeComponent();
            
        }

        private void submitBTN_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in FlowerShopDBContext.GetContext().Users.Include(x=>x.IdNavigation))
            {
                if (item.Login == logTB.Text && item.Password == passTB.Text)
                {
                    CurrentUser.Current = item;
                    MessageBox.Show("Авторизация прошла успешно.");
                    if(item.RoleId == 2)
                    {
                        HomeWindow home = new HomeWindow();
                        home.Show();
                        MainWindow._mainWindowContext.Hide();
                    }
                    else if(item.RoleId == 1)
                    {
                        HomeWindow home = new HomeWindow();
                        HomeWindow.WindowContext.home_frame.Navigate(new AdminPage());
                        home.Show();
                        MainWindow._mainWindowContext.Hide();
                    }
                   
                    return;
                }
            }
            MessageBox.Show("Введите корректные данные!");
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow._mainWindowContext
                .mainFrame.Navigate(new RegPage());
        }
    }
}
