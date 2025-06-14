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

namespace Circus.Win
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }
        private void btnAnimals_Click(object sender, RoutedEventArgs e)
        {
            var animalWindow = new AnimalWindow();
            animalWindow.Show();
        }

        private void btnArtists_Click(object sender, RoutedEventArgs e)
        {
            var artistsWindow = new Artists();
            artistsWindow.Show();
        }

        private void btnHolograms_Click(object sender, RoutedEventArgs e)
        {
            var hologramsWindow = new HologramsWindow();
            hologramsWindow.Show();
        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            var eventsWindow = new EventsWindow();
            eventsWindow.Show();
        }

    
    }
}