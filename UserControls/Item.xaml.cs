﻿using Microsoft.EntityFrameworkCore;
using ProDiaryApplication.MenuItem;
using ProDiaryApplication.Models;
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
    /// Interaction logic for Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        public Item()
        {
            InitializeComponent();
             
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Item));


        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(string), typeof(Item));


        public SolidColorBrush Color
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(SolidColorBrush), typeof(Item));


        public FontAwesome.WPF.FontAwesomeIcon Icon
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(Item));


        public FontAwesome.WPF.FontAwesomeIcon IconBell
        {
            get { return (FontAwesome.WPF.FontAwesomeIcon)GetValue(IconBellProperty); }
            set { SetValue(IconBellProperty, value); }
        }

        public static readonly DependencyProperty IconBellProperty = DependencyProperty.Register("IconBell", typeof(FontAwesome.WPF.FontAwesomeIcon), typeof(Item));


        
        public int Id { get; set; }
        private void Delete(object sender, MouseButtonEventArgs e)
        {
            
            var context = new DiaryNoteContext();
            var title = Title.ToString();
            
            var tasktitle = context.Tasks.FirstOrDefault(x => x.TaskTitle == title); 
            context.Tasks.Remove(tasktitle);
            context.SaveChanges();
            Window window = Window.GetWindow(this);
            window.Close();
            MenuItem.Task task1 = new MenuItem.Task();
            task1.Show();
            MessageBox.Show("Xoa thanh cong");

        }
        

        private void UpdateTask(object sender, MouseButtonEventArgs e)
        {
            var context = new DiaryNoteContext();
            var title = Title.ToString();
            EditTask eTask = new EditTask();
            eTask.a = title;
            eTask.Show();
        }
    }
}
