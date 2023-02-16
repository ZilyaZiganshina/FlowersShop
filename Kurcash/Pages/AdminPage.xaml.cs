using Kurcash.Models.DatabaseContextModels;
using Kurcash.Models.StaticModels;
using Kurcash.Models.TemporaryModels;
using Kurcash.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml.Linq;

namespace Kurcash.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            FlowerShopDBContext.GetContext().Flowers
                .ToList()
                .ForEach(content => FlowersGrid.Items.Add(new Flower()
                {
                    Id = content.Id,
                    Price = content.Price,
                    Name = content.Name,
                    Image = content.Image,
                 
               
                    BucketId = content.BucketId,

                    ColorId = content.ColorId,
                    TypeId = content.TypeId,    
                   
                }));

           
        }
        private void LoadGoods()
        {
            FlowersGrid.Items.Clear();

            FlowerShopDBContext.GetContext().Flowers
                 .ToList()
                 .ForEach(content => FlowersGrid.Items.Add(new Flower()
                 {
                     Id = content.Id,
                     Price = content.Price,
                     Name = content.Name,
                     Image = content.Image,
                     Type = content.Type,
                     
                     BucketId = content.BucketId,
                     ColorId = content.ColorId,
                     
                     
                 }));

        }
        Flower selectedFlower = null;
        static byte[] _data = null;
        private void Refresh()
        {
            selectedFlower = null;
            
            priceTB.Text = "";
            _data = null;
            NameTB.Text = "";
            goofIMG.Source = null;
        }
        private void saveBTN_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFlower == null)
                return;
            if (String.IsNullOrEmpty(NameTB.Text) ||
                   String.IsNullOrWhiteSpace(priceTB.Text) ||
                   ColorTB.SelectedItem == null ||
                  
                   typeTB.SelectedItem == null
                   )
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            using (var db = new FlowerShopDBContext())
            {


                var flower =
                    db.Flowers.FirstOrDefault(x => x.Id == selectedFlower.Id);
                
                flower.Price = int.Parse(priceTB.Text);
                flower.Name = NameTB.Text;
                flower.Image = _data;
                flower.TypeId = typeTB.SelectedIndex + 1;
                flower.ColorId = ColorTB.SelectedIndex + 1;
                db.SaveChanges();
            }
            LoadGoods();
        }

        private void refreshBTN_Click(object sender, RoutedEventArgs e)
        {
            selectedFlower = null;
            ColorTB.SelectedItem = null;
            typeTB.SelectedItem = null;
            
            priceTB.Text = "";
            _data = null;
            NameTB.Text = "";
            goofIMG.Source = ImageController.LoadImage(File.ReadAllBytes("./Images/no_image.png"));
        }

        private void goofIMG_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var data =
              ImageController.GetByteFileFromExplorer();
            if (data == null)
            {
                _data = File.ReadAllBytes("./Images/no_image.png");
                return;
            }
            _data = data;
            goofIMG.Source = ImageController.LoadImage(_data);
        }

        private void deleteBTN_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFlower == null)
            {
                MessageBox.Show("Вы не выбрали объект для удаления");
                return;
            }
            using (var db = new FlowerShopDBContext())
            {
                var current_good =
                        db.Flowers.FirstOrDefault(x => x.Id == selectedFlower.Id);
                db.Flowers.Remove(current_good);
                db.SaveChanges();
            }
            LoadGoods();
            Refresh();
                
            
            
        }

        private void addBTN_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FlowerShopDBContext())
            {
                if (String.IsNullOrEmpty(NameTB.Text) ||
                    String.IsNullOrWhiteSpace(priceTB.Text) ||
                    ColorTB.SelectedItem == null || 
                   
                    typeTB.SelectedItem == null 
                    )
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }    
                   
                context.Flowers.Add(new Flower
                {
                    Name = NameTB.Text,
                    Price = int.Parse(priceTB.Text),
                    TypeId = typeTB.SelectedIndex + 1,
                    ColorId = ColorTB.SelectedIndex + 1,
                  
                    Image = _data
                });
                context.SaveChanges();
                LoadGoods();
                MessageBox.Show("Данные добавились успешно");
            }

        }
        bool IsGood(char c)
        {
            if (c >= '0' && c <= '9')
                return true;
            if (c >= 'a' && c <= 'f')
                return true;
            if (c >= 'A' && c <= 'F')
                return true;
            return false;
        }
        private void FlowersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var db = new FlowerShopDBContext())
            {
                if (FlowersGrid.SelectedItem == null) return;
                var selected = (Flower)FlowersGrid.SelectedItem;
                var flower =
                    db.Flowers.FirstOrDefault(x => x.Id == selected.Id);
                if (flower == null)
                {
                    MessageBox.Show("Ошибка!");
                    return;
                }
                selectedFlower = flower;
                NameTB.Text = flower.Name;
                priceTB.Text = flower.Price.ToString();
                ColorTB.SelectedIndex = ((int)(flower.ColorId - 1));
                
                typeTB.SelectedIndex = (flower.TypeId - 1);
                goofIMG.Source = ImageController.LoadImage(flower.Image);
            }
        }

        private void priceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(IsGood);
        }
    }
}
