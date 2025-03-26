 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
namespace SOKOBAN_ASSESSMENT
{
    class EASYMODE : Window
    {
        private Canvas windowCanvas { get; set; }
        private Button returnButton { get; set; }
        private TextBlock instructionBlock { get; set; }
        private TextBlock counterBlock { get; set; }
        private Border gridBorder { get; set; }
        public Grid appGrid { get; set; }

        
        private PopulateGrid populatedGrid { get; set; }
        //public int wallRow { get; set; }
        //public int wallColumn { get; set; }
        public List<Tuple<int, int>> wallPositions = new List<Tuple<int, int>>();
        public List<Tuple<int, int>> blankPositions = new List<Tuple<int, int>>();
        public List<Tuple<int, int>> boxPositions = new List<Tuple<int, int>>();
        public List<Tuple<int, int>> goalPositions = new List<Tuple<int, int>>();
        public HashSet<(int, int)> reachedGoals = new HashSet<(int, int)>();


        public int penguinRow { get; set; }
        public int penguinColumn { get; set; }
        public int boxRow { get; set; }
        public int boxColumn { get; set; }
        public int goalRow { get; set; }
        public int goalColumn { get; set; }
        public int nextgoalRow { get; set; }
        public int nextgoalColumn { get; set; }
        private Movement mover {  get; set; }

        public int noOfRows=10; // Rows in the grid
        public int noOfCols=10; // Columns in the grid

        public int moveCount = 0; // Tracks Penguin's moves
        bool gameWon = false;  // Track if game is won
        public EASYMODE(string windowName)
        {
            this.Title = windowName;
            initializeWindow();
        }

        private void initializeWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowCanvas = new Canvas();
            createGrid();
            createSidePanel();
            appGrid.Focus();

            populatedGrid = new PopulateGrid(this);
            populatedGrid.drawGrid1();
            
            this.Content = this.windowCanvas;
            
            mover = new Movement(this);

            setupPageEvents();
        }
        
        private void createGrid()
        {
            gridBorder = new Border();
            gridBorder.BorderThickness = new Thickness(11.00);
            gridBorder.BorderBrush = Brushes.Black;

            appGrid = new Grid();
            appGrid.Width = appGrid.Height = 400;
            appGrid.HorizontalAlignment = HorizontalAlignment.Left;
            appGrid.VerticalAlignment = VerticalAlignment.Top;
            appGrid.Focusable = true;
            gridBorder.Child = appGrid;

            for (int i = 0; i < noOfCols; i++)
                appGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < noOfRows; i++)
                appGrid.RowDefinitions.Add(new RowDefinition());
        }

        private void createSidePanel()
        {
            instructionBlock = new TextBlock();
            instructionBlock.FontSize = 25;
            instructionBlock.FontFamily = new FontFamily("Goudy Stout");
            instructionBlock.Text = "Welcome to easy mode \n";
            instructionBlock.HorizontalAlignment = HorizontalAlignment.Left;

            counterBlock = new TextBlock();
            counterBlock.FontSize = 25;
            counterBlock.FontFamily = new FontFamily("Goudy Stout");
            counterBlock.Text = "MoveCount = 0";
            counterBlock.HorizontalAlignment = HorizontalAlignment.Left;

            returnButton = new Button();
            returnButton.Height = 50;
            returnButton.Width = 350;
            returnButton.FontSize = 15;
            returnButton.Focusable = false;
            returnButton.Content = "Return to Main";
            returnButton.FontFamily = new FontFamily("Goudy Stout");
            arrangeOnCanvas();

        }

        private void arrangeOnCanvas()
        {
            windowCanvas.Children.Add(instructionBlock);
            windowCanvas.Children.Add(counterBlock);
            windowCanvas.Children.Add(returnButton);
            windowCanvas.Children.Add(gridBorder);

            Canvas.SetLeft(instructionBlock, 490);
            Canvas.SetTop(instructionBlock, 100);

            Canvas.SetLeft(counterBlock, 490);
            Canvas.SetTop(counterBlock, 320);

            Canvas.SetLeft(returnButton, 490);
            Canvas.SetTop(returnButton, 380);
        }
        public void updateMoveCounter()
        {
            moveCount++;  // Increment the move count
            counterBlock.Text = "MoveCount = " + moveCount.ToString();
        }
        protected void returnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void setupPageEvents()
        {
            returnButton.Click += returnButton_Click;
            appGrid.KeyDown += appGrid_KeyDown;
        }

        protected void appGrid_KeyDown(object sender, KeyEventArgs e)
        {
            mover.MovePenguin(e);
        }

    }


}


