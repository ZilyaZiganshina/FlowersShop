using Kurcash.Models.DatabaseContextModels;
using Kurcash.Models.StaticModels;
using Kurcash.Models.TemporaryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>

    public partial class CatalogPage : Page
    {
        static int skipcount = 0;
        static int takecount = 15;
        int countelements;
        int countpage;
        static int currentPage;
        private static List<MyItem> flowerlist = new List<MyItem>();
        public CatalogPage()
        {
            InitializeComponent();
            if(flowerlist.Count() == 0)
            {
                FlowerShopDBContext.GetContext().Flowers.Include(x => x.Bucket).Include(x => x.Type).Include(x=>x.Color)
               .ToList().ToList()
               .ForEach(content => flowerlist.Add(new MyItem()
               {
                   Id = content.Id,
                   Price = content.Price,
                   Name = content.Name,
                   BucketId = content.BucketId,
                   ImagePreview = ImageController.ToImage(content.Image),
                   ItemType = content.Type.Name, 
                   colorName =  content.Color.Name

               }));
            }
           
            Thumbnails.ItemsSource = flowerlist.Take(16);
            Thumbnails.SelectionChanged += Thumbnails_SelectionChanged;
            var outer = Task.Factory.StartNew(async () =>      // внешняя задача
            {
                Thread.Sleep(100);
         
                await Dispatcher.Invoke(() => CheckAllItems());
            });

            typesCB.Items.Add("Все типы");
            FlowerShopDBContext.GetContext().Types.ToList().ForEach(type => typesCB.Items.Add(type.Name));
            RefreshPagination();
            Thumbnails.SelectionChanged += Thumbnails_SelectionChanged;
            searchTB.KeyDown += SearchTB_KeyDown;

           
        }
        private void RefreshPagination()
        {
            skipcount = 0;
            countelements = flowerlist.Count();
            countpage = (countelements % 16 > 0) ? (countelements / takecount) + 1 : countelements / takecount;
            currentPage = 1;
            currentpage.Content = $"{currentPage} / {countpage}";
            Thumbnails.ItemsSource = flowerlist.Skip(skipcount).Take(16);
        }
        private void NextLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(skipcount < countelements - (countelements % 16) && (currentPage + 1) <= countpage)
            {
                Thumbnails.ItemsSource = null;
                skipcount += 16;
                currentPage += 1;
                countelements = flowerlist.Count();
                countpage = (countelements % 16 > 0) ? (countelements / takecount) + 1 : countelements / takecount;
              
                currentpage.Content = $"{currentPage} / {countpage}";
                Thumbnails.ItemsSource = flowerlist.Skip(skipcount).Take(16);
                //Thumbnails.SelectionChanged += Thumbnails_SelectionChanged;
                var outer = Task.Factory.StartNew(async () =>      // внешняя задача
                {
                    Thread.Sleep(10);
                    await Dispatcher.Invoke(() => CheckAllItems());
                });
            }
            
        }
      
        private void BackLabel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(skipcount >= 16)
            {
                Thumbnails.ItemsSource = null;
                skipcount -= 16;
                currentPage -= 1;
                countelements = flowerlist.Count();
                countpage = (countelements % 16 > 0) ? (countelements / takecount) + 1 : countelements / takecount;
               
                currentpage.Content = $"{currentPage} / {countpage}";
                Thumbnails.ItemsSource = flowerlist.Skip(skipcount).Take(16);
                Thumbnails.SelectionChanged += Thumbnails_SelectionChanged;
                var outer = Task.Factory.StartNew(async () =>      // внешняя задача
                {
                    Thread.Sleep(10);
                  
                    await Dispatcher.Invoke(() => CheckAllItems());
                });
            }
        }
        private async Task CheckAllItems()
        {
            

            using (var context = new FlowerShopDBContext())
            {
                for (int i = 0; i < Thumbnails.Items.Count; i++)
                {
                    var data = Thumbnails.Items[i] as MyItem;
                    if (data == null)
                        return;
                    var currentSelectedListBoxItem = Thumbnails.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                    ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(currentSelectedListBoxItem);
                    if (myContentPresenter == null) { continue; }
                    DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                    Image target = (Image)myDataTemplate.FindName("LikedImageBtn", myContentPresenter);
                    
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
                }
                selected_flower.BucketId = session_user.IdNavigation.Id;

                context.SaveChanges();

                var currentSelectedListBoxItem = Thumbnails.ItemContainerGenerator.ContainerFromIndex(Thumbnails.SelectedIndex) as ListBoxItem;
                ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(currentSelectedListBoxItem);
                DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                Image target = (Image)myDataTemplate.FindName("LikedImageBtn", myContentPresenter);
                Thumbnails.SelectedItem = null;
            }
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
                   where childItem : DependencyObject
        {
            if (obj == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
           if(e.Key == Key.Enter)
           {
                CheckAllItems();
                var selected_type = typesCB.SelectedItem;
                if (selected_type == null)
                    return;
                Thumbnails.ItemsSource = null;
                Thumbnails.Items.Clear();
                updateflowers();

            }
        }
        private void searchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private void updateflowers()
        {
            flowerlist.Clear();
            FlowerShopDBContext.GetContext().Flowers.Include(x => x.Bucket).Include(x => x.Type)
             .ToList().ToList()
             .ForEach(content => flowerlist.Add(new MyItem()
             {
                 Id = content.Id,
                 Price = content.Price,
                 Name = content.Name,
                 BucketId = content.BucketId,
                 ImagePreview = ImageController.ToImage(content.Image),
                 ItemType = content.Type.Name

             }));
            if(typesCB.SelectedIndex > 0)
            {
                flowerlist = flowerlist.Where(x => x.ItemType == typesCB.SelectedItem.ToString()).ToList();
            }
            flowerlist =  flowerlist.Where(x => x.Name.ToLower().Contains(searchTB.Text.ToLower())).ToList();

            RefreshPagination();

            //Thumbnails.ItemsSource = flowerlist;

            var outer = Task.Factory.StartNew(() =>      // внешняя задача
            {

                Thread.Sleep(200);

                Dispatcher.Invoke(() => CheckAllItems());


            });
        }
        private void typesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_type = (sender as ComboBox).SelectedItem;
            if (selected_type == null)
                return;
            Thumbnails.ItemsSource = null;
            Thumbnails.Items.Clear();
            updateflowers();
        }
    }

}
    
