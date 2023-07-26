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
            LoadDataFromDatabase(date);
            // Gán danh sách các mục từ cơ sở dữ liệu vào ListView
            listView.ItemsSource = itemList;


        }
        DateTime date = DateTime.Today;
        public class Item
        {
            public string Title { get; set; }
            public string Time { get; set; }
            public string Color { get; set; }
            public string Icon { get; set; }
            public string IconBell { get; set; }
        }
        private List<Item> itemList = new List<Item>();
        private void LoadDataFromDatabase(DateTime today)
        {
            // Đoạn mã để truy vấn và lấy dữ liệu từ cơ sở dữ liệu
            // Ví dụ:
           

            var context = new DiaryNoteContext();
            
            var tasks = context.Tasks.ToList().Where(x => x.TaskDate == today);
            itemList.Clear();
            foreach (var task in tasks)
            {
                itemList.Add(new Item
                {
                    Title = task.TaskTitle,
                    Time = DateTime.Parse(task.TaskBegin.ToString()).ToString("HH:mm") + " - " + DateTime.Parse(task.TaskEnd.ToString()).ToString("HH:mm"),
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
            
            Window window = Window.GetWindow(this);
            window.Close();
            
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = clCalendar.SelectedDate ?? DateTime.Today;
            if (itemList != null)
            {
                LoadDataFromDatabase(selectedDate);

                listView.ItemsSource = null;
                listView.ItemsSource = itemList;
            }
            

        }
        private void Button_Click_2020(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayYear(2020);
        }

        private void Button_Click_2021(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayYear(2021);
        }

        private void Button_Click_2022(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayYear(2022);
        }
        private void Button_Click_2023(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayYear(2023);
        }
        private void Button_Click_2024(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayYear(2024);
        }

        private void SetCalendarDisplayYear(int year)
        {
            // Kiểm tra xem năm có nằm trong khoảng hợp lệ của Calendar hay không (thay thế bằng khoảng năm bạn muốn hỗ trợ)
            if (year >= 1900 && year <= 2100)
            {
                // Đặt năm hiển thị cho Calendar
                clCalendar.DisplayDate = new DateTime(year, 1, 1);
            }
            else
            {
                // Xử lý khi năm không hợp lệ
                // Ví dụ: Hiển thị thông báo lỗi
            }
        }
        private void SetCalendarDisplayMonth(int month)
        {
            // Kiểm tra xem tháng có nằm trong khoảng hợp lệ của Calendar (1-12) hay không
            if (month >= 1 && month <= 12)
            {
                // Lấy năm hiện tại từ Calendar
                int currentYear = clCalendar.DisplayDate.Year;

                // Đặt ngày hiển thị cho Calendar (ngày 1 của tháng được chọn)
                clCalendar.DisplayDate = new DateTime(currentYear, month, 1);
            }
            else
            {
                
            }
        }

        private void btnM7_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(7);
        }

        private void btnM6_Click(object sender, RoutedEventArgs e)
        {
           SetCalendarDisplayMonth(6);
            
        }

        private void btnM1_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(1);
        }

        private void btnM2_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(2);
        }

        private void btnM3_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(3);
        }

        private void btnM4_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(4);
        }

        private void btnM5_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(5);
        }

        private void btnM8_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(8);
        }

        private void btnM9_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(9);
        }

        private void btnM10_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(10);
        }

        private void btnM11_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(11);
        }

        private void btnM12_Click(object sender, RoutedEventArgs e)
        {
            SetCalendarDisplayMonth(12);
        }
    }
}

