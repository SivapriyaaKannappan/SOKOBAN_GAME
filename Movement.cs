using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Automation;
using System.IO;  // Required for file operations

namespace SOKOBAN_ASSESSMENT
{
    class Movement
    {
        private EASYMODE window { get; set; }
        private PopulateGrid populateGrid { get; set; }
        private int currentCellRow, currentCellColumn;
        private int targetCellRow, targetCellColumn;
        private int newBoxRow, newBoxColumn;
        private int moveCount;
        private int goalRetryCount=0;
        bool gameWon = false;
       
        
        bool isWall(int x, int y)
        {
            //return wallPositions.Contains(Tuple.Create(x, y));
            //Console.WriteLine($"Added wall: ({x}, {y})");
            return window.wallPositions.Any(w => w.Item1 == x && w.Item2 == y);
        }
        bool isBlank(int x, int y)
        {
            //return wallPositions.Contains(Tuple.Create(x, y));
            //Console.WriteLine($"Added wall: ({x}, {y})");
            return window.blankPositions.Any(bl => bl.Item1 == x && bl.Item2 == y);
        }
        bool isBox(int x, int y)
        {
            //return wallPositions.Contains(Tuple.Create(x, y));
            //Console.WriteLine($"Added wall: ({x}, {y})");
            //window.boxRow = x;
            //window.boxColumn = y;
            return window.boxPositions.Any(b => b.Item1 == x && b.Item2 == y);
        }
        bool isGoal(int x, int y)
        {
            //return wallPositions.Contains(Tuple.Create(x, y));
            //Console.WriteLine($"Added wall: ({x}, {y})");
            window.goalRow = x;
            window.goalColumn = y;
            return window.goalPositions.Any(g => g.Item1 == x && g.Item2 == y);
        }
        public Movement(EASYMODE window)
        {
            this.window = window;
        }
        
        public void MovePenguin(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left: move("left"); break;
                case Key.Up: move("up"); break;
                case Key.Right: move("right"); break;
                case Key.Down: move("down"); break;
                default: break;
            }
        }

        private void move(string direction)
        {
            if (gameWon) return;
            int i = 0, j = 0;
            Console.WriteLine($"Moving {direction}");
            switch (direction)
            {
                case "left": i = 0; j = -1; break;
                case "up": i = -1; j = 0; break;
                case "right": i = 0; j = 1; break;
                case "down": i = 1; j = 0; break;
                default: break;
            }
            // Current Penguin position
            currentCellRow = window.penguinRow;
            currentCellColumn = window.penguinColumn;

            // Calculate new Penguin position
            targetCellRow = window.penguinRow + i;
            targetCellColumn = window.penguinColumn + j;

            Console.WriteLine($"New Penguin Position: X={i}, Y={j}");
            // Stop if Penguin hits the wall
            //if (!(targetCellRow >= 1 && targetCellRow < window.noOfRows - 1 && targetCellColumn >= 1 && targetCellColumn < window.noOfCols - 1)
            //    Console.WriteLine("Penguin hit the wall! Cannot move.");
            if (isWall(targetCellRow, targetCellColumn))
            {
                Console.WriteLine("Penguin hit the wall! Cannot move.");
                File.AppendAllText("debug.log", "Penguin hit the wall! Cannot move.");
            }
            //else
            //{
            //    populateGrid = new PopulateGrid(window);
            //    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);
            //    populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn);
            //    updatePenguinLocation();
            //    window.updateMoveCounter(); // Increment move count

            //}
            //// Check whether the new penguin position is a box
            if (isBox(targetCellRow, targetCellColumn))
            {
                window.boxRow = targetCellRow;
                window.boxColumn = targetCellColumn;
                //// Calculate new box position
                newBoxRow = window.boxRow + i;
                newBoxColumn = window.boxColumn + j;
                // Check whether the space ahead the box is empty
                //if (newBoxRow > 0 && newBoxRow < window.noOfRows - 1 && newBoxColumn > 0 && newBoxColumn < window.noOfCols - 1)
                if (isBlank(newBoxRow, newBoxColumn))
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\box.bmp", newBoxRow, newBoxColumn); // Move the box
                    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);  // Move the penguin
                    populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn); // Clear old penguin position
                   

                    var oldBoxPosition = window.boxPositions.Find(b => b.Item1 == window.boxRow && b.Item2 == window.boxColumn);
                    if (oldBoxPosition != null)
                    {
                        window.boxPositions.Remove(oldBoxPosition);
                        window.boxPositions.Add(Tuple.Create(newBoxRow, newBoxColumn)); // Add New box position
                        window.blankPositions.Add(Tuple.Create(window.penguinRow, window.penguinColumn)); // Add New blank position
                        window.blankPositions.Remove(Tuple.Create(newBoxRow, newBoxColumn)); // Remove old blank position
                    }

