using Kurcash.Models.DatabaseContextModels;
using Kurcash.Models.StaticModels;
using Kurcash.Models.TemporaryModels;
using Kurcash.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для BucketPage.xaml
    /// </summary>
    public partial class BucketPage : Page
    {
        int totalSumPrice, countFlower;
        public BucketPage()
        {
            InitializeComponent();
            //var flowersInBucket = FlowerShopDBContext.GetContext().Flowers.Include(x => x.Bucket).Where(x=>x.BucketId == CurrentUser.Current.Id).ToList();
            FlowerShopDBContext.GetContext().Flowers.Include(x => x.Bucket)
                .ToList().Where(x=>x.BucketId == CurrentUser.Current.Id).ToList()
                .ForEach(content => Thumbnails.Items.Add(new MyItem()
                {
                    Id = content.Id,
                    Price = content.Price,
                    Name = content.Name,
                    BucketId = content.BucketId,
                    ImagePreview = ImageController.ToImage(content.Image) 
                }));
            totalSumPrice = FlowerShopDBContext.GetContext().Flowers.Include(x => x.Bucket)
                .ToList().Where(x => x.BucketId == CurrentUser.Current.Id).Sum(x => x.Price).Value;
            SumLabel.Content = "Cумма: " + totalSumPrice.ToString();
            CountLabel.Content = "Количество товаров " + FlowerShopDBContext.GetContext().Flowers.Include(x => x.Bucket)
                .ToList().Where(x => x.BucketId == CurrentUser.Current.Id).Count();

            var outer = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Dispatcher.Invoke(() => CheckAllItems());
            });

        }
        private void CheckAllItems()
        {
            using (var context = new FlowerShopDBContext())
            {
                for (int i = 0; i < Thumbnails.Items.Count; i++)
                {
                    var data = Thumbnails.Items[i] as MyItem;
                    if (data == null)
                        return;
                    var currentSelectedListBoxItem = Thumbnails.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    ContentPresenter myContentPresenter = FindChildClass.FindVisualChild<ContentPresenter>(currentSelectedListBoxItem);
                    if (myContentPresenter == null) { continue; }
                    DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                    Image target = (Image)myDataTemplate.FindName("LikedImageBtn", myContentPresenter);
                    //MessageBox.Show(target.Name);
                    var flower = context.Flowers.FirstOrDefault(x => x.Id == data.Id);
                    if (flower.BucketId != null)
                    {
                        target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Images/liked.png"));
                    }
                    else
                    {
                        target.Source = ImageController.FromFile(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Images/like.png"));

                    }

                }

            }
        }
        private void LikedImageBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void backBTN_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.WindowContext.home_frame.Navigate(new CatalogPage());
        }

        private void payBTN_Click(object sender, RoutedEventArgs e)
        {
            HiddenGrid.Visibility = Visibility.Visible;
            var outer = Task.Factory.StartNew(() =>
            {
                using(var context = new FlowerShopDBContext())
                {
                    string items = "";

                    var user = context.Users.Include(x=>x.IdNavigation).ThenInclude(x=>x.Flowers).FirstOrDefault(x=>x.Id == CurrentUser.Current.Id);

                    if (totalSumPrice > 0)
                    {
                        var order = new Order
                        {
                            BucketId = CurrentUser.Current.Id,
                            CreationDate = DateTime.UtcNow,
                            TotalPrice = totalSumPrice,
                        };

                        user.IdNavigation.Flowers.ToList().ForEach(x => order.Flowers.Add(x));

                        
                        context.Orders.Add(order);
                        
                    }
                    foreach(var item in context.Flowers)
                    {
                        if(item.BucketId == CurrentUser.Current.Id)
                        {
                            item.BucketId = null;
                        }
                    }
                    context.SaveChanges();
                }
                Thread.Sleep(3000);
                Dispatcher.Invoke(() => HomeWindow.WindowContext.home_frame.Navigate(new OrdersPage()));
            });
            
        }

        private void checkBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Thumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckAllItems();
            using (var context = new FlowerShopDBContext())
            {
                var selected_item = Thumbnails.SelectedItem as MyItem;
                if (selected_item == null) { return; }

                var session_user = context.Users.Include(x => x.IdNavigation).ThenInclude(x => x.Flowers).FirstOrDefault(x => x.Id == CurrentUser.Current.Id);

                var selected_flower = context.Flowers.FirstOrDefault(x => x.Id == selected_item.Id);

                if (session_user.IdNavigation.Flowers.Any(x => x.Id == selected_flower.Id))
                {

                    if (MessageBox.Show("Этот цветок и так в уже в корзине, удалить?", "Подтвердите действие", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        selected_flower.BucketId = null;

                        context.SaveChanges();

                        CheckAllItems();
                        Thumbnails.SelectedItem = null;
                        return;
                    }
                    else
                    {
                        Thumbnails.SelectedItem = null;
                        return;
                    }

                    /*MessageBox.Show("Этот цветок уже и так в базе данных!");
                    Thumbnails.SelectedItem = null;
                    return;*/
                }

                selected_flower.BucketId = session_user.IdNavigation.Id;

                context.SaveChanges();

                var currentSelectedListBoxItem = Thumbnails.ItemContainerGenerator.ContainerFromIndex(Thumbnails.SelectedIndex) as ListBoxItem;
                ContentPresenter myContentPresenter = FindChildClass.FindVisualChild<ContentPresenter>(currentSelectedListBoxItem);
                DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                Image target = (Image)myDataTemplate.FindName("LikedImageBtn", myContentPresenter);
                //MessageBox.Show(target.Name);
                Thumbnails.SelectedItem = null;
            }

        }
    }
}
