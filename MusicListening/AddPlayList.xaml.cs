using Microsoft.EntityFrameworkCore;
using ProDiaryApplication.CloneModels;
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
using System.Windows.Shapes;

namespace ProDiaryApplication.MusicListening
{
    /// <summary>
    /// Interaction logic for AddPlayList.xaml
    /// </summary>
    public partial class AddPlayList : Window
    {
        List<Song> songList = new List<Song>();
        List<Song> PlayLists = new List<Song>();

        public AddPlayList(Models.Account User)
        {
            var context = new CloneModels.DiaryNoteContext();

            songList = context.Songs.Include(e => e.AuthorNavigation).ToList();
            InitializeComponent();
            cmbSongs.ItemsSource = songList;
            cmbSongs.Items.Refresh();
            DataContext = this; // Set DataContext to the window itself

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Song selectedSong = cmbSongs.SelectedItem as Song;

            // Check if a song is selected
            if (selectedSong != null)
            {
                // Add the selected song to the playlist (PlayLists) list
                PlayLists.Add(selectedSong);

                // Refresh the ListBox to display the updated playlist
                lbSongs.ItemsSource = null; // Clear the existing items source
                lbSongs.ItemsSource = PlayLists; // Set the updated playlist as the new items source
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Song selectedSong = lbSongs.SelectedItem as Song;

            // Check if a song is selected
            if (selectedSong != null)
            {
                // Remove the selected song from the playlist (PlayLists) list
                PlayLists.Remove(selectedSong);

                // Refresh the ListBox to reflect the updated playlist
                lbSongs.ItemsSource = null; // Clear the existing items source
                lbSongs.ItemsSource = PlayLists; // Set the updated playlist as the new items source
            }

        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string playlistName = txtPlaylistName.Text;
            if (string.IsNullOrEmpty(playlistName))
            {
                MessageBox.Show("Playlist name cannot be empty.");
                return;
            }

            // Create a new Playlist object
            PlayList newPlaylist = new PlayList
            {
                PlayListName = playlistName,
                // Set the Owner ID if applicable (you need to get the owner ID from somewhere)
                // Owner = ownerId
            };
            if (PlayLists.Count < 1) // Change the condition based on your requirements
            {
                MessageBox.Show("You need to add at least one songs to create a playlist.");
                return;
            }

            // Add the new playlist to the database context
            var context = new CloneModels.DiaryNoteContext();
            context.PlayLists.Add(newPlaylist);

            // Save changes to the database
            try
            {
                context.SaveChanges();

                // Associate selected songs from PlayLists list with the newly created playlist
                var selectedSongs = PlayLists.Select(song => new PlayListSong
                {
                    SongId = song.Id,
                    PlayListId = newPlaylist.Id
                }).ToList();

                // Add the associated songs to the database context
                context.PlayListSongs.AddRange(selectedSongs);
                context.SaveChanges();

                // Display a success message (optional)
                MessageBox.Show("Playlist created successfully!");

                // Clear the PlayLists list and refresh the ListBox
                PlayLists.Clear();
                lbSongs.Items.Refresh();
                var mainWindow = Application.Current.Windows.OfType<MusicManagement>().FirstOrDefault();
                this.Close();

                if (mainWindow != null)
                {
                    mainWindow.ReloadData();
                }
                // Clear the playlist name TextBox
                txtPlaylistName.Text = string.Empty;
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during database operations
                MessageBox.Show($"Error creating playlist: {ex.Message}");
            }
        }

    }
}