                    //window.boxPositions.Remove(Tuple.Create(window.boxRow, window.boxColumn)); // Remove the old position
                    //window.boxPositions.Add(Tuple.Create(newBoxRow, newBoxColumn));    // Add the new position
                    //==============================================
                    //For Debugging Purpose
                    // Convert old position to string format
                                        
                    string oldBoxLine = $"Box cell at: ({window.boxRow}, {window.boxColumn})";
                    string oldBlankLine = $"Blank cell at: ({newBoxRow}, {newBoxColumn})";
                    string newBlankLine = $"Blank cell at: ({window.penguinRow}, {window.penguinColumn})";
                    string newBoxLine = $"Box cell at: ({newBoxRow}, {newBoxColumn})";

                    // Read all lines into a list
                    List<string> boxlines = File.ReadAllLines("Logs\\box_positions.log").ToList();
                    List<string> blanklines = File.ReadAllLines("Logs\\blank_positions.log").ToList();
                    // Find the index of the old position
                    int boxindex = boxlines.FindIndex(line => line == oldBoxLine);
                    int blankindex = blanklines.FindIndex(line => line == oldBlankLine);
                    if (boxindex != -1)  // If found, replace it
                    {
                        boxlines[boxindex] = newBoxLine;
                    }
                    else  // If not found, append the new position
                    {
                        boxlines.Add(newBoxLine);
                    }
                    if (blankindex != -1)  // If found, replace it
                    {
                        blanklines[blankindex] = newBlankLine;
                    }
                    else  // If not found, append the new position
                    {
                        blanklines.Add(newBlankLine);
                    }
                    // Write updated lines back to the file
                    File.WriteAllLines("Logs\\box_positions.log", boxlines);
                    File.WriteAllLines("Logs\\blank_positions.log", blanklines);
                    //==============================================
                    window.boxRow = newBoxRow;
                    window.boxColumn = newBoxColumn;
                    updatePenguinLocation(); 

