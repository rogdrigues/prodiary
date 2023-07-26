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
using ProDiaryApplication.Models;
using Xceed.Wpf.Toolkit;

namespace ProDiaryApplication.MenuItem
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        public AddTask()
        {
            InitializeComponent();
        }
        public Account? CurrentUser { get; set; }

        
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                var context = new DiaryNoteContext();
                Models.Task task = new Models.Task
                {
                    TaskTitle = txtTaskTitle.Text,
                    TaskContent = txtTaskContent.Text,
                    TaskBegin = tpBegin.Value,
                    TaskEnd = tpEnd.Value,
                    TaskDate = dpDate.SelectedDate,
                    Id = int.Parse(CurrentUser.Id.ToString())
                };
                
                context.Tasks.Add(task);
                if (context.SaveChanges() > 0)
                {
                    System.Windows.MessageBox.Show("Add Task Success");
                    Window window = Window.GetWindow(this);
                    window.Close();
                    MenuItem.Task task1 = new MenuItem.Task();
                    task1.Show();
                    
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Please enter correct information");
            }
        }
      
    }
}

