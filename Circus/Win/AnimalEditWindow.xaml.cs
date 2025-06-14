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
    /// Логика взаимодействия для AnimalEditWindow.xaml
    /// </summary>
    public partial class AnimalEditWindow : Window
    {
        private Animals _animal;
        public AnimalEditWindow()
        {
            InitializeComponent();
            LoadTrainers();
        }
        public AnimalEditWindow(Animals animal) : this()
        {
            _animal = animal;
            LoadAnimalData();
        }
        private void LoadTrainers()
        {
            TrainerComboBox.ItemsSource = DB.circus.Trainers.ToList();
            if (TrainerComboBox.Items.Count > 0)
                TrainerComboBox.SelectedIndex = 0;
        }
        private void LoadAnimalData()
        {
            if (_animal != null)
            {
                NameTextBox.Text = _animal.Name;
                SpeciesTextBox.Text = _animal.Species;
                AgeTextBox.Text = _animal.Age.ToString();
                GenderComboBox.SelectedItem = GenderComboBox.Items .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == _animal.Gender);
                TrainerComboBox.SelectedItem = DB.circus.Trainers
                    .FirstOrDefault(t => t.TrainerID == _animal.TrainerID);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(SpeciesTextBox.Text) ||  string.IsNullOrWhiteSpace(AgeTextBox.Text) ||
                    GenderComboBox.SelectedItem == null ||
                    TrainerComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все обязательные поля", "Ошибка",  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(AgeTextBox.Text, out int age))
                {
                    MessageBox.Show("Введите корректный возраст", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var selectedTrainer = (Trainers)TrainerComboBox.SelectedItem;
                var selectedGender = ((ComboBoxItem)GenderComboBox.SelectedItem).Content.ToString();
                if (_animal == null)
                {
                    _animal = new Animals
                    {
                        Name = NameTextBox.Text,
                        Species = SpeciesTextBox.Text,
                        Age = age,
                        Gender = selectedGender,
                        TrainerID = selectedTrainer.TrainerID
                    };
                    DB.circus.Animals.Add(_animal);
                }
                else
                {
                    _animal.Name = NameTextBox.Text;
                    _animal.Species = SpeciesTextBox.Text;
                    _animal.Age = age;
                    _animal.Gender = selectedGender;
                    _animal.TrainerID = selectedTrainer.TrainerID;
                }

                DB.circus.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
