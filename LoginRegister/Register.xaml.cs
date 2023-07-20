﻿using Microsoft.Win32;
using ProDiaryApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ProDiaryApplication
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            using (DiaryNoteContext context = new DiaryNoteContext())
            {
                string username = txtUserName.Text;
                string password = pswPassWord.Password;
                string email = txtEmail.Text;
                string fullName = txtFullName.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
                if (!Regex.IsMatch(password, passwordPattern))
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự, bao gồm ít nhất một chữ cái viết thường, một chữ cái viết hoa và một số", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, emailPattern))
                {
                    MessageBox.Show("Email không đúng định dạng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (imgAvatar.Source == null)
                {
                    MessageBox.Show("Vui lòng chọn Avatar", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                byte[] avatarData;
                BitmapSource avatarSource = (BitmapSource)imgAvatar.Source;
                using (MemoryStream stream = new MemoryStream())
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(avatarSource));
                    encoder.Save(stream);
                    avatarData = stream.ToArray();
                }

                Account newAccount = new Account
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    FullName = fullName,
                    Avatar = avatarData
                };

                context.Accounts.Add(newAccount);
                context.SaveChanges();

                MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    MessageBox.Show("Bạn chưa chọn file ảnh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            Login login = new Login();
            login.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
