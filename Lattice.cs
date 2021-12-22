using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public class Lattice : Control
{
    public static readonly DependencyProperty RodPenProperty;

    public Pen? RodPen
    {
        get => GetValue(RodPenProperty) as Pen;
        set => SetValue(RodPenProperty, value);
    }

    static Lattice()
    {
        RodPenProperty = DependencyProperty.Register(
            "RodPen",
            typeof(Pen),
            typeof(Lattice));
    }

    protected override void OnRender(DrawingContext context)
    {
        const double maxCellSide = 50.0;
        const int splitCnt = 2;
        const double splitTreshold = 6.0;

        if (!(Background is null)) context.DrawRectangle(Background, null!, new Rect(default, RenderSize));

        if (RodPen is null) return;

        var side = Math.Max(ActualWidth, ActualHeight);
        var cellSide = side / splitTreshold;

        for (; cellSide >= maxCellSide; cellSide /= splitCnt) { }

        var horRodCnt = (int)(ActualWidth / cellSide) + 1;
        var verRodCnt = (int)(ActualHeight / cellSide) + 1;

        for (var i = 1; i < horRodCnt; i++)
        {
            var offsetX = i * cellSide;

            context.DrawLine(RodPen, new Point(offsetX, 0), new Point(offsetX, ActualHeight));
        }
        for (var i = 1; i < verRodCnt; i++)
        {
            var offsetY = i * cellSide;

            context.DrawLine(RodPen, new Point(0, offsetY), new Point(ActualWidth, offsetY));
        }
    }
}