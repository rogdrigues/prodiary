using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProDiaryApplication.CloneModels;
using ProDiaryApplication.Models;

namespace ProDiaryApplication.MusicListening
{
    /// <summary>
    /// Interaction logic for AddSongDialog.xaml
    /// </summary>
    public partial class AddSongDialog : Window
    {
        public AddSongDialog(Models.Account User)
        {
            CurrentUser = User;
            var context = new CloneModels.DiaryNoteContext();
            List<Singer> singers = context.Singers.ToList();

            InitializeComponent();
            Author.ItemsSource = singers;
        }
        public string TitleValue { get; set; }
        public string TimeValue { get; set; }
        public string ImageURLValue { get; set; }
        public string LinkToFileValue { get; set; }
        public int IDValue { get; set; }
        public int? CategoryValue { get; set; }
        public int? StatusValue { get; set; }
        public int AuthorID { get; set; }

        // AddSongDialog constructor
        public AddSongDialog(Models.Account user, string title, string time, string imageURL, string linkToFile, int id, int? category, int status, int authorID)
        {
            CurrentUser = user;
            TitleValue = title;
            TimeValue = time;
            ImageURLValue = imageURL;
            LinkToFileValue = linkToFile;
            IDValue = id;
            CategoryValue = category;
            StatusValue = status;
            AuthorID = authorID;
            InitializeComponent();
            // Rest of your existing code
        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            var context = new CloneModels.DiaryNoteContext();
            try
            {
                if (CategoryValue != null)
                    Category.SelectedIndex = Convert.ToInt32(CategoryValue) - 1;
                if (StatusValue != null)
                    Status.SelectedIndex = Convert.ToInt32(StatusValue) - 1;
                List<Singer> singers = context.Singers.ToList();
                Author.ItemsSource = singers;
                if (AuthorID != null)
                {
                    Singer defaultAuthor = context.Singers.FirstOrDefault(s => s.Id == AuthorID);
                    if (defaultAuthor != null)
                    {
                        Author.SelectedItem = defaultAuthor;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            
        }

        public int ID { get; set; }

        private async void  chooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                LinkToFile.Text = fileName;
                try
                {
                    // Get the duration of the audio file and update the "Time" TextBox
                    TimeSpan duration = await GetAudioFileDurationAsync(fileName);
                    Time.Text = duration.ToString(@"mm\:ss");

                    // Rest of the existing code...
                }
                catch (Exception error)
                {
                    MessageBox.Show("Cannot import list employee");
                }
            }
        }

        private async Task<TimeSpan> GetAudioFileDurationAsync(string filePath)
        {
            try
            {
                MediaPlayer mediaPlayer = new MediaPlayer();
                TaskCompletionSource<TimeSpan> tcs = new TaskCompletionSource<TimeSpan>();

                mediaPlayer.MediaOpened += (sender, e) =>
                {
                    tcs.SetResult(mediaPlayer.NaturalDuration.TimeSpan);
                };

                mediaPlayer.Open(new Uri(filePath));
                await tcs.Task;

                mediaPlayer.Close(); // Release resources after getting the duration
                return tcs.Task.Result;
            }
            catch (Exception)
            {
                // Handle any exceptions related to media playback
                return TimeSpan.Zero;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        Models.Account CurrentUser = new Models.Account();

        private void addSong_Click(object sender, RoutedEventArgs e)
        {
            var context = new CloneModels.DiaryNoteContext();
            try
            {
                string selectedCategory = null;
                string selectedStatus = null;
                string selectedAuthor = null;
                if (string.IsNullOrWhiteSpace(Title.Text) ||
                    Category.SelectedItem == null ||
                    Author.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(LinkToFile.Text))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (Category.SelectedItem is ComboBoxItem selectedItem)
                {
                    // Get the selected tag value
                    selectedCategory = selectedItem.Tag.ToString();
                    // Do something with the selected tag value
                }
                selectedAuthor = Author.SelectedValue.ToString();
                if (Status.SelectedItem is ComboBoxItem selectedSta)
                {
                    // Get the selected tag value
                    selectedStatus = selectedSta.Tag.ToString();
                    // Do something with the selected tag value
                }

                if (ID == null)
                {
                    // Add a new song
                    var newSong = new Song
                    {
                        Title = Title.Text,
                        Category = Convert.ToInt32(selectedCategory),
                        Author = Convert.ToInt32(selectedAuthor),
                        Time = Time.Text,
                        ImageUrl = ImageURL.Text,
                        LinkToFile = LinkToFile.Text,
                        Status = selectedStatus,
                        Owner = CurrentUser.Id,
                        // Set other properties as needed...
                    };

                    // Add the newSong to the context
                    context.Songs.Add(newSong);
                }
                else
                {
                    // Update the existing song
                    int songId = ID; // Assuming the ID is of type int

                    var existingSong = context.Songs.FirstOrDefault(s => s.Id == songId);
                    if (existingSong != null)
                    {
                        existingSong.Title = Title.Text;
                        existingSong.Category = Convert.ToInt32(selectedCategory);
                        existingSong.Author = Convert.ToInt32(selectedAuthor);
                        existingSong.Time = Time.Text;
                        existingSong.ImageUrl = ImageURL.Text;
                        existingSong.LinkToFile = LinkToFile.Text;
                        existingSong.Status = selectedStatus;
                        // Update other properties as needed...
                    }
                    else
                    {
                        MessageBox.Show("Song not found for updating.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Save the changes to the database
                context.SaveChanges();
                MessageBox.Show("Song added/updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                var mainWindow = Application.Current.Windows.OfType<MusicManagement>().FirstOrDefault();
                if (mainWindow != null)
                {
                    mainWindow.ReloadData();
                }

            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the save process
                MessageBox.Show($"An error occurred while adding/updating the song:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
