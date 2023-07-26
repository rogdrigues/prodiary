using System;
using System.Collections;
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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using ProDiaryApplication.Models;
namespace ProDiaryApplication.MenuItem
{
    /// <summary>
    /// Interaction logic for EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        public EditTask()
        {
            InitializeComponent();
            LoadDataFromDatabase();
        }
        public Models.Task uTask { get; set; }
        public String a;
        private void LoadDataFromDatabase()
        {
            
            

                var context = new DiaryNoteContext();

                var tasks = context.Tasks.FirstOrDefault(x => x.TaskTitle == "a");

               txtTaskTitle.Text = tasks.TaskTitle;
              txtTaskContent.Text= tasks.TaskContent;
               tpBegin.Value = tasks.TaskBegin;
              tpEnd.Value= tasks.TaskEnd;
              dpDate.SelectedDate = tasks.TaskDate;
                
                
                
            

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var context = new DiaryNoteContext();

                var tasks = context.Tasks.FirstOrDefault(x => x.TaskTitle == "a");

                tasks.TaskTitle = txtTaskTitle.Text;
                tasks.TaskContent = txtTaskContent.Text;
                tasks.TaskBegin = tpBegin.Value;
                tasks.TaskEnd = tpEnd.Value;
                tasks.TaskDate = dpDate.SelectedDate;
                context.Tasks.Update(tasks);

                if (context.SaveChanges() > 0)
                {
                    System.Windows.MessageBox.Show("Update Task Success");
                    Window window = Window.GetWindow(this);
                    window.Close();


                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Please enter correct information");
            }
        }
    }
}
