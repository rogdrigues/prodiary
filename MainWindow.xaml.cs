using ProDiaryApplication.MenuItem;
using ProDiaryApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;
using Path = System.IO.Path;

namespace ProDiaryApplication
{
    public partial class MainWindow : Window
    {
        public Account? CurrentUser { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timeUpdate();
            loadDataUser();
        }

        private void loadDataUser()
        {
            if (CurrentUser != null)
            {
                byte[]? avatarData = CurrentUser.Avatar;
                if (avatarData != null && avatarData.Length > 0)
                {
                    using (MemoryStream stream = new MemoryStream(avatarData))
                    {
                        BitmapImage avatarImage = new BitmapImage();
                        avatarImage.BeginInit();
                        avatarImage.StreamSource = stream;
                        avatarImage.CacheOption = BitmapCacheOption.OnLoad;
                        avatarImage.EndInit();

                        imgAvatar.Source = avatarImage;
                    }
                }

                txtUserName.Text = CurrentUser.Username;

                string userID = $"{CurrentUser.Username.Substring(0, 3)}#{CurrentUser.Id.ToString("D4")}";
                txtUserID.Text = userID;
            }
        }
        [DebuggerHidden]
        private void timeUpdate()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new System.TimeSpan(0, 0, 1);
            timer.Tick += time_update;
            timer.Start();
        }
        [DebuggerHidden]
        void time_update(object? sender, EventArgs e)
        {
            this.lblTimeCurrent.Content = System.DateTime.Now.ToString("HH:mm:ss");
            this.lblDate.Content = System.DateTime.Now.ToString("D");
        }

        private int currentThemeIndex = 1;

        private void imgBtnLeft_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentThemeIndex--;

            string currentDirectory = Directory.GetCurrentDirectory();
            string? projectDirectory = null;

            while (!string.IsNullOrEmpty(currentDirectory))
            {
                if (Path.GetFileName(currentDirectory) == "ProDiaryApplication")
                {
                    projectDirectory = currentDirectory;
                    break;
                }

                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }

            if (string.IsNullOrEmpty(projectDirectory))
            {
                return;
            }

            string themeFolderPath = Path.Combine(projectDirectory, "Resource", "Theme") + Path.DirectorySeparatorChar;

            string[] themeFiles = Directory.GetFiles(themeFolderPath);

            string themeFileName = currentThemeIndex.ToString("D2");
            string themeFilePath = FindNextThemeFile(themeFiles, themeFileName);

            if (string.IsNullOrEmpty(themeFilePath))
            {
                return;
            }

            LoadThemeImage(themeFilePath);
        }

        private void imgBtnRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            currentThemeIndex++;

            string? currentDirectory = Directory.GetCurrentDirectory();
            string? projectDirectory = null;

            while (!string.IsNullOrEmpty(currentDirectory))
            {
                if (Path.GetFileName(currentDirectory) == "ProDiaryApplication")
                {
                    projectDirectory = currentDirectory;
                    break;
                }

                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }

            if (string.IsNullOrEmpty(projectDirectory))
            {
                return;
            }

            string themeFolderPath = Path.Combine(projectDirectory, "Resource", "Theme") + Path.DirectorySeparatorChar;

            string[] themeFiles = Directory.GetFiles(themeFolderPath);

            string themeFileName = currentThemeIndex.ToString("D2");
            string themeFilePath = FindNextThemeFile(themeFiles, themeFileName);

            if (string.IsNullOrEmpty(themeFilePath))
            {
                return;
            }

            LoadThemeImage(themeFilePath);
        }

        private string GetThemeDescriptionFromFileName(string fileName)
        {
            int lastIndexOfNumber = fileName.Length - 2;
            fileName = fileName.Substring(0, lastIndexOfNumber).Trim();

            string[] parts = fileName.Split(new char[] { '_', '.' });
            string themeDescription = "";

            foreach (string part in parts)
            {
                if (!string.IsNullOrEmpty(part))
                {
                    string formattedPart = char.ToUpper(part[0]) + part.Substring(1).ToLower();
                    themeDescription += formattedPart + " ";
                }
            }

            themeDescription = currentThemeIndex.ToString("D2") + " - " + themeDescription.Trim();

            return themeDescription;
        }

        private string FindNextThemeFile(string[] themeFiles, string themeFileName)
        {
            string? themeFilePath = null;

            foreach (string file in themeFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);

                if (fileName.EndsWith(themeFileName) && (file.EndsWith(".gif") || file.EndsWith(".png") || file.EndsWith(".jpg")))
                {
                    themeFilePath = file;
                    break;
                }
            }

            return themeFilePath;
        }

        private void LoadThemeImage(string imagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            string themeDescription = GetThemeDescriptionFromFileName(Path.GetFileNameWithoutExtension(imagePath));
            lblThemeName.Content = themeDescription;


            windowBG.ImageSource = bitmapImage;
            imgThemeAlt.Source = bitmapImage;
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Memos memos = new Memos();
            memos.CurrentUser = CurrentUser;
            memos.Show();
        }
    }
}
