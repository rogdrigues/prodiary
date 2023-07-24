using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using Microsoft.EntityFrameworkCore;
using ProDiaryApplication.CloneModels;
using ProDiaryApplication.Models;
using System.Windows.Media;
using System.Windows.Threading;
using ProDiaryApplication.UserControls;

namespace ProDiaryApplication.MusicListening
{
    /// <summary>
    /// Interaction logic for MusicManagement.xaml
    /// </summary>
    public partial class MusicManagement : Window
    {
        
        public MusicManagement(Models.Account User)
        {
            var context = new CloneModels.DiaryNoteContext();
            foreach (var item in context.Songs.Include(e => e.AuthorNavigation))
            {
                songList.Add(new DisplaySong { Author = item.AuthorNavigation.SingerName, Title = item.Title, Time = item.Time, LinkToFile = item.LinkToFile, ID = item.Id + "", IsClicked = false, Owner = item.Owner, ImageURL = item.ImageUrl, Category = item.Category, Status = item.Status, AuthorID = Convert.ToInt32(item.Author) }); ;
            }
            foreach (var item in context.Singers)
            {
                singerList.Add(new DisplaySinger { ID = item.Id+"", Name = item.SingerName, IsClicked = false });
            }
            CurrentUser = User;
            InitializeComponent();
            artistsList.ItemsSource = singerList;
            songListItems.ItemsSource = songList;
            songTimeSlider.ValueChanged += SongTimeSlider_ValueChanged;
            musicVolume.ValueChanged += MusicVolume_ValueChanged;

        }
        
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
            }
        }
        List<DisplaySong> songList = new List<DisplaySong>();
        List<DisplaySinger> singerList = new List<DisplaySinger>();
        Models.Account CurrentUser = new Models.Account();
        DisplaySong currentSong = new DisplaySong();
        private int currentSongIndex = -1; // Initialize to -1 to indicate no song is currently playing

        private void SongItem_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddSong_Click(object sender, RoutedEventArgs e)
        {
            var addSongDialog = new AddSongDialog(CurrentUser);
            addSongDialog.ShowDialog();

        }
        MediaPlayer musicPlayer = new MediaPlayer();

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (songListItems.SelectedItem is DisplaySong selectedItem)
            {
                try
                {
                    var selectedSong = (DisplaySong)songListItems.SelectedItem;
                    foreach (var item in songList)
                    {
                        item.IsClicked = (item.ID == selectedItem.ID); // Set IsClicked to true only for the selected item
                    }

                    // Refresh the ListBox to reflect the updated IsClicked values
                    songListItems.Items.Refresh();


                    if (musicPlayer != null)
                    {
                        // Stop the previous song
                        musicPlayer.Stop();
                        musicPlayer = null;
                    }

                    // Create a new MediaPlayer instance and play the selected song
                    musicPlayer = new MediaPlayer();
                    musicPlayer.MediaOpened += (sender, e) =>
                    {
                        // Media has been opened, so you can access NaturalDuration now
                        TimeSpan songDuration = musicPlayer.NaturalDuration.TimeSpan;
                        double songDurationInSeconds = songDuration.TotalSeconds;

                        // Update the slider to show the song progress
                        songTimeSlider.IsEnabled = true;
                        songTimeSlider.Minimum = 0;
                        songTimeSlider.Maximum = songDurationInSeconds;
                        songTimeSlider.Value = 0;

                        // Start a timer to update the slider continuously while the song is playing
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromMilliseconds(500);
                        timer.Tick += (s, ev) =>
                        {
                            songTimeSlider.Value = musicPlayer.Position.TotalSeconds;
                        };
                        timer.Start();
                    };

                    musicPlayer.Open(new Uri(selectedItem.LinkToFile));
                    musicPlayer.Play();
                    currentSong = selectedSong;
                    currentSongIndex = songList.FindIndex(s => s.ID == selectedItem.ID);
                    SongName.Text = selectedItem.Title;
                    Singer.Text = selectedItem.Author;
                }
                catch (Exception ex)
                {
                    // Handle the exception
                    MessageBox.Show($"An error occurred while playing the audio:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ListBoxArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (artistsList.SelectedItem is DisplaySinger selectedItem)
            {
                try
                {
                    var selectedSinger = (DisplaySinger)artistsList.SelectedItem;
                    foreach (var item in singerList)
                    {
                        item.IsClicked = (item.ID == selectedItem.ID); // Set IsClicked to true only for the selected item
                    }

                    // Refresh the ListBox to reflect the updated IsClicked values
                    artistsList.Items.Refresh();
                    songList = new List<DisplaySong>();
                    var context = new CloneModels.DiaryNoteContext();

                    foreach (var item in context.Songs.Where(s => s.Author+"" == selectedItem.ID).Include(e => e.AuthorNavigation))
                    {
                        songList.Add(new DisplaySong { Author = item.AuthorNavigation.SingerName, Title = item.Title, Time = item.Time, LinkToFile = item.LinkToFile, ID = item.Id + "", IsClicked = false  , Owner = item.Owner, ImageURL = item.ImageUrl, Category = item.Category, Status = item.Status, AuthorID = Convert.ToInt32(item.Author) }); ;
                    }
                    songListItems.ItemsSource = songList;
                    ArtistsName.Text = selectedSinger.Name;
                    currentSongIndex = songList.FindIndex(s => s.ID == selectedItem.ID);
                    songListItems.Items.Refresh();




                }
                catch (Exception ex)
                {
                    // Handle the exception
                    MessageBox.Show($"An error occurred while playing the audio:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Item_Click(object sender, MouseButtonEventArgs e)
        {
            // Get the clicked Border element
            Border clickedBorder = sender as Border;

            // Set the background color to #03bf69
            clickedBorder.Background = new SolidColorBrush(Color.FromRgb(3, 191, 105));
        }
        private void SongTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Check if the media player is initialized and currently playing
            if (musicPlayer != null && musicPlayer.NaturalDuration.HasTimeSpan)
            {
                // Get the new slider value (position in seconds)
                double newPositionInSeconds = e.NewValue;

                // Set the media player's position to the new value
                musicPlayer.Position = TimeSpan.FromSeconds(newPositionInSeconds);
            }
        }
        private void MusicVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Check if the media player is initialized
            if (musicPlayer != null)
            {
                // Get the new volume value from the slider (values range from 0 to 1)
                double newVolume = e.NewValue / musicVolume.Maximum;

                // Set the volume of the media player to the new value
                musicPlayer.Volume = newVolume;
            }
        }
        private bool isPlaying = true;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (musicPlayer != null && isPlaying)
            {
                musicPlayer.Pause();
                isPlaying = false;
                playBtn.Visibility = Visibility.Visible;
                pauseBtn.Visibility = Visibility.Collapsed;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (musicPlayer != null && !isPlaying)
            {
                musicPlayer.Play();
                isPlaying = true;
                playBtn.Visibility = Visibility.Collapsed;
                pauseBtn.Visibility = Visibility.Visible;
            }

        }

        private void SkipNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if there is a next song to play
                if (currentSongIndex < songList.Count - 1)
                {
                    currentSongIndex++;

                    // Get the next song to play from the songList
                    DisplaySong nextSong = songList[currentSongIndex];
                    foreach (var item in songList)
                    {
                        item.IsClicked = (item.ID == nextSong.ID);
                    }
                    songListItems.Items.Refresh();
                    // Stop the previous song, if playing
                    if (musicPlayer != null)
                    {
                        musicPlayer.Stop();
                        musicPlayer = null;
                    }

                    // Create a new MediaPlayer instance and play the next song
                    musicPlayer = new MediaPlayer();
                    musicPlayer.MediaOpened += (sender, e) =>
                    {
                        // Rest of your existing code...
                    };

                    musicPlayer.Open(new Uri(nextSong.LinkToFile));
                    musicPlayer.Play();

                    // Update the song information on the UI
                    currentSong = nextSong;
                    SongName.Text = nextSong.Title;
                    Singer.Text = nextSong.Author;

                    // Highlight the next song in the ListBox
                   
                }
                else
                {
                    // If there is no next song, you can show a message or handle it as needed
                    MessageBox.Show("No next song available.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show($"An error occurred while playing the audio:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SkipPrevious_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if there is a previous song to play
                if (currentSongIndex > 0)
                {
                    currentSongIndex--;

                    // Get the previous song to play from the songList
                    DisplaySong previousSong = songList[currentSongIndex];
                    foreach (var item in songList)
                    {
                        item.IsClicked = (item.ID == previousSong.ID);
                    }
                    songListItems.Items.Refresh();

                    // Stop the previous song, if playing
                    if (musicPlayer != null)
                    {
                        musicPlayer.Stop();
                        musicPlayer = null;
                    }

                    // Create a new MediaPlayer instance and play the previous song
                    musicPlayer = new MediaPlayer();
                    musicPlayer.MediaOpened += (sender, e) =>
                    {
                        // Rest of your existing code...
                    };

                    musicPlayer.Open(new Uri(previousSong.LinkToFile));
                    musicPlayer.Play();

                    // Update the song information on the UI
                    currentSong = previousSong;
                    SongName.Text = previousSong.Title;
                    Singer.Text = previousSong.Author;

                    // Highlight the previous song in the ListBox
                }
                else
                {
                    // If there is no previous song, you can show a message or handle it as needed
                    MessageBox.Show("No previous song available.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show($"An error occurred while playing the audio:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                songList = new List<DisplaySong>();
                var context = new CloneModels.DiaryNoteContext();

                foreach (var item in context.Songs.Where(s => s.Owner == CurrentUser.Id).Include(e => e.AuthorNavigation))
                {
                    songList.Add(new DisplaySong { Author = item.AuthorNavigation.SingerName, Title = item.Title, Time = item.Time, LinkToFile = item.LinkToFile, ID = item.Id + "", IsClicked = false, Owner = item.Owner, ImageURL = item.ImageUrl, Category = item.Category, Status = item.Status, AuthorID = Convert.ToInt32(item.Author) }); ;
                }
                foreach (var item in singerList)
                {
                    item.IsClicked = false; // Set IsClicked to true only for the selected item
                }
                ArtistsName.Text = "All Artists";
                artistsList.ItemsSource = null;
                artistsList.ItemsSource = singerList;
                songListItems.ItemsSource = songList;
                ArtistsName.Text = "Your Song";
                songListItems.Items.Refresh();




            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show($"An error occurred while playing the audio:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            var context = new CloneModels.DiaryNoteContext();
            songList = new List<DisplaySong>();

            foreach (var item in context.Songs.Include(e => e.AuthorNavigation))
            {
                songList.Add(new DisplaySong { Author = item.AuthorNavigation.SingerName, Title = item.Title, Time = item.Time, LinkToFile = item.LinkToFile, ID = item.Id + "", IsClicked = false, Owner = item.Owner, ImageURL = item.ImageUrl, Category = item.Category, Status = item.Status, AuthorID = Convert.ToInt32(item.Author) }); ;
            }
            foreach (var item in singerList)
            {
                item.IsClicked = false; // Set IsClicked to true only for the selected item
            }
            ArtistsName.Text = "All Artists";
            artistsList.ItemsSource = null;
            artistsList.ItemsSource = singerList;
            songListItems.ItemsSource = null;
            songListItems.ItemsSource = songList;

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var addSongDialog = new AddSongDialog(CurrentUser, currentSong.Title, currentSong.Time, currentSong.ImageURL, currentSong.LinkToFile, Convert.ToInt32(currentSong.ID), currentSong.Category, Convert.ToInt32(currentSong.Status), Convert.ToInt32(currentSong.AuthorID));
            if (currentSong.Owner == CurrentUser.Id)
            {
                
                    addSongDialog.Time.Text = currentSong.Time.ToString();
                    addSongDialog.Title.Text = currentSong.Title.ToString();
                    addSongDialog.ImageURL.Text = currentSong.ImageURL.ToString();
                    addSongDialog.LinkToFile.Text = currentSong.LinkToFile.ToString();
                    addSongDialog.ID = Convert.ToInt32(currentSong.ID);
                
                addSongDialog.addSong.Content = "Update Song";
                            addSongDialog.ShowDialog();

            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void ReloadData()
        {
            var context = new CloneModels.DiaryNoteContext();
            songList.Clear();
            foreach (var item in context.Songs.Include(e => e.AuthorNavigation))
            {
                songList.Add(new DisplaySong { Author = item.AuthorNavigation.SingerName, Title = item.Title, Time = item.Time, LinkToFile = item.LinkToFile, ID = item.Id + "", IsClicked = false, Owner = item.Owner, ImageURL = item.ImageUrl, Category = item.Category, Status = item.Status, AuthorID = Convert.ToInt32(item.Author) });
            }

            singerList.Clear();
            foreach (var item in context.Singers)
            {
                singerList.Add(new DisplaySinger { ID = item.Id + "", Name = item.SingerName, IsClicked = false });
            }

            songListItems.ItemsSource = null;
            songListItems.ItemsSource = songList;
            artistsList.ItemsSource = null;
            artistsList.ItemsSource = singerList;
        }

    }
}
