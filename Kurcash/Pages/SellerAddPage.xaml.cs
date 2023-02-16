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
    /// Логика взаимодействия для SellerAddPage.xaml
    /// </summary>
    public partial class SellerAddPage : Page
    {
        public SellerAddPage()
        {
            InitializeComponent();
        }

        private void submitBTN_Click(object sender, RoutedEventArgs e)
        {
            using(var db = new FlowerShopDBContext())
            {
                DateTime created = DateTime.UtcNow;
                db.Buckets.Add(new Bucket
                {
                    CreationDate = created,
                });

                db.SaveChanges();
                
                var bckt = db.Buckets.FirstOrDefault(x => x.CreationDate == created);

                db.Users.Add(new User
                {
                    Id = bckt.Id,
                    Login = logTB.Text,
                    Password = passTB.Text,
                    Email = emailTB.Text,
                    Fullname = fullnameTB.Text
                });
                db.SaveChanges();
                MessageBox.Show("Новый продавец добавлен.");   
            }
        }
    }
}