                    window.updateMoveCounter(); // Increment move count
                }
                
                // Check whether the space ahead the box is a goal
                //if (window.boxRow == window.goalRow && window.boxColumn == window.goalColumn)
                if (!window.reachedGoals.Contains((window.goalRow, window.goalColumn)) && isGoal(newBoxRow, newBoxColumn))
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\boxgoal.bmp", newBoxRow, newBoxColumn); // Move the box
                    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);  // Move the penguin
                    populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn); // Clear old penguin position
                    var oldBoxPosition = window.boxPositions.Find(b => b.Item1 == window.boxRow && b.Item2 == window.boxColumn);
                    if (oldBoxPosition != null)
                    {
                        window.boxPositions.Remove(oldBoxPosition);
                        window.boxPositions.Add(Tuple.Create(newBoxRow, newBoxColumn)); // Add New box position
                        window.blankPositions.Add(Tuple.Create(window.penguinRow, window.penguinColumn)); // Add New blank position
                    }

                    //==============================================
                    //For Debugging Purpose
                    // Convert old position to string format

                    string oldBoxLine = $"Box cell at: ({window.boxRow}, {window.boxColumn})";
                    //string oldBlankLine = $"Blank cell at: ({newBoxRow}, {newBoxColumn})";
                    string newBlankLine = $"Blank cell at: ({window.penguinRow}, {window.penguinColumn})";
                    string newBoxLine = $"Box cell at: ({newBoxRow}, {newBoxColumn})";

                    // Read all lines into a list
                    List<string> boxlines = File.ReadAllLines("Logs\\box_positions.log").ToList();
                    List<string> blanklines = File.ReadAllLines("Logs\\blank_positions.log").ToList();
                    // Find the index of the old position
                    int boxindex = boxlines.FindIndex(line => line == oldBoxLine);
                    //int blankindex = blanklines.FindIndex(line => line == oldBlankLine);
                    if (boxindex != -1)  // If found, replace it
                    {
                        boxlines[boxindex] = newBoxLine;
                    }
                    else  // If not found, append the new position
                    {
                        boxlines.Add(newBoxLine);
                    }
                    blanklines.Add(newBlankLine);
                    // Write updated lines back to the file
                    File.WriteAllLines("Logs\\box_positions.log", boxlines);
                    File.WriteAllLines("Logs\\blank_positions.log", blanklines);
                    //==============================================
                    window.boxRow = newBoxRow;
                    window.boxColumn = newBoxColumn;
                    if (window.boxRow == window.goalRow && window.boxColumn == window.goalColumn && !window.reachedGoals.Contains((window.boxRow, window.boxColumn)))
                    {
                        window.boxPositions.Remove(Tuple.Create(window.boxRow, window.boxColumn)); // Remove the box which reached the goal
                        window.reachedGoals.Add((window.goalRow, window.goalColumn)); // Add Reached Goal position
                    }
                    else if (window.boxRow == window.goalRow && window.boxColumn == window.goalColumn && window.reachedGoals.Contains((window.boxRow, window.boxColumn)))
                    {
                       Console.WriteLine("Can't move two boxes to the same goal!");
                       goalRetryCount++;

                    }

                    if (window.boxPositions.Count == 0 && window.reachedGoals.Contains((newBoxRow, newBoxColumn)))
                    {
                        Console.WriteLine("No boxes are present.");
                        // Set the flag to stop further movement
                        gameWon = true;

                        //Display a message
                        MessageBox.Show($"Congratulations! You completed the level with {window.moveCount} moves.");
                    }
                    else if (window.boxPositions.Count != 0 && window.reachedGoals.Contains((newBoxRow, newBoxColumn)) && goalRetryCount == 0)
                    {
                        gameWon = false;

                        //Console.WriteLine("A goal has been reached!");
                        //Display a message
                        MessageBox.Show($"A goal has been reached!");
                    }
                    else if (window.boxPositions.Count != 0 && window.reachedGoals.Contains((newBoxRow, newBoxColumn)) && goalRetryCount >0)
                    {
                        gameWon = false;

                        //Console.WriteLine("A goal has been reached!");
                        //Display a message
                        MessageBox.Show($"Can't move two boxes to the same goal!");
                    }
                    updatePenguinLocation();
                    window.updateMoveCounter(); // Increment move count
                }
               
            }
            // If penguin tries to walk into a boxgoal position
            else if (isGoal(targetCellRow, targetCellColumn) && isBox(targetCellRow, targetCellColumn)
                || window.reachedGoals.Contains((targetCellRow, targetCellColumn)))
            {
                populateGrid = new PopulateGrid(window);
                populateGrid.drawContents("Images\\boxgoal.bmp", targetCellRow, targetCellColumn); // Don't move the box from the goal
                //populateGrid.drawContents("Images\\blank.bmp", newBoxRow, newBoxColumn); // Don't move the box from the goal
                populateGrid.drawContents("Images\\penguin.bmp", window.penguinRow, window.penguinColumn);  // Don't move the penguin
                //Display a message
                MessageBox.Show($"Penguin can't move {direction}");
                Console.WriteLine("Penguin can't move.");
                return; // Exit function to prevent movement
                //updatePenguinLocation();
                //window.updateMoveCounter();
            }
            //// Check whether the new penguin position is a goal
            //else if (targetCellRow == window.goalRow && targetCellColumn == window.goalColumn)
            else if (isGoal(targetCellRow, targetCellColumn))
            {

                populateGrid = new PopulateGrid(window);
                populateGrid.drawContents("Images\\penguingoal.bmp", targetCellRow, targetCellColumn);  // Move the penguin
                //populateGrid.drawContents("Images\\goal.bmp", window.goalRow, window.goalColumn);  
                populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn); // Clear old penguin position

                window.blankPositions.Add(Tuple.Create(window.penguinRow, window.penguinColumn)); // Add New blank position
                //==============================================
                //For Debugging Purpose
                List<string> blanklines = File.ReadAllLines("Logs\\blank_positions.log").ToList();
                string newBlankLine = $"Blank cell at: ({window.penguinRow}, {window.penguinColumn})";
                blanklines.Add(newBlankLine);
                // Write updated lines back to the file
                File.WriteAllLines("Logs\\blank_positions.log", blanklines);
                //=======================================================
                updatePenguinLocation();

                window.updateMoveCounter(); // Increment move count
            
            }

            //// Otherwise, move the penguin if it's an empty space
            //else if (targetCellRow >= 1 && targetCellRow < window.noOfRows - 1 && targetCellColumn >= 1 && targetCellColumn < window.noOfCols - 1)
            else if (isBlank(targetCellRow, targetCellColumn))
            {
                //Check whether the current penguin position is a goal
                //if (currentCellRow == window.goalRow && currentCellColumn == window.goalColumn)
                if (isGoal(currentCellRow, currentCellColumn))
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\goal.bmp", currentCellRow, currentCellColumn);
                    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);

                    var oldBlankPosition = window.blankPositions.Find(bl => bl.Item1 == targetCellRow && bl.Item2 == targetCellColumn);
                    if (oldBlankPosition != null)
                    {
                        window.blankPositions.Remove(oldBlankPosition);
                        window.blankPositions.Add(Tuple.Create(window.penguinRow, window.penguinColumn)); // Add New blank position
                    }

                    updatePenguinLocation();
                    window.updateMoveCounter(); // Increment move count
                }
                else
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);
                    populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn);

                    var oldBlankPosition = window.blankPositions.Find(bl => bl.Item1 == targetCellRow && bl.Item2 == targetCellColumn);
                    if (oldBlankPosition != null)
                    {
                        window.blankPositions.Remove(oldBlankPosition);
                        window.blankPositions.Add(Tuple.Create(window.penguinRow, window.penguinColumn)); // Add New blank position
                    }

                    updatePenguinLocation();
                    window.updateMoveCounter(); // Increment move count
                }
            }
        }
        private void updatePenguinLocation()
        {
            window.penguinRow = targetCellRow;
            window.penguinColumn = targetCellColumn;
        }

    }
}
