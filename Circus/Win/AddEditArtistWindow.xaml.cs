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
using Circus.DBconn;

namespace Circus.Win
{
    /// <summary>
    /// Логика взаимодействия для AddEditArtistWindow.xaml
    /// </summary>
    public partial class AddEditArtistWindow : Window
    {
        public DBconn.Artists    Artist { get; private set; }
        private bool isEditMode = false;

        public List<string> ArtistTypes { get; } = new List<string> { "Beginner", "Promoting", "VIP" };

        public AddEditArtistWindow()
        {
            InitializeComponent();
            InitializeWindow(null);
        }

        public AddEditArtistWindow(DBconn.Artists artistToEdit)
        {
            InitializeComponent();
            InitializeWindow(artistToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(DBconn.Artists artist)
        {
            DataContext = this;

            if (artist != null)
            {
                Artist = artist;
                txtFullName.Text = artist.FullName;
                txtSuccessfulPerformances.Text = artist.SuccessfulPerformances.ToString();
                cmbType.SelectedItem = artist.Type;
                txtDressingRoom.Text = artist.DressingRoom;
            }
            else
            {
                Artist = new DBconn.Artists();
                cmbType.SelectedIndex = 0; 
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Введите полное имя артиста!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtSuccessfulPerformances.Text, out int performances) || performances < 0)
                {
                    MessageBox.Show("Введите корректное количество успешных выступлений (положительное число)!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Artist.FullName = txtFullName.Text.Trim();
                Artist.SuccessfulPerformances = performances;
                Artist.Type = cmbType.SelectedItem.ToString();

                if (Artist.Type == "VIP")
                {
                    Artist.DressingRoom = txtDressingRoom.Text.Trim();
                }
                else
                {
                    Artist.DressingRoom = null;
                }

                if (!isEditMode)
                {
                    DB.circus.Artists.Add(Artist);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void cmbType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbType.SelectedItem != null)
            {
                bool isVip = cmbType.SelectedItem.ToString() == "VIP";
                txtDressingRoom.IsEnabled = isVip;
                lblDressingRoom.IsEnabled = isVip;
            }
        }
    }
}