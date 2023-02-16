using Kurcash.Models.DatabaseContextModels;
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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void submitBTN_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in FlowerShopDBContext._context.Users)
            {
                if (item.Login == logTB.Text && item.Password == passTB.Text)
                {
                    MessageBox.Show("Такой пользователь уже существует.");
                    return;
                }
            }
            //linking user and bucket 1:1 throw datetime 
            DateTime created = DateTime.UtcNow;
            using(var _context = new FlowerShopDBContext())
            {
                _context.Buckets.Add(new Bucket
                {
                    CreationDate = created,
                });
                _context.SaveChanges();
                var bckt = _context.Buckets.FirstOrDefault(x => x.CreationDate == created);
                
                
                _context.Users.Add(new User
                {
                    Id = bckt.Id,
                    Login = logTB.Text,
                    Password = passTB.Text
                });


                _context.SaveChanges();
            }
            

            

            

            
            MessageBox.Show("Пользователь добавлен успешно, пройдите авторизацию.");
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow._mainWindowContext.mainFrame.Navigate(new LogPage());
        }
    }
}
