using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static DiagramCreator.FigureCreator;
using static DiagramCreater.Path;
using Brushes = System.Windows.Media.Brushes;

namespace DiagramCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow() 
        {
            Background = new ImageBrush(new BitmapImage(new Uri($"{path}\\Resources\\Images\\background.jpg")));
            InitializeComponent();
            Title = "123";
            _mainWindow = this;
        }

        private void Add(object sender, EventArgs e)
        {
            var window = new InputWindow();
            if (window.ShowDialog() != true) return;
            var w = ShapeType.Rectangle;
            foreach (var i in window.Panel.Children.OfType<RadioButton>())
            {
                if (i.IsChecked == true)
                {
                    w = (ShapeType)Enum.Parse(typeof(ShapeType), i.Name, true);
                }
            }
            AddFigure(window.Box.Text, w);
        }

        private void AddFigure(string name, ShapeType shapeType)
        {
            var img = GetFigure(name, shapeType);
            var txtBlock = new TextBlock() {Foreground = Brushes.White, 
                                            HorizontalAlignment = HorizontalAlignment.Center, 
                                            VerticalAlignment = VerticalAlignment.Center,
                                            Text = name
            };
            var grid = new Grid(){Name = name, Width = img.Width, Height = img.Height};
            grid.Children.Add(img);
            grid.Children.Add(txtBlock);
            grid.MouseLeftButtonDown += control.StartDrag;
            grid.PreviewMouseWheel += control.UIElement_OnMouseWheel;
            Warp.canvas.Children.Add(grid);
        }

        private void DeleteAllFigures(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            var renderTargetBitmap = 
                new RenderTargetBitmap(1400, 700, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(FigureCreator.control); 
            var pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using Stream fileStream = File.Create($"{path}Screenshots\\file2.png");
            pngImage.Save(fileStream);
        }
    }
}