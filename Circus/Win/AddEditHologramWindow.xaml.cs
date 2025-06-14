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
    /// Логика взаимодействия для AddEditHologramWindow.xaml
    /// </summary>
    public partial class AddEditHologramWindow : Window
    {
        public DBconn.Holograms Hologram { get; private set; }
        private bool isEditMode = false;

        public AddEditHologramWindow()
        {
            InitializeComponent();
            InitializeWindow(null);
        }

        public AddEditHologramWindow(DBconn.Holograms hologramToEdit)
        {
            InitializeComponent();
            InitializeWindow(hologramToEdit);
            isEditMode = true;
        }
        private void InitializeWindow(DBconn.Holograms hologram)
        {
            cbResponsible.ItemsSource = DB.circus.Employees.ToList();
            cbResponsible.DisplayMemberPath = "FullName";
            cbDevelopmentStage.ItemsSource = new[] { "Планирование", "Разработка", "Тестирование", "Завершено" };

            if (hologram != null)
            {
                Hologram = hologram;
                txtName.Text = hologram.Name;
                cbDevelopmentStage.SelectedItem = hologram.DevelopmentStage;
                txtCabinetNumber.Text = hologram.CabinetNumber.ToString();
                var selectedEmployee = DB.circus.Employees.FirstOrDefault(e => e.EmployeeID == hologram.ResponsibleID);
                if (selectedEmployee != null)
                {
                    cbResponsible.SelectedItem = selectedEmployee;
                }
            }
            else
            {
                Hologram = new DBconn.Holograms();
                cbDevelopmentStage.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название голограммы!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cbDevelopmentStage.SelectedItem == null)
                {
                    MessageBox.Show("Выберите стадию разработки!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!int.TryParse(txtCabinetNumber.Text, out int cabinet) || cabinet <= 0)
                {
                    MessageBox.Show("Введите корректный номер кабинета (положительное число)!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (cbResponsible.SelectedItem == null)
                {
                    MessageBox.Show("Выберите ответственного сотрудника!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Hologram.Name = txtName.Text.Trim();
                Hologram.DevelopmentStage = cbDevelopmentStage.SelectedItem.ToString();
                Hologram.CabinetNumber = cabinet;
                Hologram.ResponsibleID = ((DBconn.Employees)cbResponsible.SelectedItem).EmployeeID;

                if (!isEditMode)
                {
                    DB.circus.Holograms.Add(Hologram);
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
