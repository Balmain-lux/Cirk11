using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Circus.DBconn;

namespace Circus.Win
{
    /// <summary>
    /// Логика взаимодействия для Artists.xaml
    /// </summary>
    public partial class Artists : Window
    {
        private DBconn.Artists selectedArtist;
        public ObservableCollection<DBconn.Artists> ArtistsCollection { get; set; }

        public Artists()
        {
            InitializeComponent();
            DataContext = this;
            LoadArtists();
            cbTypeFilter.SelectedIndex = 0;
        }

        private void LoadArtists()
        {
            try
            {
                ArtistsCollection = new ObservableCollection<DBconn.Artists>(DB.circus.Artists.ToList());
                dgArtists.ItemsSource = ArtistsCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке артистов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateTypes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
             
                DB.circus.SaveChanges();
                LoadArtists();

                MessageBox.Show("Типы артистов успешно обновлены", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении типов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgArtists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedArtist = dgArtists.SelectedItem as DBconn.Artists;
        }

        private void btnAddArtist_Click(object sender, RoutedEventArgs e)
        {
            var addArtist = new AddEditArtistWindow();

            if (addArtist.ShowDialog() == true)
            {
                try
                {
                    DB.circus.Artists.Add(addArtist.Artist);
                    DB.circus.SaveChanges();
                    LoadArtists();

                    MessageBox.Show("Артист успешно добавлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnEditArtist_Click(object sender, RoutedEventArgs e)
        {
            if (selectedArtist == null)
            {
                MessageBox.Show("Выберите артиста для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var editArtist = new AddEditArtistWindow(selectedArtist);

            if (editArtist.ShowDialog() == true)
            {
                try
                {
                    DB.circus.SaveChanges();
                    LoadArtists();

                    MessageBox.Show("Изменения сохранены успешно", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnDeleteArtist_Click(object sender, RoutedEventArgs e)
        {
            if (selectedArtist == null)
            {
                MessageBox.Show("Выберите артиста для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Удалить артиста {selectedArtist.FullName}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.circus.Artists.Remove(selectedArtist);
                    DB.circus.SaveChanges();
                    LoadArtists();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                if (string.IsNullOrEmpty(searchText))
                {
                    dgArtists.ItemsSource = ArtistsCollection.ToList();
                    return;
                }

                dgArtists.ItemsSource = ArtistsCollection
                    .Where(a => a.FullName != null &&
                              a.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}");
                dgArtists.ItemsSource = ArtistsCollection.ToList();
            }
        }

        private void cbTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyTypeFilter();
        }

        private void ApplyTypeFilter()
        {
            try
            {
                if (ArtistsCollection == null || !ArtistsCollection.Any()) return;

                ComboBoxItem selectedItem = cbTypeFilter.SelectedItem as ComboBoxItem;
                string filterType = "All";

                if (selectedItem != null)
                {
                    switch (selectedItem.Content.ToString())
                    {
                        case "начинающий":
                            filterType = "Beginner";
                            break;
                        case "продвигающийся":
                            filterType = "Promoting";
                            break;
                        case "VIP":
                            filterType = "VIP";
                            break;
                        default:
                            filterType = "All";
                            break;
                    }
                }

               var filtered = filterType == "All"
                    ? ArtistsCollection.ToList()
                    : ArtistsCollection.Where(a => a.Type != null && a.Type.Equals(filterType, StringComparison.OrdinalIgnoreCase)) .ToList();
                dgArtists.ItemsSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}");
                dgArtists.ItemsSource = ArtistsCollection?.ToList() ?? new List<DBconn.Artists>();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}