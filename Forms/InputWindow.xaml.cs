using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static DiagramCreater.Path;

namespace DiagramCreator
{
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            Background = new ImageBrush(new BitmapImage(new Uri($"{path}\\Resources\\Images\\background.jpg")));
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}