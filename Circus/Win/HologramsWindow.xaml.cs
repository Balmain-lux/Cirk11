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
    /// Логика взаимодействия для HologramsWindow.xaml
    /// </summary>
    public partial class HologramsWindow : Window
    {
        private List<Holograms> allHolograms;
        private DBconn.Holograms selectedHologram;

        public HologramsWindow()
        {
            InitializeComponent();
            LoadHolograms();
        }
        private void LoadHolograms()
        {
            try
            {
                allHolograms = DB.circus.Holograms.Include("Employees").ToList();
                dgHolograms.ItemsSource = allHolograms;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке голограмм: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtered = allHolograms.AsQueryable();
                if (cbStageFilter.SelectedItem is ComboBoxItem selectedStage &&
                    selectedStage.Content.ToString() != "все")
                {
                    string stage = selectedStage.Content.ToString();
                    filtered = filtered.Where(h => h.DevelopmentStage == stage);
                }
                if (!string.IsNullOrWhiteSpace(txtCabinetFilter.Text) &&
                    int.TryParse(txtCabinetFilter.Text, out int cabinet))
                {
                    filtered = filtered.Where(h => h.CabinetNumber == cabinet);
                }
                dgHolograms.ItemsSource = filtered.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            cbStageFilter.SelectedIndex = 0;
            txtCabinetFilter.Text = "";
            dgHolograms.ItemsSource = allHolograms;
        }
        private void dgHolograms_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedHologram = dgHolograms.SelectedItem as DBconn.Holograms;
        }
  
        private void btnAddHologram_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditHologramWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    DB.circus.Holograms.Add(addWindow.Hologram);
                    DB.circus.SaveChanges();
                    LoadHolograms();
                    MessageBox.Show("Голограмма успешно добавлена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void btnEditHologram_Click(object sender, RoutedEventArgs e)
        {
            if (selectedHologram == null)
            {
                MessageBox.Show("Выберите голограмму для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var editWindow = new AddEditHologramWindow(selectedHologram);
            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    DB.circus.SaveChanges();
                    LoadHolograms();
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
        private void btnDeleteHologram_Click(object sender, RoutedEventArgs e)
        {
            if (selectedHologram == null)
            {
                MessageBox.Show("Выберите голограмму для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную голограмму?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.circus.Holograms.Remove(selectedHologram);
                    DB.circus.SaveChanges();
                    LoadHolograms();
                    MessageBox.Show("Голограмма успешно удалена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}