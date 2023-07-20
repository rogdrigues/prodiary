using ProDiaryApplication.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ProDiaryApplication.MenuItem
{
    /// <summary>
    /// Interaction logic for Memos.xaml
    /// </summary>
    public partial class Memos : Window
    {
        public Memos()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                var tags = context.Tags.ToList();
                tagLists.ItemsSource = tags;

                var memos = context.Memos.ToList();
                foreach (var memo in memos)
                {
                    if (memo.MemoAttachments != null)
                    {
                        var image = new Image();
                        image.Source = LoadImageFromByteArray(memo.MemoAttachments);
                        image.Width = 100; 
                        image.Height = 100;
                    }
                }
                memoList.ItemsSource = memos;
            }
        }

        private BitmapImage LoadImageFromByteArray(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            var image = new BitmapImage();
            using (var memoryStream = new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memoryStream;
                image.EndInit();
            }

            return image;
        }
    }
}
