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
    /// Логика взаимодействия для AddSellerPage.xaml
    /// </summary>
    public partial class AddSellerPage : Page
    {
        private User CorrectCurrentUser;
        public AddSellerPage()
        {
            InitializeComponent();
            CorrectCurrentUser = FlowerShopDBContext.GetContext().Users.Include(x=>x.Role).FirstOrDefault(x => x.Id == CurrentUser.Current.Id);
            if(CorrectCurrentUser == null) {
                MessageBox.Show("упс... похоже, вас удалили :3, попытайтесь войти или пройти регистрацию по новой.");
            }
            if(CorrectCurrentUser.Role.Name == "Администратор"){
                adminBTN.Visibility = Visibility.Visible;
            }
            else
            {
                adminBTN.Visibility = Visibility.Collapsed;
            }
            LoadUser();
        }

        private void LoadUser()
        {
            logTB.Text = CorrectCurrentUser.Login;
            passTB.Text = CorrectCurrentUser.Password;
            emailTB.Text = CorrectCurrentUser.Email;
            fullnameTB.Text = CorrectCurrentUser.Fullname;
            
        }

        private void adminBTN_Click(object sender, RoutedEventArgs e)
        {
            PanelChooseWindow win = new PanelChooseWindow();
            win.Show();

            /*HomeWindow.WindowContext.home_frame.Navigate(new AdminPage());*/
        }

        private void goodsBTN_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.WindowContext.home_frame.Navigate(new CatalogPage());
        }

        private void exitBTN_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.WindowContext.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void submitBTN_Click(object sender, RoutedEventArgs e)
        {
           using(var context = new FlowerShopDBContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == CurrentUser.Current.Id);
                if(user == null) { return; }

                user.Login = logTB.Text; user.Password = passTB.Text; user.Fullname = fullnameTB.Text;
                user.Email = emailTB.Text;
                
                context.SaveChanges();

                CurrentUser.Current = user;
            }
        }
    }
}
