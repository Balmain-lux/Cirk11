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
    /// Логика взаимодействия для AnimalWindow.xaml
    /// </summary>
    public partial class AnimalWindow : Window
    {
        private List<Animals> animals;
        private List<Trainers> trainers;

        public AnimalWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                animals = DB.circus.Animals.ToList();
                trainers = DB.circus.Trainers.ToList();
                AnimalsGrid.ItemsSource = animals;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filteredAnimals = DB.circus.Animals.AsQueryable();
                if (!string.IsNullOrWhiteSpace(SpeciesFilter.Text))
                {
                    filteredAnimals = filteredAnimals.Where(a => a.Species.Contains(SpeciesFilter.Text));
                }
                if (GenderFilter.SelectedIndex > 0)
                {
                    var selectedGender = GenderFilter.SelectedItem as ComboBoxItem;
                    string gender = selectedGender.Content.ToString();
                    filteredAnimals = filteredAnimals.Where(a => a.Gender == gender);
                }
                AnimalsGrid.ItemsSource = filteredAnimals.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AnimalEditWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadData(); 
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsGrid.SelectedItem is Animals selectedAnimal)
            {
                var editWindow = new AnimalEditWindow(selectedAnimal);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData(); 
                }
            }
            else
            {
                MessageBox.Show("Выберите животное для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsGrid.SelectedItem is Animals selectedAnimal)
            {
                if (MessageBox.Show("Удалить выбранное животное?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DB.circus.Animals.Remove(selectedAnimal);
                        DB.circus.SaveChanges();
                        LoadData(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите животное для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowTrainersButton_Click(object sender, RoutedEventArgs e)
        {
            var trainersWindow = new TrainersWindow();
            trainersWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
