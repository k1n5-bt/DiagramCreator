using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static DiagramCreator.FigureCreator;

namespace DiagramCreator
{
    public class Node
    {
        public bool isConnected = false;
        public List<Node> connectedNode = new List<Node>();
        public Image Figure { get; }
        public string Name { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }
        public ShapeType shapeType { get; }
        public Point connectPoint { get; set; }
        
        public Node(int width, int height, string name, string path)
        {
            Width = width;
            Height = height;
            Name = name;
            shapeType = ShapeType.Ellipse;
            Figure = new Image{Name = name, ContextMenu = GetContextMenu(), Source = 
                new BitmapImage(new Uri(path)), Width=width, Height=height};
            connectPoint = new Point()
            {
                X = Canvas.GetLeft(Figure),
                Y = Canvas.GetTop(Figure)
            };
        }

        public virtual void ChangeName(string name)
        {
            Name = name;
            Figure.Name = name;
        }

        public virtual void ChangeSize(int delta)
        {
            Height += delta;
            Width += delta;
            Figure.Height += delta;
            Figure.Width += delta;
            connectPoint = new Point()
            {
                X = Canvas.GetLeft(Figure),
                Y = Canvas.GetTop(Figure)
            };
        }

        protected virtual ContextMenu GetContextMenu()
        {
            var cont = new ContextMenu();
            var delete = new MenuItem() {Header = "Delete"};
            delete.Click += (sender, args) => Dispose(Name);
            foreach (var e in control.canvas.Children.OfType<Grid>())
            {
                if (e.Name == Name)
                    continue;
                var m = new MenuItem() {Header = $"Connect to {e.Name}"};
                m.Click += (sender, args) =>
                {
                    this.connectedNode.Add(figures[e.Name]);
                    figures[e.Name].connectedNode.Add(this);
                    this.isConnected = true;
                    figures[e.Name].isConnected = true;
                    this.ConnectFigures(figures[e.Name]);
                };
                cont.Items.Add(m);
            }
            cont.Items.Add(delete);
            return cont;
        }

        public void UpdateContextMenu() => Figure.ContextMenu = GetContextMenu();
    }
}