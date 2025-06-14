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
    /// Логика взаимодействия для EventsWindow.xaml
    /// </summary>
    public partial class EventsWindow : Window
    {
        private List<DBconn.Events> allEvents;
        private DBconn.Events selectedEvent;

        public EventsWindow()
        {
            InitializeComponent();
            LoadEvents();
        }

        private void LoadEvents()
        {
            try
            {
                allEvents = DB.circus.Events.ToList();
                dgEvents.ItemsSource = allEvents;
                UpdateStatusColumn();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке мероприятий: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateStatusColumn()
        {
            foreach (var ev in allEvents)
            {
                ev.NetIncome = ev.Profit - ev.Expenses;
            }
            dgEvents.Items.Refresh();
        }

        private void btnApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filtered = allEvents.AsQueryable();
                if (cbEventTypeFilter.SelectedItem is ComboBoxItem selectedType &&
                    selectedType.Content.ToString() != "Все")
                {
                    string type = selectedType.Content.ToString() == "частное" ? "Private" : "Guest";
                    filtered = filtered.Where(ev => ev.EventType == type);
                }
                if (cbStatusFilter.SelectedItem is ComboBoxItem selectedStatus &&
                    selectedStatus.Content.ToString() != "Все")
                {
                    bool isCompleted = selectedStatus.Content.ToString() == "Завершенные";
                    filtered = filtered.Where(ev => ev.IsCompleted == isCompleted);
                }
                if (dpDateFrom.SelectedDate != null)
                {
                    filtered = filtered.Where(ev => ev.EventDate >= dpDateFrom.SelectedDate);
                }

                if (dpDateTo.SelectedDate != null)
                {
                    var endDate = dpDateTo.SelectedDate.Value.AddDays(1);
                    filtered = filtered.Where(ev => ev.EventDate < endDate);
                }

                dgEvents.ItemsSource = filtered.ToList();
                UpdateStatusColumn();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            cbEventTypeFilter.SelectedIndex = 0;
            cbStatusFilter.SelectedIndex = 0;
            dpDateFrom.SelectedDate = null;
            dpDateTo.SelectedDate = null;
            dgEvents.ItemsSource = allEvents;
            UpdateStatusColumn();
        }

        private void dgEvents_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedEvent = dgEvents.SelectedItem as DBconn.Events;
        }

        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditEventWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    DB.circus.Events.Add(addWindow.Event);
                    DB.circus.SaveChanges();
                    LoadEvents();
                    MessageBox.Show("Мероприятие успешно добавлено", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnEditEvent_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEvent == null)
            {
                MessageBox.Show("Выберите мероприятие для редактирования", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var editWindow = new AddEditEventWindow(selectedEvent);
            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    DB.circus.SaveChanges();
                    LoadEvents();
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

        private void btnDeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEvent == null)
            {
                MessageBox.Show("Выберите мероприятие для удаления", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Удалить мероприятие '{selectedEvent.Name}'?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    DB.circus.Events.Remove(selectedEvent);
                    DB.circus.SaveChanges();
                    LoadEvents();
                    MessageBox.Show("Мероприятие успешно удалено", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void btnCompleteEvent_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEvent == null)
            {
                MessageBox.Show("Выберите мероприятие для завершения", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (selectedEvent.IsCompleted)
            {
                MessageBox.Show("Мероприятие уже завершено", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Завершить мероприятие '{selectedEvent.Name}'?",
                "Подтверждение завершения",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    selectedEvent.IsCompleted = true;
                    DB.circus.SaveChanges();
                    LoadEvents();
                    MessageBox.Show("Мероприятие успешно завершено", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при завершении мероприятия: {ex.Message}", "Ошибка",
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