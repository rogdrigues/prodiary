using ProDiaryApplication.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Threading;

namespace ProDiaryApplication.MenuItem
{
    /// <summary>
    /// Interaction logic for Task.xaml
    /// </summary>
    public partial class Task : Window
    {
        public Task()
        {
            InitializeComponent();
            UpdateCurrentMonth();
            Loaded += MainWindow_Loaded;
            LoadDataFromDatabase();
            // Gán danh sách các mục từ cơ sở dữ liệu vào ListView
            listView.ItemsSource = itemList;


        }

        public class Item
        {
            public string Title { get; set; }
            public string Time { get; set; }
            public string Color { get; set; }
            public string Icon { get; set; }
            public string IconBell { get; set; }
        }
        private List<Item> itemList = new List<Item>();
        private void LoadDataFromDatabase()
        {
            // Đoạn mã để truy vấn và lấy dữ liệu từ cơ sở dữ liệu
            // Ví dụ:
           

            var context = new DiaryNoteContext();
            
            var tasks = context.Tasks.ToList();
            foreach (var task in tasks)
            {
                itemList.Add(new Item
                {
                    Title = task.TaskTitle,
                    Time = DateTime.Parse(task.TaskBegin.ToString()).ToString("HH:MM") + " - " + DateTime.Parse(task.TaskEnd.ToString()).ToString("HH:MM"),
                    Color = "#EBA5BB",
                    Icon = "CheckCircle",
                    IconBell = "BellSlash"
                });
            }
            int num = itemList.Count;
            txtNumTask.Text = num + " tasks -" + " 2 dates left";

        }

        public Account? CurrentUser { get; set; }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Sử dụng DispatcherTimer để cập nhật tháng hiện tại mỗi giây
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCurrentMonth();
        }

        private void UpdateCurrentMonth()
        {
            txtMonth.Text = DateTime.Now.ToString("MMMM");
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            Color buttonColor = (Color)ColorConverter.ConvertFromString("#C73F69");
            SolidColorBrush buttonBrush = new SolidColorBrush(buttonColor);
            txtDay.Text = DateTime.Now.ToString("dd");
            txtMonth1.Text = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture).ToUpper();
            txtDay1.Text = DateTime.Now.ToString("dddd", CultureInfo.InvariantCulture);
            if (btn2020.Content.ToString() == currentYear.ToString())
            {
                btn2020.Foreground = buttonBrush;
            }
                
            if (btn2021.Content.ToString() == currentYear.ToString())
            {
                btn2021.Foreground = buttonBrush;
            }
                
            if (btn2022.Content.ToString() == currentYear.ToString())
            {
                btn2022.Foreground = buttonBrush;
            }
            if (btn2023.Content.ToString() == currentYear.ToString())
            {
                btn2023.Foreground = buttonBrush;
            }
            if (btn2024.Content.ToString() == currentYear.ToString())
            { 
                btn2024.Foreground = buttonBrush; 
            }

            if(btnM7.Content.ToString() == currentMonth.ToString())
            {
                btnM7.Foreground = buttonBrush;
            }
            
        }       
        




















        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void lblNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNote.Focus();
        }

        private void lblTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtTime.Focus();
        }

        private void txtNote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNote.Text) && txtNote.Text.Length > 0)
                lblNote.Visibility = Visibility.Collapsed;
            else
                lblNote.Visibility = Visibility.Visible;
        }

        private void txtTime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTime.Text) && txtTime.Text.Length > 0)
                lblTime.Visibility = Visibility.Collapsed;
            else
                lblTime.Visibility = Visibility.Visible;
        }

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            AddTask task = new AddTask();
            task.CurrentUser = CurrentUser;
            task.Show();
        }

        

        
    }
}

//< us:Item Title = "ahihi" Time="09:45 - 10:30" Color="#EBA5BB" Icon="CheckCircle" IconBell="BellSlash" Loaded="Item_Loaded" />
//                <us:Item Title = "Quan Dai CA" Time="11:30 - 12:00" Color="#EBA5BB" Icon="CheckCircle" IconBell="BellSlash"/>
//                <us:Item Title = "leu Leu" Time="14:00 - 15:30" Color="#f1f1f1" Icon="CircleThin" IconBell="Bell"/>
//                <us:Item Title = "aaaaa" Time="20:15 - 21:45" Color="#f1f1f1" Icon="CircleThin" IconBell="Bell"/>
//                <us:Item Title = "bbbbb" Time="23:00 - 23:20" Color="#f1f1f1" Icon="CircleThin" IconBell="Bell"/>