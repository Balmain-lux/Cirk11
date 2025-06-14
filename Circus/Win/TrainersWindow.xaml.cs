﻿using System;
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
    /// Логика взаимодействия для TrainersWindow.xaml
    /// </summary>
    public partial class TrainersWindow : Window
    {
        public TrainersWindow()
        {
            InitializeComponent();
            TrainersGrid.ItemsSource = DB.circus.Trainers.ToList();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
