using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;

namespace DiagramCreator
{
    public partial class Control : ContentControl
    {
        public Control()
        {
            InitializeComponent();
            FigureCreator.control = this;
        }
        
        Vector relativeMousePos;
        FrameworkElement draggedObject;

        public void StartDrag(object sender, MouseButtonEventArgs e)
        {
            draggedObject = (FrameworkElement)sender;
            relativeMousePos = e.GetPosition(draggedObject) - new Point();
            draggedObject.MouseMove += OnDragMove;
            draggedObject.LostMouseCapture += OnLostCapture;
            draggedObject.MouseUp += OnMouseUp;
            Mouse.Capture(draggedObject);
        }
        
        void OnDragMove(object sender, MouseEventArgs e)
        {
            UpdatePosition(e);
        }
        
        void UpdatePosition(MouseEventArgs e)
        {
            var point = e.GetPosition(canvas);
            var newPos = point - relativeMousePos;
            Console.WriteLine(12);
            Canvas.SetLeft(draggedObject, newPos.X);
            Canvas.SetTop(draggedObject, newPos.Y);
            var list = new List<Line>();
            if (FigureCreator.figures[draggedObject.Name].isConnected)
            {
                foreach (var i in canvas.Children.OfType<Line>())
                {
                    foreach (var j in FigureCreator.figures[draggedObject.Name].connectedNode)
                    {
                        if (j.Name+draggedObject.Name == i.Name || draggedObject.Name+j.Name == i.Name)
                            list.Add(i);
                    }
                }
                list.AddRange(canvas.Children.OfType<Line>().Where(i => FigureCreator.points[draggedObject.Name].Name == i.Name));

                foreach (var i in list)
                {
                    canvas.Children.Remove(i);
                }

                foreach (var i in  FigureCreator.figures[draggedObject.Name].connectedNode)
                {
                    FigureCreator.figures[draggedObject.Name].ConnectFigures(i);
                }
            }
        }
        
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishDrag(sender, e);
            Mouse.Capture(null);
        }

        void OnLostCapture(object sender, MouseEventArgs e)
        {
            FinishDrag(sender, e);
        }
        
        void FinishDrag(object sender, MouseEventArgs e)
        {
            draggedObject.MouseMove -= OnDragMove;
            draggedObject.LostMouseCapture -= OnLostCapture;
            draggedObject.MouseUp -= OnMouseUp;
            UpdatePosition(e);
        }
        
        public void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Grid.Height + e.Delta > 650))
            {
                Grid.Height += e.Delta;
                Grid.Width += e.Delta;
            }
            foreach (var child in canvas.Children.OfType<Grid>())
            {
                foreach (var i in child.Children.OfType<FrameworkElement>())
                {
                    // ReSharper disable once PossibleLossOfFraction
                    if (!(i.Height + e.Delta / 2 > 50)) continue;
                    if (FigureCreator.figures.ContainsKey(i.Name))
                    {
                        FigureCreator.figures[i.Name].ChangeSize(e.Delta / 2);
                        child.Width += e.Delta / 2;
                        child.Height += e.Delta / 2;
                    }
                    else
                    {
                        // ReSharper disable once PossibleLossOfFraction
                        i.Height += e.Delta / 2;
                        // ReSharper disable once PossibleLossOfFraction
                        i.Width += e.Delta / 2;
                    }
                }
            }
        }
    }
}