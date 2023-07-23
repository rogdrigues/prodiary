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
    /// Interaction logic for PopularSong.xaml
    /// </summary>
    public partial class PopularSong : Page
    {
        public PopularSong()
        {
            InitializeComponent();
        }
        public PopularSong(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public string Title { get; set; }
        public string Category { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Time { get; set; }
    }
}
