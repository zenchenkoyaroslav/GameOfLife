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

        int num = Globals.igNum;

        List<Ellipse> elements;

        GameLogic game;

        TimeSpan startTimeSpan;
        TimeSpan periodTimeSpan;

        public MainWindow()
        {
            InitializeComponent();
            initGrid(num);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            elements = initCells();
        }

        private void initGrid(int num)
        {
            for (int i = 0; i < num; i++)
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

        private bool[,] gridToArr()
        {
            bool[,] Cells = new bool[num, num];
            int iRow = -1;
            foreach (RowDefinition row in GridMap.RowDefinitions)
            {
                iRow++;
                int iCol = -1;
                foreach (ColumnDefinition col in GridMap.ColumnDefinitions)
                {
                    iCol++;
                    Ellipse ellipse = GridMap.Children.Cast<Ellipse>()
                        .First(ee => Grid.GetRow(ee) == iRow && Grid.GetColumn(ee) == iCol);
                    if (ellipse.Fill == System.Windows.Media.Brushes.White) Cells[iRow, iCol] = false;
                    else if(ellipse.Fill == System.Windows.Media.Brushes.DarkBlue) Cells[iRow, iCol] = true;
                }
            }
            return Cells;
        }

        private void updateCells(bool[,] cells)
        {
            int iRow = -1;
            foreach (RowDefinition row in GridMap.RowDefinitions)
            {
                iRow++;
                int iCol = -1;
                foreach (ColumnDefinition col in GridMap.ColumnDefinitions)
                {
                    iCol++;

                    Ellipse ellipse = GridMap.Children.Cast<Ellipse>()
                        .First(ee => Grid.GetRow(ee) == iRow && Grid.GetColumn(ee) == iCol);
                    if (cells[iRow, iCol] == false) ellipse.Fill = System.Windows.Media.Brushes.White;
                    else if(cells[iRow, iCol] == true) ellipse.Fill = System.Windows.Media.Brushes.DarkBlue;
                }
            }
        }

        async Task RunPeriodicSave()
        {
            while (start)
            {
                await Task.Delay(100);
                game.update();
                await Task.Delay(100);
                updateCells(game.Cells);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (!start)
            {
                button.Content = "Stop";
                start = true;
                game = new GameLogic(gridToArr());
                try
                {
                    await RunPeriodicSave();
                }
                finally
                {

                }

                /*
                startTimeSpan = TimeSpan.Zero;
                periodTimeSpan = TimeSpan.FromSeconds(2);

                var timer = new System.Threading.Timer((ee) =>
                {
                    game.update();
                    updateCells(game.Cells);
                }, null, startTimeSpan, periodTimeSpan);*/
            }
            else
            {
                button.Content = "Start";
                start = false;
            }
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
