using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

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
            File.WriteAllText("Logs\\wall_positions.log", "");
            File.WriteAllText("Logs\\blank_positions.log", "");
            File.WriteAllText("Logs\\goal_positions.log", "");
            File.WriteAllText("Logs\\box_positions.log", "");
            
            for (int x = 0; x < window.noOfRows; x++)
            {
                for (int y = 0; y < window.noOfCols; y++)
                {
                    drawContents("Images\\blank.bmp", x, y);
                    //WALLS
                    //if (x == 0 || y == 0 || x == window.noOfRows - 1 || y == window.noOfCols - 1)
                    //{
                    //    wallPositions.Add(Tuple.Create(x, y)); // Mark as wall
                    //    drawContents("Images\\wall.bmp", x, y);
                    //}
                    //if (x == 1 && y == 3 || x == 2 && y == 3 || x == 3 && y == 3 || x == 1 && y == 4 || x == 2 && y == 4
                    //   || x == 7 && y == 3 || x == 8 && y == 3 || x == 8 && y == 4 || x == 4 && y == 6
                    //   || x == 5 && y == 6 || x == 6 && y == 6 || x == 4 && y == 7 || x == 5 && y == 7
                    //   || x == 6 && y == 7)
                    //{
                    //    wallPositions.Add(Tuple.Create(x, y));  // Mark as wall
                    //    drawContents("Images\\wall.bmp", x, y);
                    //}
                    //if ((x == 0 && (y >= 0 && y<= window.noOfCols-1)) || ((x>=0 && x <= window.noOfRows-1) && y == 0) 
                    //   || (x == window.noOfRows-1 && (y >= 0 && y <= window.noOfCols-1)) || ((x >= 0 && x <= window.noOfRows-1) && y == window.noOfCols-1)
                    if (x == 0 || y == 0 || x == window.noOfRows - 1 || y == window.noOfCols - 1
                       || (x == 1 && y == 3) || (x == 2 && y == 3) || (x == 3 && y == 3) || x == 1 && y == 4 || (x == 2 && y == 4)
                       || (x == 7 && y == 3) || (x == 8 && y == 3) || (x == 8 && y == 4) || (x == 4 && y == 6)
                       || (x == 5 && y == 6) || (x == 6 && y == 6) || (x == 4 && y == 7) || (x == 5 && y == 7)
                       || (x == 6 && y == 7))
                    {

                        window.wallPositions.Add(Tuple.Create(x, y)); // Mark as wall
                        drawContents("Images\\wall.bmp", x, y);

                    }
                }
            }
         
            File.AppendAllLines("Logs\\wall_positions.log", window.wallPositions.Select(w => $"Wall cell at: ({w.Item1}, {w.Item2})"));
            //foreach (var wall in window.wallPositions)
            //{
            //    File.AppendAllText("Logs\\wall_positions.log", $"Wall at: ({wall.Item1}, {wall.Item2})\n");
            //    //Console.WriteLine($"Wall at: ({wall.Item1}, {wall.Item2})");
            //}
            //=============================================================================================
            //drawContents("Images\\penguin.bmp", 4, 4);
            //window.penguinRow = 4;
            //window.penguinColumn = 4;
            drawContents("Images\\penguin.bmp", 5, 2);
            window.penguinRow = 5;
            window.penguinColumn = 2;
            //=============================================================================================
            //GOALS
            //drawContents("Images\\goal.bmp", 7, 7);
            //window.goalRow = 7;
            //window.goalColumn = 7;

            window.goalPositions.Add(Tuple.Create(4, 8)); // Mark as box
            window.goalPositions.Add(Tuple.Create(6, 8)); // Mark as box

            // Clear the log file at the start (overwrites existing content)
           
            File.AppendAllLines("Logs\\goal_positions.log", window.goalPositions.Select(g => $"Goal cell at: ({g.Item1}, {g.Item2})"));
            foreach (var goal in window.goalPositions)
            {
                drawContents("Images\\goal.bmp", goal.Item1, goal.Item2);
                //File.AppendAllText("Logs\\goal_positions.log", $"Goal at: ({goal.Item1}, {goal.Item2})\n");
            }
            //=============================================================================================
            //BOXES
            //drawContents("Images\\box.bmp", 5, 4);
            //window.boxRow = 5;
            //window.boxColumn = 4;

            window.boxPositions.Add(Tuple.Create(4, 3)); // Mark as goal
            window.boxPositions.Add(Tuple.Create(6, 3)); // Mark as goal
            // Clear the log file at the start (overwrites existing content)
           
            File.AppendAllLines("Logs\\box_positions.log", window.boxPositions.Select(b => $"Box cell at: ({b.Item1}, {b.Item2})"));

            foreach (var box in window.boxPositions)
            {
                drawContents("Images\\box.bmp", box.Item1, box.Item2);
                //File.AppendAllText("Logs\\box_positions.log", $"Box at: ({box.Item1}, {box.Item2})\n");
            }

            //=============================================================================================
            // Find blank cells in the grid
            for (int x1 = 0; x1 < window.noOfRows; x1++)
            {
                for (int y1 = 0; y1 < window.noOfCols; y1++)
                {
                    Tuple<int, int> cell = Tuple.Create(x1, y1);

                    // Check if the cell is NOT in walls, boxes, or goals
                    if (!window.wallPositions.Contains(cell) && !window.boxPositions.Contains(cell) && !window.goalPositions.Contains(cell))
                    {
                        window.blankPositions.Add(cell); // Store the blank cell
                    }
                }
            }
            // Clear the log file at the start (overwrites existing content)

            File.AppendAllLines("Logs\\blank_positions.log", window.blankPositions.Select(bl => $"Blank cell at: ({bl.Item1}, {bl.Item2})"));
            //// Debugging: Log the blank cells
            //foreach (var blank in window.blankPositions)
            //{
            //    File.AppendAllText("Logs\\blank_positions.log", $"Blank cell at: ({blank.Item1}, {blank.Item2})\n");

            //}
            //=============================================================================================
        }
    }
}
