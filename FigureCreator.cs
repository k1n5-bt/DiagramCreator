using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static DiagramCreater.Path;

namespace DiagramCreator
{
    public static class FigureCreator
    {
        public static Dictionary<string, Node> figures = new Dictionary<string, Node>();
        public static Dictionary<string, Line> points = new Dictionary<string, Line>();
        public static MainWindow _mainWindow { get; set; }
        public static Control control { get; set; }

        public static Image GetFigure(string name, ShapeType shapeType)
        {
            var pathToImg = shapeType switch
            {
                ShapeType.Ellipse => $"{path}\\Resources\\Images\\ellipse.png",
                ShapeType.Circle => $"{path}\\Resources\\Images\\circle.png",
                ShapeType.Rectangle => $"{path}\\Resources\\Images\\rectangle.png",
                ShapeType.Star => $"{path}\\Resources\\Images\\star.png",
                ShapeType.Triangle => $"{path}\\Resources\\Images\\triangle.png",
                ShapeType.Plus => $"{path}\\Resources\\Images\\plus.png",
                ShapeType.Trapezium => $"{path}\\Resources\\Images\\trapezium.png",
                ShapeType.Square => $"{path}\\Resources\\Images\\square.png",
                ShapeType.Ring => $"{path}\\Resources\\Images\\ring.png",
                _ => null
            };
            var result = new Node(100, 100, name, pathToImg);
            figures[result.Name] = result;
            foreach (var e in figures.Values)
                e.UpdateContextMenu();
            return result.Figure;
        }

        public static void ConnectFigures(this Node node1, Node node2)
        {
            foreach (var e in control.canvas.Children.OfType<Grid>())
            {
                if (e.Name == node1.Name)
                    node1.connectPoint = new Point(){X = Canvas.GetLeft(e) - e.Width / 2, Y = Canvas.GetTop(e) + e.Height / 2};
                if (e.Name == node2.Name)
                    node2.connectPoint = new Point(){X = Canvas.GetLeft(e) - e.Width / 2, Y = Canvas.GetTop(e) + e.Height / 2};
            }
            Line line = new Line(){Name = node1.Name + node2.Name};
            Thickness thickness = new Thickness(101,-11,362,250);
            line.Margin = thickness;
            line.Visibility = Visibility.Visible;
            line.StrokeThickness = 4;
            line.Stroke = Brushes.Black;
            line.X1 = node1.connectPoint.X;
            line.X2 = node2.connectPoint.X;
            line.Y1 = node1.connectPoint.Y;
            line.Y2 = node2.connectPoint.Y;
            control.canvas.Children.Add(line);
            points[node1.Name] = line;
            points[node2.Name] = line;
        }
        
        public static void Dispose(string name)
        {
            if (!figures.ContainsKey(name))
                return;
            foreach (var i in control.canvas.Children.OfType<Grid>())
            {
                if (i.Name != name) continue;
                control.canvas.Children.Remove(i);
                figures.Remove(name);
                break;
            }
        }
        
        public static void Dispose()
        {
            control.canvas.Children.Clear();
            figures.Clear();
        }
    }
}