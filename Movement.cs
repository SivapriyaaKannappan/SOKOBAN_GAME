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

namespace SOKOBAN_ASSESSMENT
{
    class Movement
    {
        private EASYMODE window { get; set; }
        private PopulateGrid populateGrid { get; set; }
        private int currentCellRow, currentCellColumn;
        private int targetCellRow, targetCellColumn;
        private int newBoxRow, newBoxColumn, nextGoalRow, nextGoalColumn;
        private int moveCount;
        bool gameWon = false;
        
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
            if (!(targetCellRow >= 1 && targetCellRow < window.noOfRows - 1 && targetCellColumn >= 1 && targetCellColumn < window.noOfCols - 1))
                Console.WriteLine("Penguin hit the wall! Cannot move.");
            // Check whether the new penguin position is a box
            if (targetCellRow == window.boxRow && targetCellColumn == window.boxColumn)
            {
                //// Calculate new box position
                newBoxRow = window.boxRow + i;
                newBoxColumn = window.boxColumn + j;
                // Check whether the space ahead the box is empty
                if (newBoxRow > 0 && newBoxRow < window.noOfRows - 1 && newBoxColumn > 0 && newBoxColumn < window.noOfCols - 1)
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\box.bmp", newBoxRow, newBoxColumn); // Move the box
                    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);  // Move the penguin
                    populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn); // Clear old penguin position
                    window.boxRow = newBoxRow;
                    window.boxColumn = newBoxColumn;
                    updatePenguinLocation(); // Increment move count

                    window.updateMoveCounter();
                }
                // Check whether the space ahead the box is a goal
                if (window.boxRow == window.goalRow && window.boxColumn == window.goalColumn)
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\boxgoal.bmp", newBoxRow, newBoxColumn); // Move the box
                    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);  // Move the penguin
                    //populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn); // Clear old penguin position
                    window.boxRow = newBoxRow;
                    window.boxColumn = newBoxColumn;
                    updatePenguinLocation();
                    
                    window.updateMoveCounter(); // Increment move count
                    // Set the flag to stop further movement
                    gameWon = true;

                    //Display a message
                    MessageBox.Show("Congratulations! You completed the level.");

                }
                if (targetCellRow == window.goalRow && targetCellColumn == window.goalColumn)
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\blank.bmp", newBoxRow, newBoxColumn); // Don't move the box from the goal
                    populateGrid.drawContents("Images\\penguin.bmp", window.penguinRow, window.penguinColumn);  // Move the penguin
                    updatePenguinLocation();
                    window.updateMoveCounter();
                }
            }
            // Check whether the new penguin position is a goal
            else if (targetCellRow == window.goalRow && targetCellColumn == window.goalColumn)
            {
                populateGrid = new PopulateGrid(window);
                populateGrid.drawContents("Images\\penguingoal.bmp", targetCellRow, targetCellColumn);  // Move the penguin
                //populateGrid.drawContents("Images\\goal.bmp", window.goalRow, window.goalColumn);  
                populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn); // Clear old penguin position
                updatePenguinLocation();
   
                window.updateMoveCounter(); // Increment move count
            }

            // Otherwise, move the penguin if it's an empty space
            else if (targetCellRow >= 1 && targetCellRow < window.noOfRows - 1 && targetCellColumn >= 1 && targetCellColumn < window.noOfCols - 1)
            {
                //Check whether the current penguin position is a goal
                if (currentCellRow == window.goalRow && currentCellColumn == window.goalColumn)
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\goal.bmp", currentCellRow, currentCellColumn);
                    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);
                    updatePenguinLocation();
                    window.updateMoveCounter(); // Increment move count
                }
                else
                {
                    populateGrid = new PopulateGrid(window);
                    populateGrid.drawContents("Images\\penguin.bmp", targetCellRow, targetCellColumn);
                    populateGrid.drawContents("Images\\blank.bmp", window.penguinRow, window.penguinColumn);
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
