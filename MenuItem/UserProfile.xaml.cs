using Microsoft.Win32;
using ProDiaryApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        public Account? CurrentUser { get; set; }

        public UserProfile()
        {
            InitializeComponent();
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
                txtEmail.Text = CurrentUser.Email;
                txtFullname.Text = CurrentUser.FullName;
                txtUsername.Text = CurrentUser.Username;
            }
        }

        private void btnBrowserFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    imgAvatar.Source = new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    MessageBox.Show("You haven't selected an image file.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataUser();
        }

        private void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                string fullName = txtFullname.Text;

                if (string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Please enter all required information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Account? loggedInAccount = context.Accounts.SingleOrDefault(a => a.Id == CurrentUser.Id);

                byte[] avatarData;
                BitmapSource avatarSource = (BitmapSource)imgAvatar.Source;
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(avatarSource));
                    encoder.Save(stream);
                    avatarData = stream.ToArray();
                }

                loggedInAccount.FullName = fullName;
                loggedInAccount.Avatar = avatarData;

                context.Accounts.Update(loggedInAccount);

                var memosToUpdate = context.Memos.Where(m => m.MemoAuthor == loggedInAccount.FullName);
                foreach (var memo in memosToUpdate)
                {
                    memo.MemoAuthor = fullName;
                    context.Memos.Update(memo);
                }

                context.SaveChanges();

                MessageBox.Show("Update successful.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
 }
