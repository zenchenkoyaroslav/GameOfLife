using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace GameOfLife
{
    
    public partial class MainWindow : Window
    {

        bool start = false;

        List<Ellipse> elements;

        public MainWindow()
        {
            InitializeComponent();
            initGrid(Globals.igNum);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            elements = initCells();
        }

        private void initGrid(int num)
        {
            for(int i = 0; i < num; i++)
            {
                GridMap.RowDefinitions.Add(new RowDefinition());
                GridMap.ColumnDefinitions.Add(new ColumnDefinition());
            }

            
        }
        private List<Ellipse> initCells()
        {

            List<Ellipse> aEllipse = new List<Ellipse>();

            double dHeight = GridMap.RowDefinitions[0].ActualHeight;
            double dWidth = GridMap.ColumnDefinitions[0].ActualWidth;

            int iRow = -1;
            foreach (RowDefinition row in GridMap.RowDefinitions)
            {
                iRow++;
                int iCol = -1;
                foreach (ColumnDefinition col in GridMap.ColumnDefinitions)
                {
                    iCol++;

                    Ellipse ellipse = new Ellipse();
                    ellipse.Height = dHeight;
                    ellipse.Width = dWidth;
                    ellipse.Stroke = System.Windows.Media.Brushes.Black;
                    ellipse.Fill = System.Windows.Media.Brushes.White;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Center;
                    ellipse.VerticalAlignment = VerticalAlignment.Center;
                    Grid.SetRow(ellipse, iRow);
                    Grid.SetColumn(ellipse, iCol);
                    GridMap.Children.Add(ellipse);
                    aEllipse.Add(ellipse);
                }
            }
            return aEllipse;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!start) start = true;
            else start = false;
        }

        private void GridMap_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!start)
            {
                var point = Mouse.GetPosition(GridMap);

                int row = 0;
                int col = 0;
                double accumulatedHeight = 0.0;
                double accumulatedWidth = 0.0;

                foreach (var rowDefinition in GridMap.RowDefinitions)
                {
                    accumulatedHeight += rowDefinition.ActualHeight;
                    if (accumulatedHeight >= point.Y)
                        break;
                    row++;
                }

                foreach (var columnDefinition in GridMap.ColumnDefinitions)
                {
                    accumulatedWidth += columnDefinition.ActualWidth;
                    if (accumulatedWidth >= point.X)
                        break;
                    col++;
                }

                Ellipse ellipse = GridMap.Children.Cast<Ellipse>()
                    .First(ee => Grid.GetRow(ee) == row && Grid.GetColumn(ee) == col);
                if (ellipse.Fill == System.Windows.Media.Brushes.White)
                {
                    ellipse.Fill = System.Windows.Media.Brushes.DarkBlue;
                }
                else
                {
                    ellipse.Fill = System.Windows.Media.Brushes.White;
                }
            }

        }
    }
}
