using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SOKOBAN_ASSESSMENT
{
    internal class PopulateGrid //EASY
    {
        private EASYMODE window { get; set; }
        private int targetCellRow, targetCellColumn;
        
        public PopulateGrid(EASYMODE window)
        {
            this.window = window;
        }

        public void drawContents(string uriLocation, int row, int column)
        {
            Image img = new Image() { Source = new BitmapImage(new Uri(uriLocation, UriKind.Relative)) };
            window.appGrid.Children.Add(img);
            Grid.SetRow(img, row);
            Grid.SetColumn(img, column);
        }
        public void drawGrid1()
        {
            for (int x = 0; x < window.noOfRows; x++)
            {
                for (int y = 0; y < window.noOfCols; y++)
                {
                    drawContents("Images\\blank.bmp", x, y);
                    //WALLS
                    if (x == 0 || y == 0 || x == window.noOfRows - 1 || y == window.noOfCols - 1)
                        drawContents("Images\\wall.bmp", x, y);
                }
            }

            drawContents("Images\\penguin.bmp", 4, 4);
            window.penguinRow = 4;
            window.penguinColumn = 4;
           
            //GOALS
            drawContents("Images\\goal.bmp", 7, 7);
            window.goalRow = 7;
            window.goalColumn = 7;

            //BOXES
            drawContents("Images\\box.bmp", 5, 4);

            window.boxRow = 5;
            window.boxColumn = 4;
        }
    }
}
