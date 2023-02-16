
using iTextSharp.text;
using iTextSharp.text.pdf;
using Kurcash.Models.DatabaseContextModels;
using Kurcash.Models.StaticModels;
using Kurcash.Models.TemporaryModels;
using Microsoft.EntityFrameworkCore;
using System.Web;

using System.Linq;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Document = iTextSharp.text.Document;
using System.Text;
using System.Diagnostics;
using Kurcash.Windows;
using System.IO;
using System;

namespace Kurcash.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        static MyOrderItem myOrderItem = new MyOrderItem();
        public OrdersPage()
        {
            InitializeComponent();
             
            LoadOrders();
        }
       
        private void LoadOrders()
        {
            Orders.Items.Clear();
            Orders.ItemsSource = null;
            using(var context = new FlowerShopDBContext())
            {
                var current_user = context.Users.Include(x=>x.IdNavigation).ThenInclude(x=>x.Orders).ThenInclude(x=>x.Flowers)
                    .FirstOrDefault(x=>x.Id == CurrentUser.Current.Id);
                string items = "";
                var orders = current_user.IdNavigation.Orders.ToList();

                foreach(var order in orders)
                {
                    items = "";
             
                    order.Flowers.ToList().ForEach(x=>items+= $"{x.Name}\n");
                    Orders.Items.Add(new MyOrderItem()
                    {
                        Id = order.Id,
                        TotalPrice = order.TotalPrice,
                        CreationDate = order.CreationDate,
                        Items = items
                    }) ;
                }

                Orders.SelectionChanged += Orders_SelectionChanged;
            }
            
        }

        private void Orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           myOrderItem = Orders.SelectedItem as MyOrderItem;
            if(myOrderItem != null)
            {
                DeleteBTN.IsEnabled = true;
                checkBTN.IsEnabled = true;
            }
        }

        private void backBTN_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow.WindowContext.home_frame.Navigate(new CatalogPage());
        }

        private void DeleteBTN_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FlowerShopDBContext())
            {
                //myOrderItem = Orders.SelectedItem as MyOrderItem;
                if (myOrderItem == null) { return; }

                var session_user = context.Users.Include(x => x.IdNavigation).ThenInclude(x => x.Flowers).FirstOrDefault(x => x.Id == CurrentUser.Current.Id);

                var selected_order = context.Orders.FirstOrDefault(x => x.Id == myOrderItem.Id);

                if (session_user.IdNavigation.Orders.Any(x => x.Id == selected_order.Id))
                {
                    if (MessageBox.Show("Точно хотите удалить заказ?", "Подтвердите действие", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        foreach(var item in context.Flowers)
                        {
                            item.OrderId = null;
                        }
                        context.SaveChanges();

                        context.Orders.Remove(selected_order);
                        context.SaveChanges();
                        
                        LoadOrders();
                        Orders.SelectedItem = null;
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                context.SaveChanges();
                Orders.SelectedItem = null;

            }
            LoadOrders();
        }
        BaseFont bf;
        Font f_title;
        Font f_text;
        private void checkBTN_Click(object sender, RoutedEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfPTable table = new PdfPTable(7);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 20, 20, 40, 20);
            PdfWriter writer = PdfWriter.GetInstance(doc, new System.IO.FileStream("Чек.pdf", System.IO.FileMode.Create));
            string workingDirectory = Environment.CurrentDirectory;
            string file = $"{workingDirectory}\\Fonts\\eirik-raude.ttf";
            bf = BaseFont.CreateFont(file, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            
            f_title = new Font(bf, 20);
            f_text = new Font(bf, 14);

            doc.Open();
            using (var context = new FlowerShopDBContext())
            {
                if (myOrderItem != null)
                {
                    doc.Add(new Phrase("Чек об оплате\n", f_title));
                    
                    var user = context.Users.Include(x => x.IdNavigation).ThenInclude(x => x.Orders).FirstOrDefault(x => x.Id == CurrentUser.Current.Id);
                    doc.Add(new Phrase("Дата создания заказа: " + myOrderItem.CreationDate.ToString() + "\n", f_text));
                    doc.Add(new Phrase("Товары: " + myOrderItem.Items.ToString() + "\n", f_text));
                    doc.Add(new Phrase("Цена: " + myOrderItem.TotalPrice.ToString() + "\n" , f_text));
                    doc.Add(new Phrase("Компания: ООО \"Цветок\"\n", f_text));
                }
            }
           

            doc.Close();
              
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@"Чек.pdf")
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}
