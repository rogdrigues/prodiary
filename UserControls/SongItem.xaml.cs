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

namespace ProDiaryApplication.UserControls
{
    /// <summary>
    /// Interaction logic for SongItem.xaml
    /// </summary>
    public partial class SongItem : UserControl
    {
        public SongItem()
        {
            InitializeComponent();
        }

        public SongItem(string title, string author, string time)
        {
            Title = title;
            Author = author;
            Time = time;
        }

        public string Title { get; set; }
        public string Category { get; set; } 
        public string Author { get; set; }
        public string Time { get; set; }

    }
}
