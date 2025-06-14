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
    /// Логика взаимодействия для AddEditEventWindow.xaml
    /// </summary>
    public partial class AddEditEventWindow : Window
    {
        public DBconn.Events Event { get; private set; }
        private bool isEditMode = false;

        public AddEditEventWindow()
        {
            InitializeComponent();
            InitializeWindow(null);
        }

        public AddEditEventWindow(DBconn.Events eventToEdit)
        {
            InitializeComponent();
            InitializeWindow(eventToEdit);
            isEditMode = true;
        }

        private void InitializeWindow(DBconn.Events ev)
        {
            // Set event type options
            cbEventType.ItemsSource = new[] { "Private", "Guest" };

            if (ev != null)
            {
                Event = ev;
                txtName.Text = ev.Name;
                dpEventDate.SelectedDate = ev.EventDate;
                cbEventType.SelectedItem = ev.EventType;
                txtProfit.Text = ev.Profit?.ToString();
                txtExpenses.Text = ev.Expenses?.ToString();
                txtPrepayment.Text = ev.Prepayment.ToString();
                txtOrganizingCompany.Text = ev.OrganizingCompany;
                chkIsCompleted.IsChecked = ev.IsCompleted;
            }
            else
            {
                Event = new DBconn.Events
                {
                    EventDate = DateTime.Now,
                    Prepayment = 0,
                    IsCompleted = false
                };
                cbEventType.SelectedIndex = 0;
                dpEventDate.SelectedDate = DateTime.Now;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название мероприятия!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dpEventDate.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату мероприятия!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbEventType.SelectedItem == null)
                {
                    MessageBox.Show("Выберите тип мероприятия!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Event.Name = txtName.Text.Trim();
                Event.EventDate = dpEventDate.SelectedDate.Value;
                Event.EventType = cbEventType.SelectedItem.ToString();

                if (decimal.TryParse(txtProfit.Text, out decimal profit))
                    Event.Profit = profit;
                else
                    Event.Profit = null;

                if (decimal.TryParse(txtExpenses.Text, out decimal expenses))
                    Event.Expenses = expenses;
                else
                    Event.Expenses = null;

                if (decimal.TryParse(txtPrepayment.Text, out decimal prepayment))
                    Event.Prepayment = prepayment;
                else
                    Event.Prepayment = 0;

                Event.OrganizingCompany = string.IsNullOrWhiteSpace(txtOrganizingCompany.Text) ?
                    null : txtOrganizingCompany.Text.Trim();

                Event.IsCompleted = chkIsCompleted.IsChecked ?? false;

                if (!isEditMode)
                {
                    DB.circus.Events.Add(Event);
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
    }
}